package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import org.junit.Assert;

public class WhatIsSession {

    private static class Company {
        private String id;
        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    public WhatIsSession() {
        try (IDocumentStore store = new DocumentStore()) {
            //region session_usage_1
            String companyId;

            //store new object
            try (IDocumentSession session = store.openSession()) {
                Company entity = new Company();
                entity.setName("Company");
                session.store(entity);
                session.saveChanges();

                // after calling saveChanges(), an id field if exists
                // is filled by the entity's id
                companyId = entity.getId();
            }

            try (IDocumentSession session = store.openSession()) {
                //load by id
                Company entity = session.load(Company.class, companyId);

                // do something with the loaded entity
            }
            //endregion

            //region session_usage_2
            try (IDocumentSession session = store.openSession()) {
                Company entity = session.load(Company.class, companyId);
                entity.setName("Another Company");

                // when a document is loaded by Id (or by query),
                // its changes are tracked (by default).
                // A call to saveChanges() sends all accumulated changes to the server
                session.saveChanges();
            }
            //endregion

            try (IDocumentSession session = store.openSession()) {
                //region session_usage_3
                Assert.assertSame(session.load(Company.class, companyId), session.load(Company.class, companyId));
                //endregion
            }
        }
    }
}
