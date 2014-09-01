using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class Sorting
	{
		#region sorting_1_4
		public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
		{
			public Products_ByUnitsInStock()
			{
				Map = products => from product in products
								  select new
									{
										product.UnitsInStock
									};

				Sort(x => x.UnitsInStock, SortOptions.Int);
			}
		}
		#endregion

		public class Products_ByName : AbstractIndexCreationTask<Product>
		{
			public Products_ByName()
			{
				Map = products => from product in products
								  select new
									{
										product.Name
									};
			}
		}

		public Sorting()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region sorting_1_1
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()
						.Where(x => x.UnitsInStock > 10)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region sorting_1_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.ToList();
					#endregion
				}

				#region sorting_1_3
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
					#region sorting_2_1
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()
						.Where(x => x.UnitsInStock > 10)
						.OrderByDescending(x => x.UnitsInStock)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region sorting_2_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.OrderByDescending(x => x.UnitsInStock)
						.ToList();
					#endregion
				}

				#region sorting_2_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock:{Ix10 TO NULL}",
							SortedFields = new[]
							{
								new SortedField("-UnitsInStock")
							}
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region sorting_3_1
					IList<Product> results = session
						.Query<Product, Products_ByUnitsInStock>()
						.Customize(x => x.RandomOrdering())
						.Where(x => x.UnitsInStock > 10)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region sorting_3_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.RandomOrdering()
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.ToList();
					#endregion
				}

				#region sorting_3_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock:{Ix10 TO NULL}",
							SortedFields = new[]
							{
								new SortedField(Constants.RandomFieldName + ";" + Guid.NewGuid())
							}
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region sorting_4_1
					IList<Product> results = session
						.Query<Product, Products_ByName>()
						.Where(x => x.UnitsInStock > 10)
						.OrderByScore()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region sorting_4_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByUnitsInStock>()
						.WhereGreaterThan(x => x.UnitsInStock, 10)
						.OrderByScore()
						.ToList();
					#endregion
				}

				#region sorting_4_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByUnitsInStock",
						new IndexQuery
						{
							Query = "UnitsInStock:{Ix10 TO NULL}",
							SortedFields = new[]
							{
								new SortedField(Constants.TemporaryScoreValue) // Temp-Index-Score
							}
						});
				#endregion
			}
		}
	}
}