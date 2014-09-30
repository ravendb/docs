using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	#region map_reduce_0_0
	public class Products_ByCategory : AbstractIndexCreationTask<Product, Products_ByCategory.Result>
	{
		public class Result
		{
			public string Category { get; set; }

			public int Count { get; set; }
		}

		public Products_ByCategory()
		{
			Map = products => from product in products
						let categoryName = LoadDocument<Category>(product.Category).Name
						select new
						{
							Category = categoryName,
							Count = 1
						};

			Reduce = results => from result in results
						group result by result.Category into g
						select new
						{
							Category = g.Key,
							Count = g.Sum(x => x.Count)
						};
		}
	}
	#endregion

	public class MapReduceIndexes
	{
		public MapReduceIndexes()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region map_reduce_0_1
					IList<Products_ByCategory.Result> results = session
						.Query<Products_ByCategory.Result, Products_ByCategory>()
						.Where(x => x.Category == "Seafood")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region map_reduce_0_2
					IList<Products_ByCategory.Result> results = session
						.Advanced
						.DocumentQuery<Products_ByCategory.Result, Products_ByCategory>()
						.WhereEquals(x => x.Category, "Seafood")
						.ToList();
					#endregion
				}
			}
		}
	}
}