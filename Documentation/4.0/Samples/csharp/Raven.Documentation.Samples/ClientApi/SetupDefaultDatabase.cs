﻿using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;

namespace Raven.Documentation.Samples.ClientApi
{
    public class SetupDefaultDatabase
	{
		public SetupDefaultDatabase()
		{
			#region default_database_1
			// without specifying `Database`
			// we will need to specify the database in each action
			// if no database is passed explicitly we will get an exception
			using (IDocumentStore store = new DocumentStore
				                   {
					                   Urls = new []{"http://localhost:8080/"}
				                   }.Initialize())
			{
				using (IDocumentSession session = store.OpenSession(database:"NorthWind"))
				{
					// ...
				}
			    store.Operations.ForDatabase("NorthWind").Send(new CompactDatabaseOperation(new CompactSettings()));
            }
			#endregion

			#region default_database_2
			// when `Database` is set to `Northwind`
			// created `Operations` or opened `Sessions`
			// will work on `Northwind` database by default
			// if no database is passed explicitly
			using (IDocumentStore store = new DocumentStore
			{
				Urls = new []{"http://localhost:8080/"},
				Database = "Northwind"
			}.Initialize())
			{
				using (IDocumentSession northwindSession = store.OpenSession())
				{
					// ...
				}
			    store.Admin.Send(new DeleteIndexOperation("NorthWindIndex"));


                using (IDocumentSession adventureWorksSession = store.OpenSession("AdventureWorks"))
				{
					// ...
				}
			    store.Admin.ForDatabase("AdventureWorks").Send(new DeleteIndexOperation("AdventureWorksIndex"));
            }
			#endregion
		}
	}
}
