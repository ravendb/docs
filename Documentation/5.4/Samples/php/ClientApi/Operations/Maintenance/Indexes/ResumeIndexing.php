<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use PHPUnit\Framework\TestCase;
use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexingStatus;
use RavenDB\Documents\Operations\Indexes\GetIndexingStatusOperation;
use RavenDB\Documents\Operations\Indexes\StartIndexingOperation;

class ResumeIndexing extends TestCase
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region resume_indexing
            // Define the resume indexing operation
            $resumeIndexingOp = new StartIndexingOperation();

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($resumeIndexingOp);

            // At this point,
            // you can be sure that all indexes on the preferred node are 'running'

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
    // class name prefix is "Start", but this is still the "Resume" operation
    public StartIndexingOperation()
    # endregion
}
*/
