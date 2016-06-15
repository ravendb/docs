using System;
using Raven.Abstractions.Commands;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session
{
	public class DeletingEntities
	{
		private interface IFoo
		{
			#region deleting_1
			void Delete<T>(T entity);

			void Delete<T>(ValueType id);

			void Delete(string id);
			#endregion
		}

		public DeletingEntities()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region deleting_2
					// if UseOptimisticConcurrency is set to 'true' (default 'false')
					// this 'Delete' method will use loaded 'employees/1' etag for concurrency check
					// and might throw ConcurrencyException
					Employee employee = session.Load<Employee>("employees/1");
					session.Delete(employee);
					session.SaveChanges();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region deleting_3
					// this 'Delete' method will not do any Etag-based concurrency checks
					// because Etag for 'employees/1' is unknown
					session.Delete("employees/1");
					session.SaveChanges();
					#endregion
				}

                using (var session = store.OpenSession())
                {
                    #region deleting_4
                    session.Delete("employees/1");
                    #endregion

                    #region deleting_5
                    session.Advanced.Defer(new DeleteCommandData { Key = "employees/1" });
                    #endregion
                }
            }
		}
	}
}