using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    class FilterByNonExistingField
    {
        public async void Examples()
        {
            using (var store = new DocumentStore())
            {
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
                        .WhereExists("missingFieldName")
                        .ToList();

                    #endregion
                }
            }
        }

        class HowToFilterByNonExistingField
        {
            public async void Examples<T, TIndexCreator>(string missingFieldName)
                where TIndexCreator : AbstractIndexCreationTask, new()
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
                            .WhereExists("missingFieldName")
                            .ToList();

                        #endregion
                    }
                }
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

            public Address Address
            {
                get;
                set;
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
}
