package net.ravendb.clientapi.session.howto;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import java.util.List;
import java.util.Map;

import net.ravendb.abstractions.data.DocumentsChanges;
import net.ravendb.abstractions.data.DocumentsChanges.ChangeType;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class WhatChanged {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region what_changed_1
    public boolean hasChanges();
    //endregion

    //region what_changed_3
    public Map<String, List<DocumentsChanges>> whatChanged();
    //endregion
  }

  @SuppressWarnings("unused")
  public WhatChanged() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region what_changed_2
      try (IDocumentSession session = store.openSession()) {
        assertFalse(session.advanced().hasChanges());

        Employee employee = new Employee();
        employee.setFirstName("John");
        employee.setLastName("Doe");
        session.store(employee);

        assertTrue(session.advanced().hasChanges());
      }
      //endregion

      //region what_changed_4
      try (IDocumentSession session = store.openSession()) {
        Employee employee = new Employee();
        employee.setFirstName("John");
        employee.setLastName("Doe");
        session.store(employee);

        Map<String, List<DocumentsChanges>> changes = session.advanced().whatChanged();
        List<DocumentsChanges> employeeChanges = changes.get("employees/1");
        ChangeType change = employeeChanges.get(0).getChange(); // ChangeType.DOCUMENT_ADDED
      }
      //endregion

      //region what_changed_5
      try (IDocumentSession session = store.openSession()) {
        Employee employee = session.load(Employee.class, "employees/1"); // 'Joe Doe'
        employee.setFirstName("John");
        employee.setLastName("Shmoe");

        Map<String, List<DocumentsChanges>> changes = session.advanced().whatChanged();
        List<DocumentsChanges> employeeChanges = changes.get("employees/1");
        ChangeType change1 = employeeChanges.get(0).getChange(); // ChangeType.FIELD_CHANGED
        ChangeType change2 = employeeChanges.get(1).getChange(); // ChangeType.FIELD_CHANGED
      }
      //endregion
    }
  }
}
