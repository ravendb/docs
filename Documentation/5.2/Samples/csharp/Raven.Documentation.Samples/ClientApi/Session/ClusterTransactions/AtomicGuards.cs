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
            using (var session = store.OpenAsyncSession
                   (new SessionOptions
                    // A cluster-wide session is opened.
                    {TransactionMode = TransactionMode.ClusterWide}))
            {
                await session.StoreAsync(new User(), "users/johndoe");
                await session.SaveChangesAsync();
                // An atomic guard is now automatically created for the new document "users/johndoe".
            }

            // Two cluster-wide sessions are opened.
            using (var session1 = store.OpenAsyncSession(
                   new SessionOptions 
                   {TransactionMode = TransactionMode.ClusterWide}))
            using (var session2 = store.OpenAsyncSession(
                   new SessionOptions 
                   {TransactionMode = TransactionMode.ClusterWide}))
            {
                // Two sessions will load the same document at the same time.
                var loadedUser1 = await session1.LoadAsync<User>("users/johndoe");
                loadedUser1.Name = "jindoe";

                var loadedUser2 = await session2.LoadAsync<User>("users/johndoe");
                loadedUser2.Name = "jandoe";

                // Session1 will save changes first, which triggers a change in the 
                // version number of the associated Atomic Guard.
                await session1.SaveChangesAsync();

                // Session2.SaveChanges() will be rejected with a ConcurrencyException
                // since session1 already changed the Atomic Guard version.  
                // Session2 is expecting the version that it had when it loaded the document.
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
            using (var session = store.OpenAsyncSession
                   (new SessionOptions
                    {TransactionMode = TransactionMode.ClusterWide,
                       // no atomic guards will be used in this session
                       DisableAtomicDocumentWritesInClusterWideTransaction = true}))
            {
                await session.StoreAsync(new User(), "users/johndoe");
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
