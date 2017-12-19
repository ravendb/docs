using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.HowTo
{
    public class HandleDocumentRelationships
    {
        #region includes_3_3
        public class Orders_ByTotalPrice : AbstractIndexCreationTask<Order>
        {
            public Orders_ByTotalPrice()
            {
                Map = orders => from order in orders
                                select new
                                {
                                    order.TotalPrice
                                };
            }
        }
        #endregion

        #region includes_8_5
        public class Order2s_ByTotalPrice : AbstractIndexCreationTask<Order2>
        {
            public Order2s_ByTotalPrice()
            {
                Map = orders => from order in orders
                                select new
                                {
                                    order.TotalPrice
                                };
            }
        }
        #endregion

        #region order
        public class Order
        {
            public string CustomerId { get; set; }

            public string[] SupplierIds { get; set; }

            public Referral Refferal { get; set; }

            public LineItem[] LineItems { get; set; }

            public double TotalPrice { get; set; }
        }
        #endregion

        #region customer
        public class Customer
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
        #endregion

        #region denormalized_customer
        public class DenormalizedCustomer
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }
        }
        #endregion

        #region referral
        public class Referral
        {
            public string CustomerId { get; set; }

            public double CommissionPercentage { get; set; }
        }
        #endregion

        #region line_item
        public class LineItem
        {
            public string ProductId { get; set; }

            public string Name { get; set; }

            public int Quantity { get; set; }
        }
        #endregion

        #region order_2
        public class Order2
        {
            public int CustomerId { get; set; }

            public Guid[] SupplierIds { get; set; }

            public Referral Refferal { get; set; }

            public LineItem[] LineItems { get; set; }

            public double TotalPrice { get; set; }
        }
        #endregion

        #region customer_2
        public class Customer2
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
        #endregion

        #region referral_2
        public class Referral2
        {
            public int CustomerId { get; set; }

            public double CommissionPercentage { get; set; }
        }
        #endregion

        #region order_3
        public class Order3
        {
            public DenormalizedCustomer Customer { get; set; }

            public string[] SupplierIds { get; set; }

            public Referral Refferal { get; set; }

            public LineItem[] LineItems { get; set; }

            public double TotalPrice { get; set; }
        }
        #endregion

        #region person_1
        public class Person
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public Dictionary<string, string> Attributes { get; set; }
        }
        #endregion

        #region person_2
        public class PersonWithAttribute
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public Dictionary<string, Attribute> Attributes { get; set; }
        }

        public class Attribute
        {
            public string Ref { get; set; }
        }
        #endregion

        public void Includes()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_1_0
                    Order order = session
                        .Include<Order>(x => x.CustomerId)
                        .Load("orders/1-A");

                    // this will not require querying the server!
                    Customer customer = session
                        .Load<Customer>(order.CustomerId);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_1_1
                    var command = new GetDocumentsCommand("orders/1-A", new[] { "CustomerId" }, metadataOnly: false);

                    session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);

                    var order = (BlittableJsonReaderObject)command.Result.Results[0];
                    var customer = (BlittableJsonReaderObject)command.Result.Includes["customers/1-A"];

                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_2_0
                    Dictionary<string, Order> orders = session
                        .Include<Order>(x => x.CustomerId)
                        .Load("orders/1-A", "orders/2-A");

                    foreach (Order order in orders.Values)
                    {
                        // this will not require querying the server!
                        Customer customer = session.Load<Customer>(order.CustomerId);
                    }
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_2_1
                    var command = new GetDocumentsCommand(new[] { "orders/1-A", "orders/2-A" }, new[] { "CustomerId" }, metadataOnly: false);

                    session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);

                    var orders = command.Result.Results;
                    var customers = command.Result.Includes;
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {

                using (var session = store.OpenSession())
                {
                    #region includes_3_0
                    IList<Order> orders = session
                        .Query<Order, Orders_ByTotalPrice>()
                        .Include(o => o.CustomerId)
                        .Where(x => x.TotalPrice > 100)
                        .ToList();

                    foreach (Order order in orders)
                    {
                        // this will not require querying the server!
                        Customer customer = session
                            .Load<Customer>(order.CustomerId);
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region includes_3_1
                    IList<Order> orders = session
                        .Advanced
                        .DocumentQuery<Order, Orders_ByTotalPrice>()
                        .Include(x => x.CustomerId)
                        .WhereGreaterThan(x => x.TotalPrice, 100)
                        .ToList();

                    foreach (Order order in orders)
                    {
                        // this will not require querying the server!
                        Customer customer = session
                            .Load<Customer>(order.CustomerId);
                    }
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_4_0
                    Order order = session
                        .Include<Order>(x => x.SupplierIds)
                        .Load("orders/1-A");

                    foreach (string supplierId in order.SupplierIds)
                    {
                        // this will not require querying the server!
                        Supplier supplier = session.Load<Supplier>(supplierId);
                    }
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_4_1
                    var command = new GetDocumentsCommand("orders/1-A", new[] { "SupplierIds" }, metadataOnly: false);

                    session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);

                    var order = (BlittableJsonReaderObject)command.Result.Results[0];
                    var suppliers = command.Result.Includes;
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_5_0
                    Dictionary<string, Order> orders = session
                        .Include<Order>(x => x.SupplierIds)
                        .Load("orders/1-A", "orders/2-A");

                    foreach (Order order in orders.Values)
                    {
                        foreach (string supplierId in order.SupplierIds)
                        {
                            // this will not require querying the server!
                            Supplier supplier = session.Load<Supplier>(supplierId);
                        }
                    }
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_5_1

                    var command = new GetDocumentsCommand(new[] { "orders/1-A", "orders/2-A" }, new[] { "SupplierIds" },
                        metadataOnly: false);

                    session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);

                    var orders = command.Result.Results;
                    var suppliers = command.Result.Includes;

                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_6_0
                    Order order = session
                        .Include<Order>(x => x.Refferal.CustomerId)
                        .Load("orders/1-A");

                    // this will not require querying the server!
                    Customer customer = session.Load<Customer>(order.Refferal.CustomerId);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region includes_6_2
                    Order order = session.Include("Refferal.CustomerId")
                        .Load<Order>("orders/1-A");

                    // this will not require querying the server!
                    Customer customer = session.Load<Customer>(order.Refferal.CustomerId);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_6_1
                    var command = new GetDocumentsCommand("orders/1-A", new[] { "Refferal.CustomerId" },
                        metadataOnly: false);

                    var order = (BlittableJsonReaderObject)command.Result.Results[0];
                    var customers = command.Result.Includes;
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_7_0
                    Order order = session
                        .Include<Order>(x => x.LineItems.Select(l => l.ProductId))
                        .Load("orders/1-A");

                    foreach (LineItem lineItem in order.LineItems)
                    {
                        // this will not require querying the server!
                        Product product = session.Load<Product>(lineItem.ProductId);
                    }
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_7_1
                    var command = new GetDocumentsCommand("orders/1-A", new[] { "LineItems[].ProductId" },
                        metadataOnly: false);

                    var order = (BlittableJsonReaderObject)command.Result.Results[0];
                    var products = command.Result.Includes;
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_9_0
                    Order3 order = session
                        .Include<Order3, Customer>(x => x.Customer.Id)
                        .Load("orders/1-A");

                    // this will not require querying the server!
                    Customer customer = session.Load<Customer>(order.Customer.Id);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_9_1
                    var command = new GetDocumentsCommand("orders/1-A", new[] { "Customer.Id" },
                        metadataOnly: false);

                    var order = (BlittableJsonReaderObject)command.Result.Results[0];
                    var customer = command.Result.Includes;
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_10_0
                    session.Store(
                        new Person
                        {
                            Id = "people/1-A",
                            Name = "John Doe",
                            Attributes = new Dictionary<string, string>
                            {
                                { "Mother", "people/2" },
                                { "Father", "people/3" }
                            }
                        });

                    session.Store(
                        new Person
                        {
                            Id = "people/2",
                            Name = "Helen Doe",
                            Attributes = new Dictionary<string, string>()
                        });

                    session.Store(
                        new Person
                        {
                            Id = "people/3",
                            Name = "George Doe",
                            Attributes = new Dictionary<string, string>()
                        });
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region includes_10_1
                    var person = session
                        .Include<Person>(x => x.Attributes.Values)
                        .Load("people/1-A");

                    var mother = session
                        .Load<Person>(person.Attributes["Mother"]);

                    var father = session
                        .Load<Person>(person.Attributes["Father"]);

                    Assert.Equal(1, session.Advanced.NumberOfRequests);
                    #endregion

                    #region includes_10_2
                    var command = new GetDocumentsCommand("people/1-A", new[] { "Attributes.$Values" },
                        metadataOnly: false);

                    var includes = command.Result.Includes;
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_10_3
                    var person = session
                        .Include<Person>(x => x.Attributes.Keys)
                        .Load("people/1-A");
                    #endregion

                    #region includes_10_4
                    var command = new GetDocumentsCommand("people/1-A", new[] { "Attributes.$Keys" },
                        metadataOnly: false);
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region includes_11_0
                    session.Store(
                        new PersonWithAttribute
                        {
                            Id = "people/1-A",
                            Name = "John Doe",
                            Attributes = new Dictionary<string, Attribute>
                            {
                                { "Mother", new Attribute { Ref = "people/2" } },
                                { "Father", new Attribute { Ref = "people/3" } }
                            }
                        });

                    session.Store(
                        new Person
                        {
                            Id = "people/2",
                            Name = "Helen Doe",
                            Attributes = new Dictionary<string, string>()
                        });

                    session.Store(
                        new Person
                        {
                            Id = "people/3",
                            Name = "George Doe",
                            Attributes = new Dictionary<string, string>()
                        });
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region includes_11_1
                    var person = session
                        .Include<PersonWithAttribute>(x => x.Attributes.Values.Select(v => v.Ref))
                        .Load("people/1-A");

                    var mother = session
                        .Load<Person>(person.Attributes["Mother"].Ref);

                    var father = session
                        .Load<Person>(person.Attributes["Father"].Ref);

                    Assert.Equal(1, session.Advanced.NumberOfRequests);
                    #endregion
                }

                #region includes_11_2
                var command = new GetDocumentsCommand("people/1-A", new[] { "Attributes.$Values.Ref" },
                    metadataOnly: false);

                var includes = command.Result.Includes;
                #endregion
            }
        }
    }
}
