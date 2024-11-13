<?php

namespace RavenDB\Samples\ClientApi\Operations\Server;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\DisableDatabaseToggleResult;
use RavenDB\Documents\Operations\ToggleDatabasesStateOperation;

class ToggleDatabasesState
{
    public function samples(): void
    {
        $documentStore = new DocumentStore();
        try {
            # region enable
            // Define the toggle state operation
            // specify the database name & pass 'false' to enable
            $enableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", false);

            // To enable multiple databases use:
            // $enableDatabaseOp = new ToggleDatabasesStateOperation([ "DB1", "DB2", ... ], false);

            // Execute the operation by passing it to the 'send' method
            /** @var DisableDatabaseToggleResult $toggleResult */
            $toggleResult = $documentStore->maintenance()->server()->send($enableDatabaseOp);
            # endregion


            # region disable
            // Define the toggle state operation
            // specify the database name(s) & pass 'true' to disable
            $disableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", true);

            // To disable multiple databases use:
            // $disableDatabaseOp = new ToggleDatabasesStateOperation([ "DB1", "DB2", ... ], true);

            // Execute the operation by passing it to the 'send' method
            /** @var DisableDatabaseToggleResult $toggleResult */
            $toggleResult = $documentStore->maintenance()->server()->send($disableDatabaseOp);
            # endregion

        } finally {
            $documentStore->close();
        }
    }
}
