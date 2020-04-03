using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	using System.Linq;

	public class FanoutIndexes
	{
		#region fanout_index_def_1

		public class Orders_ByProduct : AbstractIndexCreationTask<Order>
		{
			public Orders_ByProduct()
			{
				Map = orders => from order in orders
					from orderLine in order.Lines
					select new
					{
						orderLine.Product,
						orderLine.ProductName
					};
			}
		}
		#endregion

		#region fanout_index_def_2

		public class Product_Sales : AbstractIndexCreationTask<Order, Product_Sales.Result>
		{
			public class Result
			{
				public string Product { get; set; }
				public int Count { get; set; }
				public decimal Total;
			}

			public Product_Sales()
			{
				Map = orders => from order in orders
					from line in order.Lines
					select new Result
					{
						Product = line.Product,
						Count = 1,
						Total = ((line.Quantity*line.PricePerUnit)*(1 - line.Discount))
					};

				Reduce = results => from result in results
					group result by result.Product
					into g
					select new
					{
						Product = g.Key,
						Count = g.Sum(x => x.Count),
						Total = g.Sum(x => x.Total)
					};
			}
		}
		#endregion
	}
}
