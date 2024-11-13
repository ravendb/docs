<?php

namespace RavenDB\Samples\ClientApi\Operations\Server;

use RavenDB\Documents\DocumentStore;
use RavenDB\ServerWide\Operations\AddDatabaseNodeOperation;
use RavenDB\ServerWide\Operations\DatabasePutResult;

class AddDatabaseNode
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region add_1
            // Create the AddDatabaseNodeOperation
            // Add a random node to 'Northwind' database-group
            $addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind");

            // Execute the operation by passing it to Maintenance.Server.Send
            /** @var DatabasePutResult $result */
            $result = $store->maintenance()->server()->send($addDatabaseNodeOp);

            // Can access the new topology
            $numberOfReplicas = count($result->getTopology()->getMembers());
            # endregion
        } finally {
            $store->close();
        }

        $store = new DocumentStore();
        try {
            # region add_2
            // Create the AddDatabaseNodeOperation
            // Add node C to 'Northwind' database-group
            $addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind", "C");

            // Execute the operation by passing it to Maintenance.Server.Send
            /** @var DatabasePutResult $result */
            $result = $store->maintenance()->server()->send($addDatabaseNodeOp);
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
    AddDatabaseNodeOperation(?string $databaseName, ?string $nodeTag = null)
    # endregion
}
*/
