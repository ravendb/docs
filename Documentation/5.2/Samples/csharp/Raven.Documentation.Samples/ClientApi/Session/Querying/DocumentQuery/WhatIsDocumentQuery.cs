using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using System.Threading.Tasks;
using Raven.Client.Documents.Queries.Explanation;
using Raven.Client.Documents.Linq;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying.DocumentQuery
{
    public class WhatIsDocumentQuery
    {
        public async Task DocumentQueryExamples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region documentQuery_1
                    // Query for all documents from 'Employees' collection
                    List<Employee> allEmployees = session.Advanced
                        .DocumentQuery<Employee>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region documentQuery_1_async
                    // Query for all documents from 'Employees' collection
                    List<Employee> allEmployees = await asyncSession.Advanced
                        .AsyncDocumentQuery<Employee>() 
                        .ToListAsync();  
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region documentQuery_2
                    // Query collection by document ID
                    Employee employee = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.Id, "employees/1-A")
                        .FirstOrDefault();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region documentQuery_2_async
                    // Query collection by document ID
                    Employee employee = await asyncSession.Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereEquals(x => x.Id, "employees/1-A")
                        .FirstOrDefaultAsync();  
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region documentQuery_3
                    // Query collection - filter by document field
                    List<Employee> employees = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region documentQuery_3_async
                    // Query collection - filter by document field
                    List<Employee> employees = await asyncSession.Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToListAsync();  
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region documentQuery_4
                    // Query collection - page results
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                        .Skip(5)   // Skip first 5 results
                        .Take(10)  // Load up to 10 entities from 'Products' collection
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region documentQuery_4_async
                    // Query collection - page results
                    List<Product> products = await asyncSession.Advanced
                        .AsyncDocumentQuery<Product>()
                        .Skip(5)   // Skip first 5 results
                        .Take(10)  // Load up to 10 entities from 'Products' collection
                        .ToListAsync();  
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region documentQuery_5
                    // Define a DocumentQuery
                    var docQuery = session.Advanced
                        .DocumentQuery<Order>(); // 'IDocumentQuery' instance
                    
                    // Convert to Query
                    var query = docQuery.ToQueryable(); // 'IRavenQueryable' instance
                    
                    // Apply any 'IRavenQueryable' LINQ extension
                    var queryResults = query
                        .Where(x => x.Freight > 25)
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region documentQuery_5_async
                    // Define a DocumentQuery
                    var docQuery = asyncSession.Advanced
                        .AsyncDocumentQuery<Order>(); // 'IAsyncDocumentQuery' instance
                    
                    // Convert to Query
                    var query = docQuery.ToQueryable(); // 'IRavenQueryable' instance
                    
                    // Apply any 'IRavenQueryable' LINQ extension
                    var queryResults = await query
                        .Where(x => x.Freight > 25) 
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region documentQuery_6
                    // Define a DocumentQuery
                    var docQuery = session.Advanced
                        .DocumentQuery<Order>()
                        .WhereGreaterThan("Freight", 25);
                    
                    // Convert to Query
                    var query = docQuery.ToQueryable();
                    
                    // Define the projection on the query using LINQ
                    var projectedQuery = from order in query
                        // Load the related document
                        let company = session.Load<Company>(order.Company)
                        // Define the projection
                        select new
                        {
                            Freight = order.Freight,   // data from the Order document
                            CompanyName = company.Name // data from the related Company document
                        };

                    // Execute the query
                    var queryResults = projectedQuery.ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region documentQuery_7
                    // Define a Query
                    var query = session
                        .Query<Order>()
                        .Where(x => x.Freight > 25);
                        
                    // Convert to DocumentQuery
                    var docQuery = query.ToDocumentQuery();

                    // Apply a DocumentQuery method (e.g. IncludeExplanations is Not available on Query)
                    docQuery.IncludeExplanations(out Explanations exp);
                    
                    // Execute the query
                    var docQueryResults = docQuery.ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region documentQuery_7_async
                    // Define a Query
                    var query = asyncSession
                        .Query<Order>()
                        .Where(x => x.Freight > 25);
                        
                    // Convert to DocumentQuery
                    var docQuery = query.ToAsyncDocumentQuery();

                    // Apply a DocumentQuery method (e.g. IncludeExplanations is Not available on Query)
                    docQuery.IncludeExplanations(out Explanations exp);
                    
                    // Execute the query
                    var docQueryResults = docQuery.ToListAsync();
                    #endregion
                }
            }
        }
    }
}
