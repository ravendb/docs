package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class StoringEntities {

    private interface IFoo {
        //region store_entities_1
        void store(Object entity);
        //endregion

        //region store_entities_2
        void store(Object entity, String id);
        //endregion

        //region store_entities_3
        void store(Object entity, String changeVector, String id);
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

    public StoringEntities() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region store_entities_5
                Employee employee = new Employee();
                employee.setFirstName("John");
                employee.setLastName("Doe");

                // generate Id automatically
                session.store(employee);

                // send all pending operations to server, in this case only `Put` operation
                session.saveChanges();
                //endregion
            }
        }
    }
}
