import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function putClientConfig() {
    {
        //region put_config_1
        // You can customize the client-configuration options in the client
        // when creating the Document Store (this is optional):
        // =================================================================
        
        const documentStore = new DocumentStore(["serverUrl_1", "serverUrl_2", "..."], "DefaultDB");
        
        documentStore.conventions.maxNumberOfRequestsPerSession = 100;
        documentStore.conventions.identityPartsSeparator = '$';
        // ...
        
        documentStore.initialize();
        //endregion
    }
    {
        //region put_config_2
        // Override the initial client-configuration in the server using the put operation:
        // ================================================================================

        // Define the client-configuration object
        const clientConfiguration = {
            maxNumberOfRequestsPerSession: 200,
            readBalanceBehavior: "FastestNode",
            // ...
        };

        // Define the put client-configuration operation, pass the configuration 
        const putClientConfigOp = new PutClientConfigurationOperation(clientConfiguration);

        // Execute the operation by passing it to maintenance.send
        await documentStore.maintenance.send(putClientConfigOp);
        //endregion
    }
}

{
    //region syntax_1
    const putClientConfigOp = new PutClientConfigurationOperation(configuration);
    //endregion

    //region syntax_2
    // The client-configuration object
    {
        identityPartsSeparator,
        etag,
        disabled,
        maxNumberOfRequestsPerSession,
        readBalanceBehavior,
        loadBalanceBehavior,
        loadBalancerContextSeed
    }
    //endregion
}
