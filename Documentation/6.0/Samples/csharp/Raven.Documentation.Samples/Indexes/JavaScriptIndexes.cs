using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    #region index_1
    public class Employees_ByFirstAndLastName_JS : AbstractJavaScriptIndexCreationTask
    {
        public class IndexEntry
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
            
        public Employees_ByFirstAndLastName_JS()
        {
            Maps = new HashSet<string>
            {
                // Define the 'map' function:
                // Index content from documents in the 'Employees' collection
                @"map('Employees', function (employee) {
 
                      // Provide your JavaScript code here
                      // Return an object that defines the index-entry:
                      // ==============================================
                           
                      return {
                          // Define the index-fields:
                          // ========================

                          FirstName: employee.FirstName, 
                          LastName: employee.LastName
                      };
                  })",
            };
        }
    }
    #endregion
    
    #region index_2
    public class BlogPosts_ByCommentAuthor_JS : AbstractJavaScriptIndexCreationTask
    {
        public class IndexEntry
        {
            public string[] Authors { get; set; }
        }

        public BlogPosts_ByCommentAuthor_JS()
        {
            Maps = new HashSet<string>()
            {
                @"map('BlogPosts', function(post) {
                      const names = [];

                      // Get names of authors from the additional source code:
                      if (post.Comments) {
                          post.Comments.forEach(x => getNames(x, names));
                      }

                      return {
                          Authors: names
                      };
                  })"
            };
                
            AdditionalSources = new Dictionary<string, string>
            {
                ["The getNames method"] = @"
                    function getNames(comment, names) {
                        names.push(comment.Author);

                        if (comment.Comments) {
                            comment.Comments.forEach(x => getNames(x, names));
                        }
                    }"
            };
        }
    }
    #endregion
    
    #region index_3
    public class Products_ByStock1_JS : AbstractJavaScriptIndexCreationTask
    {
        public class IndexEntry
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
            
        public Products_ByStock1_JS()
        {
            Maps = new HashSet<string>
            {
                @"map('Products', function(product) {
                      // Define a string expression to check for low stock.
                      const functionBody = 'return product.UnitsInStock < 10';

                      // Create a dynamic function that evaluates the expression at runtime.
                      const dynamicFunc = new Function(""product"", functionBody);

                      return {
                          StockIsLow: dynamicFunc(product)
                      };
                  });",
            };

            // Enable string‑compilation so this index can execute the inline script
            Configuration["Indexing.AllowStringCompilation"] = "true";
        }
    }
    #endregion
    
    #region index_4
    public class Products_ByStock2_JS : AbstractJavaScriptIndexCreationTask
    {
        public class IndexEntry
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
            
        public Products_ByStock2_JS()
        {
            Maps = new HashSet<string>
            {
                @"map('Products', function(product) {
                      // Define a string expression with your condition
                      const expression = 'product.UnitsInStock < 10';

                      // Evaluate the string expression at runtime using eval.
                      const isLowOnStock = eval(expression);

                      return {
                          StockIsLow: isLowOnStock
                      };
                  });",
            };
            
            // Enable string‑compilation so this index can execute the inline script
            Configuration["Indexing.AllowStringCompilation"] = "true";
        }
    }
    #endregion
    
    #region index_5
    public class Animals_ByName_JS : AbstractJavaScriptIndexCreationTask
    {
        public class IndexEntry
        {
            public string Name { get; set; }
        }
            
        public Animals_ByName_JS()
        {
            Maps = new HashSet<string>()
            {
                // Define a map function on the 'Cats' collection
                @"map('Cats', function(c) { return { Name: c.Name }})",
                    
                // Define a map function on the 'Dogs' collection
                @"map('Dogs', function(d) { return { Name: d.Name }})"
            };
        }
    }
    #endregion
    
    #region index_6
    public class Products_ByCategory_JS : AbstractJavaScriptIndexCreationTask
    {
        public class IndexEntry
        {
            public string Category { get; set; }
            public int Count { get; set; }
        }

        public Products_ByCategory_JS()
        {
            // The Map stage:
            // For each product document -
            // * load its related Category document using the 'load' function,
            // * extract the category name, and return a count of 1.
            Maps = new HashSet<string>()
            {
                @"map('Products', function(p) {
                      return {
                          Category: load(p.Category, 'Categories').Name,
                          Count: 1
                      }
                  })"
            };

            // The Reduce stage:
            // * group the mapped results by Category
            // * and count the number of products in each category.
            Reduce = @"groupBy(x => x.Category).aggregate(g => {
                           return {
                               Category: g.key,
                               Count: g.values.reduce((count, val) => val.Count + count, 0)
                           };
                      })";
        }
    }
    #endregion
    
    #region index_7
    public class ProductSales_ByMonth_JS : AbstractJavaScriptIndexCreationTask
    {
        public class IndexEntry
        {
            public string Product { get; set; }
            public DateTime Month { get; set; }
            public int Count { get; set; }
            public decimal Total { get; set; }
        }
        
        public ProductSales_ByMonth_JS()
        {
            // The Map stage:
            // For each order, emit one entry per line with:
            // * the product,
            // * the first day of the order’s month,
            // * a count of 1,
            // * and the line’s total value.
            Maps = new HashSet<string>()
            {
                @"map('orders', function(order) {
                      var res = [];
                      var orderDate = new Date(order.OrderedAt);

                      order.Lines.forEach(l => {
                          res.push({
                              Product: l.Product,
                              Month: new Date(orderDate.getFullYear(), orderDate.getMonth(), 1),
                              Count: 1,
                              Total: (l.Quantity * l.PricePerUnit) * (1- l.Discount)
                          })
                      });

                      return res;
                })"
            };
            
            // The Reduce stage:
            // Group by product and month, then sum up counts and totals.
            Reduce = @"
                groupBy(x => ({Product: x.Product, Month: x.Month}))
                    .aggregate(g => {
                         return {
                             Product: g.key.Product,
                             Month: g.key.Month,
                             Count: g.values.reduce((sum, x) => x.Count + sum, 0),
                             Total: g.values.reduce((sum, x) => x.Total + sum, 0)
                         }
                    })";
            
            // Output the reduce results into a dedicated collection
            OutputReduceToCollection = "MonthlyProductSales";
            PatternReferencesCollectionName = "MonthlyProductSales/References";
            PatternForOutputReduceToCollectionReferences = "sales/monthly/{Month}";
        }
    }
    #endregion

    public class QueryExamples
    {
        public async void Queries()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1
                    List<Employee> employees = session
                         // Query the map index
                        .Query<Employees_ByFirstAndLastName_JS.IndexEntry,
                            Employees_ByFirstAndLastName_JS>()
                        .Where(x => x.LastName == "King")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                    
                    #region query_2
                    var animalsNamedMilo = session
                         // Query the multi-map index
                        .Query<Animals_ByName_JS.IndexEntry, Animals_ByName_JS>()
                        .Where(x => x.Name == "Milo")
                        .ToList();
                    #endregion
                    
                    #region query_3
                    var topCategories = session
                         // Query the map-reduce index
                        .Query<Products_ByCategory_JS.IndexEntry, Products_ByCategory_JS>()
                        .OrderByDescending(x => x.Count)
                        .ToList();
                    #endregion
                }
            }
        }
    }
    
    public class JavaScriptIndexes
    {
        /*
        #region js_index
        public class Documents_ByName_JS : AbstractJavaScriptIndexCreationTask
        {
             Maps = new HashSet<string>()
            {
                // Define a map function:
                @"map(<CollectionName>, function(doc) { 
                      return {
                          Name: doc.Name
                          // ...
                      }
                  })",
                
                // ...
            };
        }
        #endregion
        */

        #region blog_post_class
        public class BlogPost
        {
            public string Author { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public List<BlogPostComment> Comments { get; set; }
        }

        public class BlogPostComment
        {
            public string Author { get; set; }
            public string Text { get; set; }
            public List<BlogPostComment> Comments { get; set; }
        }
        #endregion
    }
}
