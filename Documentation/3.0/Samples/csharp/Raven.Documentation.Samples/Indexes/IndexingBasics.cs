using System.Linq;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	public class IndexingBasics
	{
		public IndexingBasics()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region indexes_1
					var employees = from employee in session.Query<Employee>("Employees/ByFirstName")
									where employee.FirstName == "Robert"
									select employee;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_2
					var employees = session
						.Query<Employee>("Employees/ByFirstName")
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_3
					var employees = session
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
	}
}