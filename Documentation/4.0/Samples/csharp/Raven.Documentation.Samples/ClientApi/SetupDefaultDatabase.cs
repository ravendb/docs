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
            // when no default database, i.e. `Database` property, is set
            // we will need to specify the database for each action
            // if no database is passed explicitly we will get an exception
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://your_RavenDB_server_URL" }
            }.Initialize())
            {
                using (IDocumentSession session = store.OpenSession(database: "NorthWind"))
                {
                    // do your work here
                }
                store.Maintenance.Server.Send(new CompactDatabaseOperation(new CompactSettings
                                                                                        { DatabaseName = "NorthWind" }));
            }
            #endregion

            #region default_database_2
            // when `Database` is set to `Northwind`
            // any `Operation` or `Session` created through `store`
            // will operate on `Northwind` database by default
            // if no other database is passed explicitly
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://your_RavenDB_server_URL" },
                Database = "Northwind"
            }.Initialize())
            {
                using (IDocumentSession northwindSession = store.OpenSession()) //default
                {
                    // do your work here
                }
                store.Maintenance.Send(new DeleteIndexOperation("NorthWindIndex"));


                using (IDocumentSession adventureWorksSession = store.OpenSession("AdventureWorks")) //explicit pass
                {
                    // do your work here
                }
                store.Maintenance.ForDatabase("AdventureWorks").Send(new DeleteIndexOperation("AdventureWorksIndex"));
            }
            #endregion
        }
    }
}
