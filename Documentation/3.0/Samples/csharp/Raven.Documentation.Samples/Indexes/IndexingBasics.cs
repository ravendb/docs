using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	using Abstractions.Indexing;
	using Client.Indexes;

	public class IndexingBasics
	{
		public IndexingBasics()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region indexes_1
					IRavenQueryable<Employee> employees = from employee in session.Query<Employee>("Employees/ByFirstName")
														  where employee.FirstName == "Robert"
														  select employee;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_2
					List<Employee> employees = session
						.Query<Employee>("Employees/ByFirstName")
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_3
					List<Employee> employees = session
						.Advanced
						.DocumentQuery<Employee>("Employees/ByFirstName")
						.WhereEquals(x => x.FirstName, "Robert")
						.ToList();
					#endregion
				}

				#region indexes_4
				store
					.DatabaseCommands
					.Query(
						"Employees/ByFirstName",
						new IndexQuery
						{
							Query = "FirstName:Robert"
						});
				#endregion
			}
		}

		#region raven_by_entity_name
		public class RavenDocumentsByEntityName : AbstractIndexCreationTask
		{
			public override bool IsMapReduce
			{
				get { return false; }
			}

			public override string IndexName
			{
				get { return "Raven/DocumentsByEntityName"; }
			}

			public override IndexDefinition CreateIndexDefinition()
			{
				return new IndexDefinition
				{
					Map = @"from doc in docs 
							select new 
							{ 
								Tag = doc[""@metadata""][""Raven-Entity-Name""], 
								LastModified = (DateTime)doc[""@metadata""][""Last-Modified""],
								LastModifiedTicks = ((DateTime)doc[""@metadata""][""Last-Modified""]).Ticks 
							};",

					Indexes =
					{
						{ "Tag", FieldIndexing.NotAnalyzed },
						{ "LastModified", FieldIndexing.NotAnalyzed },
						{ "LastModifiedTicks", FieldIndexing.NotAnalyzed }
					},

					SortOptions =
					{
						{ "LastModified",SortOptions.String },
						{ "LastModifiedTicks", SortOptions.Long }
					},

					DisableInMemoryIndexing = true,
					LockMode = IndexLockMode.LockedIgnore,
				};
			}
		}
		#endregion
	}
}