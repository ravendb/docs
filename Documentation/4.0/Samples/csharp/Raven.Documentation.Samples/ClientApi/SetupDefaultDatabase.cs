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
            }.Initialize())
            {
                using (IDocumentSession session = store.OpenSession(database: "NorthWind"))
                {
                    //Do your work here
                }
                store.Maintenance.Server.Send(new CompactDatabaseOperation(new CompactSettings
                                                                                        { DatabaseName = "NorthWind" }));
            }
            #endregion

            #region default_database_2
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://your_RavenDB_server_URL" },
                Database = "Northwind"
            }.Initialize())
            {
                using (IDocumentSession northwindSession = store.OpenSession()) //Default database
                {
                    //Do your work here
                }
                store.Maintenance.Send(new DeleteIndexOperation("NorthWindIndex"));


                using (IDocumentSession adventureWorksSession = store.OpenSession("AdventureWorks")) //Specified database
                {
                    //Do your work here
                }
                store.Maintenance.ForDatabase("AdventureWorks").Send(new DeleteIndexOperation("AdventureWorksIndex"));
            }
            #endregion
        }
    }
}
