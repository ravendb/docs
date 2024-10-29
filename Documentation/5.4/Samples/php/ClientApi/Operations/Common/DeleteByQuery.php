<?php

namespace RavenDB\Samples\ClientApi\Operations\Common;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Operations\Operation;
use RavenDB\Documents\Operations\OperationInterface;
use RavenDB\Documents\Queries\IndexQuery;
use RavenDB\Type\Duration;

# region syntax_1
class DeleteByQueryOperation implements OperationInterface
{
    /**
     * Usage:
     *   - new DeleteByQueryOperation("from 'Orders'")
     *   - new DeleteByQueryOperation("from 'Orders'", $options)
     *
     *   - new DeleteByQueryOperation(new IndexQuery("from 'Orders'"))
     *   - new DeleteByQueryOperation(new IndexQuery("from 'Orders'"), $options)
     *
     * @param IndexQuery|string|null $queryToDelete
     * @param QueryOperationOptions|null $options
     */
    public function __construct(IndexQuery|string|null $queryToDelete, ?QueryOperationOptions $options = null) {
        // ...
    }

    // ...
}
# endregion

# region syntax_2
class QueryOperationOptions
{
    // Indicates whether operations are allowed on stale indexes.
    private bool $allowStale = false;

    // Limits the number of base operations per second allowed.
    // DEFAULT: no limit
    private ?int $maxOpsPerSecond = null;

    // If AllowStale is set to false and index is stale,
    // then this is the maximum timeout to wait for index to become non-stale.
    // If timeout is exceeded then exception is thrown.
    // DEFAULT: null (if index is stale then exception is thrown immediately)
    private ?Duration $staleTimeout = null;

    // Determines whether operation details about each document should be returned by server.
    private bool $retrieveDetails = false;

    // Ignore the maximum number of statements a script can execute.
    // Note: this is only relevant for the patchByQueryOperation.
    private bool $ignoreMaxStepsForScript = false;

    // getters and setters
}
# endregion


# region the_index
// The index definition:
// =====================

class IndexEntry
{
    public float $price;

    public function getPrice(): float
    {
        return $this->price;
    }

    public function setPrice(float $price): void
    {
        $this->price = $price;
    }
}

class Products_ByPrice extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from product in products select new {price = product.PricePerUnit}";
    }
}

# endregion


class DeleteByQuery
{
    public function examples(): void
    {
        $store = new DocumentStore();
        try {
            # region delete_by_query_0
            // Define the delete by query operation, pass an RQL querying a collection
            $deleteByQueryOp = new DeleteByQueryOperation("from 'Orders'");

            // Execute the operation by passing it to Operations.Send
            $operation = $store->operations()->send($deleteByQueryOp);

            // All documents in collection 'Orders' will be deleted from the server.
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region delete_by_query_1
            // Define the delete by query operation, pass an RQL querying a collection
            $deleteByQueryOp = new DeleteByQueryOperation("from 'Orders' where Freight > 30");

            // Execute the operation by passing it to Operations.Send
            $operation = $store->operations()->send($deleteByQueryOp);

            // * All documents matching the specified RQL will be deleted from the server.

            // * Since the dynamic query was made with a filtering condition,
            //   an auto-index is generated (if no other matching auto-index already exists).
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region delete_by_query_2
            // Define the delete by query operation, pass an RQL querying the index
            $deleteByQueryOp = new DeleteByQueryOperation("from index 'Products/ByPrice' where Price > 10");

            // Execute the operation by passing it to Operations.Send
            $operation = $store->operations()->send($deleteByQueryOp);


            // All documents with document-field PricePerUnit > 10 will be deleted from the server.
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region delete_by_query_3
            // Define the delete by query operation
            $deleteByQueryOp = new DeleteByQueryOperation(
                // Provide an RQL querying the index
                new IndexQuery("from index 'Products/ByPrice' where Price > 10")
            );

            // Execute the operation by passing it to Operations.Send
            $operation = $store->operations()->send($deleteByQueryOp);

            // All documents with document-field PricePerUnit > 10 will be deleted from the server.
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region delete_by_query_6

            // OPTIONS: Specify the options for the operation
            // (See all other available options in the Syntax section below)
            $options = new QueryOperationOptions();
            // Allow the operation to operate even if index is stale
            $options->setAllowStale(true);
            // Get info in the operation result about documents that were deleted
            $options->setRetrieveDetails(true);

            // Define the delete by query operation
            $deleteByQueryOp = new DeleteByQueryOperation(
                new IndexQuery("from index 'Products/ByPrice' where Price > 10"), // QUERY: Specify the query
                $options // OPTIONS:
            );

            // Execute the operation by passing it to Operations.Send
            /** @var Operation $operation */
            $operation = $store->operations()->sendAsync($deleteByQueryOp);

            // Wait for operation to complete
            /** @var BulkOperationResult $result */
            $result = $operation->waitForCompletion(Duration::ofSeconds(15));

            // * All documents with document-field PricePerUnit > 10 will be deleted from the server.

            // * Details about deleted documents are available:
            $details =  $result->getDetails();
            $documentIdThatWasDeleted = $details[0]->getId();
            # endregion
        } finally {
            $store->close();
        }
    }
}
