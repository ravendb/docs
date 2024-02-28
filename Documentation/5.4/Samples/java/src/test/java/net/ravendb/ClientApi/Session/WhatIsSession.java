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
            // Obtain a Session from your Document Store
            try (IDocumentSession session = store.openSession()) {
            
                // Create a new entity
                Company entity = new Company();
                entity.setName("Company");
                
                // Store the entity in the Session's internal map
                session.store(entity);
                // From now on, any changes that will be made to the entity will be tracked by the Session.
                // However, the changes will be persisted to the server only when 'SaveChanges()' is called.
                
                session.saveChanges();
                // At this point the entity is persisted to the database as a new document.
                // Since no database was specified when opening the Session, the Default Database is used.
            }
            //endregion

            //region session_usage_2
            // Open a session
            try (IDocumentSession session = store.openSession()) {
                // Load an existing document to the Session using its ID
                // The loaded entity will be added to the session's internal map
                Company entity = session.load(Company.class, companyId);
                
                // Edit the entity, the Session will track this change
                entity.setName("NewCompanyName");

                session.saveChanges();
                // At this point, the change made is persisted to the existing document in the database
            }
            //endregion

            try (IDocumentSession session = store.openSession()) {
                //region session_usage_3
                // A document is fetched from the server
                Company entity1 = session.load(Company.class, companyId);
                
                // Loading the same document will now retrieve its entity from the Session's map
                Company entity2 = session.load(Company.class, companyId);
                
                // This command will Not throw an exception
                Assert.assertSame(entity1, entity2);
                //endregion
            }
        }
    }
}
