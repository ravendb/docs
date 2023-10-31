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
                #region seed_identity
                // Seed the identity value on the server:
                // ======================================
                
                // Define the seed identity operation
                // Pass the collection name (can be with or without a pipe) and the value to set
                var seedIdentityOp = new SeedIdentityForOperation("companies|", 23);
                
                // Execute the operation by passing it to Maintenance.Send
                // The latest value will be incremented to "23"
                long incrementedValue = store.Maintenance.Send(seedIdentityOp);
                
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
        }
        
        public async Task SeedIdentityAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region seed_identity_async
                    // Seed the identity value on the server:
                    // ======================================
                
                    // Define the seed identity operation
                    // Pass the collection name (can be with or without a pipe) and the value to set
                    var seedIdentityOp = new SeedIdentityForOperation("companies|", 23);
                
                    // Execute the operation by passing it to Maintenance.SendAsync
                    // The latest value will be incremented to "23"
                    long incrementedValue = await store.Maintenance.SendAsync(seedIdentityOp);
                
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
