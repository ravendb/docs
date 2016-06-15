package net.ravendb.clientapi.commands.indexes.howto;

import net.ravendb.abstractions.indexing.IndexLockMode;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class ChangeIndexLockMode {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region change_index_lock_1
    void setIndexLock(String name, IndexLockMode lockMode);
    //endregion
  }

  public ChangeIndexLockMode() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region change_index_lock_2
      store
        .getDatabaseCommands()
        .setIndexLock("Orders/Totals", IndexLockMode.LOCKED_IGNORE);
      //endregion
    }
  }
}
