package net.ravendb.ClientApi.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.conventions.DocumentConventions;
import net.ravendb.client.primitives.CleanCloseable;

import java.time.Duration;
import java.util.List;

public class SetupAggressiveCaching {
    public SetupAggressiveCaching() {
        //region aggressive_cache_conventions
        try (IDocumentStore documentStore = new DocumentStore()) {
            DocumentConventions conventions = documentStore.getConventions();

            conventions.aggressiveCache().setDuration(Duration.ofMinutes(5));
            conventions.aggressiveCache().setMode(AggressiveCacheMode.TRACK_CHANGES);
            // Do your work here
        }
            //endregion
            
            //region aggressive_cache_global
            documentStore.aggressivelyCacheFor(Duration.ofMinutes(5));

            documentStore.aggressivelyCache(); // Defines the cache duration for 1 day
            //endregion

            try (IDocumentSession session = documentStore.openSession()) {
                //region aggressive_cache_load
                try (CleanCloseable cacheScope = session.advanced().getDocumentStore()
                        .aggressivelyCacheFor(Duration.ofMinutes(5))) {
                    Order user = session.load(Order.class, "orders/1");
                }
                //endregion

                //region disable_aggressive_cache
                try (CleanCloseable cacheScope = session.advanced().getDocumentStore()
                        .disableAggressiveCaching()) {
                    Order order = session.load(Order.class, "orders/1");
                }
                //endregion
            }

            try (IDocumentSession session = documentStore.openSession()) {
                //region aggressive_cache_query
                try (CleanCloseable cacheScope = session.advanced().getDocumentStore()
                        .aggressivelyCacheFor(Duration.ofMinutes(5))) {
                    List<Order> orders = session.query(Order.class)
                        .toList();
                }
                //endregion
                
                //region disable_change_tracking
                documentStore.aggressivelyCacheFor(Duration.ofMinutes(5), AggressiveCacheMode.DO_NOT_TRACK_CHANGES);

                // Disable change tracking for just one session:
                try (session.advanced().getDocumentStore().aggressivelyCacheFor(Duration.ofMinutes(5),
                    AggressiveCacheMode.DO_NOT_TRACK_CHANGES)) {
                }
                //endregion

                //region aggressive_cache_for_one_day_1
                try (CleanCloseable cacheScope = session
                    .advanced().getDocumentStore().aggressivelyCache()) {

                }
                //endregion

                //region aggressive_cache_for_one_day_2
                try (CleanCloseable cacheScope = session
                    .advanced().getDocumentStore().aggressivelyCacheFor(Duration.ofDays(1))) {

                }
                //endregion
            }
        }
    }

    private static class Order {
    }
}
