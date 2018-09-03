using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
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

            #region cluster_store_with_compare_exchange
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession(new SessionOptions
                {
                    //default is:     TransactionMode.SingleNode
                    TransactionMode = TransactionMode.ClusterWide
                }))
                {
                    var user = new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                    session.Store(user);

                    // this transaction is now conditional on this being 
                    // successfully created (so, no other users with this name)
                    // it also creates an association to the new user's id
                    session.Advanced.ClusterTransaction
                        .CreateCompareExchangeValue("usernames/John", user.Id);

                    session.SaveChanges();
                }
                #endregion

                #region cluster_store_with_compare_exchange_async
                using (var session = store.OpenAsyncSession(new SessionOptions
                {
                    //default is:     TransactionMode.SingleNode
                    TransactionMode = TransactionMode.ClusterWide
                }))
                {
                    var user = new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                    await session.StoreAsync(user);

                    // this transaction is now conditional on this being 
                    // successfully created (so, no other users with this name)
                    // it also creates an association to the new user's id
                    session.Advanced.ClusterTransaction
                        .CreateCompareExchangeValue("usernames/John", user.Id);

                    await session.SaveChangesAsync();
                }
                #endregion
            }
        }
    }
}
