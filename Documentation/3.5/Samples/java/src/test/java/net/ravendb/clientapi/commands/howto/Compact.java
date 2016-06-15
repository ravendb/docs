package net.ravendb.clientapi.commands.howto;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.connection.Operation;
import net.ravendb.client.document.DocumentStore;


public class Compact {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region compact_1
    public Operation compactDatabase(String databaseName);
    //endregion
  }

  public Compact() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
		//region compact_2
		Operation operation = store.getDatabaseCommands().getGlobalAdmin().compactDatabase("Northwind");
		operation.waitForCompletion();
		//endregion
    }
  }
}
