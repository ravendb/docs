using System;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Documents.Session;

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
                    DeleteFrequencyInSec = 60
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
}
