package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class Clear {

    private interface IFoo {
        //region clear_1
        void clear()
        //endregion
        ;
    }

    public Clear() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region clear_2
                Employee employee = new Employee();
                employee.setFirstName("John");
                employee.setLastName("Doe");
                session.store(employee);

                session.advanced().clear();

                session.saveChanges(); // nothing will hapen
                //endregion
            }
        }
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
