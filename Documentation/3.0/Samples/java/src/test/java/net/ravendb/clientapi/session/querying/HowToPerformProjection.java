package net.ravendb.clientapi.session.querying;

import com.mysema.query.annotations.QueryEntity;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class HowToPerformProjection {

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

  public HowToPerformProjection() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region projection_1
        // request 'FirstName' and 'LastName' from server
        // and project it to 'EmployeeFirstAndLastName'
        session
          .query(Employee.class)
          .select(EmployeeFirstAndLastName.class, "firstName", "lastName")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region projection_2
        // request 'FirstName' and 'LastName' from server
        // and project it to 'EmployeeFirstAndLastName'
        QHowToPerformProjection_EmployeeFirstAndLastName e =
          QHowToPerformProjection_EmployeeFirstAndLastName.employeeFirstAndLastName;
        session
          .query(Employee.class)
          .select(EmployeeFirstAndLastName.class, e.firstName, e.lastName)
          .toList();
        //endregion
      }
    }
  }

}
