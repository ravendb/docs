package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.DeleteIndexOperation;

public class DeleteIndex {
    private interface IFoo {
        /*
        //region delete_1
        public DeleteIndexOperation(String indexName)
        //endregion
         */
    }

    public DeleteIndex() {
        try (IDocumentStore store = new DocumentStore()) {
            //region delete_2
            store.maintenance().send(new DeleteIndexOperation("Orders/Totals"));
            //endregion
        }
    }
}
