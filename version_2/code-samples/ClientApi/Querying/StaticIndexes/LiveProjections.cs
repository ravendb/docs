namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Linq;

	using Raven.Client;
	using Raven.Client.Linq;
	using Raven.Client.Indexes;
	using Raven.Database.Linq.PrivateExtensions;

	public class LiveProjections : CodeSampleBase
	{
		public void Query()
		{
			int userId = 1;

			using (var documentStore = NewDocumentStore())
			{
				using (var documentSession = documentStore.OpenSession())
				{
					#region live_projections_2
					documentSession.Query<Shipment, PurchaseHistoryIndex>()
						.Where(x => x.UserId == userId)
						.As<PurchaseHistoryViewItem>()
						.ToArray();

					#endregion
				}
			}
		}

		private class Shipment
		{
			public int UserId { get; set; }
		}

		private class PurchaseHistoryViewItem
		{
		}

		#region live_projections_1
		public class PurchaseHistoryIndex : AbstractIndexCreationTask<Order, Order>
		{
			public PurchaseHistoryIndex()
			{
				Map = orders => from order in orders
								from item in order.Items
								select new
								{
									UserId = order.UserId,
									ProductId = item.Id
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

		public class Order
		{
			public string UserId { get; set; }

			public Item[] Items { get; set; }
		}

		public class Item
		{
			public string Id { get; set; }
		}
	}

}