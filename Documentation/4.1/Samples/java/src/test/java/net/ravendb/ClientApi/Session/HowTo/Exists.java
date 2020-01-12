package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class Exists {

    private interface IExists {
        //region exists_1
        boolean exists(String id);
        //endregion
    }

    public Exists() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region exists_2
                boolean exists = session.advanced().exists("employees/1-A");

                if (exists) {
                    // do something
                }
                //endregion
            }
        }
    }
}
