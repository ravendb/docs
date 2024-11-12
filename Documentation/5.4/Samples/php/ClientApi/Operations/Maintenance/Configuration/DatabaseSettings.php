<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Configuration;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\ToggleDatabasesStateOperation;
use RavenDB\ServerWide\Operations\Configuration\GetDatabaseSettingsOperation;
use RavenDB\ServerWide\Operations\Configuration\PutDatabaseSettingsOperation;
use RavenDB\Type\StringMap;

class DatabaseSettings
{
    public function putDatabaseSettings(): void
    {
            $documentStore = new DocumentStore();
            try  {
                # region put_database_settings
                // 1. Modify the database settings:
                // ================================

                // Define the settings dictionary with the key-value pairs to set, for example:
                $settings = [
                    "Databases.QueryTimeoutInSec" => "350",
                    "Indexing.Static.DeploymentMode" => "Rolling"
                ];

                // Define the put database settings operation,
                // specify the database name & pass the settings dictionary
                $putDatabaseSettingsOp = new PutDatabaseSettingsOperation($documentStore->getDatabase(), $settings);

                // Execute the operation by passing it to Maintenance.Send
                $documentStore->maintenance()->send($putDatabaseSettingsOp);

                // 2. RELOAD the database for the change to take effect:
                // =====================================================

                // Disable database
                $disableDatabaseOp = new ToggleDatabasesStateOperation($documentStore->getDatabase(), true);
                $documentStore->maintenance()->server()->send($disableDatabaseOp);

                // Enable database
                $enableDatabaseOp = new ToggleDatabasesStateOperation($documentStore->getDatabase(), false);
                $documentStore->maintenance()->server()->send($enableDatabaseOp);
                # endregion
            } finally {
                $documentStore->close();
            }
        }

    public function GetDatabaseSettings(): void
    {
            $documentStore = new DocumentStore();
            try {
                # region get_database_settings
                // Define the get database settings operation, specify the database name
                $getDatabaseSettingsOp = new GetDatabaseSettingsOperation($documentStore->getDatabase());

                // Execute the operation by passing it to Maintenance.Send
                /** @var DatabaseSettings $customizedSettings */
                $customizedSettings = $documentStore->maintenance()->send($getDatabaseSettingsOp);

                // Get the customized value
                $customizedValue = $customizedSettings->getSettings()["Databases.QueryTimeoutInSec"];
                # endregion
            } finally {
                $documentStore->close();
            }
        }
}

/*
interface IFoo
{

    # region syntax_1
    PutDatabaseSettingsOperation(?string $databaseName, StringMap|array|null $configurationSettings)
    # endregion

    # region syntax_2
    GetDatabaseSettingsOperation(?string $databaseName);
    # endregion

    # region syntax_3
    // Executing the operation returns the following object:
    class DatabaseSettings
    {
    // Configuration settings that have been customized
        private ?StringMap $settings = null;
        // ...getter and setter
    }
    # endregion
}
*/
