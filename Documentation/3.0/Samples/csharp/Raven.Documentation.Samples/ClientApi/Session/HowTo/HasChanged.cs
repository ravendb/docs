using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class HasChanged
	{
		private interface IFoo
		{
			#region has_changed_1
			bool HasChanged(object entity);
			#endregion
		}

		public HasChanged()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region has_changed_2
					var person = session.Load<Person>("people/1");
					var hasChanged = session.Advanced.HasChanged(person); // false
					person.LastName = "Shmoe";
					hasChanged = session.Advanced.HasChanged(person); // true
					#endregion
				}
			}
		}
	}
}