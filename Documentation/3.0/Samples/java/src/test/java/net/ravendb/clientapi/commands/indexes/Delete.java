package net.ravendb.clientapi.commands.indexes;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Delete {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region delete_1
    public void deleteIndex(final String name);
    //endregion
  }

  public Delete() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region delete_2
      store.getDatabaseCommands().deleteIndex("Orders/Totals");
      //endregion
    }
  }
}
