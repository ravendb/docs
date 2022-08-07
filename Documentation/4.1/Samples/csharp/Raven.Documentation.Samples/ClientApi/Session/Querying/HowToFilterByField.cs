using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    class HowToFilterByField
    {
        public interface IFoo<T>
        {
            #region whereexists_1
            IDocumentQuery<T> WhereExists(string fieldName);

            IDocumentQuery<T> WhereExists<TValue>(Expression<Func<T, TValue>> propertySelector);
            #endregion
        }

        public async void Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereexists_2
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists("FirstName")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereexists_2_async
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists("FirstName")
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region whereNotexists_1
                    List<Order> results = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .Not
                        .WhereExists("Freight")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region whereNotexists_signature
                    List<T> results = session
                        .Advanced
                        .DocumentQuery<T>()
                        .Not
                        .WhereExists("missing field")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereexists_3
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists("Address.Location.Latitude")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereexists_3_async
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists("Address.Location.Latitude")
                        .ToListAsync();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereexists_4
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists(x => x.Address.Location.Latitude)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereexists_4_async
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists(x => x.Address.Location.Latitude)
                        .ToListAsync();
                    #endregion
                }
            }
        }

        class HowToFilterByNonExistingField
        {
            public async void Examples<T, TIndexCreator>(string fieldName) where TIndexCreator : AbstractIndexCreationTask, new()
            {
                using (var store = new DocumentStore())
                {

                    using (var session = store.OpenSession())
                    {
                        #region whereNotexists_1
                        // Create a list of documents in `results` from Order
                        List<Order> results = session
                            .Advanced
                            .DocumentQuery<Order>()
                            // Make sure that the index has finished before listing the results
                            .WaitForNonStaleResults(TimeSpan.MaxValue)
                            // Negate the next method
                            .Not
                            // Specify the field that is suspected to be missing
                            .WhereExists("Freight")
                            .ToList();
                        #endregion
                    }

                    using (var session = store.OpenSession())
                    {
                        #region whereNotexists_signature
                        List<T> results = session
                            .Advanced
                            .DocumentQuery<T>()
                            .Not
                            .WhereExists("fieldName")
                            .ToList();
                        #endregion
                    }
                    
                    using (var session = store.OpenSession())
                    {
                        #region whereNotexists_StaticSignature
                        List<T> results = session
                            .Advanced
                            .DocumentQuery<T, TIndexCreator>()
                            .Not
                            .WhereExists(fieldName)
                            .ToList();
                        #endregion
                    }
                }
            }

            private interface IFoo
            {
                #region document_query_1
                IDocumentQuery<T> DocumentQuery<T, TIndexCreator>();

                IDocumentQuery<T> DocumentQuery<T>(
                    string indexName = null,
                    string collectionName = null,
                    bool isMapReduce = false);
                #endregion
            }

            class HowToFilterByNonExistingField2
            {

                #region IndexwhereNotexists_example
                // Create or modify a static index called Orders_ByFreight
                public class Orders_ByFreight : AbstractIndexCreationTask<Order>
                {
                    public Orders_ByFreight()
                    {
                        // Specify collection name
                        Map = orders => from doc in orders
                                           select new
                                           {
                                               // Field that is missing in some documents
                                               doc.Freight,
                                               // Field that exists in all documents
                                               doc.Id
                                           };
                    }
                }
                #endregion


                public void FilteringByNonExistingFieldStaticIndexQuery()
                {
                    using (var store = new DocumentStore())
                    {
                        using (var session = store.OpenSession())
                        {
                            #region QuerywhereNotexists_example
                            List<Order> results = session
                                .Advanced
                                // Query the static index 
                                .DocumentQuery<Order, Orders_ByFreight>()
                                // Verify that the index is not stale (optional)
                                .WaitForNonStaleResults(TimeSpan.MaxValue)
                                // Negate the next method
                                .Not
                                // Specify the field that is suspected to be missing
                                .WhereExists(x => x.Freight)
                                .ToList();
                                // `results` will contain the list of incomplete documents.
                            #endregion
                        }
                    }
                }

            }
        }


        public class Employee
        {
            public string FirstName { get; set; }
            public Address Address { get; set; }

        }

        public class Address
        {
            public Location Location { get; set; }
        }

        public class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }

    internal class TIndexCreator
    {
    }

    internal class T
    {
    }
}
