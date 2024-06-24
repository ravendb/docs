using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.Indexes
{
    using System.Linq;

    #region online_shop_class
    public class OnlineShop
    {
        public string ShopName { get; set; }
        public string Email { get; set; }
        public List<TShirt> TShirts { get; set; } // Nested data
    }

    public class TShirt
    {
        public string Color { get; set; }
        public string Size { get; set; }
        public string Logo { get; set; }
        public decimal Price { get; set; }
        public int Sold { get; set; }
    }
    #endregion
    
    #region simple_index
    public class Shops_ByTShirt_Simple : AbstractIndexCreationTask<OnlineShop>
    {
        public class IndexEntry
        {
            // The index-fields:
            public IEnumerable<string> Colors { get; set; }
            public IEnumerable<string> Sizes { get; set; }
            public IEnumerable<string> Logos { get; set; }
        }
        
        public Shops_ByTShirt_Simple()
        {
            Map = shops => from shop in shops
                // Creating a SINGLE index-entry per document:
                select new IndexEntry
                {
                    // Each index-field will hold a collection of nested values from the document
                    Colors = shop.TShirts.Select(x => x.Color),
                    Sizes = shop.TShirts.Select(x => x.Size),
                    Logos = shop.TShirts.Select(x => x.Logo)
                };
        }
    }
    #endregion
    
    #region fanout_index_1
    // A fanout map-index:
    // ===================
    public class Shops_ByTShirt_Fanout : AbstractIndexCreationTask<OnlineShop>
    {
        public class IndexEntry
        {
            // The index-fields:
            public string Color { get; set; }
            public string Size { get; set; }
            public string Logo { get; set; }
        }
        
        public Shops_ByTShirt_Fanout()
        {
            Map = shops =>
                from shop in shops
                from shirt in shop.TShirts
                // Creating MULTIPLE index-entries per document,
                // an index-entry for each sub-object in the TShirts list
                select new IndexEntry
                {
                    Color = shirt.Color,
                    Size = shirt.Size,
                    Logo = shirt.Logo
                };
        }
    }
    #endregion

    #region fanout_index_2
    // A fanout map-reduce index:
    // ==========================
    public class Sales_ByTShirtColor_Fanout : 
        AbstractIndexCreationTask<OnlineShop, Sales_ByTShirtColor_Fanout.IndexEntry>
    {
        public class IndexEntry
        {
            // The index-fields:
            public string Color { get; set; }
            public int ItemsSold { get; set; }
            public decimal TotalSales { get; set; }
        }

        public Sales_ByTShirtColor_Fanout()
        {
            Map = shops => 
                from shop in shops
                from shirt in shop.TShirts
                // Creating MULTIPLE index-entries per document,
                // an index-entry for each sub-object in the TShirts list
                select new IndexEntry
                {
                    Color = shirt.Color,
                    ItemsSold = shirt.Sold,
                    TotalSales = shirt.Price * shirt.Sold
                };

            Reduce = results => from result in results
                group result by result.Color
                into g
                select new
                {
                    // Calculate sales per color
                    Color = g.Key,
                    ItemsSold = g.Sum(x => x.ItemsSold),
                    TotalSales = g.Sum(x => x.TotalSales)
                };
        }
    }
    #endregion
        
    #region fanout_index_js
    public class Shops_ByTShirt_JS : AbstractJavaScriptIndexCreationTask
    {
        public Shops_ByTShirt_JS()
        {
            Maps = new HashSet<string>
            {
                @"map('OnlineShops', function (shop){ 
                       var res = [];
                       shop.TShirts.forEach(shirt => {
                           res.push({
                               Color: shirt.Color,
                               Size: shirt.Size,
                               Logo: shirt.Logo
                           })
                        });
                        return res;
                    })"
            };
        }
    }
    #endregion
    
    public class IndexingNestedData
    {
        public void CreateSampleData()
        {
            using (var store = new DocumentStore())
            {
                #region sample_data
                // Creating sample data for the examples in this article:
                // ======================================================

                var onlineShops = new[]
                {
                  // Shop1
                  new OnlineShop { ShopName = "Shop1", Email = "sales@shop1.com", TShirts = new List<TShirt> {
                      new TShirt { Color = "Red", Size = "S", Logo = "Bytes and Beyond", Price = 25, Sold = 2 },
                      new TShirt { Color = "Red", Size = "M", Logo = "Bytes and Beyond", Price = 25, Sold = 4 },
                      new TShirt { Color = "Blue", Size = "M", Logo = "Query Everything", Price = 28, Sold = 5 },
                      new TShirt { Color = "Green", Size = "L", Logo = "Data Driver", Price = 30, Sold = 3}
                  }},
                  // Shop2
                  new OnlineShop { ShopName = "Shop2", Email = "sales@shop2.com", TShirts = new List<TShirt> {
                      new TShirt { Color = "Blue", Size = "S", Logo = "Coffee, Code, Repeat", Price = 22, Sold = 12 },
                      new TShirt { Color = "Blue", Size = "M", Logo = "Coffee, Code, Repeat", Price = 22, Sold = 7 },
                      new TShirt { Color = "Green", Size = "M", Logo = "Big Data Dreamer", Price = 25, Sold = 9 },
                      new TShirt { Color = "Black", Size = "L", Logo = "Data Mining Expert", Price = 20, Sold = 11 }
                  }},
                  // Shop3
                  new OnlineShop { ShopName = "Shop3", Email = "sales@shop3.com", TShirts = new List<TShirt> {
                      new TShirt { Color = "Red", Size = "S", Logo = "Bytes of Wisdom", Price = 18, Sold = 2 },
                      new TShirt { Color = "Blue", Size = "M", Logo = "Data Geek", Price = 20, Sold = 6 },
                      new TShirt { Color = "Black", Size = "L", Logo = "Data Revolution", Price = 15, Sold = 8 },
                      new TShirt { Color = "Black", Size = "XL", Logo = "Data Revolution", Price = 15, Sold = 10 }
                  }}
                };

                using (var session = store.OpenSession())
                {
                    foreach (var shop in onlineShops)
                    {
                        session.Store(shop);
                    }

                    session.SaveChanges();
                }
                #endregion
            }
        }
        
        public async Task QueryNestedData()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region simple_index_query_1
                    // Query for all shop documents that have a red TShirt
                    var shopsThatHaveRedShirts = session
                        .Query<Shops_ByTShirt_Simple.IndexEntry, Shops_ByTShirt_Simple>()
                         // Filter query results by a nested value
                        .Where(x => x.Colors.Contains("red"))
                        .OfType<OnlineShop>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region simple_index_query_2
                    // Query for all shop documents that have a red TShirt
                    var shopsThatHaveRedShirts = await asyncSession
                        .Query<Shops_ByTShirt_Simple.IndexEntry, Shops_ByTShirt_Simple>()
                         // Filter query results by a nested value
                        .Where(x => x.Colors.Contains("red"))
                        .OfType<OnlineShop>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region simple_index_query_3
                    // Query for all shop documents that have a red TShirt
                    var shopsThatHaveRedShirts = session.Advanced
                        .DocumentQuery<Shops_ByTShirt_Simple.IndexEntry, Shops_ByTShirt_Simple>()
                         // Filter query results by a nested value
                        .ContainsAny(x => x.Colors, new[] { "Red" })
                        .OfType<OnlineShop>()
                        .ToList();
                    #endregion
                }
                
                #region results_1
                // Results will include the following shop documents:
                // ==================================================
                // * Shop1
                // * Shop3
                #endregion
                
                using (var session = store.OpenSession())
                {
                    #region results_2
                    // You want to query for shops containing "Large Green TShirts",
                    // aiming to get only "Shop1" as a result since it has such a combination,
                    // so you attempt this query:
                    var GreenAndLarge = session
                        .Query<Shops_ByTShirt_Simple.IndexEntry, Shops_ByTShirt_Simple>()
                        .Where(x => x.Colors.Contains("green") && x.Sizes.Contains("L"))
                        .OfType<OnlineShop>()
                        .ToList();
                    
                    // But, the results of this query will include BOTH "Shop1" & "Shop2"
                    // since the index-entries do not keep the original sub-objects structure.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region fanout_index_query_1
                    // Query the fanout index:
                    // =======================
                    var shopsThatHaveMediumRedShirts = session
                        .Query<Shops_ByTShirt_Fanout.IndexEntry, Shops_ByTShirt_Fanout>()
                         // Query for documents that have a "Medium Red TShirt"
                        .Where(x => x.Color == "red" && x.Size == "M")
                        .OfType<OnlineShop>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fanout_index_query_2
                    // Query the fanout index:
                    // =======================
                    var shopsThatHaveMediumRedShirts = await asyncSession
                        .Query<Shops_ByTShirt_Fanout.IndexEntry, Shops_ByTShirt_Fanout>()
                         // Query for documents that have a "Medium Red TShirt"
                        .Where(x => x.Color == "red" && x.Size == "M")
                        .OfType<OnlineShop>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region fanout_index_query_3
                    // Query the fanout index:
                    // =======================
                    var shopsThatHaveMediumRedShirts = session.Advanced
                        .DocumentQuery<Shops_ByTShirt_Fanout.IndexEntry, Shops_ByTShirt_Fanout>()
                         // Query for documents that have a "Medium Red TShirt"
                        .WhereEquals(x => x.Color, "red")
                        .AndAlso()
                        .WhereEquals(x=> x.Size, "M")
                        .OfType<OnlineShop>()
                        .ToList();
                    #endregion
                }
                
                #region results_3
                // Query results:
                // ==============
                
                // Only the 'Shop1' document will be returned,
                // since it is the only document that has the requested combination within the TShirt list.
                #endregion
                
                using (var session = store.OpenSession())
                {
                    #region fanout_index_query_4
                    // Query the fanout index:
                    // =======================
                    var queryResult = session
                        .Query<Sales_ByTShirtColor_Fanout.IndexEntry, Sales_ByTShirtColor_Fanout>()
                         // Query for index-entries that contain "black"
                        .Where(x => x.Color == "black")
                        .FirstOrDefault();

                    // Get total sales for black TShirts
                    var blackShirtsSales = queryResult?.TotalSales ?? 0;
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region fanout_index_query_5
                    // Query the fanout index:
                    // =======================
                    var queryResult = await asyncSession
                        .Query<Sales_ByTShirtColor_Fanout.IndexEntry, Sales_ByTShirtColor_Fanout>()
                         // Query for index-entries that contain "black"
                        .Where(x => x.Color == "black")
                        .FirstOrDefaultAsync();

                    // Get total sales for black TShirts
                    var blackShirtsSales = queryResult?.TotalSales ?? 0;
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region fanout_index_query_6
                    // Query the fanout index:
                    // =======================
                    var queryResult = session.Advanced
                        .DocumentQuery<Sales_ByTShirtColor_Fanout.IndexEntry, Sales_ByTShirtColor_Fanout>()
                        // Query for index-entries that contain "black"
                        .WhereEquals(x => x.Color, "black")
                        .FirstOrDefault();

                    // Get total sales for black TShirts
                    var blackShirtsSales = queryResult?.TotalSales ?? 0;
                    #endregion
                }
                
                #region results_4
                // Query results:
                // ==============
                
                // With the sample data used in this article,
                // The total sales revenue from black TShirts sold (in all shops) is 490.0
                #endregion
            }
        }
    }
}
