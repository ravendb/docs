package net.ravendb.clientapi.session.howto;

import static org.junit.Assert.assertEquals;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class Refresh {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region refresh_1
    public <T> void refresh(T entity);
    //endregion
  }

  public Refresh() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region refresh_2
        Employee employee = session.load(Employee.class, "employees/1");
        assertEquals("Doe", employee.getLastName());

        // LastName changed to 'Shmoe'

        session.advanced().refresh(employee);
        assertEquals("Shmoe", employee.getLastName());
        //endregion
      }
    }
  }
}
