package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.QueryStatistics;
import net.ravendb.client.primitives.Reference;

import java.time.Duration;
import java.util.List;

public class StaleIndexes {
    private static class Product {

    }

    public StaleIndexes() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region stale1
                Reference<QueryStatistics> stats = new Reference<>();

                List<Product> results = session.query(Product.class)
                    .statistics(stats)
                    .whereGreaterThan("PricePerUnit", 10)
                    .toList();

                if (stats.value.isStale()) {
                    // results are known to be stale
                }
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region stale2
                List<Product> results = session
                    .query(Product.class)
                    .waitForNonStaleResults(Duration.ofSeconds(5))
                    .whereGreaterThan("PricePerUnit", 10)
                    .toList();
                //endregion

                //region stale3
                store.addBeforeQueryListener(((sender, event) -> {
                    event.getQueryCustomization().waitForNonStaleResults();
                }));
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region stale4
                session.advanced().waitForIndexesAfterSaveChanges();
                //endregion

                //region stale5
                session
                    .advanced()
                    .waitForIndexesAfterSaveChanges(builder -> {
                        builder.withTimeout(Duration.ofSeconds(5))
                            .throwOnTimeout(false)
                            .waitForIndexes("Products/ByName");
                    });
                //endregion
            }

        }
    }
}
