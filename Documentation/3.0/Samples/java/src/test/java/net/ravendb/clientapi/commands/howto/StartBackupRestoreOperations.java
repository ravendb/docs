package net.ravendb.clientapi.commands.howto;

import net.ravendb.abstractions.data.DatabaseDocument;
import net.ravendb.abstractions.data.DatabaseRestoreRequest;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.connection.Operation;
import net.ravendb.client.document.DocumentStore;


public class StartBackupRestoreOperations {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region backup_restore_1
    public void startBackup(String backupLocation, DatabaseDocument databaseDocument, boolean incremental, String databaseName);
    //endregion

    //region backup_restore_2
    public Operation startRestore(DatabaseRestoreRequest restoreRequest);
    //endregion
  }

  public StartBackupRestoreOperations() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region backup_restore_3
      store.getDatabaseCommands().getGlobalAdmin().startBackup(
        "c:\\temp\\backup\\Northwind\\",
        new DatabaseDocument(),
        false,
        "Northwind"
        );
      //endregion

      //region backup_restore_4
      DatabaseRestoreRequest restoreRequest = new DatabaseRestoreRequest();
      restoreRequest.setBackupLocation("c:\\temp\\backup\\Northwind\\");
      restoreRequest.setDatabaseLocation("~\\Databases\\NewNorthwind\\");
      restoreRequest.setDatabaseName("NewNorthwind");
      Operation operation = store.getDatabaseCommands().getGlobalAdmin().startRestore(restoreRequest);
      operation.waitForCompletion();
      //endregion
    }
  }
}
