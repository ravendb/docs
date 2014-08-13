using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToQuery
	{
		private class MyCustomIndex : AbstractIndexCreationTask<Person>
		{
		}

		private interface IFoo
		{
			#region query_1_0
			IRavenQueryable<T> Query<T>(); // perform query on a dynamic index

			IRavenQueryable<T> Query<T>(string indexName, bool isMapReduce = false);

			IRavenQueryable<T> Query<T, TIndexCreator>()
				where TIndexCreator : AbstractIndexCreationTask, new();
			#endregion
		}

		public HowToQuery()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region query_1_1
					// load up to 128 entities from 'People' collection
					var people = session
						.Query<Person>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_2
					// load up to 128 entities from 'People' collection
					// where FirstName equals 'John'
					var people = session
						.Query<Person>()
						.Where(x => x.FirstName == "John")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_3
					// load up to 128 entities from 'People' collection
					// where FirstName equals 'John'
					var people = from person in session.Query<Person>()
								 where person.FirstName == "John"
								 select person;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_4
					// load up to 128 entities from 'People' collection
					// where FirstName equals 'John'
					// using 'My/Custom/Index'
					var people = from person in session.Query<Person>("My/Custom/Index")
								 where person.FirstName == "John"
								 select person;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_5
					// load up to 128 entities from 'People' collection
					// where FirstName equals 'John'
					// using 'My/Custom/Index'
					var people = from person in session.Query<Person, MyCustomIndex>()
								 where person.FirstName == "John"
								 select person;
					#endregion
				}
			}
		}
	}
}