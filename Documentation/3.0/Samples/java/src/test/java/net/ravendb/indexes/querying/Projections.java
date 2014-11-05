package net.ravendb.indexes.querying;

import java.util.List;

import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.indexing.FieldStorage;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;

import com.mysema.query.annotations.QueryEntity;


public class Projections {

  //region projections_3_3
  @QueryEntity
  public static class EmployeeFirstAndLastName {
    private String firstName;
    private String lastName;

    public String getFirstName() {
      return firstName;
    }

    public void setFirstName(String firstName) {
      this.firstName = firstName;
    }

    public String getLastName() {
      return lastName;
    }

    public void setLastName(String lastName) {
      this.lastName = lastName;
    }
  }
  //endregion

  //region projections_1_3
  public static class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
    public Employees_ByFirstAndLastName() {
      map =
        " from employee in docs.Employees       " +
        " select new                            " +
        "  {                                    " +
        "      FirstName = employee.FirstName,  " +
        "      LastName = employee.LastName     " +
        "  }; ";
    }
  }
  //endregion

  //region projections_2_3
  public class Employees_ByFirstAndLastNameWithStoredFields extends AbstractIndexCreationTask {
    public Employees_ByFirstAndLastNameWithStoredFields() {
      QEmployee e = QEmployee.employee;
      map =
       " from employee in docs.Employees     " +
       " select new                          " +
       " {                                   " +
       "     FirstName = employee.FirstName, " +
       "     LastName = employee.LastName    " +
       " }; ";
      store(e.firstName, FieldStorage.YES);
      store(e.lastName, FieldStorage.YES);
    }
  }
  //endregion

  @SuppressWarnings("unused")
  public Projections() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region projections_1_0
        List<EmployeeFirstAndLastName> results = session
          .query(Employee.class, Employees_ByFirstAndLastName.class)
          .select(EmployeeFirstAndLastName.class)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region projections_1_1
        List<EmployeeFirstAndLastName> results = session
          .advanced()
          .documentQuery(Employee.class, Employees_ByFirstAndLastName.class)
          .selectFields(EmployeeFirstAndLastName.class)
          .toList();
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region projections_1_2
      IndexQuery query = new IndexQuery();
      query.setFieldsToFetch(new String[] { "FirstName", "LastName" });
      QueryResult result = store
        .getDatabaseCommands()
        .query("Employees/ByFirstAndLastName", query);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region projections_2_0
        List<EmployeeFirstAndLastName> results = session
          .query(Employee.class, Employees_ByFirstAndLastNameWithStoredFields.class)
          .select(EmployeeFirstAndLastName.class)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region projections_2_1
        List<EmployeeFirstAndLastName> results = session
          .advanced()
          .documentQuery(Employee.class, Employees_ByFirstAndLastNameWithStoredFields.class)
          .selectFields(EmployeeFirstAndLastName.class)
          .toList();
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region projections_2_2
      IndexQuery query = new IndexQuery();
      query.setFieldsToFetch(new String[] { "FirstName", "LastName" });
      QueryResult result = store
        .getDatabaseCommands()
        .query("Employees/ByFirstAndLastNameWithStoredFields", query);
      //endregion
    }
  }
}
