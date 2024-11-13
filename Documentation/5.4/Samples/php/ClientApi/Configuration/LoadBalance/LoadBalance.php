<?php

namespace RavenDB\Samples\ClientApi\Configuration\LoadBalance;

use RavenDB\Documents\Conventions\DocumentConventions;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Configuration\ClientConfiguration;
use RavenDB\Documents\Operations\Configuration\PutClientConfigurationOperation;
use RavenDB\Http\LoadBalanceBehavior;
use RavenDB\Samples\Infrastructure\Entity\Employee;

class LoadBalance
{
    public function loadBalanceExamples(): void
    {
        # region LoadBalance_1
        // Initialize 'LoadBalanceBehavior' on the client:
        $documentStore = new DocumentStore(["ServerURL_1", "ServerURL_2", "..."], "DefaultDB");

        $conventions = new DocumentConventions();
        // Enable the session-context feature
        // If this is not enabled then a context string set in a session will be ignored
        $conventions->setLoadBalanceBehavior(LoadBalanceBehavior::useSessionContext());


        // Assign a method that sets the default context string
        // This string will be used for sessions that do Not provide a context string
        // A sample GetDefaultContext method is defined below
        $conventions->setLoadBalancerPerSessionContextSelector(\Closure::fromCallable([$this, 'GetDefaultContext']));

        // Set a seed
        // The seed is 0 by default, provide any number to override
        $conventions->setLoadBalancerContextSeed(5);

        $documentStore->setConventions($conventions);
        $documentStore->initialize();
        # endregion

        # region LoadBalance_2
        // Open a session that will use the DEFAULT store values:
        $session = $documentStore->openSession();
        try {
            // For all Read & Write requests made in this session,
            // node to access is calculated from string & seed values defined on the store
            $employee = $session->load(Employee::class, "employees/1-A");
        } finally {
            $session->close();
        }
        # endregion

        # region LoadBalance_3
        // Open a session that will use a UNIQUE context string:
        $session = $documentStore->openSession();
        try {
            // Call SetContext, pass a unique context string for this session
            $session->advanced()->getSessionInfo()->setContext("SomeOtherContext");

            // For all Read & Write requests made in this session,
            // node to access is calculated from the unique string & the seed defined on the store
            $employee = $session->load(Employee::class, "employees/1-A");
        } finally {
            $session->close();
        }
        # endregion

        # region LoadBalance_4
        // Setting 'LoadBalanceBehavior' on the server by sending an operation:
        $documentStore = new DocumentStore();
        try {
            // Define the client configuration to put on the server
            $configurationToSave = new ClientConfiguration();
            // Enable the session-context feature
            // If this is not enabled then a context string set in a session will be ignored
            $configurationToSave->setLoadBalanceBehavior(LoadBalanceBehavior::useSessionContext());

            // Set a seed
            // The seed is 0 by default, provide any number to override
            $configurationToSave->setLoadBalancerContextSeed(10);

            // NOTE:
            // The session's context string is Not set on the server
            // You still need to set it on the client:
            //   * either as a convention on the document store
            //   * or pass it to 'SetContext' method on the session

            // Configuration will be in effect when Disabled is set to false
            $configurationToSave->setDisabled(false);


            // Define the put configuration operation for the DEFAULT database
            $putConfigurationOp = new PutClientConfigurationOperation($configurationToSave);

            // Execute the operation by passing it to Maintenance.Send
            $documentStore->maintenance()->send($putConfigurationOp);

            // After the operation has executed:
            // all Read & Write requests, per session, will address the node calculated from:
            //   * the seed set on the server &
            //   * the session's context string set on the client
        } finally {
            $documentStore->close();
        }
        # endregion

        # region LoadBalance_5
        // Setting 'LoadBalanceBehavior' on the server by sending an operation:
        $documentStore = new DocumentStore();
        try {
            // Define the client configuration to put on the server
            $configurationToSave = new ClientConfiguration();
            // Enable the session-context feature
            // If this is not enabled then a context string set in a session will be ignored
            $configurationToSave->setLoadBalanceBehavior(LoadBalanceBehavior::useSessionContext());

            // Set a seed
            // The seed is 0 by default, provide any number to override
            $configurationToSave->setLoadBalancerContextSeed(10);

            // NOTE:
            // The session's context string is Not set on the server
            // You still need to set it on the client:
            //   * either as a convention on the document store
            //   * or pass it to 'SetContext' method on the session

            // Configuration will be in effect when Disabled is set to false
            $configurationToSave->setDisabled(false);


            // Define the put configuration operation for ALL databases
            $putConfigurationOp = new PutServerWideClientConfigurationOperation($configurationToSave);

            // Execute the operation by passing it to Maintenance.Server.Send
            $documentStore->maintenance()->server()->send($putConfigurationOp);

            // After the operation has executed:
            // all Read & Write requests, per session, will address the node calculated from:
            //   * the seed set on the server &
            //   * the session's context string set on the client
        } finally {
            $documentStore->close();
        }
        # endregion
    }

    # region LoadBalance_6
    // A customized method for getting a default context string
    private function GetDefaultContext(string $dbName): string
    {
        // Method is invoked by RavenDB with the database name
        // Use that name - or return any string of your choice
        return "DefaultContextString";
    }
    # endregion
}
