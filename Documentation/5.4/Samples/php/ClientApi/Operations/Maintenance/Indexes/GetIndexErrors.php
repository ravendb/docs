<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexErrorsArray;
use RavenDB\Documents\Operations\Indexes\GetIndexErrorsOperation;

class GetIndexErrors
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region get_errors_all
            // Define the get index errors operation
            $getIndexErrorsOp = new GetIndexErrorsOperation();

            // Execute the operation by passing it to maintenance.send
            /** @var IndexErrorsArray $indexErrors */
            $indexErrors = $store->maintenance()->send($getIndexErrorsOp);

            // indexErrors will contain errors for ALL indexes
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region get_errors_specific
            // Define the get index errors operation for specific indexes
            $getIndexErrorsOp = new GetIndexErrorsOperation([ "Orders/Totals" ]);

            // Execute the operation by passing it to Maintenance.Send
            // An exception will be thrown if any of the specified indexes do not exist
            /** @var IndexErrorsArray $indexErrors */
            $indexErrors = $store->maintenance()->send($getIndexErrorsOp);

            // indexErrors will contain errors only for index "Orders/Totals"
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{
    # region syntax_1
    // Available overloads:
    GetIndexErrorsOperation()                  // Get errors for all indexes
    GetIndexErrorsOperation(array $indexNames) // Get errors for specific indexes
    # endregion
}
*/

/*
# region syntax_2
public class IndexErrors
{
    private ?string $name = null;               // Index name
    private ?IndexingErrorArray $errors = null; // List of errors for this index

    // ... getters and setters
}
# endregion
*/

/*
# region syntax_3
public class IndexingError
{
    // The error message
    private ?string $error = null;

    // Time of error
    private ?DateTimeInterface $timestamp = null;

    // If Action is 'Map'    - field will contain the document ID
    // If Action is 'Reduce' - field will contain the Reduce key value
    // For all other Actions - field will be null
    private ?string $document = null;

    // Area where error has occurred, e.g. Map/Reduce/Analyzer/Memory/etc.
    private ?string $action = null;

    // ... getters and setters
}
# endregion
*/
