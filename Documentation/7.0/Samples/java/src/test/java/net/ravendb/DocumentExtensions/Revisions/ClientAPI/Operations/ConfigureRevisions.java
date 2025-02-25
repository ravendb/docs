package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.revisions;
import net.ravendb.client.documents.operations.revisions.RevisionsCollectionConfiguration;

public class ConfigureRevisions {

    public ConfigureRevisions() {
        try (IDocumentStore documentStore = new DocumentStore()) {
            //region operation
            // Create a configuration for the Employees collection
            RevisionsCollectionConfiguration employeesRevConfig = new RevisionsCollectionConfiguration();
            employeesRevConfig.setMinimumRevisionAgeToKeep(Duration.ofDays(1));
            employeesRevConfig.setMinimumRevisionsToKeep(42l);
            employeesRevConfig.setPurgeOnDelete(true);

            // Add the Employees configuration to a map
            Map<String, RevisionsCollectionConfiguration> collections = new HashMap<>();
            collectionConfig.put("Employees", employeesRevConfig);

            // Create a default collection configuration
            RevisionsCollectionConfiguration defaultRevConfig = new RevisionsCollectionConfiguration();
            defaultRevConfig.setMinimumRevisionAgeToKeep(Duration.ofDays(7));
            defaultRevConfig.setMinimumRevisionsToKeep(100l);
            defaultRevConfig.setPurgeOnDelete(false);

            // Combine to create a configuration for the database
            RevisionsConfiguration northwindRevConfig = new RevisionsConfiguration();
            northwindRevConfig.setCollections(collections);
            northwindRevConfig.setDefaultConfig(defaultRevConfig);

            // Execute the operation to update the database
            documentStore.maintenance().send(new ConfigureRevisionsOperation(northwindRevConfig));
            //endregion
        }

    }
}
