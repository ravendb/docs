using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	#region boosting_2
	public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
	{
		public Employees_ByFirstAndLastName()
		{
			Map =
				employees =>
				from employee in employees
				select new
				{
					FirstName = employee.FirstName.Boost(10),
					LastName = employee.LastName
				};
		}
	}

	#endregion

	public class Boosting
	{
		public Boosting()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region boosting_3
					// employees with 'FirstName' equal to 'Bob'
					// will be higher in results
					// than the ones with 'LastName' match
					IList<Employee> results = session
						.Query<Employee, Employees_ByFirstAndLastName>()
						.Where(x => x.FirstName == "Bob" || x.LastName == "Bob")
						.ToList();
					#endregion
				}

				#region boosting_4
				store
					.DatabaseCommands
					.PutIndex(
						"Employees/ByFirstAndLastName",
						new IndexDefinition
						{
							Map = @"from employee in docs.Employees
								select new
								{
									FirstName = employee.FirstName.Boost(10),
									LastName = employee.LastName
								}"
						});
				#endregion
			}
		}
	}
}