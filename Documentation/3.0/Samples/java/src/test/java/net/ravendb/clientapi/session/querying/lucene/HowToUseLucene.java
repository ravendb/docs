package net.ravendb.clientapi.session.querying.lucene;

import java.util.List;

import net.ravendb.client.IDocumentQuery;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;


public class HowToUseLucene {

  private class MyCustomIndex extends AbstractIndexCreationTask {
    // ...
  }

  @SuppressWarnings("unused")
  private interface IFoo {
    //region document_query_1
    public <T, S extends AbstractIndexCreationTask> IDocumentQuery<T> documentQuery(Class<T> clazz, Class<S> indexClass);

    public <T> IDocumentQuery<T> documentQuery(Class<T> clazz, String indexName);

    public <T> IDocumentQuery<T> documentQuery(Class<T> clazz, String indexName, boolean isMapReduce);

    public <T> IDocumentQuery<T> documentQuery(Class<T> clazz);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToUseLucene() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region document_query_2
        // load up to 128 entities from 'Employees' collection
        List<Employee> employees = session.advanced().documentQuery(Employee.class).toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region document_query_3
        // load up to 128 entities from 'Employees' collection
        // where FirstName equals 'Robert'
        QEmployee e = QEmployee.employee;
        List<Employee> employees = session
          .advanced()
          .documentQuery(Employee.class)
          .whereEquals(e.firstName, "Robert")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region document_query_4
        // load up to 128 entities from 'Employees' collection
        // where FirstName equals 'Robert'
        // using 'My/Custom/Index'
        QEmployee e = QEmployee.employee;
        List<Employee> employees = session
          .advanced()
          .documentQuery(Employee.class, "My/Custom/Index")
          .whereEquals(e.firstName, "Robert")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region document_query_5
        // load up to 128 entities from 'Employees' collection
        // where FirstName equals 'Robert'
        // using 'My/Custom/Index'
        QEmployee e = QEmployee.employee;
        List<Employee> employees = session
          .advanced()
          .documentQuery(Employee.class, MyCustomIndex.class)
          .whereEquals(e.firstName, "Robert")
          .toList();
        //endregion
      }
    }
  }
}
