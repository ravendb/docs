import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function toggleDatabasesState() {
    {
        //region enable
        // Define the toggle state operation
        // specify the database name & pass 'false' to enable
        const enableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", disable: false);

        // To enable multiple databases use:
        // const enableDatabaseOp =
        //     new ToggleDatabasesStateOperation(["DB1", "DB2", ...], disable: false);

        // Execute the operation by passing it to maintenance.server.send
        const toggleResult = await documentStore.maintenance.server.send(enableDatabaseOp);
        //endregion
    }
    {
        //region disable
        // Define the toggle state operation
        // specify the database name(s) & pass 'true' to disable
        const disableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", disable: true);

        // To disable multiple databases use:
        // const disableDatabaseOp =
        //     new ToggleDatabasesStateOperation(["DB1", "DB2", ...], disable: true);
        
        // Execute the operation by passing it to maintenance.server.send
        const toggleResult = await documentStore.maintenance.server.send(disableDatabaseOp);
        //endregion
    }
}

{
    //region syntax_1
    // Available overloads:
    const enableDatabaseOp = new ToggleDatabasesStateOperation(databaseName, disable);
    const enableDatabaseOp = new ToggleDatabasesStateOperation(databaseNames, disable);
    //endregion

    //region syntax_2
    // Executing the operation returns an object with the following properties:
    {
        disabled, // Is database disabled
        name,     // Name of the database
        success,  // Has request succeeded
        reason    // Reason for success or failure
    }
    //endregion
}
