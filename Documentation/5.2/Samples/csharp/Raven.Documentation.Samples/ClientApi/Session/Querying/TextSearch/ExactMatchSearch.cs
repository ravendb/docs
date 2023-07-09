using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.TextSearch
{
    public class ExactMatchSearch
    {
        private interface IFoo<T>
        {
            #region query_1_0
            IRavenQueryable<T> Where<T>(Expression<Func<T, int, bool>> predicate, bool exact);

            IRavenQueryable<T> Where<T>(Expression<Func<T, bool>> predicate, bool exact);
            #endregion
        }

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1_1
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert' (case sensitive match)
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert", exact: true)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_1_async
                    // load all entities from 'Employees' collection
                    // where FirstName equals 'Robert' (case sensitive match)
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert", exact: true)
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_2_1
                    // return all entities from 'Orders' collection
                    // which contain at least one order line with
                    // 'Singaporean Hokkien Fried Mee' product
                    // perform a case-sensitive match
                    List<Order> orders = session
                        .Query<Order>()
                        .Where(x => x.Lines.Any(p => p.ProductName == "Singaporean Hokkien Fried Mee"), exact: true)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2_1_async
                    // return all entities from 'Orders' collection
                    // which contain at least one order line with
                    // 'Singaporean Hokkien Fried Mee' product
                    // perform a case-sensitive match
                    List<Order> orders = await asyncSession
                        .Query<Order>()
                        .Where(x => x.Lines.Any(p => p.ProductName == "Singaporean Hokkien Fried Mee"), exact: true)
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
