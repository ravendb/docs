using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.ClusterTransactions
{
    public class AtomicGuards
    {
        public async Task CreateAtomicGuard()
        {
            using var store = new DocumentStore();
            
            #region atomic_guards_enabled
            using (var session = store.OpenSession(new SessionOptions
                   {
                       // Open a cluster-wide session:
                       TransactionMode = TransactionMode.ClusterWide
                   }))
            {
                session.Store(new User(), "users/johndoe");
                session.SaveChanges();
                // An atomic guard is now automatically created for the new document "users/johndoe".
            }
            
            // Open two concurrent cluster-wide sessions:
            using (var session1 = store.OpenSession(
                       new SessionOptions 
                           {TransactionMode = TransactionMode.ClusterWide}))
            using (var session2 = store.OpenSession(
                       new SessionOptions 
                           {TransactionMode = TransactionMode.ClusterWide}))
            {
                // Both sessions load the same document:
                var loadedUser1 = session1.Load<User>("users/johndoe");
                loadedUser1.Name = "jindoe";

                var loadedUser2 = session2.Load<User>("users/johndoe");
                loadedUser2.Name = "jandoe";

                // session1 saves its changes first —
                // this increments the Raft index of the associated atomic guard.
                session1.SaveChanges();

                // session2 tries to save using an outdated atomic guard version
                // and fails with a ConcurrencyException.
                session2.SaveChanges();
            }
            #endregion
            
            #region atomic_guards_enabled_async
            using (var asyncSession = store.OpenAsyncSession(new SessionOptions
                   {
                       // Open a cluster-wide session:
                       TransactionMode = TransactionMode.ClusterWide
                   }))
            {
                await asyncSession.StoreAsync(new User(), "users/johndoe");
                await asyncSession.SaveChangesAsync();
                // An atomic guard is now automatically created for the new document "users/johndoe".
            }
            
            // Open two concurrent cluster-wide sessions:
            using (var asyncSession1 = store.OpenAsyncSession(
                   new SessionOptions 
                   {TransactionMode = TransactionMode.ClusterWide}))
            using (var asyncSession2 = store.OpenAsyncSession(
                   new SessionOptions 
                   {TransactionMode = TransactionMode.ClusterWide}))
            {
                // Both sessions load the same document:
                var loadedUser1 = await asyncSession1.LoadAsync<User>("users/johndoe");
                loadedUser1.Name = "jindoe";

                var loadedUser2 = await asyncSession2.LoadAsync<User>("users/johndoe");
                loadedUser2.Name = "jandoe";

                // asyncSession1 saves its changes first —
                // this increments the Raft index of the associated atomic guard.
                await asyncSession1.SaveChangesAsync();

                // asyncSession2 tries to save using an outdated atomic guard version
                // and fails with a ConcurrencyException.
                await asyncSession2.SaveChangesAsync();
            }
            #endregion
        }

        public async Task DoNotCreateAtomicGuard()
        {
            using var store = new DocumentStore();

            #region atomic_guards_disabled
            using (var session = store.OpenSession(new SessionOptions
                   {
                       TransactionMode = TransactionMode.ClusterWide,
                       // Disable atomic-guards
                       DisableAtomicDocumentWritesInClusterWideTransaction = true
                   }))
            {
                session.Store(new User(), "users/johndoe");

                // No atomic-guard will be created upon saveChanges
                session.SaveChanges();
            }
            #endregion
            
            #region atomic_guards_disabled_async
            using (var asyncSession = store.OpenAsyncSession(new SessionOptions
                   {
                       TransactionMode = TransactionMode.ClusterWide,
                       // Disable atomic-guards
                       DisableAtomicDocumentWritesInClusterWideTransaction = true
                   }))
            {
                await asyncSession.StoreAsync(new User(), "users/johndoe");

                // No atomic-guard will be created upon saveChanges
                await asyncSession.SaveChangesAsync();
            }
            #endregion
        }
        
        public async Task LoadBeforeStoring()
        {
            using var store = new DocumentStore();

            #region load_before_storing
            using (var session = store.OpenSession(new SessionOptions
                   {
                       // Open a cluster-wide session
                       TransactionMode = TransactionMode.ClusterWide
                   }))
            {
                // Load the user document BEFORE creating a new one or modifying if already exists
                var user = session.Load<User>("users/johndoe");
                
                if (user == null)
                {
                    // Document doesn't exist => create a new document:
                    var newUser = new User
                    {
                        Id = "users/johndoe",
                        Name = "John Doe",
                        // ... initialize other properties
                    };

                    // Store the new user document in the session
                    session.Store(newUser);
                }
                else
                {
                    // Document exists => apply your modifications:
                    user.Name = "New name";
                    // ... make any other updates
                    
                    // No need to call Store() again
                    // RavenDB tracks changes on loaded entities
                }

                // Commit your changes
                session.SaveChanges();
            }
            #endregion
            
            #region load_before_storing_async
            using (var asyncSession = store.OpenAsyncSession(new SessionOptions
                   {
                       // Open a cluster-wide session
                       TransactionMode = TransactionMode.ClusterWide
                   }))
            {
                // Load the user document BEFORE creating or updating
                var user = await asyncSession.LoadAsync<User>("users/johndoe");
               
                if (user == null)
                {
                    // Document doesn't exist => create a new document:
                    var newUser = new User
                    {
                        Id = "users/johndoe",
                        Name = "John Doe",
                        // ... initialize other properties
                    };

                    // Store the new user document in the session
                    await asyncSession.StoreAsync(newUser);
                }
                else
                {
                    // Document exists => apply your modifications:
                    user.Name = "New name";
                    // ... make any other updates
                    
                    // No need to call Store() again
                    // RavenDB tracks changes on loaded entities
                }

                // Commit your changes
                await asyncSession.SaveChangesAsync();
            }
            #endregion
        }

        private class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
