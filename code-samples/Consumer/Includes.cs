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
			}
		}
	}
}