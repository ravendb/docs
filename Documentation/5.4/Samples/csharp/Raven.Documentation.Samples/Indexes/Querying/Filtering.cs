using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Filtering
    {
        #region filtering_0_4
        public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
        {
            public Employees_ByFirstAndLastName()
            {
                Map = employees => from employee in employees
                                   select new
                                   {
                                       FirstName = employee.FirstName,
                                       LastName = employee.LastName
                                   };
            }
        }
        #endregion

        #region filtering_1_4
        private class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
        {
            public Products_ByUnitsInStock()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      product.UnitsInStock
                                  };
            }
        }
        #endregion

        #region filtering_7_4
        public class Orders_ByTotalPrice : AbstractIndexCreationTask<Order>
        {
            public class Result
            {
                public decimal TotalPrice;
            }

            public Orders_ByTotalPrice()
            {
                Map = orders => from order in orders
                                select new Result
                                {
                                    TotalPrice = order.Lines.Sum(x => (x.Quantity * x.PricePerUnit) * (1 - x.Discount))
                                };
            }
        }
        #endregion

        #region filtering_2_4
        private class Order_ByOrderLinesCount : AbstractIndexCreationTask<Order>
        {
            public Order_ByOrderLinesCount()
            {
                Map = orders => from order in orders
                                select new
                                {
                                    Lines_Count = order.Lines.Count
                                };
            }
        }
        #endregion

        #region filtering_3_4
        public class Order_ByOrderLines_ProductName : AbstractIndexCreationTask<Order>
        {
            public Order_ByOrderLines_ProductName()
            {
                Map = orders => from order in orders
                                select new
                                {
                                    Lines_ProductName = order.Lines.Select(x => x.ProductName)
                                };
            }
        }
        #endregion

        #region filtering_5_4
        public class BlogPosts_ByTags : AbstractIndexCreationTask<BlogPost>
        {
            public BlogPosts_ByTags()
            {
                Map = posts => from post in posts
                               select new
                               {
                                   post.Tags
                               };
            }
        }
        #endregion

        public Filtering()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_0_1
                    IList<Employee> results = session
                        .Query<Employee, Employees_ByFirstAndLastName>()                // query 'Employees/ByFirstAndLastName' index
                        .Where(x => x.FirstName == "Robert" && x.LastName == "King")    // filtering predicates
                        .ToList();                                                      // materialize query by sending it to server for processing
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_0_2
                    IList<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee, Employees_ByFirstAndLastName>()    // query 'Employees/ByFirstAndLastName' index
                        .WhereEquals(x => x.FirstName, "Robert")                    // filtering predicates
                        .AndAlso()                                                  // by default OR is between each condition
                        .WhereEquals(x => x.LastName, "King")                       // filtering predicates
                        .ToList();                                                  // materialize query by sending it to server for processing
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_1_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()  // query 'Products/ByUnitsInStock' index
                        .Where(x => x.UnitsInStock > 50)            // filtering predicates
                        .ToList();                                  // materialize query by sending it to server for processing
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_1_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()  // query 'Products/ByUnitsInStock' index
                        .WhereGreaterThan(x => x.UnitsInStock, 50)          // filtering predicates
                        .ToList();                                          // materialize query by sending it to server for processing
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_7_1
                    IList<Order> results = session
                        .Query<Orders_ByTotalPrice.Result, Orders_ByTotalPrice>()
                        .Where(x => x.TotalPrice > 50)
                        .OfType<Order>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_7_2
                    IList<Order> results = session
                        .Advanced
                        .DocumentQuery<Orders_ByTotalPrice.Result, Orders_ByTotalPrice>()
                        .WhereGreaterThan(x => x.TotalPrice, 50)
                        .OfType<Order>()
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_2_1
                    IList<Order> results = session
                        .Query<Order, Order_ByOrderLinesCount>()    // query 'Order/ByOrderLinesCount' index
                        .Where(x => x.Lines.Count > 50)             // filtering predicates
                        .ToList();                                  // materialize query by sending it to server for processing
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_2_2
                    IList<Order> results = session
                        .Advanced
                        .DocumentQuery<Order, Order_ByOrderLinesCount>()    // query 'Order/ByOrderLinesCount' index
                        .WhereGreaterThan(x => x.Lines.Count, 50)           // filtering predicates
                        .ToList();                                          // materialize query by sending it to server for processing
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_3_1
                    IList<Order> results = session
                        .Query<Order, Order_ByOrderLines_ProductName>()                                 // query 'Order/ByOrderLines/ProductName' index
                        .Where(x => x.Lines.Any(l => l.ProductName == "Teatime Chocolate Biscuits"))    // filtering predicates
                        .ToList();                                                                      // materialize query by sending it to server for processing
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_3_2
                    IList<Order> results = session
                        .Advanced
                        .DocumentQuery<Order, Order_ByOrderLines_ProductName>()         // query 'Order/ByOrderLines/ProductName' index
                        .WhereEquals("Lines_ProductName", "Teatime Chocolate Biscuits") // filtering predicates
                        .ToList();                                                      // materialize query by sending it to server for processing
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_4_1
                    IList<Employee> results = session
                        .Query<Employee, Employees_ByFirstAndLastName>()    // query 'Employees/ByFirstAndLastName' index
                        .Where(x => x.FirstName.In("Robert", "Nancy"))      // filtering predicates (remember to add `Raven.Client.Linq` namespace to usings)
                        .ToList();                                          // materialize query by sending it to server for processing
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_4_2
                    IList<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee, Employees_ByFirstAndLastName>()    // query 'Employees/ByFirstAndLastName' index
                        .WhereIn(x => x.FirstName, new[] { "Robert", "Nancy" })     // filtering predicates
                        .ToList();                                                  // materialize query by sending it to server for processing
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_5_1
                    IList<BlogPost> results = session
                        .Query<BlogPost, BlogPosts_ByTags>()                                    // query 'BlogPosts/ByTags' index
                        .Where(x => x.Tags.ContainsAny(new[] { "Development", "Research" }))    // filtering predicates (remember to add `Raven.Client.Linq` namespace to usings)
                        .ToList();                                                              // materialize query by sending it to server for processing
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_5_2
                    IList<BlogPost> results = session
                        .Advanced
                        .DocumentQuery<BlogPost, BlogPosts_ByTags>()                // query 'BlogPosts/ByTags' index
                        .ContainsAny("Tags", new[] { "Development", "Research" })   // filtering predicates
                        .ToList();                                                  // materialize query by sending it to server for processing
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_6_1
                    IList<BlogPost> results = session
                        .Query<BlogPost, BlogPosts_ByTags>()                                    // query 'BlogPosts/ByTags' index
                        .Where(x => x.Tags.ContainsAll(new[] { "Development", "Research" }))    // filtering predicates (remember to add `Raven.Client.Linq` namespace to usings)
                        .ToList();                                                              // materialize query by sending it to server for processing
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_6_2
                    IList<BlogPost> results = session
                        .Advanced
                        .DocumentQuery<BlogPost, BlogPosts_ByTags>()                // query 'BlogPosts/ByTags' index
                        .ContainsAll("Tags", new[] { "Development", "Research" })   // filtering predicates
                        .ToList();                                                  // materialize query by sending it to server for processing
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_8_1
                    // return all products which name starts with 'ch'
                    IList<Product> results = session
                        .Query<Product>()
                        .Where(x => x.Name.StartsWith("ch"))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_8_2
                    // return all products which name starts with 'ch'
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product>()
                        .WhereStartsWith(x => x.Name, "ch")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_9_1
                    // return all products which name ends with 'ra'
                    IList<Product> results = session
                        .Query<Product>()
                        .Where(x => x.Name.EndsWith("ra"))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_9_2
                    // return all products which name ends with 'ra'
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product>()
                        .WhereEndsWith(x => x.Name, "ra")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region filtering_10_1
                    // return all orders that were shipped to 'Albuquerque'
                    IList<Order> results = session
                        .Query<Order>()
                        .Where(x => x.ShipTo.City == "Albuquerque")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_10_2
                    // return all orders that were shipped to 'Albuquerque'
                    IList<Order> results = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .WhereEquals(x => x.ShipTo.City, "Albuquerque")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_11_1
                    Order order = session
                        .Query<Order>()
                        .Where(x => x.Id == "orders/1-A")
                        .FirstOrDefault();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_11_2
                    Order order = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .WhereEquals(x => x.Id, "orders/1-A")
                        .FirstOrDefault();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_12_1
                    IList<Order> orders = session
                        .Query<Order>()
                        .Where(x => x.Id.StartsWith("orders/1"))
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region filtering_12_2
                    IList<Order> orders = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .WhereStartsWith(x => x.Id, "orders/1")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
