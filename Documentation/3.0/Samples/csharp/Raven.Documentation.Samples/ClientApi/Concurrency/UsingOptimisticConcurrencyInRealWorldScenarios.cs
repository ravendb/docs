using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Abstractions.Exceptions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Listeners;
using Raven.Documentation.Samples.Indexes.Querying;
using Raven.Imports.Newtonsoft.Json;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Concurrency
{
    class UsingOptimisticConcurrencyInRealWorldScenarios
    {
        #region concurrency_1
        public class Person
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        #endregion

        public void Sample2()
        {
            var id = 1;
            using (var documentStore = new DocumentStore())
            {
                #region concurrency_2

                using (var session = documentStore.OpenSession())
                {
                    session.Advanced.UseOptimisticConcurrency = true;
                    var person = session.Load<HandlingDocumentRelationships.Person>(id);

                    //…

                    try
                    {
                        session.Store(person);
                        session.SaveChanges();
                    }
                    catch (ConcurrencyException ex)
                    {
                        //Inform user or merge changes
                    }
                }

                #endregion
            }
        }
    }

        #region concurrency_3
        public class Person
        {
            public string Id { get; set; }
            [JsonIgnore]
            public Guid? Etag { get; set; }
            public string Name { get; set; }
        }
        #endregion

    class UsingOptimisticConcurrencyInRealWorldScenarios2
    {
        private DocumentStore documentStore;
        public UsingOptimisticConcurrencyInRealWorldScenarios2()
        {
            documentStore = new DocumentStore();
        }

        #region concurrency_4
        public Person Get(string id)
        {
            using (var session = documentStore.OpenSession())
            {
                var person = session.Load<Person>(id);
                person.Etag = session.Advanced.GetEtagFor(person);
                return person;
            }
        }

        public IList<Person> GetAll()
        {
            using (var session = documentStore.OpenSession())
            {
                return session.Query<Person>()
                    .ToList()
                    .Select(x =>
                    {
                        x.Etag = session.Advanced.GetEtagFor(x);
                        return x;
                    })
                    .ToList();
            }
        }

        public void Update(Person person)
        {
            using (var session = documentStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;
                session.Store(person, person.Etag, person.Id);
                session.SaveChanges();
                person.Etag = session.Advanced.GetEtagFor(person);
            }
        }
        #endregion
    }

    class UsingOptimisticConcurrencyInRealWorldScenarios3
    {
        #region concurrency_5
        public interface IDocumentStoreListener
        {
            void AfterStore(string key, object entityInstance, RavenJObject metadata);
            bool BeforeStore(string key, object entityInstance, RavenJObject metadata);
        }

        public interface IDocumentConversionListener
        {
            void DocumentToEntity(object entity, RavenJObject document, RavenJObject metadata);
            void EntityToDocument(object entity, RavenJObject document, RavenJObject metadata);
        }
        #endregion
    }

    class UsingOptimisticConcurrencyInRealWorldScenarios4
    {
        #region concurrency_6
        public class DocumentStoreListener : IDocumentStoreListener
        {
            public void AfterStore(string key, object entityInstance, RavenJObject metadata)
            {
                var person = entityInstance as Person;
                if (person != null)
                {
                    person.Etag = metadata.Value<Guid>("@etag");
                }
            }

            public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
            {
                return true;
            }
        }

        public class DocumentConversionListener : IDocumentConversionListener
        {
            public void BeforeConversionToDocument(string key, object entity, RavenJObject metadata)
            {
            }

            public void AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
            {
            }

            public void BeforeConversionToEntity(string key, RavenJObject document, RavenJObject metadata)
            {
            }

            public void AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity)
            {
                var person = entity as Person;
                if (person != null)
                {
                    person.Etag = metadata.Value<Guid>("@etag");
                }
            }
        }
        #endregion
    }

}
