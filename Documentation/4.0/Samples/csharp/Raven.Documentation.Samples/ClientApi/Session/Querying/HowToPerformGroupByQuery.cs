using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents.Queries;

#region group_by_using

using Raven.Client.Documents;

#endregion

using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToPerformGroupByQuery
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region group_by_1

                    var results = (from o in session.Query<Order>()
                            group o by o.ShipTo.Country
                            into g
                            select new
                            {
                                Country = g.Key,
                                OrderedQuantity = g.Sum(order => order.Lines.Sum(line => line.Quantity))
                            })
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_1_async

                    var results = await (from o in asyncSession.Query<Order>()
                            group o by o.ShipTo.Country
                            into g
                            select new
                            {
                                Country = g.Key,
                                OrderedQuantity = g.Sum(order => order.Lines.Sum(line => line.Quantity))
                            })
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region group_by_2

                    var results = session.Query<Order>()
                        .GroupBy(x => new
                        {
                            x.Employee,
                            x.Company
                        })
                        .Select(x => new
                        {
                            EmployeeIdentifier = x.Key.Employee,
                            x.Key.Company,
                            Count = x.Count()
                        })
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_2_async

                    var results = await asyncSession.Query<Order>()
                        .GroupBy(x => new
                        {
                            x.Employee,
                            x.Company
                        })
                        .Select(x => new
                        {
                            EmployeeIdentifier = x.Key.Employee,
                            x.Key.Company,
                            Count = x.Count()
                        })
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region group_by_3

                    var results = session.Query<Order>()
                        .GroupBy(x => new EmployeeAndCompany
                        {
                            Employee = x.Employee,
                            Company = x.Company
                        })
                        .Select(x => new CountOfEmployeeAndCompanyPairs
                        {
                            EmployeeCompanyPair = x.Key,
                            Count = x.Count()
                        })
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_3_async

                    var results = await asyncSession.Query<Order>()
                        .GroupBy(x => new EmployeeAndCompany
                        {
                            Employee = x.Employee,
                            Company = x.Company
                        })
                        .Select(x => new CountOfEmployeeAndCompanyPairs
                        {
                            EmployeeCompanyPair = x.Key,
                            Count = x.Count()
                        })
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region group_by_4

                    var results = session.Query<Order>()
                        .GroupByArrayValues(x => x.Lines.Select(y => y.Product))
                        .Select(x => new
                        {
                            Count = x.Count(),
                            Product = x.Key
                        })
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_4_async

                    var results = await asyncSession.Query<Order>()
                        .GroupByArrayValues(x => x.Lines.Select(y => y.Product))
                        .Select(x => new
                        {
                            Count = x.Count(),
                            Product = x.Key
                        })
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region group_by_5

                    var results = session.Advanced.DocumentQuery<Order>()
                        .GroupBy("Lines[].Product", "ShipTo.Country")
                        .SelectKey("Lines[].Product", "Product")
                        .SelectKey("ShipTo.Country", "Country")
                        .SelectCount()
                        .OfType<ProductInfo>()
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_5_async

                    var results = await asyncSession.Advanced.AsyncDocumentQuery<Order>()
                        .GroupBy("Lines[].Product", "ShipTo.Country")
                        .SelectKey("Lines[].Product", "Product")
                        .SelectKey("ShipTo.Country", "Country")
                        .SelectCount()
                        .OfType<ProductInfo>()
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region group_by_6

                    var results = session.Query<Order>()
                        .GroupByArrayValues(x => x.Lines.Select(y => new
                        {
                            y.Product,
                            y.Quantity
                        }))
                        .Select(x => new ProductInfo
                        {
                            Count = x.Count(),
                            Product = x.Key.Product,
                            Quantity = x.Key.Quantity
                        })
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_6_async

                    var results = await asyncSession.Query<Order>()
                        .GroupByArrayValues(x => x.Lines.Select(y => new
                        {
                            y.Product,
                            y.Quantity
                        }))
                        .Select(x => new ProductInfo
                        {
                            Count = x.Count(),
                            Product = x.Key.Product,
                            Quantity = x.Key.Quantity
                        })
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region group_by_7

                    var results = session.Query<Order>()
                        .GroupByArrayContent(x => x.Lines.Select(y => y.Product))
                        .Select(x => new ProductsInfo
                        {
                            Count = x.Count(),
                            Products = x.Key
                        })
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_7_async

                    var results = await asyncSession.Query<Order>()
                        .GroupByArrayContent(x => x.Lines.Select(y => y.Product))
                        .Select(x => new ProductsInfo
                        {
                            Count = x.Count(),
                            Products = x.Key
                        })
                        .ToListAsync();

                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region group_by_8

                    var results = session.Advanced.DocumentQuery<Order>()
                        .GroupBy(("Lines[].Product", GroupByMethod.Array), ("ShipTo.Country", GroupByMethod.None))
                        .SelectKey("Lines[].Product", "Products")
                        .SelectKey("ShipTo.Country", "Country")
                        .SelectCount()
                        .OfType<ProductsInfo>()
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_8_async

                    var results = await asyncSession.Advanced.AsyncDocumentQuery<Order>()
                        .GroupBy(("Lines[].Product", GroupByMethod.Array), ("ShipTo.Country", GroupByMethod.None))
                        .SelectKey("Lines[].Product", "Products")
                        .SelectKey("ShipTo.Country", "Country")
                        .SelectCount()
                        .OfType<ProductsInfo>()
                        .ToListAsync();

                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region group_by_9

                    var results = session.Advanced.DocumentQuery<Order>()
                        .GroupBy(("Lines[].Product", GroupByMethod.Array), ("Lines[].Quantity", GroupByMethod.Array))
                        .SelectKey("Lines[].Product", "Products")
                        .SelectKey("Lines[].Quantity", "Quantities")
                        .SelectCount()
                        .OfType<ProductsInfo>()
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region group_by_9_async

                    var results = await asyncSession.Advanced.AsyncDocumentQuery<Order>()
                        .GroupBy(("Lines[].Product", GroupByMethod.Array), ("Lines[].Quantity", GroupByMethod.Array))
                        .SelectKey("Lines[].Product", "Products")
                        .SelectKey("Lines[].Quantity", "Quantities")
                        .SelectCount()
                        .OfType<ProductsInfo>()
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region order_by_count

                    var results = session.Query<Order>()
                        .GroupBy(x => x.Employee)
                        .Select(x => new
                        {
                            Employee = x.Key,
                            Count = x.Count()
                        })
                        .OrderBy(x => x.Count)
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region order_by_count_async

                    var results = await asyncSession.Query<Order>()
                        .GroupBy(x => x.Employee)
                        .Select(x => new
                        {
                            Employee = x.Key,
                            Count = x.Count()
                        })
                        .OrderBy(x => x.Count)
                        .ToListAsync();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region order_by_sum

                    var results = session.Query<Order>()
                        .GroupBy(x => x.Employee)
                        .Select(x => new
                        {
                            Employee = x.Key,
                            Sum = x.Sum(y => y.Freight)
                        })
                        .OrderBy(x => x.Sum)
                        .ToList();

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region order_by_sum_async

                    var results = await asyncSession.Query<Order>()
                        .GroupBy(x => x.Employee)
                        .Select(x => new
                        {
                            Employee = x.Key,
                            Count = x.Count()
                        })
                        .OrderBy(x => x.Count)
                        .ToListAsync();

                    #endregion
                }
            }
        }

        private class EmployeeAndCompany
        {
            public string Employee { get; set; }

            public string Company { get; set; }
        }

        private class CountOfEmployeeAndCompanyPairs
        {
            public EmployeeAndCompany EmployeeCompanyPair { get; set; }

            public int Count { get; set; }
        }

        private class ProductInfo
        {
            public int Count { get; set; }

            public string Product { get; set; }

            public int Quantity { get; set; }
        }

        private class ProductsInfo
        {
            public int Count { get; set; }

            public IEnumerable<string> Products { get; set; }

            public int Quantity { get; set; }
        }
    }
}
