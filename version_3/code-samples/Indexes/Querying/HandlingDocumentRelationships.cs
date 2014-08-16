using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Indexes.Foo;
using Raven.Documentation.CodeSamples.Orders;

using System;
using System.Linq;

namespace Raven.Documentation.CodeSamples.Indexes.Querying
{
	public class HandlingDocumentRelationships
	{
		private class Order
		{
			public int CustomerId { get; set; }
			public Guid[] SupplierIds { get; set; }
			public Referral Refferal { get; set; }
			public LineItem[] LineItems { get; set; }
			public double TotalPrice { get; set; }
		}

		private class Referral
		{
			public int CustomerId { get; set; }
			public double CommissionPercentage { get; set; }
		}

		private class LineItem
		{
			public Guid ProductId { get; set; }
			public string Name { get; set; }
			public int Quantity { get; set; }
		}

		public void Includes()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes1
					var order = session.Include<Order>(x => x.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var customer = session.Load<Customer>(order.CustomerId);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_1
					var orders = session.Include<Order>(x => x.CustomerId)
						.Load("orders/1234", "orders/4321");

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var customer = session.Load<Customer>(order.CustomerId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes1_2
					var order = session.Include<Order, Customer>(x => x.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var customer = session.Load<Customer>(order.CustomerId);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes2
					var orders = session.Query<Order>()
						.Customize(x => x.Include<Order>(o => o.CustomerId))
						.Where(x => x.TotalPrice > 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var customer = session.Load<Customer>(order.CustomerId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes2_2
					var orders = session.Query<Order>()
						.Customize(x => x.Include<Order, Customer>(o => o.CustomerId))
						.Where(x => x.TotalPrice > 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var customer = session.Load<Customer>(order.CustomerId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes3
					var order = session.Include<Order>(x => x.SupplierIds)
						.Load("orders/1234");

					foreach (var supplierId in order.SupplierIds)
					{
						// this will not require querying the server!
						var supplier = session.Load<Supplier>(supplierId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_3
					var orders = session.Include<Order>(x => x.SupplierIds)
						.Load("orders/1234", "orders/4321");

					foreach (var order in orders)
					{
						foreach (var supplierId in order.SupplierIds)
						{
							// this will not require querying the server!
							var supplier = session.Load<Supplier>(supplierId);
						}
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes3_2
					var order = session.Include<Order, Supplier>(x => x.SupplierIds)
						.Load("orders/1234");

					foreach (var supplier2Id in order.SupplierIds)
					{
						// this will not require querying the server!
						var supplier = session.Load<Supplier>(supplier2Id);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes4
					var order = session.Include<Order>(x => x.Refferal.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var customer = session.Load<Customer>(order.Refferal.CustomerId);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_4
					var order = session.Include("Refferal.CustomerId")
						.Load<Order>("orders/1234");

					// this will not require querying the server!
					var customer = session.Load<Customer>(order.Refferal.CustomerId);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes4_2
					var order = session.Include<Order, Customer>(x => x.Refferal.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var customer = session.Load<Customer>(order.Refferal.CustomerId);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes5
					var order = session.Include<Order>(x => x.LineItems.Select(li => li.ProductId))
						.Load("orders/1234");

					foreach (var lineItem in order.LineItems)
					{
						// this will not require querying the server!
						var product = session.Load<Product>(lineItem.ProductId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes5_2
					var order = session.Include<Order, Product>(x => x.LineItems.Select(li => li.ProductId))
					.Load("orders/1234");

					foreach (var lineItem2 in order.LineItems)
					{
						// this will not require querying the server!
						var product = session.Load<Product>(lineItem2.ProductId);
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes6
					var order = session.Include<Order, Customer>(x => x.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var customer = session.Load<Customer>(order.CustomerId);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_7
					var orders = session.Advanced.DocumentQuery<Order>()
						.Include(x => x.CustomerId)
						.WhereGreaterThan(x => x.TotalPrice, 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var customer = session.Load<Customer>(order.CustomerId);
					}
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_8
					var orders = session.Advanced.DocumentQuery<Order>()
						.Include("CustomerId")
						.WhereGreaterThan(x => x.TotalPrice, 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var customer = session.Load<Customer>(order.CustomerId);
					}
					#endregion
				}
			}
		}
	}
}