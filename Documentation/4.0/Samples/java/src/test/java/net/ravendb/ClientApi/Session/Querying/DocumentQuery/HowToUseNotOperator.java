package net.ravendb.ClientApi.Session.Querying.DocumentQuery;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class HowToUseNotOperator {
    public HowToUseNotOperator() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region not_1
                // load all entities from 'Employees' collection
                // where firstName NOT equals 'Robert'
                List<Employee> employees = session
                    .advanced()
                    .documentQuery(Employee.class)
                    .not()
                    .whereEquals("firstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region not_2
                // load all entities from 'Employees' collection
                // where firstName NOT equals 'Robert'
                // and lastName NOT equals 'King'
                List<Employee> employees = session
                    .advanced()
                    .documentQuery(Employee.class)
                    .not()
                    .openSubclause()
                    .whereEquals("firstName", "Robert")
                    .andAlso()
                    .whereEquals("lastName", "King")
                    .closeSubclause()
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region not_3
                // load all entities from 'Employees' collection
                // where firstName NOT equals 'Robert'
                // and lastName NOT equals 'King'
                // identical to 'Example II' but 'whereNotEquals' is used
                List<Employee> employees = session
                    .advanced()
                    .documentQuery(Employee.class)
                    .whereNotEquals("firstName", "Robert")
                    .andAlso()
                    .whereNotEquals("lastName", "King")
                    .toList();
                //endregion
            }
        }
    }

    private static class Employee {

    }
}
