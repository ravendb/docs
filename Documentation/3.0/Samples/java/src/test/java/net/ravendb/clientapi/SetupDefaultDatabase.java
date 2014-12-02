package net.ravendb.clientapi;

import net.ravendb.abstractions.data.Constants;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.connection.IDatabaseCommands;
import net.ravendb.client.document.DocumentStore;


public class SetupDefaultDatabase {

  @SuppressWarnings("unused")
  public SetupDefaultDatabase() throws Exception {
    //region default_database_1
    // without specifying `DefaultDb`
    // created `DatabaseCommands` or opened `Sessions`
    // will work on `<system>` database by default
    // if no database is passed explicitly
    try (IDocumentStore store = new DocumentStore("http://localhost:8080").initialize()) {
      IDatabaseCommands commands = store.getDatabaseCommands();
      try (IDocumentSession session = store.openSession()) {
        // ...
      }

      IDatabaseCommands northwindCommands = commands.forDatabase("Northwind");
      try (IDocumentSession northwindSession =  store.openSession("Northwind")) {
        // ...
      }
    }

    //endregion

    //region default_database_2
    // when `DefaultDatabase` is set to `Northwind`
    // created `DatabaseCommands` or opened `Sessions`
    // will work on `Northwind` database by default
    // if no database is passed explicitly
    try (IDocumentStore store = new DocumentStore("http://localhost:8080", "Northwind").initialize()) {
      IDatabaseCommands northwindCommands = store.getDatabaseCommands();
      try (IDocumentSession northwindSession = store.openSession()) {
        // ...
      }

      IDatabaseCommands adventureWorksCommands = northwindCommands.forDatabase("AdventureWorks");
      try (IDocumentSession adventureWorksSession = store.openSession("AdventureWorks")) {
        // ...
      }

      IDatabaseCommands systemCommands = northwindCommands.forSystemDatabase();
      try (IDocumentSession systemSession = store.openSession(Constants.SYSTEM_DATABASE)) {
        // ...
      }
    }

    //endregion
  }
}
