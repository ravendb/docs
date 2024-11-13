<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\EnableIndexOperation;

class EnableIndex
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region enable_1
            // Define the enable index operation
            // Use this overload to enable on a single node
            $enableIndexOp = new EnableIndexOperation("Orders/Totals");

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($enableIndexOp);

            // At this point, the index is enabled on the 'preferred node'
            // New data will be indexed on this node
            # endregion

        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region enable_2
            // Define the enable index operation
            // Pass 'true' to enable the index on all nodes in the database-group
            $enableIndexOp = new EnableIndexOperation("Orders/Totals", true);

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($enableIndexOp);

            // At this point, the index is enabled on ALL nodes
            // New data will be indexed
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
    // Available overloads:
    EnableIndexOperation(?string $indexName, bool clusterWide = false)
    # endregion
}
*/
