package net.ravendb.clientapi.session.querying;

import net.ravendb.abstractions.basic.CloseableIterator;
import net.ravendb.abstractions.basic.Reference;
import net.ravendb.abstractions.data.QueryHeaderInformation;
import net.ravendb.abstractions.data.StreamResult;
import net.ravendb.client.IDocumentQuery;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;


public class HowToStream {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region stream_1
    public <T> CloseableIterator<StreamResult<T>> stream(IRavenQueryable<T> query);

    public <T> CloseableIterator<StreamResult<T>> stream(IRavenQueryable<T> query, Reference<QueryHeaderInformation> queryHeaderInformation);

    public <T> CloseableIterator<StreamResult<T>> stream(IDocumentQuery<T> query);

    public <T> CloseableIterator<StreamResult<T>> stream(IDocumentQuery<T> query, Reference<QueryHeaderInformation> queryHeaderInformation);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToStream() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region stream_2
        QEmployee e = QEmployee.employee;
        IRavenQueryable<Employee> query = session
          .query(Employee.class)
          .where(e.firstName.eq("Robert"));

        try (CloseableIterator<StreamResult<Employee>> results = session.advanced().stream(query)) {
          while (results.hasNext()) {
            StreamResult<Employee> employee = results.next();
          }
        }
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region stream_3
        QEmployee e = QEmployee.employee;
        IDocumentQuery<Employee> query = session
          .advanced()
          .documentQuery(Employee.class)
          .whereEquals(e.firstName, "Robert");

        Reference<QueryHeaderInformation> queryHeaderInfoRef = new Reference<>();
        try (CloseableIterator<StreamResult<Employee>> results = session.advanced().stream(query, queryHeaderInfoRef)) {
          while (results.hasNext()) {
            StreamResult<Employee> employee = results.next();
          }
        }
        //endregion
      }
    }
  }
}
