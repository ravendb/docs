namespace RavenCodeSamples.Server.Bundles
{
	using System;

	using Raven.Json.Linq;

	public class Expiration : CodeSampleBase
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var documentStore = NewDocumentStore())
			{
				var userSession = new User();

				#region expiration1

				var expiry = DateTime.UtcNow.AddMinutes(5);
				using (var session = documentStore.OpenSession())
				{
					session.Store(userSession);
					session.Advanced.GetMetadataFor(userSession)["Raven-Expiration-Date"] = new RavenJValue(expiry);
					session.SaveChanges();
				}

				#endregion
			}
		}
	}
}