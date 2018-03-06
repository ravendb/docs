using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Exceptions;

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
            var id = "preson/1";
            using (var documentStore = new DocumentStore())
            {
                #region concurrency_2

                using (var session = documentStore.OpenSession())
                {
                    session.Advanced.UseOptimisticConcurrency = true;
                    var person = session.Load<Person>(id);

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
            public string ChangeVector { get; set; }
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
                person.ChangeVector = session.Advanced.GetChangeVectorFor(person);
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
                        x.ChangeVector = session.Advanced.GetChangeVectorFor(x);
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
                session.Store(person, person.ChangeVector, person.Id);
                session.SaveChanges();
                person.ChangeVector = session.Advanced.GetChangeVectorFor(person);
            }
        }
        #endregion
    }
}
