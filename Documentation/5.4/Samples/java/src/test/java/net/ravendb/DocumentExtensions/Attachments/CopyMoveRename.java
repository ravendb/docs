package net.ravendb.ClientApi.Session.attachments;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.northwind.Employee;

public class CopyMoveRename {

    private interface IFoo {
        //region copy_0
        void copy(Object sourceEntity, String sourceName,
                  Object destinationEntity, String destinationName);

        void copy(String sourceDocumentId, String sourceName,
                  String destinationDocumentId, String destinationName);
        //endregion

        //region rename_0
        void rename(Object entity, String name, String newName);

        void rename(String documentId, String name, String newName);
        //endregion

        //region move_0
        void move(Object sourceEntity, String sourceName,
                  Object destinationEntity, String destinationName);

        void move(String sourceDocumentId, String sourceName,
                  String destinationDocumentId, String destinationName);
        //endregion
    }

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region copy_1
                Employee employee1 = session.load(Employee.class, "employees/1-A");
                Employee employee2 = session.load(Employee.class, "employees/2-A");

                session.advanced()
                    .attachments()
                    .copy(employee1, "photo.jpg", employee2, "photo-copy.jpg");

                session.saveChanges();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region rename_1
                Employee employee1 = session.load(Employee.class, "employees/1-A");

                session.advanced()
                    .attachments()
                    .rename(employee1, "photo.jpg", "photo-new.jpg");

                session.saveChanges();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region move_1

                Employee employee1 = session.load(Employee.class, "employees/1-A");
                Employee employee2 = session.load(Employee.class, "employees/2-A");

                session.advanced()
                    .attachments()
                    .copy(employee1, "photo.jpg", employee2, "photo.jpg");

                session.saveChanges();
                //endregion
            }
        }
    }
}
