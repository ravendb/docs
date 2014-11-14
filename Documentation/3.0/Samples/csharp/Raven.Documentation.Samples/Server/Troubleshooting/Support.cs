using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Tests.Helpers;

using Xunit;

namespace Raven.Documentation.Samples.Server.Troubleshooting
{
	public class Support
	{
		#region support_1
		public class SampleTestClass : RavenTestBase
		{
			public class Employees_ByFirstName : AbstractIndexCreationTask<Employee>
			{
				public Employees_ByFirstName()
				{
					Map = employees => from employee in employees
									   select new
									   {
										   employee.FirstName
									   };
				}
			}

			[Fact]
			public void SampleTestMethod()
			{
				using (DocumentStore store = NewRemoteDocumentStore())
				{
					new Employees_ByFirstName().Execute(store);

					using (IDocumentSession session = store.OpenSession())
					{
						session.Store(new Employee
						{
							FirstName = "John",
							LastName = "Doe"
						});

						session.SaveChanges();
					}

					WaitForIndexing(store);

					using (IDocumentSession session = store.OpenSession())
					{
						var employees = session
							.Query<Employee, Employees_ByFirstName>()
							.Where(x => x.FirstName == "John")
							.ToList();

						Assert.Equal(1, employees.Count);
						Assert.Equal("John", employees[0].FirstName);
						Assert.Equal("Doe", employees[0].LastName);
					}
				}
			}
		}
		#endregion
	}
}