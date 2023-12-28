package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class EntityChanges {
    private interface IFoo {
        //region has_changed_1
        boolean hasChanged(Object entity)
        //endregion
        ;
    }

    public HasChanged() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region has_changed_2
                Employee employee = session.load(Employee.class, "employees/1-A");
                boolean hasChanged = session.advanced().hasChanged(employee);// false
                employee.setLastName("Shmoe");
                hasChanged = session.advanced().hasChanged(employee);// true
                //endregion
            }
        }
    }

    private static class Employee {
        private String lastName;

        public String getLastName() {
            return lastName;
        }

        public void setLastName(String lastName) {
            this.lastName = lastName;
        }
    }
}
