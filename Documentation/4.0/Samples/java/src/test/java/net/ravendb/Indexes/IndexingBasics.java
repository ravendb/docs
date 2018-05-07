package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class IndexingBasics {
    private static class Employee {
    }

    public IndexingBasics() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region indexes_2
                List<Employee> employees = session.query(Employee.class, Query.index("Employees/ByFirstName"))
                    .whereEquals("firstName", "Robert")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region indexes_4
                List<Employee> employees = session.advanced()
                    .rawQuery(Employee.class, "from index 'Employees/ByFirstName' where FirstName = 'Robert'")
                    .toList();
                //endregion
            }
        }
    }
}
