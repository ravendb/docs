using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class SortQueryResults
    {
        #region index_1
        public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                public int UnitsInStock { get; set; }
            }
            
            public Products_ByUnitsInStock()
            {
                Map = products => from product in products
                    select new IndexEntry()
                    {
                        UnitsInStock = product.UnitsInStock
                    };
            }
        }
        #endregion

        #region index_2
        public class Products_BySearchName : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                // Index-field 'Name' will be configured below for full-text search
                public string Name { get; set; }
                
                // Index-field 'NameForSorting' will be used for ordering query results 
                public string NameForSorting { get; set; }
            }

            public Products_BySearchName()
            {
                Map = products => from product in products
                    select new
                    {
                        // Both index-fields are assigned the same content (the 'Name' from the document)
                        Name = product.Name,
                        NameForSorting = product.Name 
                    };

                // Configure only the 'Name' index-field for FTS
                Indexes.Add(x => x.Name, FieldIndexing.Search);
            }
        }
        #endregion

        public async Task CanOrderResults()
        {
            using (var store = new DocumentStore())
            {
                // Order by index-field value
                // ==========================
                
                using (var session = store.OpenSession())
                {
                    #region sort_1
                    List<Product> products = session
                         // Query the index
                        .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                         // Apply filtering (optional)
                        .Where(x => x.UnitsInStock > 10)
                         // Call 'OrderByDescending', pass the index-field by which to order the results
                        .OrderByDescending(x => x.UnitsInStock)
                        .OfType<Product>() 
                        .ToList();
                    
                    // Results will be sorted by the 'UnitsInStock' value in descending order,
                    // with higher values listed first.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_2
                    List<Product> products = await asyncSession
                         // Query the index
                        .Query<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                         // Apply filtering (optional)
                        .Where(x => x.UnitsInStock > 10)
                         // Call 'OrderByDescending', pass the index-field by which to order the results
                        .OrderByDescending(x => x.UnitsInStock)
                        .OfType<Product>() 
                        .ToListAsync();
                    
                    // Results will be sorted by the 'UnitsInStock' value in descending order,
                    // with higher values listed first.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sort_3
                    List<Product> products = session.Advanced
                         // Query the index
                        .DocumentQuery<Products_ByUnitsInStock.IndexEntry, Products_ByUnitsInStock>()
                         // Apply filtering (optional)
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                         // Call 'OrderByDescending', pass the index-field by which to order the results
                        .OrderByDescending(x => x.UnitsInStock)
                        .OfType<Product>() 
                        .ToList();
                    
                    // Results will be sorted by the 'UnitsInStock' value in descending order,
                    // with higher values listed first.
                    #endregion
                }
                
                // Order when field is searchable
                // ==============================
                
                using (var session = store.OpenSession())
                {
                    #region sort_4
                    List<Product> products = session
                         // Query the index 
                        .Query<Products_BySearchName.IndexEntry, Products_BySearchName>()
                         // Call 'Search':
                         // Pass the index-field that was configured for FTS and the term to search for.
                         // Here we search for terms that start with "ch" within index-field 'Name'.
                        .Search(x => x.Name, "ch*")
                         // Call 'OrderBy':
                         // Pass the other index-field by which to order the results.
                        .OrderBy(x => x.NameForSorting)
                        .OfType<Product>()
                        .ToList();
                    
                    // Running the above query on the NorthWind sample data, ordering by 'NameForSorting' field,
                    // we get the following order:
                    // =========================================================================================

                    // "Chai"
                    // "Chang"
                    // "Chartreuse verte"
                    // "Chef Anton's Cajun Seasoning"
                    // "Chef Anton's Gumbo Mix"
                    // "Chocolade"
                    // "Jack's New England Clam Chowder"
                    // "Pâté chinois"
                    // "Teatime Chocolate Biscuits"

                    // While ordering by the searchable 'Name' field would have produced the following order:
                    // ======================================================================================

                    // "Chai"
                    // "Chang"
                    // "Chartreuse verte"
                    // "Chef Anton's Cajun Seasoning"
                    // "Pâté chinois"
                    // "Chocolade"
                    // "Teatime Chocolate Biscuits"
                    // "Chef Anton's Gumbo Mix"
                    // "Jack's New England Clam Chowder"
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_5
                    List<Product> products = await asyncSession
                         // Query the index 
                        .Query<Products_BySearchName.IndexEntry, Products_BySearchName>()
                         // Call 'Search':
                         // Pass the index-field that was configured for FTS and the term to search for.
                         // Here we search for terms that start with "ch" within index-field 'Name'.
                        .Search(x => x.Name, "ch*")
                         // Call 'OrderBy':
                         // Pass the other index-field by which to order the results.
                        .OrderBy(x => x.NameForSorting)
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sort_6
                    List<Product> products = session.Advanced
                         // Query the index
                        .DocumentQuery<Products_BySearchName.IndexEntry, Products_BySearchName>()
                         // Call 'Search':
                         // Pass the index-field that was configured for FTS and the term to search for.
                         // Here we search for terms that start with "ch" within index-field 'Name'.
                        .Search("Name", "ch*")
                         // Call 'OrderBy':
                         // Pass the other index-field by which to order the results.
                        .OrderBy("NameForSorting")
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
