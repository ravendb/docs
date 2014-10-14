package net.ravendb.clientapi.session.howto;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class Evict {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region evict_1
    public <T> void evict(T entity);
    //endregion
  }

  public Evict() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region evict_2
        Employee employee1 = new Employee();
        employee1.setFirstName("John");
        employee1.setLastName("Doe");

        Employee employee2 = new Employee();
        employee2.setFirstName("John");
        employee2.setLastName("Shmoe");

        session.store(employee1);
        session.store(employee2);

        session.advanced().evict(employee1);

        session.saveChanges(); // only 'Joe Shmoe' will be saved
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region evict_3
        Employee employee = session.load(Employee.class, "employees/1"); //loading from server
        employee = session.load(Employee.class, "employees/1"); // no server call
        session.advanced().evict(employee);
        employee = session.load(Employee.class, "employees/1"); //loading from server
        //endregion
      }
    }
  }
}
