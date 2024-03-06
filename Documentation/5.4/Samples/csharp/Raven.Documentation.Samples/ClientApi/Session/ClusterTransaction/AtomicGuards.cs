using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.ClusterTransactions
{
    public class AtomicGuards
    {
        public async Task CreateAtomicGuard()
        {
            //var (nodes, leader) = await CreateRaftCluster(3);
            using var store = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "test"
            }.Initialize();

            #region atomic-guards-enabled

            using (var session = store.OpenAsyncSession(new SessionOptions
                   {
                       // Open a cluster-wide session
                       TransactionMode = TransactionMode.ClusterWide
                   }))
            {
                await session.StoreAsync(new User(), "users/johndoe");
                await session.SaveChangesAsync();
                // An atomic-guard is now automatically created for the new document "users/johndoe".
            }

            // Open two more cluster-wide sessions
            using (var session1 = store.OpenAsyncSession(
                   new SessionOptions 
                   {TransactionMode = TransactionMode.ClusterWide}))
            using (var session2 = store.OpenAsyncSession(
                   new SessionOptions 
                   {TransactionMode = TransactionMode.ClusterWide}))
            {
                // The two sessions will load the same document at the same time
                var loadedUser1 = await session1.LoadAsync<User>("users/johndoe");
                loadedUser1.Name = "jindoe";

                var loadedUser2 = await session2.LoadAsync<User>("users/johndoe");
                loadedUser2.Name = "jandoe";

                // Session1 will save changes first, which triggers a change in the 
                // version number of the associated atomic-guard.
                await session1.SaveChangesAsync();

                // session2.saveChanges() will be rejected with ConcurrencyException
                // since session1 already changed the atomic-guard version,
                // and session2 saveChanges uses the document version that it had when it loaded the document.
                await session2.SaveChangesAsync();
            }
            #endregion

            var result = await store.Operations.SendAsync(new GetCompareExchangeValuesOperation<User>(""));
        }

        public async Task DoNotCreateAtomicGuard()
        {
            //var (nodes, leader) = await CreateRaftCluster(3);
            using var store = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "test"
            }.Initialize();


            #region atomic-guards-disabled
            using (var session = store.OpenAsyncSession(new SessionOptions
                   {
                       TransactionMode = TransactionMode.ClusterWide,
                       // Disable atomic-guards
                       DisableAtomicDocumentWritesInClusterWideTransaction = true
                   }))
            {
                await session.StoreAsync(new User(), "users/johndoe");

                // No atomic-guard will be created upon saveChanges
                await session.SaveChangesAsync();
            }
            #endregion

            // WaitForUserToContinueTheTest(store);

            var result = await store.Operations.SendAsync(new GetCompareExchangeValuesOperation<User>(""));
        }

        private class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
