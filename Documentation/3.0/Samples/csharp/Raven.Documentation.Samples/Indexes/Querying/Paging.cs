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

		#region paging_6_0
		public class Orders_ByOrderLines_ProductName : AbstractIndexCreationTask<Order>
		{
			public Orders_ByOrderLines_ProductName()
			{
				Map = orders => from order in orders
								from line in order.Lines
								select new
								{
									Product = line.ProductName
								};

				MaxIndexOutputsPerDocument = 1024;
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
							Query = "UnitsInStock_Range:{Ix10 TO NULL}"
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
							Query = "UnitsInStock_Range:{Ix10 TO NULL}",
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
							Query = "UnitsInStock_Range:{Ix10 TO NULL}",
							Start = 20,	// skip 2 pages worth of products
							PageSize = 10	// take up to 10 products
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_3_1
					RavenQueryStatistics stats;
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()
						.Statistics(out stats)				// fill query statistics
						.Where(x => x.UnitsInStock > 10)
						.Skip(20)
						.Take(10)
						.ToList();

					var totalResults = stats.TotalResults;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_3_2
					RavenQueryStatistics stats;
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.Statistics(out stats)					// fill query statistics
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.Skip(20)
						.Take(10)
						.ToList();

					var totalResults = stats.TotalResults;
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region paging_3_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock_Range:{Ix10 TO NULL}",
							Start = 20,
							PageSize = 10
						});

				var totalResults = result.TotalResults;
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_4_1
					RavenQueryStatistics stats;
					IList<Product> results;
					var pageNumber = 0;
					var pageSize = 10;
					var skippedResults = 0;

					do
					{
						results = session
							.Query<Product, Products_ByUnitsInStock>()
							.Statistics(out stats)
							.Skip((pageNumber * pageSize) + skippedResults)
							.Take(pageSize)
							.Where(x => x.UnitsInStock > 10)
							.Distinct()
							.ToList();

						skippedResults += stats.SkippedResults;
						pageNumber++;
					}
					while (results.Count > 0);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_4_2
					RavenQueryStatistics stats;
					IList<Product> results;
					var pageNumber = 0;
					var pageSize = 10;
					var skippedResults = 0;

					do
					{
						results = session
							.Advanced
							.DocumentQuery<Product, Products_ByUnitsInStock>()
							.Statistics(out stats)
							.Skip((pageNumber * pageSize) + skippedResults)
							.Take(pageSize)
							.WhereGreaterThan(x => x.UnitsInStock, 10)
							.Distinct()
							.ToList();

						skippedResults += stats.SkippedResults;
						pageNumber++;
					}
					while (results.Count > 0);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region paging_4_3
				QueryResult result;
				var pageNumber = 0;
				var pageSize = 10;
				var skippedResults = 0;

				do
				{
					result = store
						.DatabaseCommands
						.Query(
							"Products/ByUnitsInStock",
							new IndexQuery
							{
								Query = "UnitsInStock_Range:{Ix10 TO NULL}",
								Start = (pageNumber * pageSize) + skippedResults,
								PageSize = pageSize,
								IsDistinct = true
							});

					skippedResults += result.SkippedResults;
					pageNumber++;
				}
				while (result.Results.Count > 0);
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_6_1
					RavenQueryStatistics stats;
					IList<Order> results;
					var pageNumber = 0;
					var pageSize = 10;
					var skippedResults = 0;

					do
					{
						results = session
							.Query<Order, Orders_ByOrderLines_ProductName>()
							.Statistics(out stats)
							.Skip((pageNumber * pageSize) + skippedResults)
							.Take(pageSize)
							.ToList();

						skippedResults += stats.SkippedResults;
						pageNumber++;
					}
					while (results.Count > 0);
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region paging_6_2
					RavenQueryStatistics stats;
					IList<Order> results;
					var pageNumber = 0;
					var pageSize = 10;
					var skippedResults = 0;

					do
					{
						results = session
							.Advanced
							.DocumentQuery<Order, Orders_ByOrderLines_ProductName>()
							.Statistics(out stats)
							.Skip((pageNumber * pageSize) + skippedResults)
							.Take(pageSize)
							.ToList();

						skippedResults += stats.SkippedResults;
						pageNumber++;
					}
					while (results.Count > 0);
					#endregion
				}
			}

			using (var store = new DocumentStore())
			{
				#region paging_6_3
				QueryResult result;
				var pageNumber = 0;
				var pageSize = 10;
				var skippedResults = 0;

				do
				{
					result = store
						.DatabaseCommands
						.Query(
							"Orders/ByOrderLines/ProductName",
							new IndexQuery
							{
								Start = (pageNumber * pageSize) + skippedResults,
								PageSize = pageSize
							});

					skippedResults += result.SkippedResults;
					pageNumber++;
				}
				while (result.Results.Count > 0);
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region paging_5_1
					var pagingInformation = new RavenPagingInformation();
					IList<Product> results = session
						.Advanced
						.LoadStartingWith<Product>(
							"products/",				// all documents starting with 'products/'
							"1*|2*",				// rest of the key must begin with "1" or "2" e.g. products/10, products/25
							0 * 25,					// skip 0 records (page 1)
							25,					// take up to 25
							pagingInformation: pagingInformation);	// fill `RavenPagingInformation` with operation data

					results = session
						.Advanced
						.LoadStartingWith<Product>(
							"products/",				// all documents starting with 'products/'
							"1*|2*",				// rest of the key must begin with "1" or "2" e.g. products/10, products/25
							1 * 25,					// skip 25 records (page 2)
							25,					// take up to 25
							pagingInformation: pagingInformation);	// since this is a next page to 'page 1' and we are passing 'RavenPagingInformation' that was filled during 'page 1' retrieval, rapid pagination will take place
					#endregion
				}
			}
		}
	}
}
