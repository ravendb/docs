using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi
{
	public class SetupConnectionString
	{
		public SetupConnectionString()
		{
			#region connection_string_1
			var store = new DocumentStore
							{
								ConnectionStringName = "MyRavenConnectionStringName"
							};
			#endregion

			/*
			#region connection_string_2
			Url = http://ravendb.mydomain.com
				// connect to a remote RavenDB instance at ravendb.mydomain.com, to the default database
			Url = http://ravendb.mydomain.com;Database=Northwind
				// connect to a remote RavenDB instance at ravendb.mydomain.com, to the Northwind database there
			Url = http://ravendb.mydomain.com;User=user;Password=secret
				// connect to a remote RavenDB instance at ravendb.mydomain.com, with the specified credentials
			Url = DataDir = ~\App_Data\RavenDB;Enlist=False
				// use embedded mode with the database located in the App_Data\RavenDB folder, without DTC support
			#endregion
			*/
		}
	}
}