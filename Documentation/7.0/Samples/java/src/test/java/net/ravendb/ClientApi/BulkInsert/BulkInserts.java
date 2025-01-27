package net.ravendb.ClientApi.BulkInsert;

import net.ravendb.client.documents.BulkInsertOperation;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;

public class BulkInserts {

    private interface IFoo {
        //region bulk_inserts_1
        BulkInsertOperation bulkInsert();

        BulkInsertOperation bulkInsert(String database);
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

    public BulkInserts() {

        try (IDocumentStore store = new DocumentStore()) {
            //region bulk_inserts_4
            try (BulkInsertOperation bulkInsert = store.bulkInsert()) {
                for (int i = 0; i < 1_000_000; i++) {
                    Employee employee = new Employee();
                    employee.setFirstName("FirstName #" + i);
                    employee.setLastName("LastName #" + i);
                    bulkInsert.store(employee);
                }
            }
            //endregion
        }
    }
}
