using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Configuration
{
	public class OptimisticConcurrency
	{
		public OptimisticConcurrency()
		{
			using (var store = new DocumentStore())
			{
				#region optimistic_concurrency_1
				using (var session = store.OpenSession())
				{
					session.Advanced.UseOptimisticConcurrency = true;

					var person = new Person { FirstName = "John", LastName = "Doe" };

					session.Store(person, "people/1");
					session.SaveChanges();

					using (var otherSession = store.OpenSession())
					{
						var otherPerson = otherSession.Load<Person>("people/1");
						otherPerson.LastName = "Shmoe";

						otherSession.SaveChanges();
					}

					person.FirstName = "Joe";
					session.SaveChanges(); // throws ConcurrencyException
				}
				#endregion
			}
		}
	}
}