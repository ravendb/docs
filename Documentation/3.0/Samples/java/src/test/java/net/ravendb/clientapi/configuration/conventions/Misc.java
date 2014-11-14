package net.ravendb.clientapi.configuration.conventions;

import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentStore;


public class Misc {
  public Misc() {
    DocumentStore store = new DocumentStore();
    DocumentConvention conventions = store.getConventions();

    //region disable_profiling
    conventions.setDisableProfiling(true);
    //endregion

    //region max_number_of_requests_per_session
    conventions.setMaxNumberOfRequestsPerSession(10);
    //endregion

    //region save_enums_as_integers
    conventions.setSaveEnumsAsIntegers(false);
    //endregion

    //region use_optimistic_concurrency_by_default
    conventions.setDefaultUseOptimisticConcurrency(false);
    //endregion

  }

}
