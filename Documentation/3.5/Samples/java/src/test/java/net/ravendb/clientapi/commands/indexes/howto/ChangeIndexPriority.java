package net.ravendb.clientapi.commands.indexes.howto;

import net.ravendb.abstractions.basic.UseSharpEnum;
import net.ravendb.abstractions.data.IndexStats.IndexingPriority;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class ChangeIndexPriority {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region change_index_priority_1
    public void setIndexPriority(String name, IndexingPriority priority);
    //endregion
  }

  @SuppressWarnings("unused")
  private static class Foo {
    //region change_index_priority_2
    @UseSharpEnum
    public static enum IndexingPriority {
      NONE(0),
      NORMAL(1),
      DISABLED(2),
      IDLE(4),
      ABANDONED(8),
      ERROR(16),
      FORCED(512);

      private int code;

      private IndexingPriority(int code) {
        this.code = code;
      }

      public int getCode() {
        return code;
      }
    }
    //endregion
  }

  public ChangeIndexPriority() {
    try (IDocumentStore store = new DocumentStore()) {
      //region change_index_priority_3
      store.getDatabaseCommands().setIndexPriority("Orders/Totals", IndexingPriority.DISABLED);
      //endregion
    }
  }
}
