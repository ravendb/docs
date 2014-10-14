package net.ravendb.clientapi.session;

import java.util.UUID;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class DeletingEntities {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region deleting_1
    public <T> void delete(T entity);

    public <T> void delete(Class<T> clazz, Number id);

    public <T> void delete(Class<T> clazz, UUID id);

    public void delete(String id);
    //endregion
  }

  public DeletingEntities() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region deleting_2
        // if UseOptimisticConcurrency is set to 'true' (default 'false')
        // this 'delete' method will use loaded 'employees/1' etag for concurrency check
        // and might throw ConcurrencyException
        Employee employee = session.load(Employee.class, "employees/1");
        session.delete(employee);
        session.saveChanges();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region deleting_3
        // this 'Delete' method will not do any Etag-based concurrency checks
        // because Etag for 'employees/1' is unknown
        session.delete("employees/1");
        session.saveChanges();
        //endregion
      }
    }
  }
}
