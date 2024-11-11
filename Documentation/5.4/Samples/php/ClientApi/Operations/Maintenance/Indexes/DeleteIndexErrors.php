<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\DeleteIndexErrorsOperation;

class DeleteIndexErrors
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region delete_errors_all
            // Define the delete index errors operation
            $deleteIndexErrorsOp = new DeleteIndexErrorsOperation();

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($deleteIndexErrorsOp);

            // All errors from ALL indexes are deleted
            # endregion

            # region delete_errors_specific
            // Define the delete index errors operation from specific indexes
            $deleteIndexErrorsOp = new DeleteIndexErrorsOperation([ "Orders/Totals" ]);

            // Execute the operation by passing it to Maintenance.Send
            // An exception will be thrown if any of the specified indexes do not exist
            $store->maintenance()->send($deleteIndexErrorsOp);

            // Only errors from index "Orders/Totals" are deleted
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
private interface IFoo
{
    # region syntax
    // Available overloads:
    DeleteIndexErrorsOperation() // Delete errors from all indexes
    DeleteIndexErrorsOperation(StringArray|array|string $indexNames) // Delete errors from specific indexes
    # endregion
}
*/
