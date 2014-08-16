using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToQuery
	{
		private class MyCustomIndex : AbstractIndexCreationTask<Employee>
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
					// load up to 128 entities from 'Employees' collection
					var people = session
						.Query<Employee>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_2
					// load up to 128 entities from 'Employees' collection
					// where FirstName equals 'Robert'
					var people = session
						.Query<Employee>()
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_3
					// load up to 128 entities from 'Employees' collection
					// where FirstName equals 'Robert'
					var people = from person in session.Query<Employee>()
								 where person.FirstName == "Robert"
								 select person;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_4
					// load up to 128 entities from 'Employees' collection
					// where FirstName equals 'Robert'
					// using 'My/Custom/Index'
					var people = from person in session.Query<Employee>("My/Custom/Index")
								 where person.FirstName == "Robert"
								 select person;
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region query_1_5
					// load up to 128 entities from 'Employees' collection
					// where FirstName equals 'Robert'
					// using 'My/Custom/Index'
					var people = from person in session.Query<Employee, MyCustomIndex>()
								 where person.FirstName == "Robert"
								 select person;
					#endregion
				}
			}
		}
	}
}