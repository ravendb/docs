using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class SortQueryResults
    {
        public async Task CanOrderResults()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Order by document field
                    // =======================
                    
                    #region sort_1
                    List<Product> products = session
                         // Make a dynamic query on the Products collection    
                        .Query<Product>()
                         // Apply filtering (optional)
                        .Where(x => x.UnitsInStock > 10)
                         // Call 'OrderBy', pass the document field by which to order the results
                        .OrderBy(x => x.UnitsInStock)
                        .ToList();
                    
                    // Results will be sorted by the 'UnitsInStock' value in ascending order,
                    // with smaller values listed first.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_2
                    List<Product> products = await asyncSession
                         // Make a dynamic query on the Products collection    
                        .Query<Product>()
                         // Apply filtering (optional)
                        .Where(x => x.UnitsInStock > 10)
                         // Call 'OrderBy', pass the document field by which to order the results
                        .OrderBy(x => x.UnitsInStock)
                        .ToListAsync();
                    
                    // Results will be sorted by the 'UnitsInStock' value in ascending order,
                    // with smaller values listed first.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_3
                    List<Product> products = session.Advanced
                         // Make a DocumentQuery on the Products collection    
                        .DocumentQuery<Product>()
                         // Apply filtering (optional)
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                         // Call 'OrderBy', pass the document field by which to order the results
                        .OrderBy(x => x.UnitsInStock)
                        .ToList();

                    // Results will be sorted by the 'UnitsInStock' value in ascending order,
                    // with smaller values listed first.
                    #endregion
                }
                
                // Order by score
                // ==============
                
                using (var session = store.OpenAsyncSession())
                {
                    #region sort_4
                    List<Product> products = session
                        .Query<Product>()
                         // Apply filtering
                        .Where(x => x.UnitsInStock < 5 || x.Discontinued)
                         // Call 'OrderByScore'
                        .OrderByScore()
                        .ToList();
                    
                    // Results will be sorted by the score value
                    // with best matching documents (higher score values) listed first.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_5
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Apply filtering
                        .Where(x => x.UnitsInStock < 5 || x.Discontinued)
                         // Call 'OrderByScore'
                        .OrderByScore()
                        .ToListAsync();
                    
                    // Results will be sorted by the score value
                    // with best matching documents (higher score values) listed first.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_6
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Apply filtering
                        .WhereLessThan(x => x.UnitsInStock, 5)
                        .OrElse()
                        .WhereEquals(x => x.Discontinued, true)
                         // Call 'OrderByScore'
                        .OrderByScore()
                        .ToList();
                    
                    // Results will be sorted by the score value
                    // with best matching documents (higher score values) listed first.
                    #endregion
                }
                
                // Order by random
                // ===============
                
                using (var session = store.OpenAsyncSession())
                {
                    #region sort_7
                    List<Product> products = session
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                         // Call 'Customize' with 'RandomOrdering'
                        .Customize(x => x.RandomOrdering())
                         // An optional seed can be passed, e.g.:
                         // .Customize(x => x.RandomOrdering('someSeed'))
                        .ToList();
                    
                    // Results will be randomly ordered.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_8
                    List<Product> products = await asyncSession
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                         // Call 'Customize' with 'RandomOrdering'
                        .Customize(x => x.RandomOrdering())
                         // An optional seed can be passed, e.g.:
                         // .Customize(x => x.RandomOrdering('someSeed'))
                        .ToListAsync();
                    
                    // Results will be randomly ordered.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_9
                    List<Product> products = session.Advanced  
                        .DocumentQuery<Product>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                         // Call 'RandomOrdering'
                        .RandomOrdering()
                         // An optional seed can be passed, e.g.:
                         // .RandomOrdering('someSeed')
                        .ToList();
                    
                    // Results will be randomly ordered.
                    #endregion
                }
                
                // Order by Count
                // ==============
                
                using (var session = store.OpenAsyncSession())
                {
                    #region sort_10
                    var numberOfProductsPerCategory = session
                        .Query<Product>()
                         // Make an aggregation query
                        .GroupBy(x => x.Category)
                        .Select(x => new
                        {
                            // Group by Category
                            Category = x.Key,
                            // Count the number of product documents per category
                            Count = x.Count()
                        })
                         // Order by the Count value
                        .OrderBy(x => x.Count)
                        .ToList();
                    
                    // Results will contain the number of Product documents per category
                    // ordered by that count in ascending order.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_11
                    var numberOfProductsPerCategory = await asyncSession
                        .Query<Product>()
                         // Make an aggregation query
                        .GroupBy(x => x.Category)
                        .Select(x => new
                        {
                            // Group by Category
                            Category = x.Key,
                            // Count the number of product documents per category
                            Count = x.Count()
                        })
                         // Order by the Count value
                        .OrderBy(x => x.Count)
                        .ToListAsync();
                    
                    // Results will contain the number of Product documents per category
                    // ordered by that count in ascending order.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_12
                    var numberOfProductsPerCategory = session.Advanced
                        .DocumentQuery<Product>()
                         // Group by Category
                        .GroupBy("Category")
                        .SelectKey("Category")
                         // Count the number of product documents per category
                        .SelectCount()
                         // Order by the Count value
                         // Here you need to specify the ordering type explicitly 
                        .OrderBy("Count", OrderingType.Long)
                        .ToList();
                    
                    // Results will contain the number of Product documents per category
                    // ordered by that count in ascending order.
                    #endregion
                }
                
                // Order by Sum
                // ============
                
                using (var session = store.OpenAsyncSession())
                {
                    #region sort_13
                    var numberOfUnitsInStockPerCategory = session
                        .Query<Product>()
                         // Make an aggregation query
                        .GroupBy(x => x.Category)
                        .Select(x => new
                        {
                            // Group by Category
                            Category = x.Key,
                            // Sum the number of units in stock per category
                            Sum = x.Sum(x => x.UnitsInStock)
                        })
                         // Order by the Sum value
                        .OrderBy(x => x.Sum)
                        .ToList();
                    
                    // Results will contain the total number of units in stock per category
                    // ordered by that number in ascending order.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_14
                    var numberOfUnitsInStockPerCategory = await asyncSession
                        .Query<Product>()
                         // Make an aggregation query
                        .GroupBy(x => x.Category)
                        .Select(x => new
                        {
                            // Group by Category
                            Category = x.Key,
                            // Sum the number of units in stock per category
                            Sum = x.Sum(x => x.UnitsInStock)
                        })
                         // Order by the Sum value
                        .OrderBy(x => x.Sum)
                        .ToListAsync();
                    
                    // Results will contain the total number of units in stock per category
                    // ordered by that number in ascending order.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_15
                    var numberOfUnitsInStockPerCategory = session.Advanced
                        .DocumentQuery<Product>()
                         // Group by Category
                        .GroupBy("Category")
                        .SelectKey("Category")
                         // Sum the number of units in stock per category
                        .SelectSum(new GroupByField
                        {
                            FieldName = "UnitsInStock",
                            ProjectedName = "Sum"
                        })
                         // Order by the Sum value
                         // Here you need to specify the ordering type explicitly 
                        .OrderBy("Sum", OrderingType.Long)
                        .ToList();
                    
                    // Results will contain the total number of units in stock per category
                    // ordered by that number in ascending order.
                    #endregion
                }
                
                // Force ordering type
                // ===================
                
                using (var session = store.OpenAsyncSession())
                {
                    #region sort_16
                    List<Product> products = session
                        .Query<Product>()
                         // Call 'OrderBy', order by field 'QuantityPerUnit'
                         // Pass a second param, requesting to order the text alphanumerically
                        .OrderBy(x => x.QuantityPerUnit, OrderingType.AlphaNumeric)
                        .ToList();
                    #endregion
                    
                    #region sort_16_results
                    // Running the above query on the NorthWind sample data,
                    // would produce the following order for the QuantityPerUnit field:
                    // ================================================================
                    
                    // "1 kg pkg."
                    // "1k pkg."
                    // "2 kg box."
                    // "4 - 450 g glasses"
                    // "5 kg pkg."
                    // ...
                    
                    // While running with the default Lexicographical ordering would have produced:
                    // ============================================================================
                    
                    // "1 kg pkg."
                    // "10 - 200 g glasses"
                    // "10 - 4 oz boxes"
                    // "10 - 500 g pkgs."
                    // "10 - 500 g pkgs."
                    // ...
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_17
                    List<Product> products = await asyncSession
                        .Query<Product>()
                         // Call 'OrderBy', order by field 'QuantityPerUnit'
                         // Pass a second param, requesting to order the text alphanumerically
                        .OrderBy(x => x.QuantityPerUnit, OrderingType.AlphaNumeric)
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_18
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                         // Call 'OrderBy', order by field 'QuantityPerUnit'
                         // Pass a second param, requesting to order the text alphanumerically
                        .OrderBy(x => x.QuantityPerUnit, OrderingType.AlphaNumeric)
                        .ToList();
                    #endregion
                }
                
                // Chain ordering
                // ==============
                
                using (var session = store.OpenAsyncSession())
                {
                    #region sort_19
                    List<Product> products = session
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                         // Apply the primary sort by 'UnitsInStock'
                        .OrderByDescending(x => x.UnitsInStock)
                         // Apply a secondary sort by the score (for products with the same # of units in stock)
                        .ThenByScore()
                         // Apply another sort by 'Name' (for products with same # of units in stock and same score)
                        .ThenBy(x => x.Name)
                        .ToList();
                    
                    // Results will be sorted by the 'UnitsInStock' value (descending),
                    // then by score,
                    // and then by 'Name' (ascending).
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_20
                    List<Product> products = await asyncSession
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                         // Apply the primary sort by 'UnitsInStock'
                        .OrderByDescending(x => x.UnitsInStock)
                         // Apply a secondary sort by the score (for products with the same # of units in stock)
                        .ThenByScore()
                         // Apply another sort by 'Name' (for products with same # of units in stock and same score)
                        .ThenBy(x => x.Name)
                        .ToListAsync();
                    
                    // Results will be sorted by the 'UnitsInStock' value (descending),
                    // then by score,
                    // and then by 'Name' (ascending).
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_21
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                         // Apply the primary sort by 'UnitsInStock'
                        .OrderByDescending(x => x.UnitsInStock)
                         // Apply a secondary sort by the score
                        .OrderByScore()
                         // Apply another sort by 'Name'
                        .OrderBy(x => x.Name)
                        .ToList();
                    
                    // Results will be sorted by the 'UnitsInStock' value (descending),
                    // then by score,
                    // and then by 'Name' (ascending).
                    #endregion
                }
                
                // Custom sorters
                // ==============
                
                using (var session = store.OpenAsyncSession())
                {
                    #region sort_22
                    List<Product> products = session
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                         // Order by field 'UnitsInStock', pass the name of your custom sorter class
                        .OrderBy(x => x.UnitsInStock, "MySorter")
                        .ToList();
                    
                    // Results will be sorted by the 'UnitsInStock' value
                    // according to the logic from 'MySorter' class
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region sort_23
                    List<Product> products = await asyncSession
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                         // Order by field 'UnitsInStock', pass the name of your custom sorter class
                        .OrderBy(x => x.UnitsInStock, "MySorter")
                        .ToListAsync();
                    
                    // Results will be sorted by the 'UnitsInStock' value
                    // according to the logic from 'MySorter' class
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region sort_24
                    List<Product> products = session.Advanced
                        .DocumentQuery<Product>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                         // Order by field 'UnitsInStock', pass the name of your custom sorter class
                        .OrderBy(x => x.UnitsInStock, "MySorter")
                        .ToList();
                    
                    // Results will be sorted by the 'UnitsInStock' value
                    // according to the logic from 'MySorter' class
                    #endregion
                }
            }
        }
        
        private interface IFoo<TResult>
        {
            #region syntax
            // OrderBy overloads:
            IOrderedQueryable<T> OrderBy<T>(string path, OrderingType ordering);
            IOrderedQueryable<T> OrderBy<T>(Expression<Func<T, object>> path, OrderingType ordering);
            IOrderedQueryable<T> OrderBy<T>(string path, string sorterName);
            IOrderedQueryable<T> OrderBy<T>(Expression<Func<T, object>> path, string sorterName);

            // OrderByDescending overloads:
            IOrderedQueryable<T> OrderByDescending<T>(string path, OrderingType ordering);
            IOrderedQueryable<T> OrderByDescending<T>(Expression<Func<T, object>> path, OrderingType ordering);
            IOrderedQueryable<T> OrderByDescending<T>(string path, string sorterName);
            IOrderedQueryable<T> OrderByDescending<T>(Expression<Func<T, object>> path, string sorterName);
            #endregion
        }
    }
}
