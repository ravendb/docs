<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexingStatus;
use RavenDB\Documents\Operations\Indexes\GetIndexingStatusOperation;
use RavenDB\Documents\Operations\Indexes\StartIndexOperation;

class ResumeIndex extends TestCase
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region resume_index
            // Define the resume index operation, pass the index name
            $resumeIndexOp = new StartIndexOperation("Orders/Totals");

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($resumeIndexOp);

            // At this point:
            // Index 'Orders/Totals' is resumed on the preferred node

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
    // class name begins with "Start" but this is still the "Resume" operation
    StartIndexOperation(?string $indexName)
    # endregion
}
*/
