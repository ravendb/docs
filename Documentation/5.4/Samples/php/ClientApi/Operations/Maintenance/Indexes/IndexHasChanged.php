<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\IndexDefinition;
use RavenDB\Documents\Operations\Indexes\IndexHasChangedOperation;

class IndexHasChanged
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region index_has_changed
            // Some index definition
            $indexDefinition = new IndexDefinition();
            $indexDefinition->setName("UsersByName");
            $indexDefinition->setMaps(["from user in docs.Users select new { user.Name }"]);

            // Define the has-changed operation, pass the index definition
            $indexHasChangedOp = new IndexHasChangedOperation($indexDefinition);

            // Execute the operation by passing it to Maintenance.Send
            $store->maintenance()->send($indexHasChangedOp);

            // Return values:
            // false: The definition of the index passed is the SAME as the one deployed on the server
            // true:  The definition of the index passed is DIFFERENT than the one deployed on the server
            //        Or - index does not exist
            # endregion

            $store->maintenance()->send($resumeIndexingOp);
        } finally {
            $store->close();
        }
    }

}

/*
interface IFoo
{
    # region syntax
    IndexHasChangedOperation(?IndexDefinition $definition)
    # endregion
}
*/
