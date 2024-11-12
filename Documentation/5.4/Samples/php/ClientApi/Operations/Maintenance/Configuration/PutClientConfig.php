<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Configuration;

use RavenDB\Documents\Conventions\DocumentConventions;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Configuration\ClientConfiguration;
use RavenDB\Documents\Operations\Configuration\PutClientConfigurationOperation;
use RavenDB\Http\ReadBalanceBehavior;

class PutClientConfig
{
    public function samples(): void
    {
        # region put_config_1
        // You can customize the client-configuration options in the client
        // when creating the Document Store (this is optional):
        // =================================================================

        $urls = ["ServerURL_1", "ServerURL_2", "..."];
        $database = "DefaultDB";

        $documentStore = new DocumentStore($urls, $database);

        $conventions = new DocumentConventions();
        $conventions->setMaxNumberOfRequestsPerSession(100);
        $conventions->setIdentityPartsSeparator('$');
        // ....

        $documentStore->setConventions($conventions);

        $documentStore->initialize();
        # endregion

        # region put_config_2
        // Override the initial client-configuration in the server using the put operation:
        // ================================================================================
        try {
            // Define the client-configuration object
            $clientConfiguration = new ClientConfiguration();
            $clientConfiguration->setMaxNumberOfRequestsPerSession(200);
            $clientConfiguration->setReadBalanceBehavior(ReadBalanceBehavior::fastestNode());
            // ...

            // Define the put client-configuration operation, pass the configuration
            $putClientConfigOp = new PutClientConfigurationOperation($clientConfiguration);

            // Execute the operation by passing it to Maintenance.Send
            $documentStore->maintenance()->send($putClientConfigOp);
        } finally {
            $documentStore->close();
        }
        # endregion
    }
}

/*
interface IFoo
{
    # region syntax_1
    PutClientConfigurationOperation(?ClientConfiguration $configuration)
    # endregion

    # region syntax_2
    class ClientConfiguration
    {
        private ?string $identityPartsSeparator = null;
        private ?int $etag = null;
        private bool $disabled = false;
        private ?int $maxNumberOfRequestsPerSession = null;
        private ?ReadBalanceBehavior $readBalanceBehavior = null;
        private ?LoadBalanceBehavior $loadBalanceBehavior = null;
        private ?int $loadBalancerContextSeed = null;

        // ... getters and setters
    }
    # endregion
}
*/
