package net.ravendb.clientapi.session.howto;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class GetEntityUrl {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_entity_url_1
    public String getDocumentUrl(Object entity);
    //endregion
  }

  @SuppressWarnings("unused")
  public GetEntityUrl() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region get_entity_url_2
        Employee employee = session.load(Employee.class, "employees/1");
        // http://localhost:8080/databases/Northwind/docs/employees/1
        String url = session.advanced().getDocumentUrl(employee);
        //endregion
      }
    }
  }
}
