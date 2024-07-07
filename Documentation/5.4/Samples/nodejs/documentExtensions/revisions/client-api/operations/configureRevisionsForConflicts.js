import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function configureRevisions() {
    {
        //region conflict_revisions_configuration
        // Define the settings that will apply for conflict revisions (for all collections)
        const conflictRevConfig  = new RevisionsCollectionConfiguration();
        conflictRevConfig.minimumRevisionAgeToKeep = TimeUtil.millisToTimeSpan(3600 * 1000 * 24 * 45) // 45 days
        conflictRevConfig.purgeOnDelete = true;
        
        // With this configuration:
        // ------------------------
        // * A revision will be created for conflict documents
        // * When the parent document is deleted all its revisions will be removed.
        // * Revisions that exceed 45 days will be removed on next revision creation.

        // Define the configure conflict revisions operation, pass the configuration
        const configureConflictRevisionsOp =
            new ConfigureRevisionsForConflictsOperation(documentStore.database, conflictRevConfig);

        // Execute the operation by passing it to maintenance.server.send
        // The existing conflict revisions configuration will be replaced by the configuration passed
        await documentStore.maintenance.server.send(configureConflictRevisionsOp);
        //endregion
    }
}

{
    //region syntax_1
    const configureRevisionsOp = new ConfigureRevisionsForConflictsOperation(database, configuration);
    //endregion

    //region syntax_2
    class RevisionsCollectionConfiguration
    {
        minimumRevisionsToKeep;
        minimumRevisionAgeToKeep;
        maximumRevisionsToDeleteUponDocumentUpdate;
        purgeOnDelete;
        disabled;
    }
    //endregion
}
