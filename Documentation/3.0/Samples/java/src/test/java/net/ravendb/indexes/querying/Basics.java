package net.ravendb.indexes.querying;

import java.util.List;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.QEmployee;
import net.ravendb.samples.northwind.QProduct;



public class Basics {
  public class Employees_ByFirstName extends AbstractIndexCreationTask
  {
    //empty
  }

  @SuppressWarnings("unused")
  public Basics() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {

      try (IDocumentSession session = store.openSession()) {
        //region basics_0_0
        // load up to 128 entities from 'Employees' collection
        List<Employee> results = session
          .query(Employee.class)
          .toList(); // send query
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region basics_0_1
        // load up to 128 entities from 'Employees' collection
        // where 'firstName' is 'Robert'
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .where(e.firstName.eq("Robert"))
          .toList(); // send query
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region basics_0_2
        // load up to 10 entities from 'Products' collection
        // where there are more than 10 units in stock
        // skip first 5 results
        QProduct p = QProduct.product;
        List<Product> results = session
          .query(Product.class)
          .where(p.unitsInStock.gt(10))
          .skip(5)
          .take(10)
          .toList(); // send query
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region basics_0_3
        // load up to 128 entities from 'Employees' collection
        // where 'FirstName' is 'Robert'
        // using 'Employees/ByFirstName' index
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class, Employees_ByFirstName.class)
          .where(e.firstName.eq("Robert"))
          .toList(); // send query
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region basics_0_4
        // load up to 128 entities from 'Employees' collection
        // where 'FirstName' is 'Robert'
        // using 'Employees/ByFirstName' index
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class, "Employees/ByFirstName")
          .where(e.firstName.eq("Robert"))
          .toList(); // send query
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region basics_1_0
        // load up to 128 entities from 'Employees' collection
        // where 'FirstName' is 'Robert'
        // using 'Employees/ByFirstName' index
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .advanced()
          .documentQuery(Employee.class, Employees_ByFirstName.class)
          .whereEquals(e.firstName, "Robert")
          .toList(); // send query
        //endregion
      }
    }
  }

}
