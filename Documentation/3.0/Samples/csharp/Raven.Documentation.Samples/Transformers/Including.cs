using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

using Xunit;

namespace Raven.Documentation.Samples.Transformers
{
	public class Including
	{
		#region transformers_1
		public class Products_NameCategoryAndSupplier : AbstractTransformerCreationTask<Product>
		{
			public class Result
			{
				public string Name { get; set; }

				public string Category { get; set; }

				public string Supplier { get; set; }
			}

			public Products_NameCategoryAndSupplier()
			{
				TransformResults =
					products =>
					from product in products
					let supplier = Include<Supplier>(product.Supplier)
					let category = Include<Category>(product.Category)
					select new
					{
						Name = product.Name,
						Category = product.Category,
						Supplier = product.Supplier
					};
			}
		}
		#endregion

		public Including()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region transformers_2
					Products_NameCategoryAndSupplier.Result result = session
						.Query<Product>()
						.Where(x => x.Name == "Chocolade")
						.TransformWith<Products_NameCategoryAndSupplier, Products_NameCategoryAndSupplier.Result>()
						.First();

					var numberOfRequests = session.Advanced.NumberOfRequests;

					var category = session.Load<Category>(result.Category); // no server call
					var supplier = session.Load<Supplier>(result.Supplier); // no server call

					Assert.Equal(numberOfRequests, session.Advanced.NumberOfRequests);
					#endregion
				}
			}
		}
	}
}