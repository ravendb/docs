using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class Evict
	{
		private interface IFoo
		{
			#region evict_1
			void Evict<T>(T entity);
			#endregion
		}

		public Evict()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region evict_2
					var employee1 = new Employee
						              {
							              FirstName = "John", 
										  LastName = "Doe"
						              };

					var employee2 = new Employee
						              {
							              FirstName = "Joe", 
										  LastName = "Shmoe"
						              };

					session.Store(employee1);
					session.Store(employee2);

					session.Advanced.Evict(employee1);

					session.SaveChanges(); // only 'Joe Shmoe' will be saved
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region evict_3
					var employee = session.Load<Employee>("employees/1"); // loading from server
					employee = session.Load<Employee>("employees/1"); // no server call
					session.Advanced.Evict(employee);
					employee = session.Load<Employee>("employees/1"); // loading from server
					#endregion
				}
			}
		}
	}
}