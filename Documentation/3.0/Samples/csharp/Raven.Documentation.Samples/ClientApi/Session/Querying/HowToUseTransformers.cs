using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
	public class HowToUseTransformers
	{
		private class Products_Name : AbstractTransformerCreationTask<Product>
		{
		}

		#region transformers_5
		public class ProductWithCategoryAndSupplier
		{
			public string Name { get; set; }

			public Supplier Supplier { get; set; }

			public Category Category { get; set; }
		}
		#endregion

		#region transformers_4
		public class Products_WithCategoryAndSupplier : AbstractTransformerCreationTask<Product>
		{
			public Products_WithCategoryAndSupplier()
			{
				TransformResults =
					products => from product in products 
								select new
								{
									Name = product.Name,
									Category = LoadDocument<Category>(product.Category),
									Supplier = LoadDocument<Supplier>(product.Supplier)
								};
			}
		}
		#endregion

		private interface IFoo
		{
			#region transformers_1
			IRavenQueryable<TResult> TransformWith<TTransformer, TResult>()
				where TTransformer : AbstractTransformerCreationTask, new();

			IRavenQueryable<TResult> TransformWith<TResult>(string transformerName);
			#endregion
		}

		public HowToUseTransformers()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_2
					// return up to 128 entities from 'Products' collection
					// transform results using 'Products_Name' transformer
					// which returns only 'Name' property, rest will be 'null'
					List<Product> results = session
						.Query<Product>()
						.Where(x => x.Name == "Chocolade")
						.TransformWith<Products_Name, Product>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_3
					// return 1 entity from 'Products' collection
					// transform results using 'Products_WithCategoryAndSupplier' transformer
					// project results to 'ProductWithCategoryAndSupplier' class
					ProductWithCategoryAndSupplier product = session
						.Query<Product>()
						.Where(x => x.Name == "Chocolade")
						.TransformWith<Products_WithCategoryAndSupplier, ProductWithCategoryAndSupplier>()
						.First();

					Assert.Equal("Chocolade", product.Name);
					Assert.Equal("Confections", product.Category.Name);
					Assert.Equal("Zaanse Snoepfabriek", product.Supplier.Name);
					#endregion
				}
			}
		}
	}
}