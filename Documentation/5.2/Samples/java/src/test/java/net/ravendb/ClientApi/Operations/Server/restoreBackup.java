import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.backups.BackupEncryptionSettings;
import net.ravendb.client.documents.operations.backups.RestoreBackupConfiguration;
import net.ravendb.client.documents.operations.backups.RestoreBackupConfigurationBase;
import net.ravendb.client.documents.operations.backups.RestoreBackupOperation;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.smuggler.DatabaseSmuggler;
import net.ravendb.client.documents.smuggler.DatabaseSmugglerExportOptions;
import net.ravendb.client.documents.smuggler.DatabaseSmugglerImportOptions;

public class restoreBackup {
    public static void main (String[] args){
        try (IDocumentStore store = new DocumentStore( new String[]{ "http://localhost:8080" }, "Northwind")) {
            store.initialize();

            try (IDocumentSession session = store.openSession()) {
                //ctor
                //region constructor
                 public RestoreBackupOperation(RestoreBackupConfigurationBase restoreConfiguration);

                 public RestoreBackupOperation(RestoreBackupConfigurationBase restoreConfiguration, String nodeTag);
                //endregion

                //region example_java
                RestoreBackupConfiguration config = new RestoreBackupConfiguration();
                config.setBackupLocation("C:\\backups\\Northwind");
                config.setDatabaseName("Northwind");
                RestoreBackupOperation restoreOperation = new RestoreBackupOperation(config);
                store.maintenance().server().send(restoreOperation);
                //endregion
            }

    }

}
    //region get_set
    public abstract class RestoreBackupConfigurationBase {


        public String getDatabaseName() {
            return databaseName;
        }

        public void setDatabaseName(String databaseName) {
            this.databaseName = databaseName;
        }

        public String getLastFileNameToRestore() {
            return lastFileNameToRestore;
        }

        public void setLastFileNameToRestore(String lastFileNameToRestore) {
            this.lastFileNameToRestore = lastFileNameToRestore;
        }

        public String getDataDirectory() {
            return dataDirectory;
        }

        public void setDataDirectory(String dataDirectory) {
            this.dataDirectory = dataDirectory;
        }

        public String getEncryptionKey() {
            return encryptionKey;
        }

        public void setEncryptionKey(String encryptionKey) {
            this.encryptionKey = encryptionKey;
        }

        public boolean isDisableOngoingTasks() {
            return disableOngoingTasks;
        }

        public void setDisableOngoingTasks(boolean disableOngoingTasks) {
            this.disableOngoingTasks = disableOngoingTasks;
        }

        public boolean isSkipIndexes() {
            return skipIndexes;
        }

        public void setSkipIndexes(boolean skipIndexes) {
            this.skipIndexes = skipIndexes;
        }

        public BackupEncryptionSettings getBackupEncryptionSettings() {
            return backupEncryptionSettings;
        }

        public void setBackupEncryptionSettings(BackupEncryptionSettings backupEncryptionSettings) {
            this.backupEncryptionSettings = backupEncryptionSettings;
        }
//endregion
    }
