using System;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.Server
{
	public class Expiration
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				var user = new User();

				#region expiration1
				DateTime expiry = DateTime.UtcNow.AddMinutes(5);
				using (IDocumentSession session = store.OpenSession())
				{
					session.Store(user);
					session.Advanced.GetMetadataFor(user)[Constants.Documents.Metadata.Expires] = expiry;
					session.SaveChanges();
				}
				#endregion
			}
		}
	}
}
