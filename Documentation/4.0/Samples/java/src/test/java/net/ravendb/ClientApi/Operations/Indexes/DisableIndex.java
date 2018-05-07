package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.DisableIndexOperation;

public class DisableIndex {

    private interface IFoo {
        /*
        //region disable_1
        public DisableIndexOperation(String indexName)
        //endregion
        */
    }

    public DisableIndex() {
        try (IDocumentStore store = new DocumentStore()) {
            //region disable_2
            store.maintenance().send(new DisableIndexOperation("Orders/Totals"));
            // index is disabled at this point, new data won't be indexed
            // but you can still query on this index
            //endregion
        }
    }
}
