namespace RavenCodeSamples.ClientApi.Querying.ResultsTransformation
{
	using System.Collections.Generic;
	using System.Linq;
	using Raven.Client.Indexes;

	#region product_item_class
	public class ProductItem
	{
		public string Id { get; set; }
		public string WarehouseId { get; set; }
		public string Name { get; set; }
		public string Manufacturer { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
	}
	#endregion

	#region product_item_view_model
	public class ProductItemViewModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
	}
	#endregion

	#region warehouse_class
	public class Warehouse
	{
		public string Id { get; set; }
		public IList<ProductItemViewModel> Products { get; set; }
		public double AverageProductPrice { get; set; }
	}
	#endregion

	#region index_def
	public class Product_ById : AbstractIndexCreationTask<ProductItem>
	{
		public Product_ById()
		{
			Map = products => from product in products
							  select new
							  {
								  product.Id
							  };
		}
	}
	#endregion

	public class TransformResults : CodeSampleBase
	{
		public TransformResults()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transform_to_warehouses
					var warehouses = session.Query<dynamic, Product_ById>()
						.Customize(x => x.TransformResults((query, results) =>
							results.Cast<dynamic>().GroupBy(p => p.WarehouseId).Select(g =>
							{
								double count = 0;
								int totalSum = 0;

								var products = g.Select(product =>
								{
									count++;
									totalSum += product.Price;
									return new ProductItemViewModel
											   {
												   Name = product.Name,
												   Description = product.Description
											   };
								}).ToList();

								return new Warehouse()
										   {
											   Id = g.Key,
											   Products = products,
											   AverageProductPrice = totalSum / count,
										   };
							}))).ToList();
					#endregion
				}
			}
		}
	}
}