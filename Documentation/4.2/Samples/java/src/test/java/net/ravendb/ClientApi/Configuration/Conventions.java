package net.ravendb.ClientApi.Configuration;

import com.fasterxml.jackson.databind.PropertyNamingStrategy;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.conventions.DocumentConventions;
import net.ravendb.client.extensions.JsonExtensions;

public class Conventions {
    public Conventions() {
        //region conventions_1
        try (IDocumentStore store = new DocumentStore()) {
            DocumentConventions conventions = store.getConventions();
            // customizations go here

            store.initialize();
        }
        //endregion
    }

    public void examples() {
        DocumentConventions conventions = new DocumentConventions();

        //region MaxHttpCacheSize
        conventions.setMaxHttpCacheSize(256 * 1024 * 1024);
        //endregion

        //region MaxNumberOfRequestsPerSession
        conventions.setMaxNumberOfRequestsPerSession(10);
        //endregion

        //region UseOptimisticConcurrency
        conventions.setUseOptimisticConcurrency(true);
        //endregion

        //region DisableTopologyUpdates
        conventions.setDisableTopologyUpdates(false);
        //endregion

        //region SaveEnumsAsIntegers
        conventions.setSaveEnumsAsIntegers(true);
        //endregion

        //region disable_cache
        conventions.setMaxHttpCacheSize(0);
        //endregion

        //region UseCompression
        conventions.setUseCompression(true);
        //endregion

        //region PropertyCasing
        conventions.getEntityMapper()
            .setPropertyNamingStrategy(
                new JsonExtensions.DotNetNamingStrategy());
        //endregion
    }
}
