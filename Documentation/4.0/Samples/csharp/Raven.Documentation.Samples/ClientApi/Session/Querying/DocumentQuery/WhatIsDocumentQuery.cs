using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.DocumentQuery
{
    public class WhatIsDocumentQuery
    {
        private class MyCustomIndex : AbstractIndexCreationTask<Employee>
        {
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

        public WhatIsDocumentQuery()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region document_query_2
                    // load all entities from 'Employees' collection
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region document_query_3
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region document_query_4
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    // using 'My/Custom/Index'
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee>("My/Custom/Index")
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region document_query_5
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert'
                    // using 'My/Custom/Index'
                    List<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee, MyCustomIndex>()
                        .WhereEquals("FirstName", "Robert")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
