using System.Linq;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class DynamicAggregation
	{
		#region currency
		public enum Currency
		{
			EUR,
			USD
		}
		#endregion

		#region order
		public class Order
		{
			public double Total { get; set; }

			public string Product { get; set; }

			public Currency Currency { get; set; }
		}
		#endregion

		#region dynamic_aggregation_index_def
		public class Orders_All : AbstractIndexCreationTask<Order>
		{
			public Orders_All()
			{
				Map = orders => from order in orders
						select new
						{
							order.Total,
							order.Product,
							Concurrency = order.Currency
						};

				Sort(x => x.Total, SortOptions.Double);
			}
		}
		#endregion

		public DynamicAggregation()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region dynamic_aggregation_1
					FacetResults facetResults = session
						.Query<Order, Orders_All>()
						.Where(x => x.Total > 500)
						.AggregateBy(x => x.Product)
							.SumOn(x => x.Total)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region dynamic_aggregation_range

					FacetResults facetResults = session.Query<Order, Orders_All>()
									.AggregateBy(x => x.Product)
									.AddRanges(
										x => x.Total < 100,
										x => x.Total >= 100 && x.Total < 500,
										x => x.Total >= 500 && x.Total < 1500,
										x => x.Total >= 1500
									)
									.SumOn(x => x.Total)
									.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region dynamic_aggregation_multiple_items
					FacetResults facetResults = session.Query<Order, Orders_All>()
									.AggregateBy(x => x.Product)
										.SumOn(x => x.Total)
										.CountOn(x => x.Total)
									.AndAggregateOn(x => x.Currency)
										.MinOn(x => x.Total)
									.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region dynamic_aggregation_different_fieldss
					FacetResults facetResults = session.Query<Order, Orders_All>()
									.AggregateBy(x => x.Product)
										.MaxOn(x => x.Total)
										.MinOn(x => x.Currency)
									.ToList();
					#endregion
				}
			}
		}
	}
}