using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi
{
    public class SetupDefaultDatabase
    {
        public SetupDefaultDatabase()
        {
            #region default_database_1
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://your_RavenDB_server_URL" }
                // Default database is not set
            }.Initialize())
            {
                // Northwind database is explicitly specified as a parameter
                using (IDocumentSession session = store.OpenSession(database: "NorthWind"))
                {
                    // Do your work here
                }
                store.Maintenance.Server.Send(new CompactDatabaseOperation(new CompactSettings
                                                                                        { DatabaseName = "NorthWind" }));
            }
            #endregion

            #region default_database_2
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://your_RavenDB_server_URL" },
                // Default database is set to 'Northwind'
                Database = "Northwind"
            }.Initialize())
            {
                // Using default database
                using (IDocumentSession northwindSession = store.OpenSession())
                {
                    //Session will operate on the default 'Northwind' database
                }
                store.Maintenance.Send(new DeleteIndexOperation("NorthWindIndex"));

                // Using specified database
                using (IDocumentSession adventureWorksSession = store.OpenSession("AdventureWorks"))
                {
                    // Session will operate on the specifed 'AdventureWorks' database
                }
                store.Maintenance.ForDatabase("AdventureWorks").Send(new DeleteIndexOperation("AdventureWorksIndex"));
            }
            #endregion
        }
    }
}
