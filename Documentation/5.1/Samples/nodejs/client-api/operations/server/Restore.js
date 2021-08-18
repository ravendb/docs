import { DocumentStore } from 'ravendb';

//document_store_creation
const store = new DocumentStore(["http://localhost:8080"], "Northwind2");
store.initialize();
//const session = store.openSession();

//region restore_1
export class RestoreBackupOperation implements IServerOperation<OperationIdResult> {
    public constructor(restoreConfiguration: RestoreBackupConfigurationBase, nodeTag?: string)
    //endregion
}

//region restore_2
export interface RestoreBackupConfigurationBase {
    databaseName: string;
    lastFileNameToRestore: string;
    dataDirectory: string;
    encryptionKey: string;
    disableOngoingTasks: boolean;
    skipIndexes: boolean;
    type: RestoreType;
    backupEncryptionSettings: BackupEncryptionSettings;
}
    //endregion

//region restore_3
restoreConfiguration = {
    databaseName: "Northwind";
    skipIndexes: false;
}
const restoreBackupOperation = RestoreBackupOperation(restoreConfiguration)
const restoreResult = await store.maintenance.server.send(restoreBackupOperation);
//endregion
