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
        #region order
        public class Order
        {
            public string CustomerId { get; set; }

            public string[] SupplierIds { get; set; }

            public Referral Referral { get; set; }

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

            public Referral Referral { get; set; }

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

            public Referral Referral { get; set; }

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
                    #region includes_3_0
                    IList<Order> orders = session
                        .Query<Order>()
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
                        .DocumentQuery<Order>()
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
                    #region includes_6_0
                    Order order = session
                        .Include<Order>(x => x.Referral.CustomerId)
                        .Load("orders/1-A");

                    // this will not require querying the server!
                    Customer customer = session.Load<Customer>(order.Referral.CustomerId);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region includes_6_2
                    Order order = session.Include("Referral.CustomerId")
                        .Load<Order>("orders/1-A");

                    // this will not require querying the server!
                    Customer customer = session.Load<Customer>(order.Referral.CustomerId);
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
            }
        }
    }
}
