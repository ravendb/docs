package net.ravendb.clientapi.session.howto;

import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.samples.northwind.Employee;

public class Clear {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region clear_1
    public void clear();
    //endregion
  }

  public Clear() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region clear_2
        Employee employee = new Employee();
        employee.setFirstName("John");
        employee.setLastName("Doe");

        session.store(employee);

        session.advanced().clear();

        session.saveChanges(); // nothing will happen
        //endregion
      }
    }
  }
}
