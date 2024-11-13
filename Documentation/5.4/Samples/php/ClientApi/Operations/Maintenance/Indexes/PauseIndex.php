<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexingStatus;
use RavenDB\Documents\Operations\Indexes\GetIndexingStatusOperation;
use RavenDB\Documents\Operations\Indexes\StopIndexOperation;

class PauseIndex extends TestCase
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region pause_index
            // Define the pause index operation, pass the index name
            $pauseIndexOp = new StopIndexOperation("Orders/Totals");

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($pauseIndexOp);

            // At this point:
            // Index 'Orders/Totals' is paused on the preferred node

            // Can verify the index status on the preferred node by sending GetIndexingStatusOperation
            /** @var IndexingStatus $indexingStatus */
            $indexingStatus = $store->maintenance()->send(new GetIndexingStatusOperation());

            $indexes = array_filter($indexingStatus->getIndexes()->getArrayCopy(), function ($v, $k) {
                return $v->getName() == "Orders/Totals";
            });
            /** @var IndexingStatus $index */
            $index = $indexes[0];

            $this->assertTrue($index->getStatus()->isRunning());
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
    // class name has "Stop", but this is ok, this is the "Pause" operation
    public StopIndexOperation(?string $indexName)
    # endregion
}
*/
