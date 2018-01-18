using System.IO;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Raven.TestDriver;
using Xunit;

namespace Raven.Documentation.Samples.Server.Troubleshooting
{
	public class Support
	{
        #region support_1
        public class RavenServerInDebugDllLocator : RavenServerLocator
        {
            public override string ServerPath
            {
                get
                {
                    var serverDllPath = @"src\Raven.Server\bin\x64\Debug\netcoreapp2.0\Raven.Server.dll";
                    if (File.Exists(serverDllPath) == false) // this can happen when running directly from CLI e.g. dotnet xunit
                        serverDllPath = @"src\Raven.Server\bin\Debug\netcoreapp2.0\Raven.Server.dll";

                    return serverDllPath;
                }
            }

            public override string Command => "dotnet";

            public override string CommandArguments => ServerPath;
        }


        public class SampleTestClass : RavenTestDriver<RavenServerInDebugDllLocator>
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
                using (IDocumentStore store = GetDocumentStore())
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
