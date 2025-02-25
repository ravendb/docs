import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function configureRevisions() {
    {
        //region replace_configuration
        // ==============================================================================
        // Define default settings that will apply to ALL collections
        // Note: this is optional
        const defaultRevConfig = new RevisionsCollectionConfiguration();
        defaultRevConfig.minimumRevisionsToKeep = 100;
        defaultRevConfig.minimumRevisionAgeToKeep = TimeUtil.millisToTimeSpan(3600 * 1000 * 24 * 7) // 7 days
        defaultRevConfig.maximumRevisionsToDeleteUponDocumentUpdate = 15;
        defaultRevConfig.purgeOnDelete = false;
        defaultRevConfig.disabled = false;
        
        // With this configuration:
        // ------------------------
        // * A revision will be created anytime a document is modified or deleted.
        // * Revisions of a deleted document can be accessed in the Revisions Bin view.
        // * Only the latest 100 revisions will be kept. Older ones will be discarded.
        // * Older revisions will be removed if they exceed 7 days on next revision creation.
        // * A maximum of 15 revisions will be deleted each time a document is updated,
        //   until the defined '# of revisions to keep' limit is reached.

        // ==============================================================================
        // Define a specific configuration for the EMPLOYEES collection
        // This will override the default settings
        const employeesRevConfig  = new RevisionsCollectionConfiguration();
        employeesRevConfig.minimumRevisionsToKeep = 50;
        employeesRevConfig.minimumRevisionAgeToKeep = TimeUtil.millisToTimeSpan(3600 * 1000 * 12);  // 12 hrs
        employeesRevConfig.purgeOnDelete = true;
        employeesRevConfig.disabled = false;

        // With this configuration:
        // ------------------------
        // * A revision will be created anytime an Employee document is modified.
        // * When a document is deleted all its revisions will be removed.
        // * At least 50 of the latest revisions will be kept.
        // * Older revisions will be removed if they exceed 12 hours on next revision creation.

        // ==============================================================================
        // Define a specific configuration for the PRODUCTS collection
        // This will override the default settings
        const productsRevConfig   = new RevisionsCollectionConfiguration();
        productsRevConfig.disabled = true;
        
        // With this configuration:
        // ------------------------
        // No revisions will be created for the Products collection,
        // even though default configuration is enabled

        // ==============================================================================
        // Combine all configurations in the RevisionsConfiguration object
        const revisionsConfig = new RevisionsConfiguration();
        revisionsConfig.defaultConfig = defaultRevConfig;
        revisionsConfig.collections = new Map<string, RevisionsCollectionConfiguration>();
        revisionsConfig.collections.set("Employees", employeesRevConfig);
        revisionsConfig.collections.set("Products", productsRevConfig);

        // ==============================================================================
        // Define the configure revisions operation, pass the configuration
        const configureRevisionsOp = new ConfigureRevisionsOperation(revisionsConfig);

        // Execute the operation by passing it to maintenance.send
        // Any existing configuration will be replaced with the new configuration passed
        await store.maintenance.send(configureRevisionsOp);
        //endregion
    }
    {
        //region modify_configuration
        // ==============================================================================
        // Define the get database record operation:
        const getDatabaseRecordOp = new GetDatabaseRecordOperation(documentStore.database);
        // Get the current revisions configuration from the database record:
        const record = await store.maintenance.server.send(getDatabaseRecordOp);
        const revisionsConfig = record.revisions;

        // ==============================================================================
        // If no revisions configuration exists, then create a new configuration
        if (!revisionsConfig) {
            const revisionsConfig = new RevisionsConfiguration();
            revisionsConfig.defaultConfig = defaultRevConfig;
            revisionsConfig.collections = new Map<string, RevisionsCollectionConfiguration>();
            revisionsConfig.collections.set("Employees", employeesRevConfig);
            revisionsConfig.collections.set("Products", productsRevConfig);
        }
        
        // ==============================================================================
        // If a revisions configuration already exists, then modify it
        else {
            revisionsConfig.defaultConfig = defaultRevConfig;
            revisionsConfig.collections.set("Employees", employeesRevConfig);
            revisionsConfig.collections.set("Products", productsRevConfig);
        }
        
        // ==============================================================================
        // Define the configure revisions operation, pass the configuration
        const configureRevisionsOp = new ConfigureRevisionsOperation(revisionsConfig);
        
        // Execute the operation by passing it to maintenance.send
        // The existing configuration will be updated
        await documentStore.maintenance.send(configureRevisionsOp);
        //endregion
    }
}

{
    //region syntax_1
    const configureRevisionsOp = new ConfigureRevisionsOperation(configuration);
    //endregion

    //region syntax_2
    class RevisionsConfiguration
    {
        defaultConfig;
        collections;
    }
    //endregion

    //region syntax_3
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
