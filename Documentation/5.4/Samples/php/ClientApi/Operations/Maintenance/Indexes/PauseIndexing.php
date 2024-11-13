<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexingStatus;
use RavenDB\Documents\Operations\Indexes\GetIndexingStatusOperation;
use RavenDB\Documents\Operations\Indexes\StopIndexingOperation;

class PauseIndexing extends TestCase
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region pause_indexing
            // Define the pause indexing operation
            $pauseIndexingOp = new StopIndexingOperation();

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($pauseIndexingOp);

            // At this point:
            // All indexes in the default database will be 'paused' on the preferred node

            // Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
            /** @var IndexingStatus $indexingStatus */
            $indexingStatus = $store->maintenance()->send(new GetIndexingStatusOperation());
            $this->assertTrue($indexingStatus->getStatus()->isPaused());
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{
    # region syntax
    // class name begins with "Stop" but this is still the "Pause" operation
    StopIndexingOperation()
    # endregion
}
*/
