package net.ravendb.clientapi.howto;

import java.util.List;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.delegates.RequestCachePolicy;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Order;


public class SetupAggressiveCaching {
  @SuppressWarnings("unused")
  public SetupAggressiveCaching() throws Exception {
    try (DocumentStore documentStore = new DocumentStore()) {
      documentStore.setUrl("http://localhost:8080");
      documentStore.initialize();

      //region should_cache_delegate
      documentStore.getConventions().setShouldCacheRequest(new RequestCachePolicy() {
        @SuppressWarnings("boxing")
        @Override
        public Boolean shouldCacheRequest(String url) {
          return true;
        }
      });
      //endregion

      //region max_number_of_requests
      documentStore.setMaxNumberOfCachedRequests(2048);
      //endregion

      //region disable_changes_tracking
      documentStore.getConventions().setShouldAggressiveCacheTrackChanges(false);
      //endregion

      try (IDocumentSession session = documentStore.openSession()) {
        //region aggressive_cache_load
        try (AutoCloseable aggressivelyCacheFor = session.advanced().getDocumentStore().aggressivelyCacheFor(5 * 60 * 1000)) {
          Order user = session.load(Order.class, "orders/1");
        }
        //endregion
      }

      try (IDocumentSession session = documentStore.openSession()) {
        //region aggressive_cache_query
        try (AutoCloseable aggressivelyCacheFor = session.advanced().getDocumentStore().aggressivelyCacheFor(5 * 60 * 1000)) {
          List<Order> order = session.query(Order.class).toList();
        }
        //endregion

        //region aggressive_cache_for_one_day_1
        try (AutoCloseable scope = session.advanced().getDocumentStore().aggressivelyCache()) {
          // empty
        }
        //endregion

        //region aggressive_cache_for_one_day_2
        try (AutoCloseable scope = session.advanced().getDocumentStore().aggressivelyCacheFor(24 * 3600 * 1000)) {
          // empty
        }
        //endregion
      }
    }

    try (DocumentStore documentStore = new DocumentStore()) {
      documentStore.setUrl("http://localhost:8080");
      //region should_save_changes_force_aggressive_cache_check_convention
      documentStore.getConventions().setShouldSaveChangesForceAggressiveCacheCheck(true);
      //endregion
    }
  }
}
