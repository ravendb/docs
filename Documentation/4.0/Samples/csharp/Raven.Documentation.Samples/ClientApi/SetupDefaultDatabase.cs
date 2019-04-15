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
                // Northwind database explicitly specified as a parameter
                using (IDocumentSession session = store.OpenSession(database: "NorthWind"))
                {
                    // Do your work here
                }

                // Operation for specified database
                store.Maintenance.ForDatabase("Northwind").Send(new DeleteIndexOperation("NorthWindIndex"));
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
                // Session for default database
                using (IDocumentSession northwindSession = store.OpenSession())
                {
                    // Do your work here
                }

                // Operation for default database
                store.Maintenance.Send(new DeleteIndexOperation("NorthWindIndex"));

                // Session for specified database
                using (IDocumentSession adventureWorksSession = store.OpenSession(database: "AdventureWorks"))
                {
                    // Do your work here
                }

                // Operation for specified database
                store.Maintenance.ForDatabase("AdventureWorks").Send(new DeleteIndexOperation("AdventureWorksIndex"));
            }
            #endregion
        }
    }
}
