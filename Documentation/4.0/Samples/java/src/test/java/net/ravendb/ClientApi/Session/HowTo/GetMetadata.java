package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.IMetadataDictionary;

public class GetMetadata {

    private static class Employee {

    }

    private static class User {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    public GetMetadata() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region get_metadata_2
                Employee employee = session.load(Employee.class, "employees/1-A");
                IMetadataDictionary metadata = session.advanced().getMetadataFor(employee);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region modify_metadata_1
                User user = new User();
                user.setName("Idan");

                session.store(user);

                IMetadataDictionary metadata = session.advanced().getMetadataFor(user);
                metadata.put("Permissions", "READ_ONLY");
                session.saveChanges();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region modify_metadata_2
                User user = session.load(User.class, "users/1-A");
                IMetadataDictionary metadata = session.advanced().getMetadataFor(user);
                metadata.put("Permissions", "READ_AND_WRITE");
                session.saveChanges();
                //endregion
            }
        }
    }

    private interface IFoo {
        //region get_metadata_1
        <T> IMetadataDictionary getMetadataFor(T instance);
        //endregion
    }
}
