package net.ravendb.clientapi.commands.indexes.howto;

import net.ravendb.abstractions.indexing.IndexDefinition;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class IndexHasChanged {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region index_has_changed_1
    public boolean indexHasChanged(String name, IndexDefinition indexDef);
    //endregion
  }

  public IndexHasChanged() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      IndexDefinition indexDef = null;
      //region index_has_changed_2
      store.getDatabaseCommands().indexHasChanged("Orders/Totals", indexDef);
      //endregion
    }
  }
}
