using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
	public class MarkAsReadonly
	{
		private interface IFoo
		{
			#region mark_as_readonly_1
			void MarkReadOnly(object entity);
			#endregion
		}

		public MarkAsReadonly()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region mark_as_readonly_2
					var employee = session.Load<Employee>("employees/1");
					session.Advanced.MarkReadOnly(employee);
					session.SaveChanges();
					#endregion
				}
			}
		}
	}
}