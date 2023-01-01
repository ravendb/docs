import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function addDatabaseNode() {
    {
        //region add_1
        // Create the AddDatabaseNodeOperation
        // Add a random node to 'Northwind' database-group
        const addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind");

        // Execute the operation by passing it to maintenance.server.send
        const result = await documentStore.maintenance.server.send(addDatabaseNodeOp);

        // Can access the new topology
        const numberOfReplicas = getAllNodesFromTopology(result.topology).length;
        //endregion    
    }
    {
        //region add_2
        // Create the AddDatabaseNodeOperation
        // Add node C to 'Northwind' database-group
        const addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind", "C"));

        // Execute the operation by passing it to maintenance.server.send
        const result = await documentStore.maintenance.server.send(addDatabaseNodeOp);
        //endregion
    }
}

{
    //region syntax
    const addDatabaseNodeOp = new AddDatabaseNodeOperation(databaseName, nodeTag?);
    //endregion
}
