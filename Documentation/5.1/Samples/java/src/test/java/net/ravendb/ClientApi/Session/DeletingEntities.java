package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.batches.DeleteCommandData;
import net.ravendb.client.documents.session.IDocumentSession;

public class DeletingEntities {

    private interface IFoo {
        //region deleting_1
        <T> void delete(T entity);

        void delete(String id);

        void delete(String id, String expectedChangeVector);
        //endregion
    }

    private static class Employee {
    }

    public void deletingEntities() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region deleting_2
                Employee employee = session.load(Employee.class, "employees/1");

                session.delete(employee);
                session.saveChanges();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region deleting_3
                session.delete("employees/1");
                session.saveChanges();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region deleting_4
                session.delete("employees/1");
                //endregion

                //region deleting_5
                session.advanced().defer(new DeleteCommandData("employees/1", null));
                //endregion
            }
        }
    }
}
