package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.DocumentsChanges;
import net.ravendb.client.documents.session.IDocumentSession;
import org.junit.Assert;

import java.util.List;
import java.util.Map;

public class SessionChanges {

    private static class Employee {
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

    private interface IFoo {
        //region what_changed_1
        boolean hasChanges()
        //endregion
        ;

        //region what_changed_3
        Map<String, List<DocumentsChanges>> whatChanged()
        //endregion
        ;
    }

    public WhatChanged() {
        try (IDocumentStore store = new DocumentStore()) {
            //region what_changed_2
            try (IDocumentSession session = store.openSession()) {
                Assert.assertFalse(session.advanced().hasChanges());

                Employee employee = new Employee();
                employee.setFirstName("John");
                employee.setLastName("Doe");

                session.store(employee);

                Assert.assertTrue(session.advanced().hasChanges());
            }
            //endregion

            //region what_changed_4
            try (IDocumentSession session = store.openSession()) {
                Employee employee = new Employee();
                employee.setFirstName("Joe");
                employee.setLastName("Doe");
                session.store(employee);

                Map<String, List<DocumentsChanges>> changes = session.advanced().whatChanged();
                List<DocumentsChanges> employeeChanges = changes.get("employees/1-A");
                DocumentsChanges.ChangeType change
                    = employeeChanges.get(0).getChange(); // DocumentsChanges.ChangeType.DOCUMENT_ADDED

            }
            //endregion

            //region what_changed_5
            try (IDocumentSession session = store.openSession()) {
                Employee employee = session.load(Employee.class, "employees/1-A");// 'Joe Doe'
                employee.setFirstName("John");
                employee.setLastName("Shmoe");

                Map<String, List<DocumentsChanges>> changes = session.advanced().whatChanged();
                List<DocumentsChanges> employeeChanges = changes.get("employees/1-A");
                DocumentsChanges change1
                    = employeeChanges.get(0); //DocumentsChanges.ChangeType.FIELD_CHANGED
                DocumentsChanges change2
                    = employeeChanges.get(1); //DocumentsChanges.ChangeType.FIELD_CHANGED
            }
            //endregion
        }
    }
}
