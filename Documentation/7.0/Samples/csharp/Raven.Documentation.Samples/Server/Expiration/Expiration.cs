using System;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Documents.Session;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.Server
{
    public class Expiration
    {
        private class User
        {
            public string Name { get; set; }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region configuration
                await store.Maintenance.SendAsync(new ConfigureExpirationOperation(new ExpirationConfiguration
                {
                    Disabled = false,
                    DeleteFrequencyInSec = 60,
                    MaxItemsToProcess = 1000
                }));
                #endregion

                var user = new User();

                #region expiration1
                DateTime expiry = DateTime.UtcNow.AddMinutes(5);
                using (IAsyncDocumentSession session = store.OpenAsyncSession())
                {
                    await session.StoreAsync(user);
                    session.Advanced.GetMetadataFor(user)[Constants.Documents.Metadata.Expires] = expiry;
                    await session.SaveChangesAsync();
                }
                #endregion
            }
        }
    }

    class exp
    {
        #region expirationConfiguration
        public class ExpirationConfiguration
        {
            // Set 'Disabled' to false to enable the deletion of expired items                    
            public bool Disabled { get; set; }

            // How frequently to delete expired items
            public long? DeleteFrequencyInSec { get; set; }

            // How many items to delete (per batch)
            public long? MaxItemsToProcess { get; set; }
        }
        #endregion
    }
}
