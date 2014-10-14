package net.ravendb.clientapi.session;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.document.OpenSessionOptions;


public class OpeningSession {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region open_session_1
    // Open session for a 'default' database configured in 'DocumentStore'
    public IDocumentSession openSession();

    // Open session for a specified database
    public IDocumentSession openSession(String database);

    public IDocumentSession openSession(OpenSessionOptions sessionOptions);
    //endregion
  }

  public OpeningSession() throws Exception {
    String databaseName = "DB1";

    try (IDocumentStore store = new DocumentStore()) {
      //region open_session_2
      store.openSession(new OpenSessionOptions());
      //endregion

      //region open_session_3
      OpenSessionOptions sessionOptions = new OpenSessionOptions();
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
