package net.ravendb.clientapi.commands.indexes;

import java.util.Collection;

import net.ravendb.abstractions.indexing.IndexDefinition;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Get {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_1_0
    public IndexDefinition getIndex(String name);
    //endregion

    //region get_2_0
    public Collection<IndexDefinition> getIndexes(int start, int pageSize);
    //endregion

    //region get_3_0
    public Collection<String> getIndexNames(int start, int pageSize);
    //endregion
  }

  @SuppressWarnings("unused")
  public Get() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_1_1
      IndexDefinition index = store.getDatabaseCommands().getIndex("Orders/Totals");
      //endregion
      //region get_2_1
      Collection<IndexDefinition> indexes = store.getDatabaseCommands().getIndexes(0, 10);
      //endregion
      //region get_3_1
      Collection<String> indexNames = store.getDatabaseCommands().getIndexNames(0, 10);
      //endregion
    }
  }
}
