<?php

namespace RavenDB\Samples\ClientApi\Configuration\LoadBalance;

use RavenDB\Documents\Conventions\DocumentConventions;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Configuration\ClientConfiguration;
use RavenDB\Documents\Operations\Configuration\PutClientConfigurationOperation;
use RavenDB\Http\ReadBalanceBehavior;

class ReadBalance
{
    public function samples(): void
    {
        # region ReadBalance_1
        // Initialize 'ReadBalanceBehavior' on the client:
        $documentStore = new DocumentStore(["ServerURL_1", "ServerURL_2", "..."], "DefaultDB");

        $conventions = new DocumentConventions();
        // With ReadBalanceBehavior set to: 'FastestNode':
        // Client READ requests will address the fastest node
        // Client WRITE requests will address the preferred node
        $conventions->setReadBalanceBehavior(ReadBalanceBehavior::fastestNode());

        $documentStore->setConventions($conventions);
        $documentStore->initialize();
        # endregion

        # region ReadBalance_2
        // Setting 'ReadBalanceBehavior' on the server by sending an operation:
        $documentStore = new DocumentStore();
        try {
            // Define the client configuration to put on the server
            $clientConfiguration = new ClientConfiguration();
            // Replace 'FastestNode' (from example above) with 'RoundRobin'
            $clientConfiguration->setReadBalanceBehavior(ReadBalanceBehavior::roundRobin());

            // Define the put configuration operation for the DEFAULT database
            $putConfigurationOp = new PutClientConfigurationOperation($clientConfiguration);

            // Execute the operation by passing it to Maintenance.Send
            $documentStore->maintenance()->send($putConfigurationOp);

            // After the operation has executed:
            // All WRITE requests will continue to address the preferred node
            // READ requests, per session, will address a different node based on the RoundRobin logic
        } finally {
            $documentStore->close();
        }
        # endregion

        # region ReadBalance_3
        // Setting 'ReadBalanceBehavior' on the server by sending an operation:
        $documentStore = new DocumentStore();
        try {
            // Define the client configuration to put on the server
            $clientConfiguration = new ClientConfiguration();

            // Replace 'FastestNode' (from example above) with 'RoundRobin'
            $clientConfiguration->setReadBalanceBehavior(ReadBalanceBehavior::roundRobin());

            // Define the put configuration operation for the ALL databases
            $putConfigurationOp = new PutServerWideClientConfigurationOperation($clientConfiguration);

            // Execute the operation by passing it to Maintenance.Server.Send
            $documentStore->maintenance()->server()->send($putConfigurationOp);

            // After the operation has executed:
            // All WRITE requests will continue to address the preferred node
            // READ requests, per session, will address a different node based on the RoundRobin logic
        } finally {
            $documentStore->close();
        }
        # endregion
    }
}
