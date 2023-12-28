using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class EntityChanges
	{
		private interface IFoo
		{
			#region has_changed_1
			bool HasChanged(object entity);
			#endregion
		}

		public EntityChanges()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region has_changed_2
					Employee employee = session.Load<Employee>("employees/1-A");
					bool hasChanged = session.Advanced.HasChanged(employee); // false
					employee.LastName = "Shmoe";
					hasChanged = session.Advanced.HasChanged(employee); // true
					#endregion
				}
			}
		}
	}
}
