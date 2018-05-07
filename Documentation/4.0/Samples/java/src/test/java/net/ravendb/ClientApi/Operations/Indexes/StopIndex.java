package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.StopIndexOperation;

public class StopIndex {

    private interface IFoo {
        /*
        //region stop_1
        public StopIndexOperation(String indexName)
        //endregion
        */
    }

    public StopIndex() {
        try (IDocumentStore store = new DocumentStore()) {
            //region stop_2
            store.maintenance().send(new StopIndexOperation("Orders/Totals"));
            //endregion
        }
    }
}
