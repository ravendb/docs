package net.ravendb.ClientApi;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;

public class CreatingDocumentStore {
    public CreatingDocumentStore() {
        //region document_store_creation
        try (IDocumentStore store = new DocumentStore( new String[]{ "http://localhost:8080" }, "Northwind")) {
            store.initialize();


        }
        //endregion
    }



    //region document_store_holder
    public static class DocumentStoreHolder {

        private static IDocumentStore store;

        static {
            store = new DocumentStore(new String[]{ "http://localhost:8080" }, "Northwind");
        }

        public static IDocumentStore getStore() {
            return store;
        }
    }
    //endregion
}
