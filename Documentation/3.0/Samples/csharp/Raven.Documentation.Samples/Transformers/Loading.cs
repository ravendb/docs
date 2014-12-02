using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Transformers
{
	public class Loading
	{
		#region transformers_1
		public class Products_ProductAndCategoryName : AbstractTransformerCreationTask<Product>
		{
			public class Result
			{
				public string ProductName { get; set; }

				public string CategoryName { get; set; }
			}

			public Products_ProductAndCategoryName()
			{
				TransformResults =
					products =>
					from product in products
					let category = LoadDocument<Category>(product.Category)
					select new
					{
						ProductName = product.Name,
						CategoryName = category.Name
					};
			}
		}
		#endregion

		public Loading()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_2
					IList<Products_ProductAndCategoryName.Result> results = session
						.Query<Product>()
						.Where(x => x.Name == "Chocolade")
						.TransformWith<Products_ProductAndCategoryName, Products_ProductAndCategoryName.Result>()
						.ToList();
					#endregion
				}
			}
		}
	}
}