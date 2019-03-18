package net.ravendb.ClientApi.Cluster;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class DocumentConflictsInClientSide {
    public DocumentConflictsInClientSide() {
        try (IDocumentStore store = new DocumentStore()) {
            //region PUT_Sample
            try (IDocumentSession session = store.openSession()) {
                User user = new User();
                user.setName("John Doe");

                session.store(user, "users/123");
                // users/123 is a conflicted document
                session.saveChanges();
                // when this request is finished, the conflict for user/132 is resolved.
            }
            //endregion

            //region DELETE_Sample
            try (IDocumentSession session = store.openSession()) {
                session.delete("users/123"); // users/123 is a conflicted document
                session.saveChanges(); //when this request is finished, the conflict for users/132 is resolved.
            }
            //endregion
        }
    }

    public static class User {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
