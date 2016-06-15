package net.ravendb.clientapi.session.howto;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class MarkAsReadonly {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region mark_as_readonly_1
    public void markReadOnly(Object entity);
    //endregion
  }

  public MarkAsReadonly() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region mark_as_readonly_2
        Employee employee = session.load(Employee.class, "employees/1");
        session.advanced().markReadOnly(employee);
        session.saveChanges();
        //endregion
      }
    }
  }
}
