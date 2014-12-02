package net.ravendb.clientapi.configuration.conventions;

import net.ravendb.client.delegates.RequestCachePolicy;
import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentStore;


public class Caching {

  public Caching() {
    DocumentStore store = new DocumentStore();
    DocumentConvention conventions = store.getConventions();

    //region should_cache
    conventions.setShouldCacheRequest(new RequestCachePolicy() {
      @Override
      public Boolean shouldCacheRequest(String url) {
        return Boolean.TRUE;
      }
    });
    //endregion

    //region should_aggressive_cache_track_changes
    conventions.setShouldAggressiveCacheTrackChanges(true);
    //endregion

    //region should_save_changes_force_aggressive_cache_check
    conventions.setShouldSaveChangesForceAggressiveCacheCheck(true);
    //endregion
  }
}
