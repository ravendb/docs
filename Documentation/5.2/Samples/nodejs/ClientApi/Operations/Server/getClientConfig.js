import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getClientConfig() {
    {
        //region get_config

        // Define the get client-configuration operation
        const getServerWideClientConfigOp = new GetServerWideClientConfigurationOperation();

        // Execute the operation by passing it to maintenance.server.send
        const config = await documentStore.maintenance.server.send(getServerWideClientConfigOp);
        //endregion
    }
}

{
    //region syntax_1
    const getServerWideClientConfigOp = new GetServerWideClientConfigurationOperation();
    //endregion

    //region syntax_2
    // Executing the operation returns the client-configuration object: 
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
