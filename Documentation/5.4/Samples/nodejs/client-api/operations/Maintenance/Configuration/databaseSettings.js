import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function databaseSettings() {
    {
        //region put_database_settings
        // 1. Modify the database settings:
        // ================================
        
        // Define a settings object with key-value pairs to set, for example:
        const settings = {
            "Databases.QueryTimeoutInSec": "350",
            "Indexing.Static.DeploymentMode": "Rolling"
        };

        // Define the put database settings operation,
        // specify the database name & pass the settings dictionary
        const putDatabaseSettingsOp = new PutDatabaseSettingsOperation(documentStore.database, settings)

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(putDatabaseSettingsOp);

        // 2. RELOAD the database for the change to take effect:
        // =====================================================
        
        // Disable database
        const disableDatabaseOp = new ToggleDatabasesStateOperation(documentStore.database, true);
        await documentStore.maintenance.server.send(disableDatabaseOp);
        
        // Enable database
        const enableDatabaseOp = new ToggleDatabasesStateOperation(documentStore.database, false);
        await documentStore.maintenance.server.send(enableDatabaseOp);
        //endregion
    }
    {
        //region get_database_settings
        // Define the get database settings operation, specify the database name
        const getDatabaseSettingsOp = new GetDatabaseSettingsOperation(documentStore.database);

        // Execute the operation by passing it to maintenance.send
        const customizedSettings = await documentStore.maintenance.send(getDatabaseSettingsOp);

        // Get the customized value
        const customizedValue = customizedSettings.settings["Databases.QueryTimeoutInSec"];
        //endregion
    }
}

{
    //region syntax_1
    const putDatabaseSettingsOp = new PutDatabaseSettingsOperation(databaseName, configurationSettings)
    //endregion

    //region syntax_2
    const getDatabaseSettingsOp = new GetDatabaseSettingsOperation(databaseName);
    //endregion

    //region syntax_3
    // Executing the operation returns the following object:
    {
        settings // An object with key-value configuration pairs
    }
    //endregion
}
