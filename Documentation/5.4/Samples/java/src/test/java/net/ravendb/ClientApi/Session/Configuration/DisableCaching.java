package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.SessionOptions;
import net.ravendb.client.documents.session.TransactionMode;
import net.ravendb.client.http.RequestExecutor;
import net.ravendb.northwind.Employee;
import org.junit.Assert;

public class DisableCaching {

    public DisableCaching() {
        String databaseName = "DB1";

        try (IDocumentStore store = new DocumentStore()) {
        
           //region disable_caching
           SessionOptions sessionOptions = new SessionOptions();
           sessionOptions.setNoCaching(true);
           try (IDocumentSession session = store.openSession(sessionOptions)) {
               // code here
           }
           //endregion
        }
    }
}
