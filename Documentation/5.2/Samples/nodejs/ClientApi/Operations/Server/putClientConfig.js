import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function putClientConfig() {
    {
        //region put_config
        // Define the client-configuration object
        const clientConfiguration = {
            maxNumberOfRequestsPerSession: 200,
            readBalanceBehavior: "FastestNode",
            // ...
        };
        
        // Define the put server-wide client-configuration operation, pass the configuration 
        const putServerWideClientConfigOp = 
            new PutServerWideClientConfigurationOperation(clientConfiguration);
        
        // Execute the operation by passing it to maintenance.server.send
        await documentStore.maintenance.server.send(putServerWideClientConfigOp);
        //endregion
    }
}

{
    //region syntax_1
    const putServerWideClientConfigOp = new PutServerWideClientConfigurationOperation(configuration);
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
