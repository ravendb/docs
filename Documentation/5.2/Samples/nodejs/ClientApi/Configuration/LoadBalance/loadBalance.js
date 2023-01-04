import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function loadBalance() {
    {
        //region loadBalance_1
        // Initialize 'loadBalanceBehavior' on the client: 
        // ===============================================

        const documentStore = new DocumentStore(["serverUrl_1", "serverUrl_2", "..."], "DefaultDB");

        // Enable the session-context feature
        // If this is not enabled then a context string set in a session will be ignored 
        documentStore.conventions.loadBalanceBehavior = "UseSessionContext";

        // Assign a method that sets the default context string
        // This string will be used for sessions that do Not provide a context string
        // A sample getDefaultContext method is defined below
        store.conventions.loadBalancerPerSessionContextSelector = getDefaultContext;
        
        // Set a seed
        // The seed is 0 by default, provide any number to override
        store.conventions.loadBalancerContextSeed = 5
        
        documentStore.initialize();
        //endregion
    }
    {
        //region loadBalance_2
        // Open a session that will use the DEFAULT store values:
        const session = documentStore.openSession();
        
        // For all Read & Write requests made in this session,
        // node to access is calculated from string & seed values defined on the store
        const employee = await session.load("employees/1-A");
        //endregion    
    }
    {
        //region loadBalance_3
        // Open a session that will use a UNIQUE context string:
        const session = documentStore.openSession();
        
        // Call setContext, pass a unique context string for this session
        session.advanced.sessionInfo.setContext("SomeOtherContext");

        // For all Read & Write requests made in this session,
        // node to access is calculated from the unique string & the seed defined on the store
        const employee = await session.load("employees/1-A");
        //endregion    
    }
    {
        //region loadBalance_4
        // Setting 'loadBalanceBehavior' on the server by sending an operation:
        // ====================================================================

        // Define the client configuration to put on the server
        const configurationToSave = {
            // Enable the session-context feature
            // If this is not enabled then a context string set in a session will be ignored 
            loadBalanceBehavior: "UseSessionContext",

            // Set a seed
            // The seed is 0 by default, provide any number to override
            loadBalancerContextSeed: 10,

            // NOTE:
            // The session's context string is Not set on the server
            // You still need to set it on the client:
            //   * either as a convention on the document store
            //   * or pass it to 'setContext' method on the session

            // Configuration will be in effect when Disabled is set to false
            disabled: false
        };

        // Define the put configuration operation for the DEFAULT database
        const putConfigurationOp = new PutClientConfigurationOperation(configurationToSave));

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(putConfigurationOp);

        // After the operation has executed:
        // all Read & Write requests, per session, will address the node calculated from:
        //   * the seed set on the server &
        //   * the session's context string set on the client
        //endregion
    }
    {
        //region loadBalance_5
        // Setting 'loadBalanceBehavior' on the server by sending an operation:
        // ====================================================================

        // Define the client configuration to put on the server
        const configurationToSave = {
            // Enable the session-context feature
            // If this is not enabled then a context string set in a session will be ignored 
            loadBalanceBehavior: "UseSessionContext",

            // Set a seed
            // The seed is 0 by default, provide any number to override
            loadBalancerContextSeed: 10,

            // NOTE:
            // The session's context string is Not set on the server
            // You still need to set it on the client:
            //   * either as a convention on the document store
            //   * or pass it to 'setContext' method on the session

            // Configuration will be in effect when Disabled is set to false
            disabled: false
        };

        // Define the put configuration operation for ALL databases
        const putConfigurationOp = new PutServerWideClientConfigurationOperation(configurationToSave));

        // Execute the operation by passing it to maintenance.server.send
        await documentStore.maintenance.server.send(putConfigurationOp);

        // After the operation has executed:
        // all Read & Write requests, per session, will address the node calculated from:
        //   * the seed set on the server &
        //   * the session's context string set on the client
        //endregion
    }
    {
        //region loadBalance_6
        // A customized method for getting a default context string 
        const getDefaultContext = (dbName) => {
            // Method is invoked by RavenDB with the database name
            // Use that name - or return any string of your choice
            return "defaultContextString";
        }
        //endregion
    }
}
