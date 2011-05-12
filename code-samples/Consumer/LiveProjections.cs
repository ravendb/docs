using System.Linq;
using Raven.Client.Indexes;

namespace RavenCodeSamples.Consumer
{
	#region liveprojection1
	public class PurchaseHistoryIndex : AbstractIndexCreationTask<Order, Order>
	{
		public PurchaseHistoryIndex()
		{
			Map = orders => from order in orders
			                from product in order.Items
			                select new
			                       	{
			                       		CustomerId = order.CustomerId,
			                       		ProductId = product.Id
			                       	};
			TransformResults = (database, orders) =>
			                   from order in orders
			                   from item in order.Items
			                   let product = database.Load<Product>(item.Id)
			                   where product != null
			                   select new
			                          	{
			                          		ProductId = item.Id,
			                          		ProductName = product.Name
			                          	};

		}
	}
	#endregion

	public class LiveProjections : CodeSampleBase
	{
		public void Test()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					var results = session
						.Include("CustomerId")
						.Load<Order>();
				}
			}
		}
	}
}
