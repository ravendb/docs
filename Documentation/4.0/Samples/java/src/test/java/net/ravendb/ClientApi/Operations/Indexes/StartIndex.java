package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.StartIndexOperation;

public class StartIndex {

    private interface IFoo {
        /*
        //region start_1
        public StartIndexOperation(String indexName)
        //endregion
        */
    }

    public StartIndex() {
        try (IDocumentStore store = new DocumentStore()) {
            //region start_2
            store.maintenance().send(new StartIndexOperation("Orders/Totals"));
            //endregion
        }
    }
}
