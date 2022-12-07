import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function addDatabaseNode() {
    {
        //region add_1
        // Create AddDatabaseNodeOperation
        // Add a random node to 'Northwind' database-group
        const addDatabaseNodeOperation = new AddDatabaseNodeOperation("Northwind");
        
        // Send operation to the store
        const result = await documentStore.maintenance.server.send(addDatabaseNodeOperation);

        // Can access the new topology
        const numberOfReplicas = getAllNodesFromTopology(result.topology).length;
        //endregion    
    }
    {
        //region add_2
        // Create AddDatabaseNodeOperation
        // Add node C to 'Northwind' database-group
        const addDatabaseNodeOperation = new AddDatabaseNodeOperation("Northwind", "C"));

        // Send operation to the store
        const result = await documentStore.maintenance.server.send(addDatabaseNodeOperation);
        //endregion
    }
}

{
    //region syntax
    const addDatabaseNodeOperation = new AddDatabaseNodeOperation(databaseName, nodeTag?);
    //endregion
}
