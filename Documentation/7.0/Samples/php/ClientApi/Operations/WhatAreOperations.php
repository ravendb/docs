<?php

namespace RavenDB\Samples\ClientApi\Operations;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexRunningStatus;
use RavenDB\Documents\Indexes\IndexStats;
use RavenDB\Documents\Operations\Counters\CountersDetail;
use RavenDB\Documents\Operations\Counters\GetCountersOperation;
use RavenDB\Documents\Operations\DeleteByQueryOperation;
use RavenDB\Documents\Operations\Indexes\GetIndexStatisticsOperation;
use RavenDB\Documents\Operations\Indexes\StopIndexOperation;
use RavenDB\Documents\Operations\MaintenanceOperationInterface;
use RavenDB\Documents\Operations\Operation;
use RavenDB\Documents\Operations\OperationInterface;
use RavenDB\Exceptions\TimeoutException;
use RavenDB\Http\ResultInterface;
use RavenDB\ServerWide\Operations\BuildNumber;
use RavenDB\ServerWide\Operations\GetBuildNumberOperation;
use RavenDB\ServerWide\Operations\ServerOperationInterface;
use RavenDB\Type\Duration;

interface SendSyntaxInterface
{
    # region operations_send
    /**
     * Usage and available overloads:
     *
     *   - send(?OperationInterface $operation, ?SessionInfo $sessionInfo = null): ResultInterface;
     *   - send(string $entityClass, ?PatchOperation $operation, ?SessionInfo $sessionInfo = null): PatchOperationResult;
     *   - send(?PatchOperation $operation, ?SessionInfo $sessionInfo = null): PatchStatus;
     *
     * @param mixed ...$parameters
     */
    public function send(...$parameters);
    # endregion

    # region maintenance_send
    public function send(MaintenanceOperationInterface $operation): ResultInterface;
    # endregion

    # region server_send
    public function send(ServerOperationInterface $operation): ?object;
    # endregion

    # region waitForCompletion_syntax
    /**
     * Wait for operation completion.
     *
     * It throws TimeoutException if $duration is set and operation execution time elapses duration interval.
     *
     * Usage:
     *   - waitForCompletion(): void;               // It will wait until operation is finished
     *   - waitForCompletion(Duration $duration);   // It will wait for given duration
     *   - waitForCompletion(int $seconds);         // It will wait for given seconds
     *
     * @param Duration|int|null $duration
     */
    public function waitForCompletion(Duration|int|null $duration = null): void;
    # endregion
}


class WhatAreOperations
{
    public function examples(): void
    {
        $documentStore = new DocumentStore([ "http://localhost:8080" ], "Northwind");

        try {
            # region operations_ex
            // Define operation, e.g. get all counters info for a document
            $getCountersOp = new GetCountersOperation("products/1-A");

            // Execute the operation by passing the operation to Operations.Send
            /** @var CountersDetail $allCountersResult */
            $allCountersResult = $documentStore->operations()->send($getCountersOp);

            // Access the operation result
            $numberOfCounters = count($allCountersResult->getCounters());
            # endregion

            # region maintenance_ex
            // Define operation, e.g. stop an index
            $stopIndexOp = new StopIndexOperation("Orders/ByCompany");

            // Execute the operation by passing the operation to Maintenance.Send
            $documentStore->maintenance()->send($stopIndexOp);

            // This specific operation returns void
            // You can send another operation to verify the index running status
            $indexStatsOp = new GetIndexStatisticsOperation("Orders/ByCompany");
            /** @var IndexStats $indexStats */
            $indexStats =  $documentStore->maintenance()->send($indexStatsOp);

            /** @var IndexRunningStatus $status */
            $status = $indexStats->getStatus(); // will be "Paused"
            # endregion

            # region server_ex
            // Define operation, e.g. get the server build number
            $getBuildNumberOp = new GetBuildNumberOperation();

            // Execute the operation by passing the operation to Maintenance.Server.Send
            /** @var BuildNumber $buildNumberResult */
            $buildNumberResult = $documentStore->maintenance()->server()->send($getBuildNumberOp);

            // Access the operation result
            $version = $buildNumberResult->getBuildVersion();
            # endregion

        } finally {
            $documentStore->close();
        }
    }

    # region wait_timeout_ex
    public function WaitForCompletionWithTimeout(DocumentStore $documentStore, Duration $duration)
    {
        // Define operation, e.g. delete all discontinued products
        // Note: This operation implements interface: 'OperationInterface'
        $deleteByQueryOp = new DeleteByQueryOperation("from Products where Discontinued = true");

        // Execute the operation
        // Send returns an 'Operation' object that can be awaited on

        /** @var Operation $operation */
        $operation = $documentStore->operations()->sendAsync($deleteByQueryOp);

        try {
            // Call method 'waitForCompletion()' to wait for the operation to complete.

            /** @var BulkOperationResult $result */
            $result = $operation->waitForCompletion($duration);

            // The operation has finished within the specified timeframe
            $numberOfItemsDeleted = $result->getTotal(); // Access the operation result


        } catch (TimeoutException $exception) {
            // The operation did Not finish within the specified timeframe
        }

    }
    # endregion
}
