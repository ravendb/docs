using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToQueryWithExactMatch
    {
        private interface IFoo<T>
        {
            /*
            #region query_1_0
            IRavenQueryable<T> Where<T>(this IQueryable<T> source, Expression<Func<T, int, bool>> predicate, bool exact)
            IRavenQueryable<T> Where<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool exact);
            #endregion
            */
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
                    #region query_1_2
                    // load all employees 
                    // where FirstName equals 'Robert' (case sensitive match)
                    List<Employee> employees = session
                        .Advanced.DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert", exact: true)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_2_async
                    // load all employees 
                    // where FirstName equals 'Robert' (case sensitive match)
                    List<Employee> employees = await asyncSession
                        .Advanced.AsyncDocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert", exact: true)
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
