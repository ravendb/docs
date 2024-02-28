package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.SessionOptions;
import net.ravendb.client.documents.session.TransactionMode;
import net.ravendb.client.http.RequestExecutor;
import net.ravendb.northwind.Employee;
import org.junit.Assert;

public class OpeningSession {
    private class IFoo2 {
        //region session_options
        private String database;
        private boolean noTracking;
        private boolean noCaching;
        private RequestExecutor requestExecutor;
        private TransactionMode transactionMode;

        // getters and setters
        //endregion
    }

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

            {
                //region open_session_3
                SessionOptions sessionOptions = new SessionOptions();
                sessionOptions.setDatabase(databaseName);
                store.openSession(sessionOptions);
                //endregion
            }

            //region open_session_4
            try (IDocumentSession session = store.openSession()) {
                // code here
            }
            //endregion

            {
                //region open_session_tracking_1
                SessionOptions sessionOptions = new SessionOptions();
                sessionOptions.setNoTracking(true);
                try (IDocumentSession session = store.openSession()) {
                    Employee employee1 = session.load(Employee.class, "employees/1-A");
                    Employee employee2 = session.load(Employee.class, "employees/1-A");

                    // because NoTracking is set to 'true'
                    // each load will create a new Employee instance
                    Assert.assertNotSame(employee1, employee2);
                }
                //endregion
            }

            {
                //region open_session_caching_1
                SessionOptions sessionOptions = new SessionOptions();
                sessionOptions.setNoCaching(true);
                try (IDocumentSession session = store.openSession()) {
                    // code here
                }
                //endregion
            }
        }
    }
}
