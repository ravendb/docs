package net.ravendb.clientapi.commands.howto;

import net.ravendb.abstractions.data.AdminStatistics;
import net.ravendb.abstractions.data.DatabaseStatistics;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class GetDatabaseAndServerStatistics {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region database_statistics_1
    public DatabaseStatistics getStatistics();
    //endregion
  }

  @SuppressWarnings("unused")
  private interface IFoo2 {
    //region server_statistics_1
    public AdminStatistics getStatistics();
    //endregion
  }

  @SuppressWarnings("unused")
  public GetDatabaseAndServerStatistics() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region database_statistics_2
      DatabaseStatistics statistics = store.getDatabaseCommands().getStatistics();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region server_statistics_2
      AdminStatistics serverStatistics = store.getDatabaseCommands().getGlobalAdmin().getStatistics();
      //endregion
    }
  }
}
