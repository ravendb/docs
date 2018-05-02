package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Collections;
import java.util.List;

public class Boosting {

    //region boosting_2
    public class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
        public Employees_ByFirstAndLastName() {
            map = "docs.Employees.Select(employee => new {" +
                "    firstName = employee.firstName.Boost(10)," +
                "    lastName = employee.lastName" +
                "})";
        }
    }
    //endregion

    private static class Employee {

    }

    public Boosting() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region boosting_3
                // employees with 'firstName' equal to 'Bob'
                // will be higher in results
                // than the ones with 'lastName' match
                List<Employee> results = session.query(Employee.class, Employees_ByFirstAndLastName.class)
                    .whereEquals("firstName", "Bob")
                    .whereEquals("lastName", "Bob")
                    .toList();
                //endregion
            }

            //region boosting_4
            IndexDefinition indexDefinition = new IndexDefinition();
            indexDefinition.setName("Employees/ByFirstAndLastName");
            indexDefinition.setMaps(Collections.singleton(
                "docs.Employees.Select(employee => new {" +
                "    firstName = employee.firstName.Boost(10)," +
                "    lastName = employee.lastName" +
                "})"));

            store.maintenance().send(new PutIndexesOperation(indexDefinition));
            //endregion
        }
    }
}
