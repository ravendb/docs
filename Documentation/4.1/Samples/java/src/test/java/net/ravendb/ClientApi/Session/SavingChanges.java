package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.SessionOptions;
import net.ravendb.client.documents.session.TransactionMode;

import java.time.Duration;

public class SavingChanges {

    private interface IFoo {
        //region saving_changes_1
        void saveChanges();
        //endregion
    }

    private class Employee {
        private String id;
        private String firstName;
        private String lastName;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getFirstName() {
            return firstName;
        }

        public void setFirstName(String firstName) {
            this.firstName = firstName;
        }

        public String getLastName() {
            return lastName;
        }

        public void setLastName(String lastName) {
            this.lastName = lastName;
        }
    }

    public SavingChanges() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region saving_changes_2
                Employee employee = new Employee();
                employee.setFirstName("John");
                employee.setLastName("Doe");

                session.store(employee);
                session.saveChanges();;
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region saving_changes_3
                session.advanced().waitForIndexesAfterSaveChanges(builder -> {
                    builder.withTimeout(Duration.ofSeconds(30))
                        .throwOnTimeout(true)
                        .waitForIndexes("index/1", "index/2");

                    Employee employee = new Employee();
                    employee.setFirstName("John");
                    employee.setLastName("Doe");
                    session.store(employee);

                    session.saveChanges();
                });
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region saving_changes_4
                session
                    .advanced()
                    .waitForReplicationAfterSaveChanges(builder -> {
                        builder.withTimeout(Duration.ofSeconds(30))
                            .throwOnTimeout(false) //default true
                            .numberOfReplicas(2)//minimum replicas to replicate
                            .majority(false);
                    });

                Employee employee = new Employee();
                employee.setFirstName("John");
                employee.setLastName("Doe");

                session.store(employee);
                session.saveChanges();
                //endregion
            }
        }

        //region cluster_store_with_compare_exchange
        try (IDocumentStore store = new DocumentStore()) {
            SessionOptions sessionOptions = new SessionOptions();
            // default is: TransactionMode.SINGLE_NODE
            sessionOptions.setTransactionMode(TransactionMode.CLUSTER_WIDE);
            try (IDocumentSession session = store.openSession(sessionOptions)) {
                Employee user = new Employee();
                user.setFirstName("John");
                user.setLastName("Doe");

                session.store(user);

                // this transaction is now conditional on this being
                // successfully created (so, no other users with this name)
                // it also creates an association to the new user's id
                session.advanced().clusterTransaction()
                    .createCompareExchangeValue("usernames/John", user.getId());

                session.saveChanges();
            }
        }
        //endregion
    }
}
