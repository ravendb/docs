package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import org.junit.Assert;

public class Refresh {
    private static class Employee {
        private String lastName;

        public String getLastName() {
            return lastName;
        }

        public void setLastName(String lastName) {
            this.lastName = lastName;
        }
    }

    public Refresh() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region refresh_2
                Employee employee = session.load(Employee.class, "employees/1");
                Assert.assertEquals("Doe", employee.getLastName());

                // LastName changed to 'Shmoe'

                session.advanced().refresh(employee);

                Assert.assertEquals("Shmoe", employee.getLastName());
                //endregion
            }
        }
    }

    private interface IFoo {
        //region refresh_1
        <T> void refresh(T entity);
        //endregion
    }
}
