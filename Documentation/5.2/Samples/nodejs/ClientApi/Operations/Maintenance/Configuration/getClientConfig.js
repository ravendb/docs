import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getClientConfig() {
    {
        //region get_config
        // Define the get client-configuration operation 
        const getClientConfigOp = new GetClientConfigurationOperation();

        // Execute the operation by passing it to maintenance.send
        const result = await store.maintenance.send(getClientConfigOp);
        
        const configuration = result.configuration;
        //endregion
    }
}

{
    //region syntax_1
    const getClientConfigOp = new GetClientConfigurationOperation();
    //endregion

    //region syntax_2
    // Object returned from store.maintenance.send(getClientConfigOp):
    {
       etag,
       configuration // The configution object
    }

    // The configuration object:
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
