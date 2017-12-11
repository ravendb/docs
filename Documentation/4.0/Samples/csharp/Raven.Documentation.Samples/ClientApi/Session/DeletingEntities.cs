using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Raven.Documentation.Samples.Orders;


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

            void Delete(string id, string expectedChangeVector);

			#endregion
        }

        public DeletingEntities()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
                    #region deleting_2

                    Employee employee = session.Load<Employee>("employees/1");

                    session.Delete(employee);
                    session.SaveChanges();

                    #endregion
				}

				using (var session = store.OpenSession())
				{
                    #region deleting_3

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

                    session.Advanced.Defer(new DeleteCommandData("employees/1", changeVector: null));

                    #endregion
                }
            }
		}
	}
}
