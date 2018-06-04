package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

import java.time.Duration;

public class SavingChanges {

    private interface IFoo {
        //region saving_changes_1
        void saveChanges();
        //endregion
    }

    private class Employee {
        private String firstName;
        private String lastName;

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
    }
}
