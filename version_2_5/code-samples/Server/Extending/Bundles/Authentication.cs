namespace RavenCodeSamples.Server.Bundles
{
	using System.Dynamic;
	using System.Net;
	using System.Security.Cryptography.X509Certificates;

	using Raven.Database.Server.Security.OAuth;

	public class Authentication : CodeSampleBase
	{
		private class AuthenticationUser
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public string[] AllowedDatabases { get; set; }

			public AuthenticationUser SetPassword(string password)
			{
				//set password here

				return this;
			}
		}

		public void AddingUsers()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region authenticate_1
				using (var session = documentStore.OpenSession())
				{
					session.Store(new AuthenticationUser
					{
						Name = "Ayende",
						Id = "Raven/Users/Ayende",
						AllowedDatabases = new[] { "*" }
					}.SetPassword("abc"));
					session.SaveChanges();
				}

				#endregion
			}
		}

		public void HowToAuthenticate()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region authenticate_2
				documentStore.Credentials = new NetworkCredential("userName", "password");

				#endregion
			}
		}

		public void ThirdPartyOAuth()
		{
			dynamic response = new ExpandoObject();

			var userId = "users/ayende";
			var certificate = new X509Certificate2();
			var authorizedDatabases = new[] { "*" };
			var key = certificate.Export(X509ContentType.Pkcs12);

			#region authenticate_3
			var token = AccessToken.Create(key, userId, authorizedDatabases);
			response.Write(token.Serialize());

			#endregion
		}
	}
}