package net.ravendb.ClientApi.DocumentIdentifiers;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class HiloAlgorithm {
    public HiloAlgorithm() {
        //region return_hilo_1
        DocumentStore store = new DocumentStore();

        try (IDocumentSession session = store.openSession()) {
            // Store an entity will give us the hilo range (ex. 1-32)
            Employee employee = new Employee();
            employee.setFirstName("John");
            employee.setLastName("Doe");

            session.store(employee);

            session.saveChanges();
        }

        store.close(); // returning unused range [last=1, max=32]
        //endregion

        //region return_hilo_2
        DocumentStore newStore = new DocumentStore();

        try (IDocumentSession session = newStore.openSession()) {
            // Store an entity will give us the hilo range (ex. 1-32)
            Employee employee = new Employee();
            employee.setFirstName("John");
            employee.setLastName("Doe");

            // Store an entity after closing the last store will give us  (ex. 2-33)
            session.store(employee);

            session.saveChanges();
        }
        //endregion
    }

    private static class Employee {
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
}
