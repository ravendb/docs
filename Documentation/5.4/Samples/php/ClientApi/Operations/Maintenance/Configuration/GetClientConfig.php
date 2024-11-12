<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Configuration;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Configuration\GetClientConfigurationOperation;
use RavenDB\Documents\Operations\Configuration\GetClientConfigurationResult;

class GetClientConfig
{
    public function GetClientConfig(): void
    {
        $store = new DocumentStore();
        try {
            # region get_config
            // Define the get client-configuration operation
            $getClientConfigOp = new GetClientConfigurationOperation();

            // Execute the operation by passing it to Maintenance.Send
            /** @var GetClientConfigurationResult $result */
            $result = $store->maintenance()->send($getClientConfigOp);

            $clientConfiguration = $result->getConfiguration();
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{

    # region syntax_1
    public GetClientConfigurationOperation()
    # endregion

    # region syntax_2
    // Executing the operation returns the following object:
    class GetClientConfigurationResult implements ResultInterface
        private ?int $etag = null;
        private ?ClientConfiguration $configuration;

        // ... getters and setters
    }
    # endregion
}
*/
