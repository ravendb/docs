package net.ravendb.clientapi.session.howto;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;

public class HasChanged {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region has_changed_1
    public boolean hasChanged(Object entity);
    //endregion
  }

  @SuppressWarnings("unused")
  public HasChanged() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region has_changed_2
        Employee employee = session.load(Employee.class, "employees/1");
        boolean hasChanged = session.advanced().hasChanged(employee); // false
        employee.setLastName("Shmoe");
        hasChanged = session.advanced().hasChanged(employee); //true
        //endregion
      }
    }
  }
}
