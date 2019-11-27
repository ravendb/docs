package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class Exists {

    private interface IExists {
        //region exists_1
        boolean exists(String documentId, String name);
        //endregion
    }

    public Exists() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region exists_2
                boolean exists = session
                                    .advanced()
                                    .attachments()
                                    .exists("categories/1-A","image.jpg");

                if (exists) {
                    // do something
                }
                //endregion
            }
        }
    }
}
