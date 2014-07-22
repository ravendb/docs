using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
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
					var person = session.Load<Person>("people/1");
					session.Advanced.MarkReadOnly(person);
					session.SaveChanges();
					#endregion
				}
			}
		}
	}
}