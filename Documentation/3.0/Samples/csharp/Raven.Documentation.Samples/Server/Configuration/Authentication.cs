using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client.Connection;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.Server.Configuration
{
	public class Authentication
	{
		public Authentication()
		{
			#region authentication_4
			var store = new DocumentStore
			{
				ApiKey = "sample/ThisIsMySecret",
				Url = "http://localhost:8080/"
			};
			#endregion
		}

		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region authentication_3
				store.DatabaseCommands.Put("Raven/ApiKeys/sample",
										   null,
										   RavenJObject.FromObject(new ApiKeyDefinition
										   {
											   Name = "sample",
											   Secret = "ThisIsMySecret",
											   Enabled = true,
											   Databases = new List<ResourceAccess>
							                           {
								                           new ResourceAccess {TenantId = "*"},
								                           new ResourceAccess {TenantId = Constants.SystemDatabase},
							                           }
										   }), new RavenJObject());
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region authentication_5
				var json = store
					.JsonRequestFactory
					.CreateHttpJsonRequest(new CreateHttpJsonRequestParams(null, store.Url + "/debug/user-info", "GET", store.DatabaseCommands.PrimaryCredentials, store.Conventions))
					.ReadResponseJson();
				#endregion
			}
		}
	}
}