package net.ravendb.clientapi.session;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class StoringEntities {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region store_entities_1
    public void store(Object entity);
    //endregion

    //region store_entities_2
    public void store(Object entity, String id);
    //endregion

    //region store_entities_3
    public void store(Object entity, Etag etag);
    //endregion

    //region store_entities_4
    public void store(Object entity, Etag etag, String id);
    //endregion
  }

  public StoringEntities() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region store_entities_5
        // generate Id automatically
        // when database is new and empty database and conventions are not changed: 'employee/1'
        Employee employee = new Employee();
        employee.setFirstName("John");
        employee.setLastName("Doe");
        session.store(employee);

        // send all pending operations to server, in this case only `put` operation
        session.saveChanges();
        //endregion
      }
    }
  }
}
