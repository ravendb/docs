import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function compact() {
    {
        //region compact_0
        // Define the compact settings
        const compactSettings = {
            // Database to compact
            databaseName: "Northwind",

            // Set 'documents' to true to compact all documents in database
            // Indexes are not set and will not be compacted
            documents: true
        };

        // Define the compact operation, pass the settings
        const compactOp = new CompactDatabaseOperation(compactSettings);

        // Execute compaction by passing the operation to maintenance.server.send
        const asyncOperation = await documentStore.maintenance.server.send(compactOp);
        
        // Wait for operation to complete, during compaction the database is offline
        await asyncOperation.waitForCompletion();
        //endregion    
    }
    {
        //region compact_1
        // Define the compact settings
        const compactSettings = {
            // Database to compact
            databaseName: "Northwind",

            // Setting 'documents' to false will compact only the specified indexes
            documents: false,

            // Specify which indexes to compact
            indexes: ["Orders/Totals", "Orders/ByCompany"] 
        };

        // Define the compact operation, pass the settings
        const compactOp = new CompactDatabaseOperation(compactSettings);

        // Execute compaction by passing the operation to maintenance.server.send
        const asyncOperation = await documentStore.maintenance.server.send(compactOp);
        // Wait for operation to complete
        await asyncOperation.waitForCompletion();
        //endregion    
    }
    {
        //region compact_2
        // Get all indexes names in the database using the 'GetIndexNamesOperation' operation
        // Use 'forDatabase' if the target database is different than the default database defined on the store
        const allIndexNames = await documentStore.maintenance.forDatabase("Northwind")
            .send(new GetIndexNamesOperation(0, 50));

        // Define the compact settings
        const compactSettings = {
            databaseName: "Northwind", // Database to compact

            documents: true,           // Compact all documents

            indexes: allIndexNames,    // All indexes will be compacted
        };

        // Define the compact operation, pass the settings
        const compactOp = new CompactDatabaseOperation(settings);
    
        // Execute compaction by passing the operation to maintenance.server.send
        const asyncOperation = await documentStore.maintenance.server.send(compactOp);
        // Wait for operation to complete
        await asyncOperation.waitForCompletion();
        //endregion    
    }
    {
        //region compact_3
        // Get all member nodes in the database-group using the 'GetDatabaseRecordOperation' operation
        const databaseRecord =
            await documentStore.maintenance.server.send(new GetDatabaseRecordOperation("Northwind"));
        const allMemberNodes = databaseRecord.topology.members;

        // Define the compact settings as needed
        const compactSettings = {
            // Database to compact
            databaseName: "Northwind",

            //Compact all documents in database
            documents: true
        };

        // Execute the compact operation on each member node
        for (let i = 0; i < allMemberNodes.length; i++) {
            // Define the compact operation, pass the settings
            const compactOp = new CompactDatabaseOperation(compactSettings);
            
            // Execute the operation on a specific node
            // Use `forNode` to specify the node to operate on
            const serverOpExecutor = await documentStore.maintenance.server.forNode(allMemberNodes[i]);
            const asyncOperation = await serverOpExecutor.send(compactOp);
            
            // Wait for operation to complete
            await asyncOperation.waitForCompletion();
        }
        //endregion
    }
}

{
    //region syntax
    const compactOperation = new CompactDatabaseOperation(compactSettings);
    //endregion
}
