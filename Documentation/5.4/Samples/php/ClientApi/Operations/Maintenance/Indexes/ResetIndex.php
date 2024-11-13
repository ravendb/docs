<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\ResetIndexOperation;

class ResetIndex
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region reset
            // Define the reset index operation, pass index name
            $resetIndexOp = new ResetIndexOperation("Orders/Totals");

            // Execute the operation by passing it to Maintenance.SendAsync
            // An exception will be thrown if index does not exist
            $store->maintenance()->send($resetIndexOp);
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
    public ResetIndexOperation(?string $indexName);
    # endregion
}
*/
