<?php

namespace RavenDB\Samples\ClientApi\Operations\HowTo;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\GetStatisticsOperation;
use RavenDB\Documents\Operations\MaintenanceOperationExecutor;
use RavenDB\Documents\Operations\OperationExecutor;
use RavenDB\Documents\Operations\Revisions\GetRevisionsOperation;
use RavenDB\Samples\Infrastructure\Orders\Company;
use RavenDB\Samples\Infrastructure\Orders\Order;

class SwitchOperationsToDifferentDatabase
{
    public function SwitchOperationToDifferentDatabase() : void
    {
        # region for_database_1
        // Define default database on the store
        $documentStore = new DocumentStore(
            ["yourServerURL"],
            "DefaultDB"
        );
        $documentStore->initialize();

        try {
            // Use 'ForDatabase', get operation executor for another database
            /** @var OperationExecutor $opExecutor */
            $opExecutor = $documentStore->operations()->forDatabase("AnotherDB");

            // Send the operation, e.g. 'GetRevisionsOperation' will be executed on "AnotherDB"
            $revisionsInAnotherDB = $opExecutor->send(new GetRevisionsOperation(Order::class, "Orders/1-A"));

            // Without 'ForDatabase', the operation is executed "DefaultDB"
            $revisionsInDefaultDB = $documentStore->operations()->send(new GetRevisionsOperation(Company::class, "Company/1-A"));
        } finally {
            $documentStore->close();
        }
        # endregion
    }

    public function SwitchMaintenanceOperationToDifferentDatabase(): void
    {
        # region for_database_2
        // Define default database on the store
        $documentStore = new DocumentStore(
            [ "yourServerURL" ],
            "DefaultDB"
        );
        $documentStore->initialize();

        try {
            // Use 'ForDatabase', get maintenance operation executor for another database
            /** @var MaintenanceOperationExecutor $opExecutor */
            $opExecutor = $documentStore->maintenance()->forDatabase("AnotherDB");

            // Send the maintenance operation, e.g. get database stats for "AnotherDB"
            $statsForAnotherDB = $opExecutor->send(new GetStatisticsOperation());

            // Without 'ForDatabase', the stats are retrieved for "DefaultDB"
            $statsForDefaultDB = $documentStore->maintenance()->send(new GetStatisticsOperation());
        } finally {
            $documentStore->close();
        }
        # endregion
    }
}

/*
interface OperationsForDatabaseSyntax
{
    # region syntax_1
    public function forDatabase(?string $databaseName): OperationExecutor;
    # endregion
}
*/

/*
interface MaintenanceForDatabaseSyntax
{
    # region syntax_2
    public function forDatabase(?string $databaseName): MaintenanceOperationExecutor;
    # endregion
}
*/
