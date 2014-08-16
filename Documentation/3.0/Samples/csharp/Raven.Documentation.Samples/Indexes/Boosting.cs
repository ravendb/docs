using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.Indexes
{
	#region boosting_2
	public class Employees_ByName : AbstractIndexCreationTask<Employee>
	{
		public Employees_ByName()
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
					session.Query<Employee, Employees_ByName>()
						   .Where(x => x.FirstName == "Bob" || x.LastName == "Bob")
						   .ToList();
					#endregion
				}
			}
		}
	}
}