package net.ravendb.clientapi.commands.howto;

import java.util.HashMap;
import java.util.Map;

import net.ravendb.abstractions.data.DatabaseDocument;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class CreateDeleteDatabase {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region ensure_database_exists_1
    public void ensureDatabaseExists(String name);
    public void ensureDatabaseExists(String name, boolean ignoreFailures);
    //endregion

    //region create_database_1
    public void createDatabase(DatabaseDocument databaseDocument);
    //endregion

    //region delete_database_1
    public void deleteDatabase(String dbName);
    public void deleteDatabase(String dbName, boolean hardDelete);
    //endregion
  }

  public CreateDeleteDatabase() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region create_database_2
      // create database 'NewDatabase' with 'PeriodicExport' bundle enabled
      DatabaseDocument databaseDocument = new DatabaseDocument();
      databaseDocument.setId("NewDatabase");
      Map<String, String> settings = new HashMap<>();
      settings.put("Raven/ActiveBundles", "PeriodicExport");
      settings.put("Raven/DataDir", "~\\Databases\\NewDatabase");
      databaseDocument.setSettings(settings);
      store.getDatabaseCommands().getGlobalAdmin().createDatabase(databaseDocument);
      //endregion

      //region delete_database_2
      store.getDatabaseCommands().getGlobalAdmin().deleteDatabase("NewDatabase", true);
      //endregion

      //region ensure_database_exists_3
      store.getDatabaseCommands().getGlobalAdmin().ensureDatabaseExists("NewDatabase", false);
      //endregion
    }
  }
}
