using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToQuery
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1_1
                    // This is a Full Collection Query
                    // No auto-index is created since no filtering is applied
                    
                    List<Employee> allEmployees = session
                        .Query<Employee>() // Query for all documents from 'Employees' collection
                        .ToList();         // Execute the query
                    
                    // All 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_2
                    // This is a Full Collection Query
                    // No auto-index is created since no filtering is applied
                    
                    List<Employee> allEmployees = await asyncSession
                        .Query<Employee>() // Query for all documents from 'Employees' collection
                        .ToListAsync();    // Execute the query
                    
                    // All 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_1_3
                    // This is a Full Collection Query
                    // No auto-index is created since no filtering is applied
                    
                    // Query for all documents from 'Employees' collection
                    IRavenQueryable<Employee> query = from employee in session.Query<Employee>()
                                                      select employee;
                    // Execute the query
                    List<Employee> allEmployees = query.ToList();
                    
                    // All 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_4
                    // This is a Full Collection Query
                    // No auto-index is created since no filtering is applied
                    
                    // Query for all documents from 'Employees' collection
                    IRavenQueryable<Employee> query = from employee in asyncSession.Query<Employee>()
                                                      select employee;
                    // Execute the query
                    List<Employee> allEmployees = await query.ToListAsync();
                    
                    // All 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_2_1
                    // Query collection by document ID
                    // No auto-index is created when querying only by ID
                    
                    Employee employee = session
                        .Query<Employee>()
                        .Where(x => x.Id == "employees/1-A") // Query for specific document from 'Employees' collection 
                        .FirstOrDefault();                   // Execute the query
                    
                    // The resulting 'Employee' entity is loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2_2
                    // Query collection by document ID
                    // No auto-index is created when querying only by ID
                    
                    Employee employee = await asyncSession
                        .Query<Employee>()
                        .Where(x => x.Id == "employees/1-A") // Query for specific document from 'Employees' collection 
                        .FirstOrDefaultAsync();              // Execute the query
                    
                    // The resulting 'Employee' entity is loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_2_3
                    // Query collection by document ID
                    // No auto-index is created when querying only by ID
                    
                    // Query for specific document from 'Employees' collection 
                    IRavenQueryable<Employee> query = from employee in session.Query<Employee>()
                                                      where employee.Id == "employees/1-A"
                                                      select employee;
                    // Execute the query
                    Employee employeeResult = query.FirstOrDefault();
                    
                    // The resulting 'Employee' entity is loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2_4
                    // Query collection by document ID
                    // No auto-index is created when querying only by ID
                    
                    // Query for specific document from 'Employees' collection 
                    IRavenQueryable<Employee> query = from employee in asyncSession.Query<Employee>()
                                                      where employee.Id == "employees/1-A"
                                                      select employee;
                    // Execute the query
                    Employee employeeResult = await query.FirstOrDefaultAsync();
                    
                    // The resulting 'Employee' entity is loaded and will be tracked by the session 
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_3_1
                    // Query collection - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    List<Employee> employees = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert") // Query for all 'Employee' documents that match this predicate 
                        .ToList();                           // Execute the query
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_3_2
                    // Query collection - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    List<Employee> employees = await asyncSession
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert") // Query for all 'Employee' documents that match this predicate 
                        .ToListAsync();                      // Execute the query
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_3_3
                    // Query collection - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    // Query for all 'Employee' documents that match the requested predicate 
                    IRavenQueryable<Employee> query = from employee in session.Query<Employee>()
                                                      where employee.FirstName == "Robert"
                                                      select employee;
                    // Execute the query
                    List<Employee> employees = query.ToList();
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_3_4
                    // Query collection - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    // Query for all 'Employee' documents that match the requested predicate 
                    IRavenQueryable<Employee> query = from employee in asyncSession.Query<Employee>()
                                                      where employee.FirstName == "Robert"
                                                      select employee;
                    // Execute the query
                    List<Employee> employees = await query.ToListAsync();
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_4_1
                    // Query collection - page results
                    // No auto-index is created since no filtering is applied
                    
                    List<Product> products = session
                        .Query<Product>()
                        .Skip(5)   // Skip first 5 results
                        .Take(10)  // Load up to 10 entities from 'Products' collection
                        .ToList(); // Execute the query
                    
                    // The resulting 'Product' entities are loaded and will be tracked by the session 
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_4_2
                    // Query collection - page results
                    // No auto-index is created since no filtering is applied
                    
                    List<Product> products = await asyncSession
                        .Query<Product>()
                        .Skip(5)        // Skip first 5 results
                        .Take(10)       // Load up to 10 entities from 'Products' collection
                        .ToListAsync(); // Execute the query
                    
                    // The resulting 'Product' entities are loaded and will be tracked by the session
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_4_3
                    // Query collection - page results
                    // No auto-index is created since no filtering is applied
                    
                    IRavenQueryable<Product> query = (from product in session.Query<Product>()
                                                     select product)
                                                     .Skip(5)   // Skip first 5 results
                                                     .Take(10); // Load up to 10 entities from 'Products' collection
                    // Execute the query
                    List<Product> products = query.ToList();
                    
                    // The resulting 'Product' entities are loaded and will be tracked by the session
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_4_4
                    // Query collection - page results
                    // No auto-index is created since no filtering is applied
                    
                    IRavenQueryable<Product> query = (from product in asyncSession.Query<Product>()
                                                     select product)
                                                     .Skip(5)   // Skip first 5 results
                                                     .Take(10); // Load up to 10 entities from 'Products' collection
                    // Execute the query
                    List<Product> products = await query.ToListAsync();
                    
                    // The resulting 'Product' entities are loaded and will be tracked by the session
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_5_1
                    // Query with DocumentQuery - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    List<Employee> employees = session
                        .Advanced.DocumentQuery<Employee>()      // Use DocumentQuery
                        .WhereEquals(x => x.FirstName, "Robert") // Query for all 'Employee' documents that match this predicate 
                        .ToList();                               // Execute the query
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_5_2
                    // Query with DocumentQuery - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    List<Employee> employees = await asyncSession
                        .Advanced.AsyncDocumentQuery<Employee>() // Use DocumentQuery
                        .WhereEquals(x => x.FirstName, "Robert") // Query for all 'Employee' documents that match this predicate 
                        .ToListAsync();                          // Execute the query
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_6_1
                    // Query with RawQuery - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    List<Employee> employees = session
                         // Provide RQL to RawQuery
                        .Advanced.RawQuery<Employee>("from 'Employees' where FirstName = 'Robert'")
                         // Execute the query
                        .ToList();
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_6_2
                    // Query with RawQuery - filter by document field
                    
                    // An auto-index will be created if there isn't already an existing auto-index
                    // that indexes this document field
                    
                    List<Employee> employees = await asyncSession
                         // Provide RQL to AsyncRawQuery
                        .Advanced.AsyncRawQuery<Employee>("from 'Employees' where FirstName = 'Robert'")
                         // Execute the query
                        .ToListAsync();
                    
                    // The resulting 'Employee' entities are loaded and will be tracked by the session 
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region syntax
            // Overloads for querying a collection OR an index:
            // ================================================
            
            IRavenQueryable<T> Query<T>(string indexName = null, 
                string collectionName = null, bool isMapReduce = false);
            
            IDocumentQuery<T> DocumentQuery<T>(string indexName = null,
                string collectionName = null, bool isMapReduce = false);
            
            // Overloads for querying an index:
            // ================================
            
            IRavenQueryable<T> Query<T, TIndexCreator>();
            
            IDocumentQuery<T> DocumentQuery<T, TIndexCreator>();
            
            // RawQuery:
            // =========
            
            IRawDocumentQuery<T> RawQuery<T>(string query);
            #endregion
        }
    }
}
