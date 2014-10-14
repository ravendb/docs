package net.ravendb.clientapi.session.configuration;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class MaxRequests {
  public MaxRequests() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region max_requests_1
        session.advanced().setMaxNumberOfRequestsPerSession(50);
        //endregion
      }

      //region max_requests_2
      store.getConventions().setMaxNumberOfRequestsPerSession(100);
      //endregion
    }
  }
}
