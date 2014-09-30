using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class Employees_ByFirstName : AbstractIndexCreationTask<Employee>
	{
	}

	public class Basics
	{
		public Basics()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region basics_0_0
					// load up to 128 entities from 'Employees' collection
					IList<Employee> results = session
						.Query<Employee>()
						.ToList(); // send query
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basics_0_1
					// load up to 128 entities from 'Employees' collection
					// where 'FirstName' is 'Robert'
					IList<Employee> results = session
						.Query<Employee>()
						.Where(x => x.FirstName == "Robert")
						.ToList(); // send query
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basics_0_2
					// load up to 10 entities from 'Products' collection
					// where there are more than 10 units in stock
					// skip first 5 results
					IList<Product> results = session
						.Query<Product>()
						.Where(x => x.UnitsInStock > 10)
						.Skip(5)
						.Take(10)
						.ToList(); // send query
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basics_0_3
					// load up to 128 entities from 'Employees' collection
					// where 'FirstName' is 'Robert'
					// using 'Employees/ByFirstName' index
					IList<Employee> results = session
						.Query<Employee, Employees_ByFirstName>()
						.Where(x => x.FirstName == "Robert")
						.ToList(); // send query
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basics_0_4
					// load up to 128 entities from 'Employees' collection
					// where 'FirstName' is 'Robert'
					// using 'Employees/ByFirstName' index
					IList<Employee> results = session
						.Query<Employee>("Employees/ByFirstName")
						.Where(x => x.FirstName == "Robert")
						.ToList(); // send query
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region basics_1_0
					// load up to 128 entities from 'Employees' collection
					// where 'FirstName' is 'Robert'
					// using 'Employees/ByFirstName' index
					IList<Employee> results = session
						.Advanced
						.DocumentQuery<Employee, Employees_ByFirstName>()
						.WhereEquals(x => x.FirstName, "Robert")
						.ToList(); // send query
					#endregion
				}
			}
		}
	}
}