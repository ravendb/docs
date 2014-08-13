using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi
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
			using (var store = new DocumentStore
				                   {
					                   Url = "http://localhost:8080/"
				                   }.Initialize())
			{
				var commands = store.DatabaseCommands;
				using (var session = store.OpenSession())
				{
					// ...
				}

				var northwindCommands = commands.ForDatabase("Northwind");
				using (var northwindSession = store.OpenSession("Northwind"))
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
			using (var store = new DocumentStore
			{
				Url = "http://localhost:8080/",
				DefaultDatabase = "Northwind"
			}.Initialize())
			{
				var northwindCommands = store.DatabaseCommands;
				using (var northwindSession = store.OpenSession())
				{
					// ...
				}

				var adventureWorksCommands = northwindCommands.ForDatabase("AdventureWorks");
				using (var adventureWorksSession = store.OpenSession("AdventureWorks"))
				{
					// ...
				}

				var systemCommands = northwindCommands.ForSystemDatabase();
				using (var systemSession = store.OpenSession(Constants.SystemDatabase))
				{
					// ..
				}
			}
			#endregion
		}
	}
}