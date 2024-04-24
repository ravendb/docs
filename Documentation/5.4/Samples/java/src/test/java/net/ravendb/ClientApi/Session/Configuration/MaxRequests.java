package net.ravendb.ClientApi.Session.Configuration;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class MaxRequests {
    public MaxRequests() {

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
