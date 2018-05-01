package net.ravendb.ClientApi.Operations.Server;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.CompactDatabaseOperation;
import net.ravendb.client.documents.operations.Operation;
import net.ravendb.client.documents.operations.OperationIdResult;
import net.ravendb.client.documents.operations.indexes.GetIndexNamesOperation;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.serverwide.CompactSettings;

public class Compact {

    private interface IFoo {
        /*
        //region compact_1
        public CompactDatabaseOperation(CompactSettings compactSettings)
        //endregion
        */
    }

    private class Foo {
        //region compact_2
        public class CompactSettings {
            private String databaseName;
            private boolean documents;
            private String[] indexes;

            // getters and setters
        }
        //endregion
    }

    public Compact() {
        try (IDocumentStore store = new DocumentStore()) {
            //region compact_3
            CompactSettings settings = new CompactSettings();
            settings.setDatabaseName("Northwind");
            settings.setDocuments(true);
            settings.setIndexes(new String[] { "Orders/Totals", "Orders/ByCompany" });

            Operation operation = store.maintenance().server().sendAsync(new CompactDatabaseOperation(settings));
            operation.waitForCompletion();
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region compact_4
            // get all index names
            String[] indexNames = store.maintenance().send(new GetIndexNamesOperation(0, Integer.MAX_VALUE));

            CompactSettings settings = new CompactSettings();
            settings.setDatabaseName("Northwind");
            settings.setDocuments(true);
            settings.setIndexes(indexNames);

            // compact entire database: documents + all indexes
            Operation operation = store.maintenance().server().sendAsync(new CompactDatabaseOperation(settings));
            operation.waitForCompletion();
            //endregion
        }

    }
}
