package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.SessionOptions;

public class OpeningSession {
    private interface IFoo {
        //region open_session_1
        // Open session for a 'default' database configured in 'DocumentStore'
        IDocumentSession openSession();

        // Open session for a specified database
        IDocumentSession openSession(String database);

        IDocumentSession openSession(SessionOptions sessionOptions);
        //endregion
    }

    public OpeningSession() {
        String databaseName = "DB1";

        try (IDocumentStore store = new DocumentStore()) {
            //region open_session_2
            store.openSession(new SessionOptions());
            //endregion

            //region open_session_3
            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.setDatabase(databaseName);
            store.openSession(sessionOptions);
            //endregion

            //region open_session_4
            try (IDocumentSession session = store.openSession()) {
                // code here
            }
            //endregion
        }
    }
}
