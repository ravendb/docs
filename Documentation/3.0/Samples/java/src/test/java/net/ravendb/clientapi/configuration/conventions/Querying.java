package net.ravendb.clientapi.configuration.conventions;

import net.ravendb.client.delegates.PropertyNameFinder;
import net.ravendb.client.document.ConsistencyOptions;
import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentStore;


public class Querying {

  public Querying() {
    DocumentStore store = new DocumentStore();
    DocumentConvention conventions = store.getConventions();

    //region find_prop_name
    conventions.setFindPropertyNameForIndex(new PropertyNameFinder() {
      @Override
      public String find(Class<?> indexedType, String indexedName, String path, String prop) {
        return (path + prop).replace(",", "_").replace(".", "_");
      }
    });
    conventions.setFindPropertyNameForDynamicIndex(new PropertyNameFinder() {
      @Override
      public String find(Class<?> indexedType, String indexedName, String path, String prop) {
        return path + prop;
      }
    });
    //endregion

    //region querying_consistency
    conventions.setDefaultQueryingConsistency(ConsistencyOptions.NONE);
    //endregion

    //region querying_consistency_2
    conventions.setDefaultQueryingConsistency(ConsistencyOptions.ALWAYS_WAIT_FOR_NON_STALE_RESULTS_AS_OF_LAST_WRITE);
    //endregion

    //region allow_queries_on_id
    conventions.setAllowQueriesOnId(false);
    //endregion
  }
}
