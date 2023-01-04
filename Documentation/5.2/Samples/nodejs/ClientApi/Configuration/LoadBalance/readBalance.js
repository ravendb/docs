import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function readBalance() {
    {
        //region readBalance_1
        // Initialize 'readBalanceBehavior' on the client: 
        // ===============================================
        
        const documentStore = new DocumentStore(["serverUrl_1", "serverUrl_2", "..."], "DefaultDB");

        // For example:
        // With readBalanceBehavior set to: 'FastestNode':
        // Client READ requests will address the fastest node
        // Client WRITE requests will address the preferred node
        documentStore.conventions.readBalanceBehavior = "FastestNode";
        
        documentStore.initialize();
        //endregion    
    }
    {
        //region readBalance_2
        // Setting 'readBalanceBehavior' on the server by sending an operation:
        // ====================================================================
        
        // Define the client configuration to put on the server
        const configurationToSave = {
            // Replace 'FastestNode' (from example above) with 'RoundRobin'
            readBalanceBehavior: "RoundRobin"
        };
        
        // Define the put configuration operation for the DEFAULT database
        const putConfigurationOp = new PutClientConfigurationOperation(configurationToSave));

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(putConfigurationOp);

        // After the operation has executed:
        // All WRITE requests will continue to address the preferred node 
        // READ requests, per session, will address a different node based on the RoundRobin logic
        //endregion
    }
    {
        //region readBalance_3
        // Setting 'readBalanceBehavior' on the server by sending an operation:
        // ====================================================================

        // Define the client configuration to put on the server
        const configurationToSave = {
            // Replace 'FastestNode' (from example above) with 'RoundRobin'
            readBalanceBehavior: "RoundRobin"
        };

        // Define the put configuration operation for ALL databases
        const putConfigurationOp = new PutServerWideClientConfigurationOperation(configurationToSave));

        // Execute the operation by passing it to maintenance.server.send
        await documentStore.maintenance.server.send(putConfigurationOp);

        // After the operation has executed:
        // All WRITE requests will continue to address the preferred node 
        // READ requests, per session, will address a different node based on the RoundRobin logic
        //endregion
    }
}
