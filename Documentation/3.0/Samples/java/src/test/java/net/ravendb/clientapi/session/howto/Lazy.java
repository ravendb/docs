package net.ravendb.clientapi.session.howto;

import java.util.List;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;

public class Lazy {

  @SuppressWarnings("unused")
  public Lazy() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region lazy_1
        net.ravendb.abstractions.basic.Lazy<Employee> employeeLazy =
          session.advanced().lazily().load(Employee.class, "employees/1");
        Employee employee = employeeLazy.getValue(); // load operation will be executed here
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region lazy_2
        net.ravendb.abstractions.basic.Lazy<List<Employee>> emplyeesLazy =
          session.query(Employee.class).lazily();
        session.advanced().eagerly().executeAllPendingLazyOperations(); // query will be executed here

        List<Employee> employees = emplyeesLazy.getValue();
        //endregion
      }
    }
  }
}
