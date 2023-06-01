using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Paging
    {
        #region paging_0_4
        public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
        {
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

        #region paging_6_0
        public class Orders_ByOrderLines_ProductName : AbstractIndexCreationTask<Order>
        {
            public Orders_ByOrderLines_ProductName()
            {
                Map = orders => from order in orders
                                from line in order.Lines
                                select new
                                {
                                    Product = line.ProductName
                                };
            }
        }
        #endregion

        #region paging_7_0
        public class Orders_ByStoredProductName : AbstractIndexCreationTask<Order>
        {
            public class Result
            {
                public string Product { get; set; }
            }

            public Orders_ByStoredProductName()
            {
                Map = orders => from order in orders
                                from line in order.Lines
                                select new Result
                                {
                                    Product = line.ProductName
                                };

                Store("Product", FieldStorage.Yes);
            }
        }
        #endregion

        public Paging()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region paging_0_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_0_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region paging_2_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .Skip(20)   // skip 2 pages worth of products
                        .Take(10)   // take up to 10 products
                        .ToList();  // execute query
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_2_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .Skip(20)   // skip 2 pages worth of products
                        .Take(10)   // take up to 10 products
                        .ToList();  // execute query
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region paging_3_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Statistics(out QueryStatistics stats)      // fill query statistics
                        .Where(x => x.UnitsInStock > 10)
                        .Skip(20)
                        .Take(10)
                        .ToList();

                    long totalResults = stats.TotalResults;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_3_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .Statistics(out QueryStatistics stats)      // fill query statistics
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .Skip(20)
                        .Take(10)
                        .ToList();

                    long totalResults = stats.TotalResults;
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region paging_4_1
                    IList<Product> results;
                    int pageNumber = 0;
                    int pageSize = 10;
                    long skippedResults = 0;

                    do
                    {
                        results = session
                            .Query<Product, Products_ByUnitsInStock>()
                            .Statistics(out QueryStatistics stats)
                            .Skip((pageNumber * pageSize) + (int)skippedResults)
                            .Take(pageSize)
                            .Where(x => x.UnitsInStock > 10)
                            .Distinct()
                            .ToList();

                        skippedResults += stats.SkippedResults;
                        pageNumber++;
                    }
                    while (results.Count > 0);
                    #endregion
                }


                using (var session = store.OpenSession())
                {
                    #region paging_4_2
                    IList<Product> results;
                    int pageNumber = 0;
                    int pageSize = 10;
                    long skippedResults = 0;

                    do
                    {
                        results = session
                            .Advanced
                            .DocumentQuery<Product, Products_ByUnitsInStock>()
                            .Statistics(out QueryStatistics stats)
                            .Skip((pageNumber * pageSize) + skippedResults)
                            .Take(pageSize)
                            .WhereGreaterThan(x => x.UnitsInStock, 10)
                            .Distinct()
                            .ToList();

                        skippedResults += stats.SkippedResults;
                        pageNumber++;
                    }
                    while (results.Count > 0);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region paging_6_1
                    IList<Order> results;
                    int pageNumber = 0;
                    int pageSize = 10;
                    long skippedResults = 0;

                    do
                    {
                        results = session
                            .Query<Order, Orders_ByOrderLines_ProductName>()
                            .Statistics(out QueryStatistics stats)
                            .Skip((pageNumber * pageSize) + (int)skippedResults)
                            .Take(pageSize)
                            .ToList();

                        skippedResults += stats.SkippedResults;
                        pageNumber++;
                    }
                    while (results.Count > 0);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_6_2
                    IList<Order> results;
                    long pageNumber = 0;
                    long pageSize = 10;
                    long skippedResults = 0;

                    do
                    {
                        results = session
                            .Advanced
                            .DocumentQuery<Order, Orders_ByOrderLines_ProductName>()
                            .Statistics(out QueryStatistics stats)
                            .Skip((pageNumber * pageSize) + skippedResults)
                            .Take(pageSize)
                            .ToList();

                        skippedResults += stats.SkippedResults;
                        pageNumber++;
                    }
                    while (results.Count > 0);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region paging_7_1
                    IList<string> results;
                    int pageNumber = 0;
                    const int pageSize = 10;

                    do
                    {
                        results = session
                            .Query<Orders_ByStoredProductName.Result, Orders_ByStoredProductName>()
                            .Select(x => x.Product)
                            .Skip((pageNumber * pageSize))
                            .Take(pageSize)
                            .Distinct()
                            .ToList();

                        pageNumber++;
                    }
                    while (results.Count > 0);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region paging_7_2
                    IList<Orders_ByStoredProductName.Result> results;
                    int pageNumber = 0;
                    const int pageSize = 10;

                    do
                    {
                        results = session
                            .Advanced
                            .DocumentQuery<Order, Orders_ByStoredProductName>()
                            .SelectFields<Orders_ByStoredProductName.Result>("Product")
                            .Skip((pageNumber * pageSize))
                            .Take(pageSize)
                            .Distinct()
                            .ToList();

                        pageNumber++;
                    }
                    while (results.Count > 0);
                    #endregion
                }
            }
        }
    }
}
