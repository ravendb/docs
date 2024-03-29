package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.indexes.StartIndexingOperation;

public class ResumeIndexing {

    private interface IFoo {
        /*
        //region start_1
        public StartIndexingOperation()
        //endregion
        */
    }

    public ResumeIndexing() {
        try (IDocumentStore store = new DocumentStore()) {
            //region start_2
            store.maintenance().send(new StartIndexingOperation());
            //endregion
        }
    }
}
