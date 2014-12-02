using System.Collections.Generic;

namespace Raven.Documentation.Samples.Start
{
	using System.Linq;

	using Raven.Client;
	using Raven.Client.Document;
	using Raven.Client.Indexes;
	using Raven.Documentation.CodeSamples.Orders;

	using Xunit;

	public class GettingStarted
	{
		public void T1()
		{
			#region start_1
			using (IDocumentStore store = new DocumentStore
			{
				Url = "http://localhost:8080/",	// server URL
				DefaultDatabase = "Northwind"	// default database
			})
			{
				store.Initialize(); // initializes document store, by connecting to server and downloading various configurations

				using (IDocumentSession session = store.OpenSession()) // opens a session that will work in context of 'DefaultDatabase'
				{
					Employee employee = new Employee
					{
						FirstName = "John",
						LastName = "Doe"
					};

					session.Store(employee); // stores employee in session, assigning it to a collection `Employees`
					string employeeId = employee.Id; // Session.Store will assign Id to employee, if it is not set

					session.SaveChanges(); // sends all changes to server

					// Session implements Unit of Work pattern,
					// therefore employee instance would be the same and no server call will be made
					Employee loadedEmployee = session.Load<Employee>(employeeId);
					Assert.Equal(employee, loadedEmployee);
				}
			}
			#endregion
		}

		#region start_2
		/// <summary>
		/// All _ in index class names will be converted to /
		/// it means that Employees_ByFirstNameAndLastName will be Employees/ByFirstNameAndLastName
		/// when deployed to server
		/// 
		/// AbstractIndexCreationTask is a helper class that gives you strongly-typed syntax
		/// for creating indexes
		/// </summary>
		public class Employees_ByFirstNameAndLastName : AbstractIndexCreationTask<Employee>
		{
			public Employees_ByFirstNameAndLastName()
			{
				// this is a simple (Map) index LINQ-flavored mapping function
				// that enables searching of Employees by
				// FirstName, LastName (or both)
				Map = employees => from employee in employees
								   select new
									{
										FirstName = employee.FirstName,
										LastName = employee.LastName
									};
			}
		}
		#endregion

		public void T2()
		{
			#region start_2
			using (IDocumentStore store = new DocumentStore
			{
				Url = "http://localhost:8080/",	// server URL
				DefaultDatabase = "Northwind"	// default database
			})
			{
				store.Initialize(); // initializes document store, by connecting to server and downloading various configurations

				new Employees_ByFirstNameAndLastName().Execute(store); // deploying index to server

				using (IDocumentSession session = store.OpenSession()) // opens a session that will work in context of 'DefaultDatabase'
				{
					List<Employee> employees = session.Query<Employee, Employees_ByFirstNameAndLastName>() // returning object of type Employee, using Employees/ByFirstNameAndLastName index
						.Where(x => x.FirstName == "Robert") // predicates (can only use fields that index defines, so FirstName, LastName or both)
						.ToList(); // materializing query - sending to server
				}
			}
			#endregion
		}
	}
}