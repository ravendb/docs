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
    public class ExactMatch
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region exact_1
                    List<Employee> employees = session
                         // Make a dynamic query on 'Employees' collection    
                        .Query<Employee>()
                         // Query for all documents where 'FirstName' equals 'Robert'
                         // Pass 'exact: true' for a case-sensitive match
                        .Where(x => x.FirstName == "Robert", exact: true)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region exact_2
                    List<Employee> employees = await asyncSession
                         // Make a dynamic query on 'Employees' collection    
                        .Query<Employee>()
                         // Query for all documents where 'FirstName' equals 'Robert'
                         // Pass 'exact: true' for a case-sensitive match
                        .Where(x => x.FirstName == "Robert", exact: true)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region exact_3
                    List<Employee> employees = session.Advanced
                         // Make a dynamic DocumentQuery on 'Employees' collection    
                        .DocumentQuery<Employee>()
                         // Query for all documents where 'FirstName' equals 'Robert'
                         // Pass 'exact: true' for a case-sensitive match
                        .WhereEquals(x => x.FirstName, "Robert", exact: true)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region exact_4
                    List<Order> orders = session
                         // Make a dynamic query on 'Orders' collection
                        .Query<Order>()
                         // Query for documents that contain at least one order line with 'Teatime Chocolate Biscuits'
                        .Where(x => x.Lines.Any(p => p.ProductName == "Teatime Chocolate Biscuits"),
                            // Pass 'exact: true' for a case-sensitive match 
                            exact: true)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region exact_5
                    List<Order> orders = await asyncSession
                         // Make a dynamic query on 'Orders' collection
                        .Query<Order>()
                         // Query for documents that contain at least one order line with 'Teatime Chocolate Biscuits'
                        .Where(x => x.Lines.Any(p => p.ProductName == "Teatime Chocolate Biscuits"),
                            // Pass 'exact: true' for a case-sensitive match 
                            exact: true)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region exact_6
                    List<Order> orders = session.Advanced
                         // Make a dynamic DocumentQuery on 'Orders' collection
                        .DocumentQuery<Order>()
                         // Query for documents that contain at least one order line with 'Teatime Chocolate Biscuits'
                        .WhereEquals("Lines[].ProductName", "Teatime Chocolate Biscuits",
                             // Pass 'exact: true' for a case-sensitive match 
                             exact: true)
                        .ToList();
                    #endregion
                }
            }
        }
        
        private interface IFoo<T>
        {
            #region syntax
            IRavenQueryable<T> Where<T>(Expression<Func<T, bool>> predicate, bool exact);
            IRavenQueryable<T> Where<T>(Expression<Func<T, int, bool>> predicate, bool exact);
            #endregion
        }
    }
}
