using Raven.Client.Document;

using Xunit;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
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
					var person = session.Load<Person>("people/1");
					Assert.Equal("Doe", person.LastName);

					// LastName changed to 'Shmoe'

					session.Advanced.Refresh(person);
					Assert.Equal("Shmoe", person.LastName);
					#endregion
				}
			}
		}
	}
}