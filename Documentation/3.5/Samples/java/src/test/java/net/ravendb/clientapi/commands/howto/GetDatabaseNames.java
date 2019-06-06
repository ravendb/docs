package net.ravendb.clientapi.commands.howto;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class GetDatabaseNames {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_database_names_1
    public String[] getDatabaseNames(int pageSize);

    public String[] getDatabaseNames(int pageSize, int start);
    //endregion
  }

  @SuppressWarnings("unused")
  public GetDatabaseNames() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_database_names_2
      String[] databaseNames =
        store.getDatabaseCommands().getGlobalAdmin().getDatabaseNames(10);
      //endregion
    }
  }
}
