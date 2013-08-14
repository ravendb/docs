// -----------------------------------------------------------------------
//  <copyright file="DynamicAggregation.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;

namespace RavenCodeSamples.ClientApi.Querying
{
	public class DynamicAggregation : CodeSampleBase
	{
		enum Concurrency
		{
			EUR,
			USD
		}

		class Order
		{
			public double Total { get; set; }
			public string Product { get; set; }
			public Concurrency Concurrency { get; set; }
		}

		public DynamicAggregation()
		{
			using (var store = NewDocumentStore())
			{

				#region dynamic_aggregation_index_def
				store.DatabaseCommands.PutIndex("Orders/All", new IndexDefinitionBuilder<Order>()
				{
					Map = orders => from order in orders
									select new
									{
										order.Total,
										order.Product,
										order.Concurrency
									},
					SortOptions = { { x => x.Product, SortOptions.Double } }
				});
				#endregion

				using (var session = store.OpenSession())
				{
					#region dynamic_aggregation_1
					var result = session.Query<Order>("Orders/All")
					                    .Where(x => x.Total > 500)
					                    .AggregateBy(x => x.Product)
											.SumOn(x => x.Total)
					                    .ToList();
					#endregion

					#region dynamic_aggregation_range

					result = session.Query<Order>("Orders/All")
					                .AggregateBy(x => x.Product)
					                .AddRanges(x => x.Total < 100,
					                           x => x.Total >= 100 && x.Total < 500,
					                           x => x.Total >= 500 && x.Total < 1500,
					                           x => x.Total >= 1500)
					                .SumOn(x => x.Total)
					                .ToList();
					#endregion

					#region dynamic_aggregation_multiple_items
					result = session.Query<Order>("Orders/All")
					                .AggregateBy(x => x.Product)
										.SumOn(x => x.Total)
										.CountOn(x => x.Total)
					                .AndAggregateOn(x => x.Concurrency)
										.MinOn(x => x.Total)
					                .ToList();
					#endregion

					#region dynamic_aggregation_different_fieldss
					result = session.Query<Order>("Orders/All")
					                .AggregateBy(x => x.Product)
										.MaxOn(x => x.Total)
										.MinOn(x => x.Concurrency)
									.ToList();
					#endregion
				}

				

			}
		}
	}
}