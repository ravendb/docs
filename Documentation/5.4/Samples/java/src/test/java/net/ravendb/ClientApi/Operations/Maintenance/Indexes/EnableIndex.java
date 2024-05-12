package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.EnableIndexOperation;

public class EnableIndex {

    private interface IFoo {
        /*
        //region enable_1
        public EnableIndexOperation(String indexName)
        //endregion
        */
    }

    public EnableIndex() {
        try (IDocumentStore store = new DocumentStore()) {
            //region enable_2
            store.maintenance().send(new EnableIndexOperation("Orders/Totals"));
            //endregion
        }
    }
}
