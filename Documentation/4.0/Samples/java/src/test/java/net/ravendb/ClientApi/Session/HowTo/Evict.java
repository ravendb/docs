package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class Evict {
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

    private interface IFoo {
        //region evict_1
        <T> void evict(T entity);
        //endregion
    }

    public Evict() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region evict_2
                Employee employee1 = new Employee();
                employee1.setFirstName("John");
                employee1.setLastName("Doe");

                Employee employee2 = new Employee();
                employee2.setFirstName("Joe");
                employee2.setLastName("Shmoe");

                session.store(employee1);
                session.store(employee2);

                session.advanced().evict(employee1);

                session.saveChanges(); // only 'Joe Shmoe' will be saved
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region evict_3
                Employee employee = session.load(Employee.class, "employees/1-A");//loading from serer
                employee = session.load(Employee.class, "employees/1-A"); // no server call
                session.advanced().evict(employee);
                employee = session.load(Employee.class, "employees/1-A"); // loading form server
                //endregion
            }
        }
    }
}
