package net.ravendb.clientapi.commands.indexes.howto;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class ResetIndex {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region reset_index_1
    public void resetIndex(String name);
    //endregion
  }

  public ResetIndex() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region reset_index_2
      store.getDatabaseCommands().resetIndex("Orders/Totals");
      //endregion
    }
  }
}
