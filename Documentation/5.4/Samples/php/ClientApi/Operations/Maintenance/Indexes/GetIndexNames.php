<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\GetIndexNamesOperation;
use RavenDB\Type\StringArrayResult;

class GetIndexNames
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region get_index_names
            // Define the get index names operation
            // Pass number of indexes to skip & number of indexes to retrieve
            $getIndexNamesOp = new GetIndexNamesOperation(0, 10);

            // Execute the operation by passing it to Maintenance.Send
            /** @var StringArrayResult $indexNames */
            $indexNames = $store->maintenance()->send($getIndexNamesOp);

            // indexNames will contain the first 10 indexes, alphabetically ordered
            # endregion
        } finally {
            $store->close();
        }
    }

}
