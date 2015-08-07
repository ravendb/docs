using System;
using System.Collections.Generic;
using System.Linq;

using Lucene.Net.Index;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class Sorting
	{
		#region sorting_5_1
		public abstract class IndexEntriesToComparablesGenerator
		{
			protected IndexQuery IndexQuery;

			protected IndexEntriesToComparablesGenerator(IndexQuery indexQuery)
			{
				IndexQuery = indexQuery;
			}

			public abstract IComparable Generate(IndexReader reader, int doc);
		}
		#endregion

		#region sorting_5_2
		public class SortByNumberOfCharactersFromEnd : IndexEntriesToComparablesGenerator
		{
			private readonly int len;

			public SortByNumberOfCharactersFromEnd(IndexQuery indexQuery)
				: base(indexQuery)
			{
				len = IndexQuery.TransformerParameters["len"].Value<int>();		// using transformer parameters to pass the length explicitly
			}

			public override IComparable Generate(IndexReader reader, int doc)
			{
				var document = reader.Document(doc);
				var name = document.GetField("FirstName").StringValue;			// this field is stored in index
				return name.Substring(name.Length - len, len);
			}
		}
		#endregion

		#region sorting_5_6
		public class Employee_ByFirstName : AbstractIndexCreationTask<Employee>
		{
			public Employee_ByFirstName()
			{
				Map = employees => from employee in employees
								   select new
									{
										employee.FirstName
									};

				Store(x => x.FirstName, FieldStorage.Yes);
			}
		}
		#endregion

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

		#region sorting_6_4
		public class Products_ByName_Search : AbstractIndexCreationTask<Product>
		{
			public class Result
			{
				public string Name { get; set; }

				public string NameForSorting { get; set; }
			}

			public Products_ByName_Search()
			{
				Map = products => from product in products
								  select new
								  {
									  Name = product.Name,
									  NameForSorting = product.Name
								  };

				Indexes.Add(x => x.Name, FieldIndexing.Analyzed);
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
							Query = "UnitsInStock_Range:{Ix10 TO NULL}"
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
							Query = "UnitsInStock_Range:{Ix10 TO NULL}",
							SortedFields = new[]
							{
								new SortedField("-UnitsInStock_Range")
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
							Query = "UnitsInStock_Range:{Ix10 TO NULL}",
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
							Query = "UnitsInStock_Range:{Ix10 TO NULL}",
							SortedFields = new[]
							{
								new SortedField(Constants.TemporaryScoreValue) // Temp-Index-Score
							}
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region sorting_5_3
					IList<Employee> results = session
						.Query<Employee, Employee_ByFirstName>()
						.Customize(x => x.CustomSortUsing(typeof(SortByNumberOfCharactersFromEnd).AssemblyQualifiedName, descending: true))
						.AddTransformerParameter("len", 1)
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region sorting_5_4
					IList<Employee> results = session
						.Advanced
						.DocumentQuery<Employee, Employee_ByFirstName>()
						.CustomSortUsing(typeof(SortByNumberOfCharactersFromEnd).AssemblyQualifiedName, descending: true)
						.SetTransformerParameters(new Dictionary<string, RavenJToken> { { "len", 1 } })
						.ToList();
					#endregion
				}

				#region sorting_5_5
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Employees/ByFirstName",
						new IndexQuery
						{
							SortedFields = new[]
							{
								new SortedField(
									Constants.CustomSortFieldName +
									"-" + // "-" - descending, "" - ascending
									";" + 
									typeof(SortByNumberOfCharactersFromEnd).AssemblyQualifiedName)
							},
							TransformerParameters = new Dictionary<string, RavenJToken>
							{
								{ "len", 1 }
							}
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region sorting_6_1
					IList<Product> results = session
						.Query< Products_ByName_Search.Result, Products_ByName_Search>()
						.Search(x => x.Name, "Louisiana")
						.OrderByDescending(x => x.NameForSorting)
						.OfType<Product>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region sorting_6_2
					IList<Product> results = session
						.Advanced
						.DocumentQuery<Product, Products_ByName_Search>()
						.Search("Name", "Louisiana")
						.OrderByDescending("NameForSorting")
						.ToList();
					#endregion
				}

				#region sorting_6_3
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Products/ByName/Search",
						new IndexQuery
						{
							Query = "Name:Louisiana*",
							SortedFields = new[]
							{
								new SortedField("-NameForSorting")
							}
						});
				#endregion
			}
		}
	}
}