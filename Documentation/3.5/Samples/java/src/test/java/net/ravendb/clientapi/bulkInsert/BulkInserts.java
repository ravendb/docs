package net.ravendb.clientapi.bulkInsert;

import net.ravendb.abstractions.data.BulkInsertOptions;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.BulkInsertOperation;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class BulkInserts {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region bulk_inserts_1
    public BulkInsertOperation bulkInsert();

    public BulkInsertOperation bulkInsert(String database);

    public BulkInsertOperation bulkInsert(String database, BulkInsertOptions options);
    //endregion
  }

  public BulkInserts() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region bulk_inserts_4
      try (BulkInsertOperation bulkInsert = store.bulkInsert()) {
        for (int i = 0; i < 1000 * 1000; i++){
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
