using System.Linq;
using Raven.Client.Linq;

namespace RavenCodeSamples.Consumer
{
	class Includes : CodeSampleBase
	{
		public void SimplePaging()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes1

					var order = session.Include<Order>(x => x.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var cust = session.Load<Customer>(order.CustomerId);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes1_2

					var order = session.Include<Order2, Customer2>(x => x.Customer2Id)
						.Load("orders/1234");

					// this will not require querying the server!
					var cust2 = session.Load<Customer2>(order.Customer2Id);

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
						var cust = session.Load<Customer>(order.CustomerId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes2_2

					var orders = session.Query<Order2>()
						.Customize(x => x.Include<Order2, Customer2>(o => o.Customer2Id))
						.Where(x => x.TotalPrice > 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var cust2 = session.Load<Customer2>(order.Customer2Id);
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
						var supp = session.Load<Supplier>(supplierId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes3_2

					var order = session.Include<Order2, Supplier2>(x => x.Supplier2Ids)
						.Load("orders/1234");

					foreach (var supplier2Id in order.Supplier2Ids)
					{
						// this will not require querying the server!
						var supp2 = session.Load<Supplier2>(supplier2Id);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes4

					var order = session.Include<Order>(x => x.Referral.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var referrer = session.Load<Customer>(order.Referral.CustomerId);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes4_2

					var order = session.Include<Order2, Customer2>(x => x.Referral2.Customer2Id)
						.Load("orders/1234");

					// this will not require querying the server!
					var referrer2 = session.Load<Customer2>(order.Referral2.Customer2Id);

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

					var order = session.Include<Order2, Product2>(x => x.LineItem2s.Select(li => li.Product2Id))
					.Load("orders/1234");

					foreach (var lineItem2 in order.LineItem2s)
					{
						// this will not require querying the server!
						var product2 = session.Load<Product2>(lineItem2.Product2Id);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes6

					var order = session.Include<Order3, Customer2>(x => x.Customer.Id)
						.Load("orders/1234");

					// this will not require querying the server!
					var fullCustomer = session.Load<Customer2>(order.Customer.Id);

					#endregion
				}
			}
		}
	}
}