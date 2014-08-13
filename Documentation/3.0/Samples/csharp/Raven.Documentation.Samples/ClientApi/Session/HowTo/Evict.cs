using Raven.Client.Document;

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
					var person1 = new Person
						              {
							              FirstName = "John", 
										  LastName = "Doe"
						              };

					var person2 = new Person
						              {
							              FirstName = "Joe", 
										  LastName = "Shmoe"
						              };

					session.Store(person1);
					session.Store(person2);

					session.Advanced.Evict(person1);

					session.SaveChanges(); // only 'Joe Shmoe' will be saved
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region evict_3
					var person = session.Load<Person>("people/1"); // loading from server
					person = session.Load<Person>("people/1"); // no server call
					session.Advanced.Evict(person);
					person = session.Load<Person>("people/1"); // loading from server
					#endregion
				}
			}
		}
	}
}