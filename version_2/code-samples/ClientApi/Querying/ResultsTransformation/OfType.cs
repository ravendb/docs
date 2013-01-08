namespace RavenCodeSamples.ClientApi.Querying.ResultsTransformation
{
	using System.Collections.Generic;
	using System.Linq;
	using Raven.Client.Indexes;

	#region linq_extensions_of_type_product_by_quantity_index
	public class Product_ByQuantity : AbstractIndexCreationTask<Product>
	{
		public Product_ByQuantity()
		{
			Map = products => from product in products
							  select new
							  {
								  QuantityInWarehouse = product.QuantityInWarehouse
							  };

			TransformResults = (database, results) => from r in results
													  select new
													  {
														  Name = r.Name,
														  Description = r.Description
													  };
		}
	}
	#endregion

	public class OfType : CodeSampleBase
	{
		public OfType()
		{
			using (var documentStore = NewDocumentStore())
			{
				new Product_ByQuantity().Execute(documentStore);

				using (var session = documentStore.OpenSession())
				{
					#region linq_extensions_of_type_of_type_query

					List<ProductViewModel> products = session.Query<Product, Product_ByQuantity>()
					                                         .Where(x => x.QuantityInWarehouse > 100)
					                                         .OfType<ProductViewModel>()
					                                         .ToList();

					#endregion
				}
			}
		}
	}
}
