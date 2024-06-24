package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class QueryIndex {

    public static class Employees_ByFirstName extends AbstractIndexCreationTask {

    }

    private static class Employee {

    }

    private static class Product {

    }

    public QueryIndex() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region basics_0_0
                // load all entities from 'Employees' collection
                List<Employee> results = session
                    .query(Employee.class)
                    .toList(); // send query
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region basics_0_1
                // load all entities from 'Employees' collection
                // where 'firstName' is 'Robert'
                List<Employee> results = session
                    .query(Employee.class)
                    .whereEquals("FirstName", "Robert")
                    .toList(); // send query
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region basics_0_2
                // load up to 10 entities from 'Products' collection
                // where there are more than 10 units in stock
                // skip first 5 results
                List<Product> results = session
                    .query(Product.class)
                    .whereGreaterThan("UnitsInStock", 10)
                    .skip(5)
                    .take(10)
                    .toList();//send query
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region basics_0_3
                // load all entities from 'Employees' collection
                // where 'firstName' is 'Robert'
                // using 'Employees/ByFirstName' index
                session
                    .query(Employee.class, Employees_ByFirstName.class)
                    .whereEquals("FirstName", "Robert")
                    .toList(); // send query
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region basics_0_4
                // load all entities from 'Employees' collection
                // where 'firstName' is 'Robert'
                // using 'Employees/ByFirstName' index
                session
                    .query(Employee.class, Query.index("Employees/ByFirstName"))
                    .whereEquals("FirstName", "Robert")
                    .toList(); // send query
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region basics_3_0
                // load up entity from 'Employees' collection
                // with ID matching 'employees/1-A'
                Employee result = session
                    .query(Employee.class)
                    .whereEquals("Id", "employees/1-A")
                    .firstOrDefault();
                //endregion
            }
        }
    }
}
