using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToPerformDynamicAggregation
	{
		private class Order
		{
			public string CompanyId { get; set; }

			public string CustomerId { get; set; }

			public decimal TotalPrice { get; set; }
		}

		private interface IFoo
		{
			/*
			#region aggregate_1
			DynamicAggregationQuery<TResult> AggregateBy<TResult>(
				this IQueryable<TResult> queryable,
				string path,
				string displayName = null) { ... }

			DynamicAggregationQuery<TResult> AggregateBy<TResult>(
				this IQueryable<TResult> queryable,
				Expression<Func<TResult, object>> path) { ... }

			DynamicAggregationQuery<TResult> AggregateBy<TResult>(
				this IQueryable<TResult> queryable,
				Expression<Func<TResult, object>> path,
				string displayName) { ... }
			#endregion
			*/
		}

		public HowToPerformDynamicAggregation()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region aggregate_2
					// sum up all order prices
					// for each customer
					// where price is higher than 500
					FacetResults aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.SumOn(x => x.TotalPrice)
						.ToList();

					FacetResult customerAggregation = aggregationResults.Results["CustomerId"];
					double? sum = customerAggregation.Values[0].Sum;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_3
					// count all orders
					// for each customer
					// where price is higher than 500
					FacetResults aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.CountOn(x => x.TotalPrice)
						.ToList();

					FacetResult customerAggregation = aggregationResults.Results["CustomerId"];
					int? count = customerAggregation.Values[0].Count;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_4
					// count price average for orders
					// for each customer
					// where price is higher than 500
					FacetResults aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.AverageOn(x => x.TotalPrice)
						.ToList();

					FacetResult customerAggregation = aggregationResults.Results["CustomerId"];
					double? average = customerAggregation.Values[0].Average;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_5
					// count max and min price for orders
					// for each customer
					// where price is higher than 500
					FacetResults aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.MinOn(x => x.TotalPrice)
							.MaxOn(x => x.TotalPrice)
						.ToList();

					FacetResult customerAggregation = aggregationResults.Results["CustomerId"];
					double? min = customerAggregation.Values[0].Min;
					double? max = customerAggregation.Values[0].Max;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_6
					FacetResults aggregationResults = session.Query<Order>("Orders/All")
						.AggregateBy(x => x.CustomerId)
							.AddRanges(x => x.TotalPrice < 100)
							.AddRanges(x => x.TotalPrice >= 100 && x.TotalPrice < 500)
							.AddRanges(x => x.TotalPrice >= 500 && x.TotalPrice < 1000)
							.AddRanges(x => x.TotalPrice >= 1000)
						.ToList();

					FacetResult customerAggregation = aggregationResults.Results["CustomerId"];
					FacetValue range1 = customerAggregation.Values.First(x => x.Range == "[NULL to Dx100]");
					FacetValue range2 = customerAggregation.Values.First(x => x.Range == "{Dx100 to Dx500]");
					FacetValue range3 = customerAggregation.Values.First(x => x.Range == "{Dx500 to Dx1000]");
					FacetValue range4 = customerAggregation.Values.First(x => x.Range == "{Dx1000 to NULL]");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_7
					// sum up all order prices
					// for each customer
					// count price average for all orders
					// for each company
					FacetResults aggregationResults = session.Query<Order>("Orders/All")
						.AggregateBy(x => x.CustomerId)
							.SumOn(x => x.TotalPrice)
						.AndAggregateOn(x => x.CompanyId)
							.AverageOn(x => x.TotalPrice)
						.ToList();

					FacetResult customerAggregation = aggregationResults.Results["CustomerId"];
					FacetResult companyAggregation = aggregationResults.Results["CompanyId"];

					double? sum = customerAggregation.Values[0].Sum;
					double? average = companyAggregation.Values[0].Average;
					#endregion
				}
			}
		}
	}
}