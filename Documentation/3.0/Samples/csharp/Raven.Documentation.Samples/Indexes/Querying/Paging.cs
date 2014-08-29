using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class Paging
	{
		#region paging_0_4
		public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
		{
			public Products_ByUnitsInStock()
			{
				Map = products => from product in products
								  select new
									{
										UnitsInStock = product.UnitsInStock
									};
			}
		}
		#endregion

		public Paging()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_0_1
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()
						.Where(x => x.UnitsInStock > 10)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_0_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.ToList();
					#endregion
				}

				#region paging_0_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock:{Ix10 TO NULL}"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_1_1
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()
						.Where(x => x.UnitsInStock > 10)
						.Take(9999)	// server will decrease this value to 1024
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_1_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.Take(9999)	// server will decrease this value to 1024
						.ToList();
					#endregion
				}

				#region paging_1_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock:{Ix10 TO NULL}",
							PageSize = 9999
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_2_1
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()
						.Where(x => x.UnitsInStock > 10)
						.Skip(20)	// skip 2 pages worth of products
						.Take(10)	// take up to 10 products
						.ToList();	// execute query
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_2_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.Skip(20)	// skip 2 pages worth of products
						.Take(10)	// take up to 10 products
						.ToList();	// execute query
					#endregion
				}

				#region paging_2_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock:{Ix10 TO NULL}",
							Start = 20,	// skip 2 pages worth of products
							PageSize = 10	// take up to 10 products
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_1
					// assuming a page size of 10, this is how will retrieve the 3rd page:
					var results = session
						.Query<Order>()
						.Skip(20) 
						.Take(10) // Take posts in the page size
						.ToArray(); // execute the query
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_2
					RavenQueryStatistics stats;
					var results = session
						.Query<Order>()
						.Statistics(out stats)
						.Where(x => x.Freight > 10)
						.Take(10)
						.ToArray();

					var totalResults = stats.TotalResults;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_3
					RavenQueryStatistics stats;

					// get the first page
					var results = session
						.Query<BlogPost>()
						.Statistics(out stats)
						.Skip(0 * 10) // retrieve results for the first page
						.Take(10) // page size is 10
						.Where(x => x.Category == "RavenDB")
						.Distinct()
						.ToArray();

					var totalResults = stats.TotalResults;
					var skippedResults = stats.SkippedResults;

					// get the second page
					results = session
						.Query<BlogPost>()
						.Statistics(out stats)
						.Skip((1 * 10) + skippedResults) // retrieve results for the second page, taking into account skipped results
						.Take(10) // page size is 10
						.Where(x => x.Category == "RavenDB")
						.Distinct()
						.ToArray();

					// and so on...
					#endregion
				}
			}
		}
	}
}
