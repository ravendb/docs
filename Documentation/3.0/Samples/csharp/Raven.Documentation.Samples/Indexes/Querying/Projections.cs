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
	#region projections_3_3
	public class EmployeeFirstAndLastName
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
	#endregion

	#region projections_1_3
	public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
	{
		public Employees_ByFirstAndLastName()
		{
			Map = employees => from employee in employees
							   select new
								{
									FirstName = employee.FirstName,
									LastName = employee.LastName
								};
		}
	}
	#endregion

	#region projections_2_3
	public class Employees_ByFirstAndLastNameWithStoredFields : AbstractIndexCreationTask<Employee>
	{
		public Employees_ByFirstAndLastNameWithStoredFields()
		{
			Map = employees => from employee in employees
							   select new
							   {
								   FirstName = employee.FirstName,
								   LastName = employee.LastName
							   };

			Store(x => x.FirstName, FieldStorage.Yes);
			Store(x => x.LastName, FieldStorage.Yes);
		}
	}
	#endregion

	public class Projections
	{
		public Projections()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region projections_1_0
					var results = session
						.Query<Employee, Employees_ByFirstAndLastName>()
						.Select(x => new
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projections_1_1
					var results = session
						.Advanced
						.DocumentQuery<Employee, Employees_ByFirstAndLastName>()
						.Select(x => new
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				#region projections_1_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Employees/ByFirstAndLastName",
						new IndexQuery
							{
								FieldsToFetch = new[]
								{
									"FirstName", 
									"LastName"
								}
							});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region projections_2_0
					var results = session
						.Query<Employee, Employees_ByFirstAndLastNameWithStoredFields>()
						.Select(x => new
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projections_2_1
					var results = session
						.Advanced
						.DocumentQuery<Employee, Employees_ByFirstAndLastNameWithStoredFields>()
						.Select(x => new
						{
							FirstName = x.FirstName,
							LastName = x.LastName
						})
						.ToList();
					#endregion
				}

				#region projections_2_2
				QueryResult result = store
					.DatabaseCommands
					.Query(
						"Employees/ByFirstAndLastNameWithStoredFields",
						new IndexQuery
						{
							FieldsToFetch = new[]
								{
									"FirstName", 
									"LastName"
								}
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region projections_3_0
					IList<EmployeeFirstAndLastName> results = session
						.Query<Employee, Employees_ByFirstAndLastName>()
						.ProjectFromIndexFieldsInto<EmployeeFirstAndLastName>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region projections_3_1
					IList<EmployeeFirstAndLastName> results = session
						.Advanced
						.DocumentQuery<Employee, Employees_ByFirstAndLastName>()
						.SelectFields<EmployeeFirstAndLastName>()
						.ToList();
					#endregion
				}
			}
		}
	}
}