<?php

namespace RavenDB\Samples\DocumentExtensions\Revisions\ClientAPI\Operations;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Revisions\ConfigureRevisionsOperation;
use RavenDB\Documents\Operations\Revisions\RevisionsCollectionConfiguration;
use RavenDB\Documents\Operations\Revisions\RevisionsConfiguration;
use RavenDB\ServerWide\GetDatabaseRecordOperation;
use RavenDB\Type\Duration;

class ConfigureRevisions
{
    public function samples(): void
    {
        // REPLACE
        $documentStore = new DocumentStore();
        try {
            # region replace_configuration
            // ==============================================================================
            // Define default settings that will apply to ALL collections
            // Note: this is optional
            $defaultRevConfig = new RevisionsCollectionConfiguration();

            // With this configuration:
            // ------------------------
            // * A revision will be created anytime a document is modified or deleted.
            // * Revisions of a deleted document can be accessed in the Revisions Bin view.
            // * At least 100 of the latest revisions will be kept.
            // * Older revisions will be removed if they exceed 7 days on next revision creation.
            // * A maximum of 15 revisions will be deleted each time a document is updated,
            //   until the defined '# of revisions to keep' limit is reached.
            $defaultRevConfig->setMinimumRevisionsToKeep(100);
            $defaultRevConfig->setMinimumRevisionAgeToKeep(Duration::ofDays(7));
            $defaultRevConfig->setMaximumRevisionsToDeleteUponDocumentUpdate(15);
            $defaultRevConfig->setPurgeOnDelete(false);
            $defaultRevConfig->setDisabled(false);


            // ==============================================================================
            // Define a specific configuration for the EMPLOYEES collection
            // This will override the default settings
            $employeesRevConfig = new RevisionsCollectionConfiguration();

            // With this configuration:
            // ------------------------
            // * A revision will be created anytime an Employee document is modified.
            // * When a document is deleted all its revisions will be removed.
            // * At least 50 of the latest revisions will be kept.
            // * Older revisions will be removed if they exceed 12 hours on next revision creation.

            $employeesRevConfig->setMinimumRevisionsToKeep(50);
            $employeesRevConfig->setMinimumRevisionAgeToKeep(Duration::ofHours(12));
            $employeesRevConfig->setPurgeOnDelete(true);
            $employeesRevConfig->setDisabled(false);

            // ==============================================================================
            // Define a specific configuration for the PRODUCTS collection
            // This will override the default settings
            $productsRevConfig = new RevisionsCollectionConfiguration();

            // No revisions will be created for the Products collection,
            // even though default configuration is enabled
            $productsRevConfig->setDisabled(true);

            // ==============================================================================
            // Combine all configurations in the RevisionsConfiguration object
            $revisionsConfig = new RevisionsConfiguration();
            $revisionsConfig->setDefaultConfig($defaultRevConfig);
            $revisionsConfig->setCollections([
                "Employees" => $employeesRevConfig,
                "Products" => $productsRevConfig
            ]);

            // ==============================================================================
            // Define the configure revisions operation, pass the configuration
            $configureRevisionsOp = new ConfigureRevisionsOperation($revisionsConfig);

            // Execute the operation by passing it to Maintenance.Send
            // Any existing configuration will be replaced with the new configuration passed
            $documentStore->maintenance()->send($configureRevisionsOp);
            # endregion
        } finally {
            $documentStore->close();
        }

        // MODIFY
        $documentStore = new DocumentStore();
        try {
            $defaultRevConfig = new RevisionsCollectionConfiguration();

            $defaultRevConfig->setMinimumRevisionsToKeep(100);
            $defaultRevConfig->setMinimumRevisionAgeToKeep(Duration::ofDays(7));
            $defaultRevConfig->setMaximumRevisionsToDeleteUponDocumentUpdate(15);
            $defaultRevConfig->setPurgeOnDelete(false);
            $defaultRevConfig->setDisabled(false);

            $employeesRevConfig = new RevisionsCollectionConfiguration();
            $employeesRevConfig->setMinimumRevisionsToKeep(50);
            $employeesRevConfig->setMinimumRevisionAgeToKeep(Duration::ofHours(12));
            $employeesRevConfig->setPurgeOnDelete(true);

            $productsRevConfig = new RevisionsCollectionConfiguration();
            $productsRevConfig->setDisabled(true);

            # region modify_configuration
            // ==============================================================================
            // Define the get database record operation:
            $getDatabaseRecordOp = new GetDatabaseRecordOperation($documentStore->getDatabase());
            // Get the current revisions configuration from the database record:
            /** @var RevisionsConfiguration $revisionsConfig */
            $revisionsConfig = $documentStore->maintenance()->server()->send($getDatabaseRecordOp)->getRevisions();

            // ==============================================================================
            // If no revisions configuration exists, then create a new configuration
            if ($revisionsConfig == null)
            {
                $revisionsConfig = new RevisionsConfiguration();
                $revisionsConfig->setDefaultConfig($defaultRevConfig);
                $revisionsConfig->setCollections([
                    "Employees" => $employeesRevConfig,
                    "Products" => $productsRevConfig
                ]);
            }

            // ==============================================================================
            // If a revisions configuration already exists, then modify it
            else
            {
                $revisionsConfig->setDefaultConfig($defaultRevConfig);
                $collections = $revisionsConfig->getCollections();
                $collections["Employees"] = $employeesRevConfig;
                $collections["Products"] = $productsRevConfig;
                $revisionsConfig->setCollections($collections);
            }

            // ==============================================================================
            // Define the configure revisions operation, pass the configuration
            $configureRevisionsOp = new ConfigureRevisionsOperation($revisionsConfig);

            // Execute the operation by passing it to Maintenance.Send
            // The existing configuration will be updated
            $documentStore->maintenance()->send($configureRevisionsOp);
            # endregion
        } finally {
            $documentStore->close();
        }
    }
}

/*
# region syntax_1
new ConfigureRevisionsOperation(?RevisionsConfiguration $configuration);
# endregion
*/

/*
# region syntax_2
class RevisionsConfiguration
{
    public function getDefaultConfig(): ?RevisionsCollectionConfiguration;
    public function setDefaultConfig(?RevisionsCollectionConfiguration $defaultConfig): void;
    public function getCollections(): ?RevisionsCollectionConfigurationArray;
    public function setCollections(null|RevisionsCollectionConfigurationArray|array $collections): void;
}
# endregion
*/

/*
# region syntax_3
class RevisionsCollectionConfiguration
{
    private ?int $minimumRevisionsToKeep = null;
    private ?Duration $minimumRevisionAgeToKeep = null;
    private bool $disabled = false;
    private bool $purgeOnDelete = false;
    private ?int $maximumRevisionsToDeleteUponDocumentUpdate = null;

    // ... getters and setters ...
}
# endregion
*/
