package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Date;

public class GetLastModified {

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
        //region get_last_modified_1
        <T> Date getLastModifiedFor(T instance)
        //endregion
        ;
    }

    public GetLastModified() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region get_last_modified_2
                Employee employee = session.load(Employee.class, "employees/1-A");
                Date lastModified = session.advanced().getLastModifiedFor(employee);
                //endregion
            }
        }
    }

}
