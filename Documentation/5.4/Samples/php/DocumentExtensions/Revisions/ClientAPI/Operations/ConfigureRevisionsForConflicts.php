<?php

namespace RavenDB\Samples\DocumentExtensions\Revisions\ClientAPI\Operations;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Revisions\RevisionsCollectionConfiguration;
use RavenDB\ServerWide\Operations\ConfigureRevisionsForConflictsOperation;
use RavenDB\Type\Duration;

class ConfigureRevisionsForConflicts
{
    public function samples(): void
    {
        $documentStore = new DocumentStore();
        try {
            # region conflict_revisions_configuration
            // Define the settings that will apply for conflict revisions (for all collections)
            $conflictRevConfig = new RevisionsCollectionConfiguration();

            // With this configuration:
            // ------------------------
            // * A revision will be created for conflict documents
            // * When the parent document is deleted all its revisions will be removed.
            // * Revisions that exceed 45 days will be removed on next revision creation.
            $conflictRevConfig->setPurgeOnDelete(true);
            $conflictRevConfig->setMinimumRevisionAgeToKeep(Duration::ofDays(45));

            // Define the configure conflict revisions operation, pass the configuration
            $configureConflictRevisionsOp =
                new ConfigureRevisionsForConflictsOperation($documentStore->getDatabase(), $conflictRevConfig);

            // Execute the operation by passing it to Maintenance.Server.Send
            // The existing conflict revisions configuration will be replaced by the configuration passed
            $documentStore->maintenance()->server()->send($configureConflictRevisionsOp);
            # endregion
        } finally {
            $documentStore->close();
        }
    }
}

/*
# region syntax_1
new ConfigureRevisionsForConflictsOperation(?string $database, ?RevisionsCollectionConfiguration $configuration)
# endregion
*/

/*
# region syntax_2
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
