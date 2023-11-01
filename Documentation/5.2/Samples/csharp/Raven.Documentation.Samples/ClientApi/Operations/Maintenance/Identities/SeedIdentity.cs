using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Identities;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Identities
{
    public class SeedIdentity
    {
        public SeedIdentity()
        {
            using (var store = new DocumentStore())
            {
                #region seed_identity_1
                // Seed a higher identity value on the server:
                // ===========================================
                
                // Define the seed identity operation. Pass:
                //   * The collection name (can be with or without a pipe)
                //   * The new value to set
                var seedIdentityOp = new SeedIdentityForOperation("companies|", 23);
                
                // Execute the operation by passing it to Maintenance.Send
                // The latest value on the server will be incremented to "23"
                // and the next document created with an identity will be assigned "24"
                long seededValue = store.Maintenance.Send(seedIdentityOp);
                
                // Create a document with an identity ID:
                // ======================================
                
                using (var session = store.OpenSession())
                { 
                    session.Store(new Company { Name = "RavenDB" }, "companies|");
                    session.SaveChanges();
                    // => Document "companies/24" will be created
                }
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region seed_identity_2
                // Force a smaller identity value on the server:
                // =============================================
                
                // Define the seed identity operation. Pass:
                //   * The collection name (can be with or without a pipe)
                //   * The new value to set
                //   * Set 'forceUpdate' to true
                var seedIdentityOp = new SeedIdentityForOperation("companies|", 5, forceUpdate: true);
                
                // Execute the operation by passing it to Maintenance.Send
                // The latest value on the server will be decremented to "5"
                // and the next document created with an identity will be assigned "6"
                long seededValue = store.Maintenance.Send(seedIdentityOp);
                
                // Create a document with an identity ID:
                // ======================================
                
                using (var session = store.OpenSession())
                { 
                    session.Store(new Company { Name = "RavenDB" }, "companies|");
                    session.SaveChanges();
                    // => Document "companies/6" will be created
                }
                #endregion
            }
        }
        
        public async Task SeedIdentityAsync()
        {
            using (var store = new DocumentStore())
            {
                #region seed_identity_1_async
                // Seed the identity value on the server:
                // ======================================
                
                // Define the seed identity operation. Pass:
                //   * The collection name (can be with or without a pipe)
                //   * The new value to set
                var seedIdentityOp = new SeedIdentityForOperation("companies|", 23);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                // The latest value on the server will be incremented to "23"
                // and the next document created with an identity will be assigned "24"
                long seededValue = await store.Maintenance.SendAsync(seedIdentityOp);
                
                // Create a document with an identity ID:
                // ======================================
                
                using (var asyncSession = store.OpenAsyncSession())
                { 
                    asyncSession.StoreAsync(new Company { Name = "RavenDB" }, "companies|");
                    asyncSession.SaveChangesAsync();
                    // => Document "companies/24" will be created
                }
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region seed_identity_2_async
                // Force a smaller identity value on the server:
                // =============================================
                
                // Define the seed identity operation. Pass:
                //   * The collection name (can be with or without a pipe)
                //   * The new value to set
                //   * Set 'forceUpdate' to true
                var seedIdentityOp = new SeedIdentityForOperation("companies|", 5, forceUpdate: true);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                // The latest value on the server will be decremented to "5"
                // and the next document created with an identity will be assigned "6"
                long seededValue = await store.Maintenance.SendAsync(seedIdentityOp);
                
                // Create a document with an identity ID:
                // ======================================
                
                using (var asyncSession = store.OpenAsyncSession())
                { 
                    asyncSession.StoreAsync(new Company { Name = "RavenDB" }, "companies|");
                    asyncSession.SaveChangesAsync();
                    // => Document "companies/6" will be created
                }
                #endregion
            }
        }

        private interface IFoo
        {
            /*
            #region syntax
            public SeedIdentityForOperation(string name, long value, bool forceUpdate = false);
            #endregion
            */
        }
    }
}
