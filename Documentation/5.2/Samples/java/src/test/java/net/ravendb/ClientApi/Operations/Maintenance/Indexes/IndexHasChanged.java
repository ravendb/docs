package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.operations.indexes.IndexHasChangedOperation;

public class IndexHasChanged {


    private interface IFoo {
        /*
        //region index_has_changed_1
        public IndexHasChangedOperation(IndexDefinition definition)
        //endregion
        */
    }

    public IndexHasChanged() {
        try (IDocumentStore store = new DocumentStore()) {
            IndexDefinition ordersIndexDefinition = null;
            //region index_has_changed_2
            Boolean ordersIndexHasChanged =
                store.maintenance().send(new IndexHasChangedOperation(ordersIndexDefinition));
            //endregion
        }
    }
}
