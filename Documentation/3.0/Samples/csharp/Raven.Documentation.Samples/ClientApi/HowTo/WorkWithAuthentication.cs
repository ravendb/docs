namespace Raven.Documentation.CodeSamples.ClientApi.HowTo
{
	using System.Collections.Generic;
	using System.Net;
	using Abstractions.Data;
	using Client.Document;

	public class WorkWithAuthentication
	{
		public WorkWithAuthentication()
		{
			#region windows_auth_default_credentials
			ICredentials Credentials = CredentialCache.DefaultNetworkCredentials;
			#endregion


			#region windows_auth_setup
			var windowsAuthStore = new DocumentStore()
			{
				Credentials = new NetworkCredential("user", "password", "domain")
			};
			#endregion

			new 
			#region api_key_def

			ApiKeyDefinition
			{
				Name = "NorthwindAdminAccess",
				Secret = "MySecret",
				Enabled = true,
				Databases = new List<ResourceAccess>
				{
					new ResourceAccess
					{
						TenantId = "Northwind",
						Admin = true
					}
				}
			}
			#endregion
			;

			#region api_key_setup
			var oAuthStore = new DocumentStore()
			{
				ApiKey = "NorthwindAdminAccess/MySecret"
			};
			#endregion
		} 
	}
}