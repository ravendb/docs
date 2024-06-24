using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class JavaScript
    {
        /*
        #region javaScriptindexes_1
        public class Employees_ByFirstAndLastName : AbstractJavaScriptIndexCreationTask
        {
            // ...
        }
        #endregion
        */

        /*
        #region javaScriptindexes_2
        public Employees_ByFirstAndLastName()
        {
            Maps = new HashSet<string>
            {
                @"map('Employees', function (employee){ 
                        return { 
                            FirstName : employee.FirstName, 
                            LastName : employee.LastName
                        };
                    })",
            };
        }
        #endregion
        */

        #region javaScriptindexes_6
        public class Employees_ByFirstAndLastName : AbstractJavaScriptIndexCreationTask
        {
            public Employees_ByFirstAndLastName()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                            return { 
                                FirstName : employee.FirstName, 
                                LastName : employee.LastName
                            };
                        })",
                };
            }
        }
        #endregion

        #region javaScriptindexes_7
        public class Employees_ByFullName : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string FullName { get; set; }
            }

            public Employees_ByFullName()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                            return { 
                                FullName  : employee.FirstName + ' ' + employee.LastName
                            };
                        })",
                };
            }
        }
        #endregion

        #region javaScriptindexes_1_0
        public class Employees_ByYearOfBirth : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public int YearOfBirth { get; set; }
            }

            public Employees_ByYearOfBirth()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                                return {
                                    Birthday : employee.Birthday.Year 
                                } 
                           })"
                };
            }
        }
        #endregion

        #region javaScriptindexes_1_2
        public class Employees_ByBirthday : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public DateTime Birthday { get; set; }
            }

            public Employees_ByBirthday()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                                return {
                                    Birthday : employee.Birthday 
                                } 
                           })"
                };
            }
        }
        #endregion

        #region javaScriptindexes_1_4
        public class Employees_ByCountry : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string Country { get; set; }
            }

            public Employees_ByCountry()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                                return {
                                    Country : employee.Address.Country 
                                 } 
                           })"
                };
            }
        }
        #endregion

        #region javaScriptindexes_1_6
        public class Employees_Query : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string[] Query { get; set; }
            }

            public Employees_Query()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee) { 
                            return { 
                                Query : [employee.FirstName, 
                                         employee.LastName,
                                         employee.Title,
                                         employee.Address.City] 
                                    } 
                            })"
                };
                Fields = new Dictionary<string, IndexFieldOptions>()
                {
                    {"Query", new IndexFieldOptions(){ Indexing = FieldIndexing.Search} }
                };
            }
        }
        #endregion

        #region multi_map_5
        public class Animals_ByName : AbstractJavaScriptIndexCreationTask
        {
            public Animals_ByName()
            {
                Maps = new HashSet<string>()
                {
                    @"map('cats', function (c){ return {Name: c.Name}})",
                    @"map('dogs', function (d){ return {Name: d.Name}})"
                };
            }
        }
        #endregion

        #region map_reduce_0_0
        public class Products_ByCategory : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string Category { get; set; }

                public int Count { get; set; }
            }

            public Products_ByCategory()
            {
                Maps = new HashSet<string>()
                {
                    @"map('products', function(p){
                        return {
                            Category: load(p.Category, 'Categories').Name,
                            Count: 1
                        }
                    })"
                };

                Reduce = @"groupBy(x => x.Category)
                            .aggregate(g => {
                                return {
                                    Category: g.key,
                                    Count: g.values.reduce((count, val) => val.Count + count, 0)
                                };
                            })";
            }
        }
        #endregion

        #region map_reduce_1_0
        public class Products_Average_ByCategory :
                                AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string Category { get; set; }

                public decimal PriceSum { get; set; }

                public double PriceAverage { get; set; }

                public int ProductCount { get; set; }
            }

            public Products_Average_ByCategory()
            {
                Maps = new HashSet<string>()
                {
                    @"map('products', function(product){
                        return {
                            Category: load(product.Category, 'Categories').Name,
                            PriceSum: product.PricePerUnit,
                            PriceAverage: 0,
                            ProductCount: 1
                        }
                    })"
                };

                Reduce = @"groupBy(x => x.Category)
                            .aggregate(g => {
                                var pricesum = g.values.reduce((sum,x) => x.PriceSum + sum,0);
                                var productcount = g.values.reduce((sum,x) => x.ProductCount + sum,0);
                                return {
                                    Category: g.key,
                                    PriceSum: pricesum,
                                    ProductCount: productcount,
                                    PriceAverage: pricesum / productcount
                                }
                            })";
            }
        }
        #endregion

        #region map_reduce_2_0
        public class Product_Sales : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string Product { get; set; }

                public int Count { get; set; }

                public decimal Total { get; set; }
            }

            public Product_Sales()
            {
                Maps = new HashSet<string>()
                {
                    @"map('orders', function(order){
                            var res = [];
                            order.Lines.forEach(l => {
                                res.push({
                                    Product: l.Product,
                                    Count: 1,
                                    Total:  (l.Quantity * l.PricePerUnit) * (1- l.Discount)
                                })
                            });
                            return res;
                        })"
                };

                Reduce = @"groupBy(x => x.Product)
                    .aggregate(g => {
                        return {
                            Product : g.key,
                            Count: g.values.reduce((sum, x) => x.Count + sum, 0),
                            Total: g.values.reduce((sum, x) => x.Total + sum, 0)
                        }
                    })";
            }
        }
        #endregion

        #region map_reduce_3_0
        public class Product_Sales_ByDate : AbstractIndexCreationTask
        {
            public override IndexDefinition CreateIndexDefinition()
            {
                return new IndexDefinition
                {
                    Maps =
                    {
                        @"from order in docs.Orders
                          from line in order.Lines
                          select new {
                              line.Product, 
                              Date = order.OrderedAt,
                              Profit = line.Quantity * line.PricePerUnit * (1 - line.Discount)
                          };"
                    },
                    Reduce = 
                        @"from r in results
                          group r by new { r.OrderedAt, r.Product }
                          into g
                          select new { 
                              Product = g.Key.Product,
                              Date = g.Key.Date,
                              Profit = g.Sum(r => r.Profit)
                          };",

                    OutputReduceToCollection = "DailyProductSales",
                    PatternReferencesCollectionName = "DailyProductSales/References",
                    PatternForOutputReduceToCollectionReferences = "sales/daily/{Date:yyyy-MM-dd}"
                };
            }
        }
        #endregion

        #region fanout_index_def_1
        public class Orders_ByProduct : AbstractJavaScriptIndexCreationTask
        {
            public Orders_ByProduct()
            {
                Maps = new HashSet<string>
                {
                    @"map('Orders', function (order){ 
                           var res = [];
                            order.Lines.forEach(l => {
                                res.push({
                                    Product: l.Product,
                                    ProductName: l.ProductName
                                })
                            });
                            return res;
                        })",
                };
            }
        }
        #endregion

        #region static_sorting2
        private class Products_ByName : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByName()
            {
                Maps = new HashSet<string>
                {
                    @"map('products', function (u){
                                    return {
                                        Name: u.Name,
                                        _: {$value: u.Name, $name:'AnalyzedName'}
                                    };
                                })",
                };
                Fields = new Dictionary<string, IndexFieldOptions>
                {
                    {
                        "AnalyzedName", new IndexFieldOptions()
                        {
                            Indexing = FieldIndexing.Search,
                            Analyzer = "StandardAnalyzer"
                        }
                    }
                };
            }
            public class Result
            {
                public string AnalyzedName { get; set; }
            }
        }
        #endregion

        #region indexing_related_documents_2
        public class Products_ByCategoryName : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string CategoryName { get; set; }
            }

            public Products_ByCategoryName()
            {
                Maps = new HashSet<string>()
                {
                    @"map('products', function(product ){
                        return {
                            CategoryName : load(product .Category, 'Categories').Name,
                        }
                    })"
                };
            }
        }
        #endregion

        #region indexing_related_documents_5
        public class Authors_ByNameAndBookNames : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string Name { get; set; }

                public IList<string> Books { get; set; }
            }

            public Authors_ByNameAndBookNames()
            {
                Maps = new HashSet<string>()
                {
                    @"map('Author', function(a){
                        return {
                            Name: a.Name,
                            Books: a.BooksIds.forEach(x => load(x, 'Book').Name)
                        }
                    })"
                };
            }
        }
        #endregion

        #region indexes_2
        public class BlogPosts_ByCommentAuthor : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string[] Authors { get; set; }

            }

            public BlogPosts_ByCommentAuthor()
            {
                Maps = new HashSet<string>()
                {
                    @"map('BlogPosts', function(b){
                        var names = [];
                        b.Comments.forEach(x => getNames(x, names));
                        return {
                            Authors : names
                        };})"
                };
                AdditionalSources = new Dictionary<string, string>
                {
                    ["The Script"] = @"function getNames(x, names){
                                        names.push(x.Author);
                                        x.Comments.forEach(x => getNames(x, names));
                                 }"
                };
            }
        }
        #endregion

        #region spatial_search_1
        private class Events_ByNameAndCoordinates : AbstractJavaScriptIndexCreationTask
        {
            public Events_ByNameAndCoordinates()
            {
                Maps = new HashSet<string>
                {
                    @"map('events', function (e){
                        return { 
                            Name: e.Name  ,
                            Coordinates: createSpatialField(e.Latitude, e.Longitude)
                        };                            
                    })"
                };

            }
        }
        #endregion

        #region indexes_3
        public class BlogPosts_ByCommentAuthor_JS : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string[] Authors { get; set; }
            }

            public BlogPosts_ByCommentAuthor_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('BlogPosts', function (blogpost) {
                        return recurse(blogpost, x => x.Comments).map(function (comment) {
                            if (comment.Author != null) {
                                return {
                                    Authors: comment.Author
                                };
                            }
                        });
                    });"
                };
            }
        }
        #endregion
    }
}
