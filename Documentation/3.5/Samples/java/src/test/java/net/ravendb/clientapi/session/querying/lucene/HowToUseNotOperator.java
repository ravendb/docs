package net.ravendb.clientapi.session.querying.lucene;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;

import java.util.List;

public class HowToUseNotOperator {

    public HowToUseNotOperator() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                QEmployee e = QEmployee.employee;
                //region not_1
                // load up to 128 entities from 'Employees' collection
                // where FirstName NOT equals 'Robert'
                List<Employee> employees = session.advanced().documentQuery(Employee.class).not()
                    .whereEquals(e.firstName, "Robert").toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region not_2
                QEmployee e = QEmployee.employee;
                // load up to 128 entities from 'Employees' collection
                // where FirstName NOT equals 'Robert'
                // and LastName NOT equals 'King'
                List<Employee> employees = session.advanced()
                    .documentQuery(Employee.class)
                    .not()
                    .openSubclause()
                    .whereEquals(e.firstName, "Robert")
                    .andAlso()
                    .whereEquals(e.lastName, "King")
                    .closeSubclause()
                    .toList();
                //endregion
            }
        }
    }
}
