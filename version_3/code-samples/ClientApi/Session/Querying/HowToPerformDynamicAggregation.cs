using System.Linq;

using Raven.Client;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
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
			DynamicAggregationQuery<T> AggregateBy<T>(
				this IQueryable<T> queryable,
				string path,
				string displayName = null) { ... }

			DynamicAggregationQuery<T> AggregateBy<T>(
				this IQueryable<T> queryable,
				Expression<Func<T, object>> path) { ... }

			DynamicAggregationQuery<T> AggregateBy<T>(
				this IQueryable<T> queryable,
				Expression<Func<T, object>> path,
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
					var aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.SumOn(x => x.TotalPrice)
						.ToList();

					var customerAggregation = aggregationResults.Results["CustomerId"];
					var sum = customerAggregation.Values[0].Sum;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_3
					// count all orders
					// for each customer
					// where price is higher than 500
					var aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.CountOn(x => x.TotalPrice)
						.ToList();

					var customerAggregation = aggregationResults.Results["CustomerId"];
					var count = customerAggregation.Values[0].Count;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_4
					// count price average for orders
					// for each customer
					// where price is higher than 500
					var aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.AverageOn(x => x.TotalPrice)
						.ToList();

					var customerAggregation = aggregationResults.Results["CustomerId"];
					var average = customerAggregation.Values[0].Average;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_5
					// count max and min price for orders
					// for each customer
					// where price is higher than 500
					var aggregationResults = session.Query<Order>("Orders/All")
						.Where(x => x.TotalPrice > 500)
						.AggregateBy(x => x.CustomerId)
							.MinOn(x => x.TotalPrice)
							.MaxOn(x => x.TotalPrice)
						.ToList();

					var customerAggregation = aggregationResults.Results["CustomerId"];
					var min = customerAggregation.Values[0].Min;
					var max = customerAggregation.Values[0].Max;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_6
					var aggregationResults = session.Query<Order>("Orders/All")
						.AggregateBy(x => x.CustomerId)
							.AddRanges(x => x.TotalPrice < 100)
							.AddRanges(x => x.TotalPrice >= 100 && x.TotalPrice < 500)
							.AddRanges(x => x.TotalPrice >= 500 && x.TotalPrice < 1000)
							.AddRanges(x => x.TotalPrice >= 1000)
						.ToList();

					var customerAggregation = aggregationResults.Results["CustomerId"];
					var range1 = customerAggregation.Values.First(x => x.Range == "[NULL to Dx100]");
					var range2 = customerAggregation.Values.First(x => x.Range == "{Dx100 to Dx500]");
					var range3 = customerAggregation.Values.First(x => x.Range == "{Dx500 to Dx1000]");
					var range4 = customerAggregation.Values.First(x => x.Range == "{Dx1000 to NULL]");
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region aggregate_7
					// sum up all order prices
					// for each customer
					// count price average for all orders
					// for each company
					var aggregationResults = session.Query<Order>("Orders/All")
						.AggregateBy(x => x.CustomerId)
							.SumOn(x => x.TotalPrice)
						.AndAggregateOn(x => x.CompanyId)
							.AverageOn(x => x.TotalPrice)
						.ToList();

					var customerAggregation = aggregationResults.Results["CustomerId"];
					var companyAggregation = aggregationResults.Results["CompanyId"];

					var sum = customerAggregation.Values[0].Sum;
					var average = companyAggregation.Values[0].Average;
					#endregion
				}
			}
		}
	}
}