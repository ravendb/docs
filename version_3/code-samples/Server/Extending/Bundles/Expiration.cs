using System;

using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.Server.Extending.Bundles
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
				var expiry = DateTime.UtcNow.AddMinutes(5);
				using (var session = store.OpenSession())
				{
					session.Store(user);
					session.Advanced.GetMetadataFor(user)["Raven-Expiration-Date"] = new RavenJValue(expiry);
					session.SaveChanges();
				}
				#endregion
			}
		}
	}
}