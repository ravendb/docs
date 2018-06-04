package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Lazy {
    public Lazy() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region lazy_1
                net.ravendb.client.documents.Lazy<Employee> employeeLazy = session
                    .advanced()
                    .lazily()
                    .load(Employee.class, "employees/1-A");

                Employee employee = employeeLazy.getValue(); // load operation will be executed here
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region lazy_2
                net.ravendb.client.documents.Lazy<List<Employee>> employeesLazy = session
                    .query(Employee.class)
                    .lazily();

                session.advanced().eagerly().executeAllPendingLazyOperations(); // query will be executed here

                List<Employee> employees = employeesLazy.getValue();
                //endregion
            }
        }
    }
    
    private class Employee {
    }
    
}
