namespace RavenCodeSamples.ClientApi.Querying.ResultsTransformation
{
	using System.Linq;
	using Raven.Abstractions.Indexing;
	using Raven.Client.Indexes;
	
	public class ProductDifferentPropNameViewModel
	{
		public string DifferentName { get; set; }
	}

	#region index_def
	public class Product_ByName : AbstractIndexCreationTask<Product>
	{
		public Product_ByName()
		{
			Map = products => from product in products
							  select new
							  {
								  Name = product.Name
							  };

			Stores.Add(x => x.Name, FieldStorage.Yes);
			Stores.Add(x => x.Description, FieldStorage.Yes);
		}
	}
	#endregion

	public class SelectFields : CodeSampleBase
	{
		public SelectFields()
		{
			using (var store = NewDocumentStore())
			{
				store.ExecuteIndex(new Product_ByName());

				using (var session = store.OpenSession())
				{
					#region select_fields_1
					var results =
						session.Advanced.LuceneQuery<Product>("Product/ByName")
						       .SelectFields<ProductViewModel>()
						       .WhereEquals(x => x.Name, "Raven")
						       .ToList();
					#endregion

					#region select_fields_2
					var resultsWithNameOnly =
						session.Advanced.LuceneQuery<Product>("Product/ByName")
							   .SelectFields<ProductViewModel>("Name")
							   .WhereEquals(x => x.Name, "Raven")
							   .ToList();
					#endregion
				}
			}
		} 
	}
}