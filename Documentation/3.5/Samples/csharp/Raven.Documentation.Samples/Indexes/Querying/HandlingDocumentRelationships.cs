using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Json.Linq;

using Xunit;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class HandlingDocumentRelationships
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

			public Guid[] SupplierIds { get; set; }

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
			public Guid ProductId { get; set; }

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
						.Load("orders/1234");

					// this will not require querying the server!
					Customer customer = session
						.Load<Customer>(order.CustomerId);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_1_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "orders/1234" }, includes: new[] { "CustomerId" });

				RavenJObject order = result.Results[0];
				RavenJObject customer = result.Includes[0];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_2_0
					Order[] orders = session
						.Include<Order>(x => x.CustomerId)
						.Load("orders/1234", "orders/4321");

					foreach (Order order in orders)
					{
						// this will not require querying the server!
						Customer customer = session.Load<Customer>(order.CustomerId);
					}
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_2_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "orders/1234", "orders/4321" }, includes: new[] { "CustomerId" });

				List<RavenJObject> orders = result.Results;
				List<RavenJObject> customers = result.Includes;
				#endregion
			}

			using (var store = new DocumentStore())
			{

				using (var session = store.OpenSession())
				{
					#region includes_3_0
					IList<Order> orders = session
						.Query<Order, Orders_ByTotalPrice>()
						.Customize(x => x.Include<Order>(o => o.CustomerId))
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
				#region includes_3_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Orders/ByTotalPrice",
						new IndexQuery
						{
							Query = "TotalPrice_Range:{Ix100 TO NULL}"
						},
						includes: new[] { "CustomerId" });

				List<RavenJObject> orders = result.Results;
				List<RavenJObject> customers = result.Includes;
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_4_0
					Order order = session
						.Include<Order>(x => x.SupplierIds)
						.Load("orders/1234");

					foreach (Guid supplierId in order.SupplierIds)
					{
						// this will not require querying the server!
						Supplier supplier = session.Load<Supplier>(supplierId);
					}
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_4_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "orders/1234" }, includes: new[] { "SupplierIds" });

				RavenJObject order = result.Results[0];
				RavenJObject customer = result.Includes[0];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_5_0
					Order[] orders = session
						.Include<Order>(x => x.SupplierIds)
						.Load("orders/1234", "orders/4321");

					foreach (Order order in orders)
					{
						foreach (Guid supplierId in order.SupplierIds)
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
				#region includes_5_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "orders/1234", "orders/4321" }, includes: new[] { "SupplierIds" });

				List<RavenJObject> orders = result.Results;
				List<RavenJObject> customers = result.Includes;
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_6_0
					Order order = session
						.Include<Order>(x => x.Referral.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					Customer customer = session.Load<Customer>(order.Referral.CustomerId);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_6_2
					Order order = session.Include("Referral.CustomerId")
						.Load<Order>("orders/1234");

					// this will not require querying the server!
					Customer customer = session.Load<Customer>(order.Referral.CustomerId);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_6_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "orders/1234" }, includes: new[] { "Referral.CustomerId" });

				RavenJObject order = result.Results[0];
				RavenJObject customer = result.Includes[0];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_7_0
					Order order = session
						.Include<Order>(x => x.LineItems.Select(li => li.ProductId))
						.Load("orders/1234");

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
				#region includes_7_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "orders/1234" }, includes: new[] { "LineItems.,ProductId" });

				RavenJObject order = result.Results[0];
				RavenJObject product = result.Includes[0];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_8_0
					Order2 order = session
						.Include<Order2, Customer2>(x => x.CustomerId)
						.Load("order2s/1234");

					// this will not require querying the server!
					Customer2 customer = session.Load<Customer2>(order.CustomerId);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_8_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "order2s/1234" }, includes: new[] { "CustomerId" });

				RavenJObject order = result.Results[0];
				RavenJObject customer = result.Includes[0];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_8_2
					IList<Order2> orders = session
						.Query<Order2, Order2s_ByTotalPrice>()
						.Customize(x => x.Include<Order2, Customer2>(o => o.CustomerId))
						.Where(x => x.TotalPrice > 100)
						.ToList();

					foreach (Order2 order in orders)
					{
						// this will not require querying the server!
						Customer2 customer = session.Load<Customer2>(order.CustomerId);
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_8_3
					IList<Order2> orders = session
						.Advanced
						.DocumentQuery<Order2, Order2s_ByTotalPrice>()
						.Include("CustomerId")
						.WhereGreaterThan(x => x.TotalPrice, 100)
						.ToList();

					foreach (Order2 order in orders)
					{
						// this will not require querying the server!
						Customer2 customer = session.Load<Customer2>(order.CustomerId);
					}
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_8_4
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Order2s/ByTotalPrice",
						new IndexQuery
						{
							Query = "TotalPrice_Range:{Ix100 TO NULL}"
						},
						includes: new[] { "CustomerId" });

				List<RavenJObject> orders = result.Results;
				List<RavenJObject> customers = result.Includes;
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_8_6
					Order2 order = session
						.Include<Order2, Supplier>(x => x.SupplierIds)
						.Load("order2s/1234");

					foreach (Guid supplierId in order.SupplierIds)
					{
						// this will not require querying the server!
						Supplier supplier = session.Load<Supplier>(supplierId);
					}
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_8_7
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "order2s/1234" }, includes: new[] { "SupplierIds" });

				RavenJObject order = result.Results[0];
				List<RavenJObject> suppliers = result.Includes;
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_8_8
					Order2 order = session
						.Include<Order2, Customer2>(x => x.Referral.CustomerId)
						.Load("order2s/1234");

					// this will not require querying the server!
					Customer2 customer = session.Load<Customer2>(order.Referral.CustomerId);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_8_9
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "order2s/1234" }, includes: new[] { "Referral.CustomerId" });

				RavenJObject order = result.Results[0];
				RavenJObject customer = result.Includes[0];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_8_10
					Order2 order = session
						.Include<Order2, Product>(x => x.LineItems.Select(li => li.ProductId))
						.Load("orders/1234");

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
				#region includes_8_11
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "order2s/1234" }, includes: new[] { "LineItems.,ProductId" });

				RavenJObject order = result.Results[0];
				List<RavenJObject> products = result.Includes;
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_9_0
					Order3 order = session
						.Include<Order3, Customer>(x => x.Customer.Id)
						.Load("orders/1234");

					// this will not require querying the server!
					Customer customer = session.Load<Customer>(order.Customer.Id);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region includes_9_1
				MultiLoadResult result = store
					.DatabaseCommands
					.Get(ids: new[] { "orders/1234" }, includes: new[] { "Customer.Id" });

				RavenJObject order = result.Results[0];
				RavenJObject customer = result.Includes[0];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_10_0
					session.Store(
						new Person
						{
							Id = "people/1",
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
						.Load("people/1");

					var mother = session
						.Load<Person>(person.Attributes["Mother"]);

					var father = session
						.Load<Person>(person.Attributes["Father"]);

					Assert.Equal(1, session.Advanced.NumberOfRequests);
					#endregion
				}

				#region includes_10_2
				var result = store
					.DatabaseCommands
					.Get(new[] { "people/1" }, new[] { "Attributes.$Values" });

				var include1 = result.Includes[0];
				var include2 = result.Includes[1];
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_10_3
					var person = session
						.Include<Person>(x => x.Attributes.Keys)
						.Load("people/1");
					#endregion
				}

				#region includes_10_4
				var result = store
					.DatabaseCommands
					.Get(new[] { "people/1" }, new[] { "Attributes.$Keys" });
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes_11_0
					session.Store(
						new PersonWithAttribute
						{
							Id = "people/1",
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
						.Load("people/1");

					var mother = session
						.Load<Person>(person.Attributes["Mother"].Ref);

					var father = session
						.Load<Person>(person.Attributes["Father"].Ref);

					Assert.Equal(1, session.Advanced.NumberOfRequests);
					#endregion
				}

				#region includes_11_2
				var result = store
					.DatabaseCommands
					.Get(new[] { "people/1" }, new[] { "Attributes.$Values,Ref" });

				var include1 = result.Includes[0];
				var include2 = result.Includes[1];
				#endregion
			}
		}
	}
}