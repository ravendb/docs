using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Paging
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region paging_0_1
                    // A simple query without paging:
                    // ==============================
                    
                    List<Product> allResults = session
                        .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .OfType<Product>()
                        .ToList();
                    
                    // Executing the query on the Northwind sample data
                    // will result in all 47 Product documents that match the query predicate.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region paging_0_2
                    // A simple query without paging:
                    // ==============================
                    
                    List<Product> allResults = await asyncSession
                        .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .OfType<Product>()
                        .ToListAsync();
                    
                    // Executing the query on the Northwind sample data
                    // will result in all 47 Product documents that match the query predicate.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_0_3
                    // A simple DocumentQuery without paging:
                    // ======================================
                    
                    List<Product> allResults = session.Advanced
                        .DocumentQuery<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .OfType<Product>()
                        .ToList();
                    
                    // Executing the query on the Northwind sample data
                    // will result in all 47 Product documents that match the query predicate.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region paging_1_1
                    // Retrieve only the 3'rd page - when page size is 10:
                    // ===================================================
                    
                    List<Product> thirdPageResults = session
                        .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                         // Get the query stats if you wish to know the TOTAL number of results
                        .Statistics(out QueryStatistics stats)
                         // Apply some filtering condition as needed
                        .Where(x => x.UnitsInStock > 10)
                        .OfType<Product>()
                         // Call 'Skip', pass the number of items to skip from the beginning of the result set
                         // Skip the first 20 resulting documents
                        .Skip(20)
                         // Call 'Take' to define the number of documents to return
                         // Take up to 10 products => so 10 is the "Page Size"
                        .Take(10)
                        .ToList();
                    
                    // When executing this query on the Northwind sample data,
                    // results will include only 10 Product documents ("products/45-A" to "products/54-A")
                    
                    int totalResults = stats.TotalResults;
                    
                    // While the query returns only 10 results,
                    // `totalResults` will hold the total number of matching documents (47).
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region paging_1_2
                    // Retrieve only the 3'rd page - when page size is 10:
                    // ===================================================
                    
                    List<Product> thirdPageResults = await asyncSession
                        .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                         // Get the query stats if you wish to know the TOTAL number of results
                        .Statistics(out QueryStatistics stats)
                         // Apply some filtering condition as needed
                        .Where(x => x.UnitsInStock > 10)
                        .OfType<Product>()
                         // Call 'Skip', pass the number of items to skip from the beginning of the result set
                         // Skip the first 20 resulting documents
                        .Skip(20)
                         // Call 'Take' to define the number of documents to return
                         // Take up to 10 products => so 10 is the "Page Size"
                        .Take(10)
                        .ToListAsync();
                    
                    // When executing this query on the Northwind sample data,
                    // results will include only 10 Product documents ("products/45-A" to "products/54-A")
                    
                    int totalResults = stats.TotalResults;
                    
                    // While the query returns only 10 results,
                    // `totalResults` will hold the total number of matching documents (47).
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_1_3
                    // Retrieve only the 3'rd page - when page size is 10:
                    // ===================================================
                    
                    List<Product> thirdPageResults = session.Advanced
                        .DocumentQuery<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                         // Get the query stats if you wish to know the TOTAL number of results
                        .Statistics(out QueryStatistics stats)
                         // Apply some filtering condition as needed
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .OfType<Product>()
                         // Call 'Skip', pass the number of items to skip from the beginning of the result set
                         // Skip the first 20 resulting documents
                        .Skip(20)
                         // Call 'Take' to define the number of documents to return
                         // Take up to 10 products => so 10 is the "Page Size"
                        .Take(10)
                        .ToList();
                    
                    // When executing this query on the Northwind sample data,
                    // results will include only 10 Product documents ("products/45-A" to "products/54-A")
                    
                    int totalResults = stats.TotalResults;
                    
                    // While the query returns only 10 results,
                    // `totalResults` will hold the total number of matching documents (47).
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region paging_2_1
                    // Query for all results - page by page:
                    // =====================================
                    
                    List<Product> pagedResults;
                    int pageNumber = 0;
                    int pageSize = 10;
                
                    do
                    {
                        pagedResults = session
                            .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                             // Apply some filtering condition as needed
                            .Where(x => x.UnitsInStock > 10)
                            .OfType<Product>()
                             // Skip the number of results that were already fetched
                            .Skip(pageNumber * pageSize)
                             // Request to get 'pageSize' results
                            .Take(pageSize)
                            .ToList();
                        
                        pageNumber++;
                        
                        // Make any processing needed with the current paged results here
                        // ...
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region paging_2_2
                    // Query for all results - page by page:
                    // =====================================
                    
                    List<Product> pagedResults;
                    int pageNumber = 0;
                    int pageSize = 10;
                
                    do
                    {
                        pagedResults = await asyncSession
                            .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                             // Apply some filtering condition as needed
                            .Where(x => x.UnitsInStock > 10)
                            .OfType<Product>()
                             // Skip the number of results that were already fetched
                            .Skip(pageNumber * pageSize)
                             // Request to get 'pageSize' results
                            .Take(pageSize)
                            .ToListAsync();
                        
                        pageNumber++;
                        
                        // Make any processing needed with the current paged results here
                        // ...
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_2_3
                    // Query for all results - page by page:
                    // =====================================
                    
                    List<Product> pagedResults;
                    int pageNumber = 0;
                    int pageSize = 10;
                
                    do
                    {
                        pagedResults = session.Advanced
                            .DocumentQuery<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                             // Apply some filtering condition as needed
                            .WhereGreaterThan(x => x.UnitsInStock, 10)
                            .OfType<Product>()
                             // Skip the number of results that were already fetched
                            .Skip(pageNumber * pageSize)
                             // Request to get 'pageSize' results
                            .Take(pageSize)
                            .ToList();
                        
                        pageNumber++;
                        
                        // Make any processing needed with the current paged results here
                        // ...
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region paging_3_1
                    List<ProjectedClass> pagedResults;
                    
                    int totalResults = 0;
                    int totalUniqueResults = 0;
                    int skippedResults = 0;
                    
                    int pageNumber = 0;
                    int pageSize = 10;
                
                    do
                    {
                        pagedResults = session
                            .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                            .Statistics(out QueryStatistics stats)
                            .Where(x => x.UnitsInStock > 10)
                            .OfType<Product>()
                             // Define a projection
                            .Select(x => new ProjectedClass
                            {
                                Category = x.Category,
                                Supplier = x.Supplier
                            })
                             // Call Distinct to remove duplicate projected results
                            .Distinct()
                             // Add the number of skipped results to the "start location"  
                            .Skip((pageNumber * pageSize) + skippedResults)
                             // Define how many items to return
                            .Take(pageSize)
                            .ToList();
                        
                        totalResults = stats.TotalResults;        // Number of total matching documents (includes duplicates)
                        skippedResults += stats.SkippedResults;   // Number of duplicate results that were skipped
                        totalUniqueResults += pagedResults.Count; // Number of unique results returned in this server call
                        
                        pageNumber++;
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    
                    // When executing the query on the Northwind sample data:
                    // ======================================================
                    
                    // The total matching results reported in the stats is 47 (totalResults),
                    // but the total unique objects returned while paging the results is only 29 (totalUniqueResults)
                    // due to the 'Distinct' usage which removes duplicates.
                    
                    // This is solved by adding the skipped results count to Skip().
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region paging_3_2
                    List<ProjectedClass> pagedResults;
                    
                    int totalResults = 0;
                    int totalUniqueResults = 0;
                    int skippedResults = 0;
                    
                    int pageNumber = 0;
                    int pageSize = 10;
                
                    do
                    {
                        pagedResults = await asyncSession
                            .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                            .Statistics(out QueryStatistics stats)
                            .Where(x => x.UnitsInStock > 10)
                            .OfType<Product>()
                             // Define a projection
                            .Select(x => new ProjectedClass
                            {
                                Category = x.Category,
                                Supplier = x.Supplier
                            })
                             // Call Distinct to remove duplicate projected results
                            .Distinct()
                             // Add the number of skipped results to the "start location"  
                            .Skip((pageNumber * pageSize) + skippedResults)
                            .Take(pageSize)
                            .ToListAsync();
                        
                        totalResults = stats.TotalResults;        // Number of total matching documents (includes duplicates)
                        skippedResults += stats.SkippedResults;   // Number of duplicate results that were skipped
                        totalUniqueResults += pagedResults.Count; // Number of unique results returned in this server call
                        
                        pageNumber++;
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    
                    // When executing the query on the Northwind sample data:
                    // ======================================================
                    
                    // The total matching results reported in the stats is 47 (totalResults),
                    // but the total unique objects returned while paging the results is only 29 (totalUniqueResults)
                    // due to the 'Distinct' usage which removes duplicates.
                    
                    // This is solved by adding the skipped results count to Skip().
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region paging_3_3
                    List<ProjectedClass> pagedResults;
                    
                    int totalResults = 0;
                    int totalUniqueResults = 0;
                    int skippedResults = 0;
                    
                    int pageNumber = 0;
                    int pageSize = 10;
                
                    do
                    {
                        pagedResults = session.Advanced
                            .DocumentQuery<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                            .Statistics(out QueryStatistics stats)
                            .WhereGreaterThan(x => x.UnitsInStock, 10)
                            .OfType<Product>()
                             // Define a projection
                            .SelectFields<ProjectedClass>()
                             // Call Distinct to remove duplicate projected results
                            .Distinct()
                             // Add the number of skipped results to the "start location"  
                            .Skip((pageNumber * pageSize) + skippedResults)
                            .Take(pageSize)
                            .ToList();
                        
                        totalResults = stats.TotalResults;        // Number of total matching documents (includes duplicates)
                        skippedResults += stats.SkippedResults;   // Number of duplicate results that were skipped
                        totalUniqueResults += pagedResults.Count; // Number of unique results returned in this server call
                        
                        pageNumber++;
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    
                    // When executing the query on the Northwind sample data:
                    // ======================================================
                    
                    // The total matching results reported in the stats is 47 (totalResults),
                    // but the total unique objects returned while paging the results is only 29 (totalUniqueResults)
                    // due to the 'Distinct' usage which removes duplicates.
                    
                    // This is solved by adding the skipped results count to Skip().
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region paging_4_1
                    List<Order> pagedResults;
                    
                    int totalResults = 0;
                    int totalUniqueResults = 0;
                    int skippedResults = 0;
                    
                    int pageNumber = 0;
                    int pageSize = 50;
                
                    do
                    {
                        pagedResults = session
                            .Query<Orders_ByProductName.IndexEntry, Orders_ByProductName>()
                            .Statistics(out QueryStatistics stats)
                            .OfType<Order>()
                             // Add the number of skipped results to the "start location"  
                            .Skip((pageNumber * pageSize) + skippedResults)
                            .Take(pageSize)
                            .ToList();
                        
                        totalResults = stats.TotalResults;
                        skippedResults += stats.SkippedResults;
                        totalUniqueResults += pagedResults.Count;
                        
                        pageNumber++;
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    
                    // When executing the query on the Northwind sample data:
                    // ======================================================
                    
                    // The total results reported in the stats is 2155 (totalResults),
                    // which represent the multiple index-entries generated as defined by the fanout index.
                    
                    // By adding the skipped results count to the Skip() method,
                    // we get the correct total unique results which is 830 Order documents.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region paging_4_2
                    List<Order> pagedResults;
                    
                    int totalResults = 0;
                    int totalUniqueResults = 0;
                    int skippedResults = 0;
                    
                    int pageNumber = 0;
                    int pageSize = 50;
                
                    do
                    {
                        pagedResults = await asyncSession
                            .Query<Orders_ByProductName.IndexEntry, Orders_ByProductName>()
                            .Statistics(out QueryStatistics stats)
                            .OfType<Order>()
                             // Add the number of skipped results to the "start location"  
                            .Skip((pageNumber * pageSize) + skippedResults)
                            .Take(pageSize)
                            .ToListAsync();
                        
                        totalResults = stats.TotalResults;
                        skippedResults += stats.SkippedResults;
                        totalUniqueResults += pagedResults.Count;
                        
                        pageNumber++;
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    
                    // When executing the query on the Northwind sample data:
                    // ======================================================
                    
                    // The total results reported in the stats is 2155 (totalResults),
                    // which represent the multiple index-entries generated as defined by the fanout index.
                    
                    // By adding the skipped results count to the Skip() method,
                    // we get the correct total unique results which is 830 Order documents.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region paging_4_3
                    List<Order> pagedResults;
                    
                    int totalResults = 0;
                    int totalUniqueResults = 0;
                    int skippedResults = 0;
                    
                    int pageNumber = 0;
                    int pageSize = 50;
                
                    do
                    {
                        pagedResults = session.Advanced
                            .DocumentQuery<Orders_ByProductName.IndexEntry, Orders_ByProductName>()
                            .Statistics(out QueryStatistics stats)
                            .OfType<Order>()
                             // Add the number of skipped results to the "start location"  
                            .Skip((pageNumber * pageSize) + skippedResults)
                            .Take(pageSize)
                            .ToList();
                        
                        totalResults = stats.TotalResults;
                        skippedResults += stats.SkippedResults;
                        totalUniqueResults += pagedResults.Count;
                        
                        pageNumber++;
                    }
                    while (pagedResults.Count > 0); // Fetch next results
                    
                    // When executing the query on the Northwind sample data:
                    // ======================================================
                    
                    // The total results reported in the stats is 2155 (totalResults),
                    // which represent the multiple index-entries generated as defined by the fanout index.
                    
                    // By adding the skipped results count to the Skip() method,
                    // we get the correct total unique results which is 830 Order documents.
                    #endregion
                }
            }
        }
        
        #region projected_class
        public class ProjectedClass
        {
            public string Category { get; set; }
            public string Supplier { get; set; }
        }
        #endregion
        
        #region index_0
        public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                public int UnitsInStock { get; set; }
            }
            
            public Products_ByUnitsInStock()
            {
                Map = products => from product in products
                    select new
                    {
                        UnitsInStock = product.UnitsInStock
                    };
            }
        }
        #endregion

        #region index_1
        // A fanout index - creating MULTIPLE index-entries per document:
        // ==============================================================
        
        public class Orders_ByProductName : AbstractIndexCreationTask<Order>
        {
            public class IndexEntry
            {
                public string ProductName { get; set; }
            }
            
            public Orders_ByProductName()
            {
                Map = orders => 
                    from order in orders
                    from line in order.Lines
                    select new IndexEntry
                    {
                        ProductName = line.ProductName
                    };
            }
        }
        #endregion
    }
}
