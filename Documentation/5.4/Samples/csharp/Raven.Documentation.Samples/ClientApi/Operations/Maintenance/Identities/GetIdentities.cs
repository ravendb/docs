using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Identities;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Identities
{
    public class GetIdentities
    {
        public GetIdentities()
        {
            using (var store = new DocumentStore())
            {
                #region get_identities
                // Create a document with an identity ID:
                // ======================================
                
                using (var session = store.OpenSession())
                {
                    // Request the server to generate an identity ID for the new document. Pass:
                    //   * The entity to store
                    //   * The collection name with a pipe (|) postfix 
                    session.Store(new Company { Name = "RavenDB" }, "companies|");
                    
                    // If this is the first identity created for this collection,
                    // and if the identity value was not customized
                    // then a document with an identity ID "companies/1" will be created
                    session.SaveChanges();
                }
                
                // Get identities information:
                // ===========================
                
                // Define the get identities operation
                var getIdentitiesOp = new GetIdentitiesOperation();
                
                // Execute the operation by passing it to Maintenance.Send
                Dictionary<string, long> identities = store.Maintenance.Send(getIdentitiesOp);
                
                // Results
                var latestIdentityValue = identities["companies|"]; // => value will be 1
                #endregion
            }
        }
        
        public async Task GetIdentitiesAsync()
        {
            using (var store = new DocumentStore())
            {
                #region get_identities_async
                // Create a document with an identity ID:
                // ======================================
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Request the server to generate an identity ID for the new document. Pass:
                    //   * The entity to store
                    //   * The collection name with a pipe (|) postfix 
                    asyncSession.StoreAsync(new Company { Name = "RavenDB" }, "companies|");
                    
                    // If this is the first identity created for this collection,
                    // and if the identity value was not customized
                    // then a document with an identity ID "companies/1" will be created
                    asyncSession.SaveChangesAsync();
                }
                
                // Get identities information:
                // ===========================
                
                // Define the get identities operation
                var getIdentitiesOp = new GetIdentitiesOperation();
                
                // Execute the operation by passing it to Maintenance.SendAsync
                Dictionary<string, long> identities = await store.Maintenance.SendAsync(getIdentitiesOp);
                
                // Results
                var latestIdentityValue = identities["companies|"]; // => value will be 1
                #endregion
            }
        }

        private interface IFoo
        {
            /*
            #region syntax
            public GetIdentitiesOperation();
            #endregion
            */
        }
    }
}
