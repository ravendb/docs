package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

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
                /* TODO
                session.Advanced.WaitForIndexesAfterSaveChanges(
                        timeout: TimeSpan.FromSeconds(30),
                        throwOnTimeout: true,
                        indexes: new[] { "index/1", "index/2" });

                    session.Store(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    session.SaveChanges();
                 */
                //endregion
            }

            /* TODO
             // storing new entity
                    //region saving_changes_4

                    session.Advanced.WaitForReplicationAfterSaveChanges(
                        timeout: TimeSpan.FromSeconds(30),
                        throwOnTimeout: false, //default true
                        replicas: 2, //minimum replicas to replicate
                        majority: false);

                    session.Store(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    session.SaveChanges();
                    //endregion
             */
        }
    }
}
