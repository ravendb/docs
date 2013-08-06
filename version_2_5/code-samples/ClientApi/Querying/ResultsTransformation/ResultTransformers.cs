namespace RavenCodeSamples.ClientApi.Querying.ResultsTransformation
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Client.Indexes;

	using RavenCodeSamples.Server.Extending.Bundles.Foo;

	public enum Status
	{
	}

	#region result_transformers_0
	public class Order
	{
		public DateTime OrderedAt { get; set; }

		public Status Status { get; set; }

		public string CustomerId { get; set; }

		public IList<OrderLine> Lines { get; set; }
	}

	#endregion

	#region result_transformers_2
	public class OrderStatistics
	{
		public DateTime OrderedAt { get; set; }

		public Status Status { get; set; }

		public string CustomerId { get; set; }

		public string CustomerName { get; set; }

		public int LinesCount { get; set; }
	}

	#endregion

	#region result_transformers_1
	public class OrderStatisticsTransformer : AbstractTransformerCreationTask<Order>
	{
		public OrderStatisticsTransformer()
		{
			TransformResults = orders => from order in orders
										 select new
												{
													order.OrderedAt,
													order.Status,
													order.CustomerId,
													CustomerName = LoadDocument<Customer>(order.CustomerId).Name,
													LinesCount = order.Lines.Count
												};
		}
	}

	#endregion

	public class ResultTransformers : CodeSampleBase
	{
		public void Method()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region result_transformers_3

					IList<Order> orders = session.Query<Order>()
						.Where(x => x.CustomerId == "customers/1")
						.ToList();

					#endregion

					#region result_transformers_4
					IList<OrderStatistics> statistics = session.Query<Order>()
						.TransformWith<OrderStatisticsTransformer, OrderStatistics>()
						.Where(x => x.CustomerId == "customers/1")
						.ToList();

					#endregion

					#region result_transformers_5
					OrderStatistics statistic = session.Load<OrderStatisticsTransformer, OrderStatistics>("orders/1");

					#endregion
				}
			}
		}
	}
}