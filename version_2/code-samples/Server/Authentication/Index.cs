using System.Collections.Generic;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Database;
using Raven.Database.Server.Security.OAuth;
using Raven.Json.Linq;

namespace RavenCodeSamples.Server.Authentication
{
	namespace Foo
	{
		#region authentication_1
		public interface IAuthenticateClient
		{
			bool Authenticate(DocumentDatabase currentDatabase, string username, string password, out List<DatabaseAccess> allowedDatabases);
		}

		#endregion
	}

	public class Account
	{
		public string UserName { get; set; }
	}

	#region authentication_2
	public class SimpleAuthenticateClient : IAuthenticateClient
	{
		public bool Authenticate(DocumentDatabase currentDatabase, string username, string password, out List<DatabaseAccess> allowedDatabases)
		{
			allowedDatabases = new List<DatabaseAccess>();

			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && username == password)
			{
				allowedDatabases.Add(new DatabaseAccess
					{
						TenantId = "*"
					});

				return true;
			}

			return false;
		}
	}

	#endregion

	public class Index : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region authentication_3

				store.DatabaseCommands.Put("Raven/ApiKeys/sample",
										   null,
										   RavenJObject.FromObject(new ApiKeyDefinition
											   {
												   Name = "sample",
												   Secret = "ThisIsMySecret",
												   Enabled = true,
												   Databases = new List<DatabaseAccess>
							                           {
								                           new DatabaseAccess {TenantId = "*"},
								                           new DatabaseAccess {TenantId = Constants.SystemDatabase},
							                           }
											   }), new RavenJObject());

				#endregion
			}

			#region authentication_4
			var documentStore = new DocumentStore
				{
					ApiKey = "sample/ThisIsMySecret",
					Url = "http://localhost:8080/"
				};

			#endregion
		}
	}
}