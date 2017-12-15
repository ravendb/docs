using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Refresh
	{
		private interface IFoo
		{
			#region refresh_1
			void Refresh<T>(T entity);
			#endregion
		}

		public Refresh()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region refresh_2
					Employee employee = session.Load<Employee>("employees/1");
					Assert.Equal("Doe", employee.LastName);

					// LastName changed to 'Shmoe'

					session.Advanced.Refresh(employee);
					Assert.Equal("Shmoe", employee.LastName);
					#endregion
				}
			}
		}
	}
}
