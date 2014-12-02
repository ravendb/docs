package net.ravendb.clientapi.session.howto;

import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class GetMetadata {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_metadata_1
    public <T> RavenJObject getMetadataFor(T instance);
    //endregion
  }

  @SuppressWarnings("unused")
  public GetMetadata() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region get_metadata_2
        Employee employee = session.load(Employee.class, "employees/1");
        RavenJObject metadata = session.advanced().getMetadataFor(employee);
        //endregion
      }
    }
  }
}
