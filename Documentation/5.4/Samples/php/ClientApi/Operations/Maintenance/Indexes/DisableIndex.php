<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\DisableIndexOperation;

class DisableIndex
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region disable_1
            // Define the disable index operation
            // Use this overload to disable on a single node
            $disableIndexOp = new DisableIndexOperation("Orders/Totals");

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($disableIndexOp);

            // At this point, the index is disabled only on the 'preferred node'
            // New data will not be indexed on this node only
            # endregion

        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region disable_2
            // Define the disable index operation
            // Pass 'true' to disable the index on all nodes in the database-group
            $disableIndexOp = new DisableIndexOperation("Orders/Totals", true);

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($disableIndexOp);

            // At this point, the index is disabled on ALL nodes
            // New data will not be indexed
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
    DisableIndexOperation(?string $indexName, bool $clusterWide = false)
    # endregion
}
*/
