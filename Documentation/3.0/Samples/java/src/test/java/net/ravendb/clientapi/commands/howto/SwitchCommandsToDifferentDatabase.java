package net.ravendb.clientapi.commands.howto;

import net.ravendb.client.IDocumentStore;
import net.ravendb.client.connection.IDatabaseCommands;
import net.ravendb.client.document.DocumentStore;


public class SwitchCommandsToDifferentDatabase {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region for_database_1
    public IDatabaseCommands forDatabase(String database);
    //endregion

    //region for_database_2
    public IDatabaseCommands forSystemDatabase();
    //endregion
  }

  @SuppressWarnings("unused")
  public SwitchCommandsToDifferentDatabase() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region for_database_3
      IDatabaseCommands commands = store.getDatabaseCommands().forDatabase("otherDatabase");
      //endregion

      //region for_database_4
      IDatabaseCommands systemCommands = store.getDatabaseCommands().forSystemDatabase();
      //endregion
    }
  }
}
