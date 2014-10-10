package net.ravendb.clientapi.commands.howto;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Compact {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region compact_1
    public void compactDatabase(String databaseName);
    //endregion
  }

  public Compact() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region compact_2
      store.getDatabaseCommands().getGlobalAdmin().compactDatabase("Northwind");
      //endregion
    }
  }
}
