<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\DeleteIndexOperation;

class DeleteIndex
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region delete_index
            // Define the delete index operation, specify the index name
            $deleteIndexOp = new DeleteIndexOperation("Orders/Totals");

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($deleteIndexOp);
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
    DeleteIndexOperation(?string $indexName)
    # endregion
}
*/
