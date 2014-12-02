package net.ravendb.clientapi.session.querying;

import java.util.List;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;


public class HowToQuery {

  private class MyCustomIndex extends AbstractIndexCreationTask {
    // ...
  }

  @SuppressWarnings("unused")
  private interface IFoo {
    //region query_1_0
    public <T> IRavenQueryable<T> query(Class<T> clazz);  // perform query on a dynamic index

    public <T> IRavenQueryable<T> query(Class<T> clazz, String indexName);

    public <T> IRavenQueryable<T> query(Class<T> clazz, String indexName, boolean isMapReduce);

    public <T> IRavenQueryable<T> query(Class<T> clazz, Class<? extends AbstractIndexCreationTask> indexCreator);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToQuery() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region query_1_1
        // load up to 128 entities from 'Employees' collection
        List<Employee> employees = session
          .query(Employee.class)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region query_1_2
        // load up to 128 entities from 'Employees' collection
        // where FirstName equals 'Robert'
        QEmployee e = QEmployee.employee;
        List<Employee> employees = session
          .query(Employee.class)
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region query_1_4
        // load up to 128 entities from 'Employees' collection
        // where FirstName equals 'Robert'
        // using 'My/Custom/Index'
        QEmployee e = QEmployee.employee;
        List<Employee> employees = session
          .query(Employee.class, "My/Custom/Index")
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region query_1_5
        // load up to 128 entities from 'Employees' collection
        // where FirstName equals 'Robert'
        // using 'My/Custom/Index'
        QEmployee e = QEmployee.employee;
        List<Employee> employees = session
          .query(Employee.class, MyCustomIndex.class)
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }
    }
  }
}
