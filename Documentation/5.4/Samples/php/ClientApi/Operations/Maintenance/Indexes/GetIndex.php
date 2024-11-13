<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexDefinition;
use RavenDB\Documents\Indexes\IndexDefinitionArray;
use RavenDB\Documents\Operations\Indexes\GetIndexesOperation;
use RavenDB\Documents\Operations\Indexes\GetIndexOperation;

class GetIndex
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region get_index

            // Define the get index operation, pass the index name
            $getIndexOp = new GetIndexOperation("Orders/Totals");

            // Execute the operation by passing it to Maintenance.Send
            /** @var IndexDefinition $index */
            $index = $store->maintenance()->send($getIndexOp);

            // Access the index definition
            $state = $index->getState();
            $lockMode = $index->getLockMode();
            $deploymentMode = $index->getDeploymentMode();
            // etc.

            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region get_indexes
            // Define the get indexes operation
            // Pass number of indexes to skip & number of indexes to retrieve
            $getIndexesOp = new GetIndexesOperation(0, 10);

            // Execute the operation by passing it to Maintenance.Send
            /** @var IndexDefinitionArray $indexes */
            $indexes = $store->maintenance()->send($getIndexesOp);

            // indexes will contain the first 10 indexes, alphabetically ordered by index name
            // Access an index definition from the resulting list:
            $name = $indexes[0]->getName();
            $state = $indexes[0]->getState();
            $lockMode = $indexes[0]->getLockMode();
            $deploymentMode = $indexes[0]->getDeploymentMode();
            // etc.
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{
    # region get_index_syntax
    GetIndexOperation(?string $indexName)
    # endregion

    # region get_indexes_syntax
    GetIndexesOperation(int $start, int $pageSize)
    # endregion
}
*/
