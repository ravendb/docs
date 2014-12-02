package net.ravendb.clientapi.commands.documents;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Delete {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region delete_1
    public void delete(String key, Etag etag);
    //endregion
  }

  public Delete() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region delete_2
      store.getDatabaseCommands().delete("employees/1", null);
      //endregion
    }
  }
}
