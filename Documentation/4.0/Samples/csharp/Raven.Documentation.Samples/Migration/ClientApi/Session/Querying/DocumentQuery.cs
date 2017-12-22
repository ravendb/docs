using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class DocumentQuery
    {
        public void Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region document_query_1
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "John")
                        .WhereEquals(x => x.FirstName, "Bob")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region document_query_2
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .UsingDefaultOperator(QueryOperator.Or)
                        .WhereEquals(x => x.FirstName, "John")
                        .WhereEquals(x => x.FirstName, "Bob")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region document_query_3
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "John")
                        .OrElse()
                        .WhereEquals(x => x.FirstName, "Bob")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
