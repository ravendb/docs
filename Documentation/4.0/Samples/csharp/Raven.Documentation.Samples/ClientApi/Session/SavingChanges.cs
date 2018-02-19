using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using Xunit.Sdk;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class SavingChanges
    {
        private interface IInterface
        {
            #region saving_changes_1
            void SaveChanges();
            #endregion

            #region saving_changes_1_async
            Task SaveChangesAsync();
            #endregion
        }

        public async Task SavingChangesXY()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region saving_changes_2
                    // storing new entity
                    session.Store(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    session.SaveChanges();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region saving_changes_2_async
                    // storing new entity
                    await asyncSession.StoreAsync(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    await asyncSession.SaveChangesAsync();
                    #endregion
                }
            }


            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    // storing new entity
                    #region saving_changes_3
                    session.Advanced.WaitForIndexesAfterSaveChanges(
                        timeout: TimeSpan.FromSeconds(30),
                        throwOnTimeout: true,
                        indexes: new[] { "index/1", "index/2" });

                    session.Store(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    session.SaveChanges();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // storing new entity
                    #region saving_changes_3_async
                    asyncSession.Advanced.WaitForIndexesAfterSaveChanges(
                        timeout: TimeSpan.FromSeconds(30),
                        throwOnTimeout: true,
                        indexes: new[] { "index/1", "index/2" });

                    await asyncSession.StoreAsync(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    await asyncSession.SaveChangesAsync();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    // storing new entity
                    #region saving_changes_4

                    session.Advanced.WaitForReplicationAfterSaveChanges(
                        timeout: TimeSpan.FromSeconds(30),
                        throwOnTimeout: false, //default true
                        replicas: 2, //minimum replicas to replicate
                        majority: false);

                    session.Store(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    session.SaveChanges();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // storing new entity
                    #region saving_changes_4_async

                    asyncSession.Advanced.WaitForReplicationAfterSaveChanges(
                        timeout: TimeSpan.FromSeconds(30),
                        throwOnTimeout: false, //default true
                        replicas: 2, //minimum replicas to replicate
                        majority: false);

                    await asyncSession.StoreAsync(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    await asyncSession.SaveChangesAsync();
                    #endregion
                }
            }
        }


    }
}
