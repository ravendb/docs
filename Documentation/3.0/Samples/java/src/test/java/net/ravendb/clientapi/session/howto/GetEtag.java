package net.ravendb.clientapi.session.howto;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class GetEtag {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_etag_1
    public <T> Etag getEtagFor(T instance);
    //endregion
  }

  @SuppressWarnings("unused")
  public GetEtag() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region get_etag_2
        Employee employee = session.load(Employee.class, "employees/1");
        Etag etag = session.advanced().getEtagFor(employee);
        //endregion
      }
    }
  }
}
