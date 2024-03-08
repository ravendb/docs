using System;
using System.Threading.Tasks;
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

            void Delete(string id);

            void Delete(string id, string expectedChangeVector);

            #endregion
        }

        public async Task DeletingEntitiesAsync()
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

                using (var session = store.OpenAsyncSession())
                {
                    #region deleting_2_async

                    Employee employee = await session.LoadAsync<Employee>("employees/1");

                    session.Delete(employee);
                    await session.SaveChangesAsync();

                    #endregion
                }


                using (var session = store.OpenSession())
                {
                    #region deleting_3

                    session.Delete("employees/1");
                    session.SaveChanges();

                    #endregion
                }

                using (var session = store.OpenAsyncSession())
                {
                    #region deleting_3_async

                    session.Delete("employees/1");
                    await session.SaveChangesAsync();

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
