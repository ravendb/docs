package net.ravendb.clientapi;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class CreatingDocumentStore {
  public CreatingDocumentStore() throws Exception {
    //region document_store_creation
    try (IDocumentStore store = new DocumentStore("http://localhost:8080").initialize()) {

    }
    //endregion
  }

  //region document_store_holder
  public static class DocumentStoreHolder {
    private static IDocumentStore store;

    public static IDocumentStore getInstance() {
      if (store == null) {
        synchronized (DocumentStoreHolder.class) {
          if (store == null) {
            store = createStore();
          }
        }
      }
      return store;
    }

    private static IDocumentStore createStore() {
      IDocumentStore store = new DocumentStore("http://localhost:8080", "Northwind");
      store.initialize();
      return store;
    }
  }
  //endregion

}
