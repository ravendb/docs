package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class GetChangeVector {

    private interface IFoo {
        //region get_change_vector_1
        <T> String getChangeVectorFor(T instance)
        //endregion
        ;
    }

    public GetChangeVector() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region get_change_vector_2
                Employee employee = session.load(Employee.class, "employees/1-A");
                String changeVector = session.advanced().getChangeVectorFor(employee);
                //endregion
            }
        }
    }

    private static class Employee {

    }
}
