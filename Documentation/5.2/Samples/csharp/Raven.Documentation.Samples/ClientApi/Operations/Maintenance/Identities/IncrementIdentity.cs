using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Identities;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Identities
{
    public class IncrementIdentity
    {
        public IncrementIdentity()
        {
            using (var store = new DocumentStore())
            {
                #region increment_identity
                // Create a document with an identity ID:
                // ======================================
                
                using (var session = store.OpenSession())
                { 
                    session.Store(new Company {Name = "RavenDB"}, "companies|");
                    session.SaveChanges(); // => Document "companies/1" will be created 
                }
                
                // Increment the identity value on the server:
                // ===========================================
                
                // Define the next identity operation
                // Pass the collection name (can be with or without a pipe)
                var nextIdentityOp = new NextIdentityForOperation("companies|");
                
                // Execute the operation by passing it to Maintenance.Send
                // Latest value will be incremented to "2"
                long incrementedValue = store.Maintenance.Send(nextIdentityOp);
                
                // Create another document with an identity ID:
                // ============================================
                
                using (var session = store.OpenSession())
                { 
                    session.Store(new Company {Name = "RavenDB"}, "companies|");
                    session.SaveChanges(); // => Document "companies/3" will be created
                }
                #endregion
            }
        }
        
        public async Task IncrementIdentityAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region increment_identity_async
                    // Create a document with an identity ID:
                    // ======================================
                
                    using (var asyncSession = store.OpenAsyncSession())
                    { 
                        asyncSession.StoreAsync(new Company {Name = "RavenDB"}, "companies|");
                        asyncSession.SaveChangesAsync(); // => Document "companies/1" will be created 
                    }
                
                    // Increment the identity value on the server:
                    // ===========================================
                
                    // Define the next identity operation
                    // Pass the collection name (can be with or without a pipe)
                    var nextIdentityOp = new NextIdentityForOperation("companies|");
                
                    // Execute the operation by passing it to Maintenance.SendAsync
                    // Latest value will be incremented to "2"
                    long incrementedValue = await store.Maintenance.SendAsync(nextIdentityOp);
                
                    // Create another document with an identity ID:
                    // ============================================
                
                    using (var asyncSession = store.OpenAsyncSession())
                    { 
                        asyncSession.StoreAsync(new Company {Name = "RavenDB"}, "companies|");
                        asyncSession.SaveChangesAsync(); // => Document "companies/3" will be created
                    }
                    #endregion
                }
            }
        }

        private interface IFoo
        {
            /*
            #region syntax
            public NextIdentityForOperation(string Name);
            #endregion
            */
        }
    }
}
