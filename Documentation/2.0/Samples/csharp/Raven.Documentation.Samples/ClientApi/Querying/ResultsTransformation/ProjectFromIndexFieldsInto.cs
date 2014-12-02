namespace RavenCodeSamples.ClientApi.Querying.ResultsTransformation
{
	using System.Collections.Generic;
	using System.Linq;
	using Raven.Abstractions.Indexing;
	using Raven.Client;
	using Raven.Client.Indexes;

	#region index_def
	public class Product_ByQuantityNameAndDescription : AbstractIndexCreationTask<Product>
	{
		public Product_ByQuantityNameAndDescription()
		{
			Map = products => from product in products
							  select new
							  {
								  QuantityInWarehouse = product.QuantityInWarehouse,
								  Name = product.Name,
								  Description = product.Description
							  };

			Stores.Add(x => x.Name, FieldStorage.Yes);
			Stores.Add(x => x.Description, FieldStorage.Yes);
		}
	}
	#endregion

	public class ProjectFromIndexFieldsInto : CodeSampleBase
	{
		public ProjectFromIndexFieldsInto()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region query

					List<ProductViewModel> products = session.Query<Product, Product_ByQuantityNameAndDescription>()
					                                         .Where(x => x.QuantityInWarehouse > 50)
					                                         .ProjectFromIndexFieldsInto<ProductViewModel>()
					                                         .ToList();

					#endregion
				}
			}
		}
	}
}
