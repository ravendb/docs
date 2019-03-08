package net.ravendb.ClientApi;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.operations.CompactDatabaseOperation;
import net.ravendb.client.documents.operations.indexes.DeleteIndexOperation;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.serverwide.CompactSettings;

public class SetupDefaultDatabase {
    public SetupDefaultDatabase() {
        //region default_database_1
        // without specifying `database`
        // we will need to specify the database in each action
        // if no database is passed explicitly we will get an exception

        try (DocumentStore store = new DocumentStore()) {
            store.setUrls(new String[]{ "http://localhost:8080" });
            store.initialize();

            try (IDocumentSession session = store.openSession("Northwind")) {
                // ...
            }

            CompactSettings compactSettings = new CompactSettings();
            compactSettings.setDatabaseName("Northwind");
            store.maintenance().server().send(new CompactDatabaseOperation(compactSettings));
        }
        //endregion

        //region default_database_2
        // when `database` is set to `Northwind`
        // created `operations` or opened `sessions`
        // will work on `Northwind` database by default
        // if no database is passed explicitly
        try (DocumentStore store = new DocumentStore(new String[]{ "http://localhost:8080" }, "Northwind")) {
            store.initialize();

            try (IDocumentSession northwindSession = store.openSession()) {
                // ...
            }

            store.maintenance().send(new DeleteIndexOperation("NorthwindIndex"));


            try (IDocumentSession adventureWorksSession = store.openSession("AdventureWorks")) {
                // ...
            }

            store.maintenance().forDatabase("AdventureWorks").send(new DeleteIndexOperation("AdventureWorksIndex"));
        }
        //endregion
    }
}
