package net.ravendb.clientapi.session;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class SavingChanges {

  @SuppressWarnings("unused")
  private interface IInterface {
    //region saving_changes_1
    public void saveChanges();
    //endregion
  }

  public SavingChanges() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region saving_changes_2
        // storing new entity
        Employee employee = new Employee();
        employee.setFirstName("John");
        employee.setLastName("Doe");
        session.store(employee);

        session.saveChanges();
        //endregion
      }
    }
  }
}
