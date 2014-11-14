using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi
{
	public class SetupDefaultDatabase
	{
		public SetupDefaultDatabase()
		{
			#region default_database_1
			// without specifying `DefaultDatabase`
			// created `DatabaseCommands` or opened `Sessions`
			// will work on `<system>` database by default
			// if no database is passed explicitly
			using (IDocumentStore store = new DocumentStore
				                   {
					                   Url = "http://localhost:8080/"
				                   }.Initialize())
			{
				IDatabaseCommands commands = store.DatabaseCommands;
				using (IDocumentSession session = store.OpenSession())
				{
					// ...
				}

				IDatabaseCommands northwindCommands = commands.ForDatabase("Northwind");
				using (IDocumentSession northwindSession = store.OpenSession("Northwind"))
				{
					// ...
				}
			}
			#endregion

			#region default_database_2
			// when `DefaultDatabase` is set to `Northwind`
			// created `DatabaseCommands` or opened `Sessions`
			// will work on `Northwind` database by default
			// if no database is passed explicitly
			using (IDocumentStore store = new DocumentStore
			{
				Url = "http://localhost:8080/",
				DefaultDatabase = "Northwind"
			}.Initialize())
			{
				IDatabaseCommands northwindCommands = store.DatabaseCommands;
				using (IDocumentSession northwindSession = store.OpenSession())
				{
					// ...
				}

				IDatabaseCommands adventureWorksCommands = northwindCommands.ForDatabase("AdventureWorks");
				using (IDocumentSession adventureWorksSession = store.OpenSession("AdventureWorks"))
				{
					// ...
				}

				IDatabaseCommands systemCommands = northwindCommands.ForSystemDatabase();
				using (IDocumentSession systemSession = store.OpenSession(Constants.SystemDatabase))
				{
					// ..
				}
			}
			#endregion
		}
	}
}