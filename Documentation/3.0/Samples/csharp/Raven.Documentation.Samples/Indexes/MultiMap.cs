using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	public class MultiMap
	{
		#region multi_map_1
		public class Dog : Animal
		{

		}
		#endregion

		#region multi_map_2
		public class Cat : Animal
		{

		}
		#endregion

		#region multi_map_3
		public abstract class Animal : IAnimal
		{
			public string Name { get; set; }
		}
		#endregion

		#region multi_map_6
		public interface IAnimal
		{
			string Name { get; set; }
		}
		#endregion

		#region multi_map_4
		public class Animals_ByName : AbstractMultiMapIndexCreationTask
		{
			public Animals_ByName()
			{
				AddMap<Cat>(cats => from c in cats select new { c.Name });

				AddMap<Dog>(dogs => from d in dogs select new { d.Name });
			}
		}
		#endregion

		#region multi_map_5
		public class Animals_ByName_ForAll : AbstractMultiMapIndexCreationTask
		{
			public Animals_ByName_ForAll()
			{
				AddMapForAll<Animal>(parents => from p in parents select new { p.Name });
			}
		}
		#endregion

		#region multi_map_1_0
		public class Smart_Search : AbstractMultiMapIndexCreationTask<Smart_Search.Result>
		{
			public class Result
			{
				public string Id { get; set; }

				public string DisplayName { get; set; }

				public string Collection { get; set; }

				public string Content { get; set; }
			}

			public class Projection
			{
				public string Id { get; set; }

				public string DisplayName { get; set; }

				public string Collection { get; set; }
			}

			public Smart_Search()
			{
				AddMap<Company>(companies => from c in companies
								select new
								{
									c.Id,
									Content = new[]
									{
										c.Name
									},
									DisplayName = c.Name,
									Collection = MetadataFor(c)["Raven-Entity-Name"]
								});

				AddMap<Product>(products => from p in products
								select new
								{
									p.Id,
									Content = new[]
									{
										p.Name
									},
									DisplayName = p.Name,
									Collection = MetadataFor(p)["Raven-Entity-Name"]
								});

				AddMap<Employee>(employees => from e in employees
								select new
								{
									e.Id,
									Content = new[]
									{
										e.FirstName, 
										e.LastName
									},
									DisplayName = e.FirstName + " " + e.LastName,
									Collection = MetadataFor(e)["Raven-Entity-Name"]
								});

				// mark 'Content' field as analyzed which enables full text search operations
				Index(x => x.Content, FieldIndexing.Analyzed);

				// storing fields so when projection (e.g. ProjectFromIndexFieldsInto)
				// requests only those fields
				// then data will come from index only, not from storage
				Store(x => x.Id, FieldStorage.Yes);
				Store(x => x.DisplayName, FieldStorage.Yes);
				Store(x => x.Collection, FieldStorage.Yes);
			}
		}
		#endregion

		public MultiMap()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region multi_map_7
					IList<IAnimal> results = session
						.Query<IAnimal, Animals_ByName>()
						.Where(x => x.Name == "Mitzy")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region multi_map_8
					IList<IAnimal> results = session
						.Advanced
						.DocumentQuery<IAnimal, Animals_ByName>()
						.WhereEquals(x => x.Name, "Mitzy")
						.ToList();
					#endregion
				}

				#region multi_map_9
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Animals/ByName",
						new IndexQuery
						{
							Query = "Name:Mitzy"
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region multi_map_1_1
					IList<Smart_Search.Projection> results = session
						.Query<Smart_Search.Result, Smart_Search>()
						.Search(x => x.Content, "Lau*", escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
						.ProjectFromIndexFieldsInto<Smart_Search.Projection>()
						.ToList();

					foreach (var result in results)
					{
						Console.WriteLine(result.Collection + ": " + result.DisplayName);
						// Companies: Laughing Bacchus Wine Cellars
						// Products: Laughing Lumberjack Lager
						// Employees: Laura Callahan
					}
					#endregion
				}
			}
		}
	}
}