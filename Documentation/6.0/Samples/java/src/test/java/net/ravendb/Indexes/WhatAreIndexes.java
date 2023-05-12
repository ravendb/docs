package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class WhatAreIndexes {

    //region indexes_1
    public static class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
        public Employees_ByFirstAndLastName() {
            map =  "docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName," +
                "    LastName = employee.LastName" +
                "})";
        }
    }
    //endregion

    private static class Employee {
    }

    public WhatAreIndexes() {
        try (IDocumentStore store = new DocumentStore()) {
            //region indexes_2
            // save index on server
            new Employees_ByFirstAndLastName().execute(store);
            //endregion

            try (IDocumentSession session = store.openSession()) {
                //region indexes_3
                List<Employee> results = session
                    .query(Employee.class, Employees_ByFirstAndLastName.class)
                    .whereEquals("FirstName", "Robert")
                    .toList();
                //endregion
            }
        }
    }
}
