using System;

using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Session
{
	public class Deleting
	{
		private interface IFoo
		{
			#region deleting_1
			void Delete<T>(T entity);

			void Delete<T>(ValueType id);

			void Delete(string id);
			#endregion
		}

		public Deleting()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region deleting_2
					// if UseOptimisticConcurrency is set to 'true' (default 'false')
					// this 'Delete' method will use loaded 'people/1' etag for concurrency check
					// and might throw ConcurrencyException
					var person = session.Load<Person>("people/1");
					session.Delete(person);
					session.SaveChanges();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region deleting_3
					// this 'Delete' method will not do any Etag-based concurrency checks
					// because Etag for 'people/1' is unknown
					session.Delete("people/1");
					session.SaveChanges();
					#endregion
				}
			}
		}
	}
}