package net.ravendb.ClientApi.Operations.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.MaintenanceOperationExecutor;
import net.ravendb.client.documents.operations.OperationExecutor;

public class SwitchOperationsToDifferentDatabase {
    private interface ForDatabase1 {
        //region for_database_1
        OperationExecutor forDatabase(String databaseName);
        //endregion
    }

    private interface ForDatabase2 {
        //region for_database_2
        MaintenanceOperationExecutor forDatabase(String databaseName);
        //endregion
    }

    public SwitchOperationsToDifferentDatabase() {
        try (IDocumentStore documentStore = new DocumentStore()) {
            //region for_database_3
            OperationExecutor operations = documentStore.operations().forDatabase("otherDatabase");
            //endregion

            //region for_database_4
            MaintenanceOperationExecutor maintenanceOperations = documentStore.maintenance().forDatabase("otherDatabase");
            //endregion
        }
    }
}
