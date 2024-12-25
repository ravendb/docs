package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class WhatAreIndexes {

    //region indexes_1
    // Define the index:
    // =================
    
    public static class Employees_ByNameAndCountry extends AbstractIndexCreationTask {
        public Employees_ByNameAndCountry() {
            map =  "docs.Employees.Select(employee => new { " +
                "    LastName = employee.LastName, " +
                "    FullName = (employee.FirstName + \" \") + employee.LastName, " +
                "    Country = employee.Address.Country " +
                "})";
        }
    }
    //endregion

    private static class Employee {
    }

    public WhatAreIndexes() {
        try (IDocumentStore store = new DocumentStore()) {
            //region indexes_2
            // Deploy the index to the server:
            // ===============================
            
            new Employees_ByNameAndCountry().execute(store);
            //endregion

            try (IDocumentSession session = store.openSession()) {
                //region indexes_3
                // Query the database using the index: 
                // ===================================
                
                List<Employee> employeesFromUK = session
                    .query(Employee.class, Employees_ByNameAndCountry.class)
                     // Here we query for all Employee documents that are from the UK
                     // and have 'King' in their LastName field:                     
                    .whereEquals("LastName", "King")
                    .whereEquals("Country", "UK")
                    .toList();
                //endregion
            }
        }
    }
}
