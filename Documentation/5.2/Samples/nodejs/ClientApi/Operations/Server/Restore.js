import {DocumentStore, RestoreBackupOperation} from 'ravendb';
import {IServerOperation} from "ravendb";
import {OperationIdResult} from "ravendb";

let restoreConfiguration;
//document_store_creation
const store = new DocumentStore(["http://localhost:8080"], "Northwind2");
store.initialize();
//const session = store.openSession();
{
    //region restore_1
    const restoreBackupOperation = new RestoreBackupOperation(restoreConfiguration, "nodeTag");
    //endregion
}

//region restore_2
export interface RestoreBackupConfigurationBase {
    databaseName,
    lastFileNameToRestore,
    dataDirectory,
    encryptionKey,
    disableOngoingTasks,
    skipIndexes,
    type,
    backupEncryptionSettings
}
//endregion
{
//region restore_3
    restoreConfiguration = {
        databaseName: "Northwind",
        skipIndexes: false
    }
    const restoreBackupOperation = RestoreBackupOperation(restoreConfiguration, "A");
    const restoreResult = await store.maintenance.server.send(restoreBackupOperation);
//endregion
}