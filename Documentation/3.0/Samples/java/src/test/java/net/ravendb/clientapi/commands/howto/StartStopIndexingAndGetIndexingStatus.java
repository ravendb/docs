package net.ravendb.clientapi.commands.howto;

import static org.junit.Assert.assertEquals;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class StartStopIndexingAndGetIndexingStatus {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region start_indexing_1
    public void startIndexing();
    //endregion

    //region stop_indexing_1
    public void stopIndexing();
    //endregion

    //region get_indexing_status_1
    public String getIndexingStatus();
    //endregion
  }

  public StartStopIndexingAndGetIndexingStatus() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region start_indexing_2
      store.getDatabaseCommands().getAdmin().stopIndexing();
      //endregion

      //region stop_indexing_2
      store.getDatabaseCommands().getAdmin().startIndexing();
      //endregion

      //region get_indexing_status_2
      store.getDatabaseCommands().getAdmin().stopIndexing();
      assertEquals("Paused", store.getDatabaseCommands().getAdmin().getIndexingStatus());
      store.getDatabaseCommands().getAdmin().startIndexing();
      assertEquals("Indexing", store.getDatabaseCommands().getAdmin().getIndexingStatus());
      //endregion
    }
  }
}
