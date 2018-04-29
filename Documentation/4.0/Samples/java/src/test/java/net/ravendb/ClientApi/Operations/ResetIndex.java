package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.ResetIndexOperation;

public class ResetIndex {

    private interface IFoo {
        /*
        //region reset_index_1
        public ResetIndexOperation(String indexName)
        //endregion
        */
    }

    public ResetIndex() {
        try (IDocumentStore store = new DocumentStore()) {
            //region reset_index_2
            store.maintenance()
                .send(new ResetIndexOperation("Orders/Totals"));
            //endregion
        }
    }
}
