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
                // Specify the 'Northwind' database when opening a Session
                using (IDocumentSession session = store.OpenSession(database: "NorthWind"))
                {
                    // Session will operate on the 'Northwind' database
                }

                // Specify the 'Northwind' database when sending an Operation
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
                // Using the default database
                using (IDocumentSession northwindSession = store.OpenSession())
                {
                    // Session will operate on the default 'Northwind' database
                }

                // Operation for default database
                store.Maintenance.Send(new DeleteIndexOperation("NorthWindIndex"));

                // Specify the 'AdventureWorks' database when opening a Session
                using (IDocumentSession adventureWorksSession = store.OpenSession(database: "AdventureWorks"))
                {
                    // Session will operate on the specifed 'AdventureWorks' database
                }

                // Specify the 'AdventureWorks' database when sending an Operation
                store.Maintenance.ForDatabase("AdventureWorks").Send(new DeleteIndexOperation("AdventureWorksIndex"));
            }
            #endregion
        }
    }
}
