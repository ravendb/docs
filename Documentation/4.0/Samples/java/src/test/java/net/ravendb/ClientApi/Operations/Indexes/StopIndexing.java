package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.StopIndexingOperation;

public class StopIndexing {

    private interface IFoo {
        /*
        //region stop_1
        public StopIndexingOperation()
        //endregion
        */
    }

    public StopIndexing() {
        try (IDocumentStore store = new DocumentStore()) {
            //region stop_2
            store.maintenance().send(new StopIndexingOperation());
            //endregion
        }
    }
}
