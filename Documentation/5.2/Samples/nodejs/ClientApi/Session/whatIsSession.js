import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function whatIsSessionUsingAwait() {
    {
        //region session_usage_1
        // Obtain a session from your Document Store
        const session = documentStore.openSession();

        // Create a new company entity
        class Company {
            constructor(name) {
                this.name = name;
            }
        }
        
        const entity = new Company("CompanyName");

        // Store the entity in the Session's internal map
        await session.store(entity);
        // From now on, any changes that will be made to the entity will be tracked by the session.
        // However, the changes will be persisted to the server only when 'saveChanges()' is called.

        await session.saveChanges();
        // At this point the entity is persisted to the database as a new document.
        // Since no database was specified when opening the session, the Default Database is used.
        //endregion
    }
    {
        //region session_usage_2
        // Open a session
        const session = documentStore.openSession();

        // Load an existing document to the session using its ID
        // The loaded entity will be added to the session's internal map
        const entity = await session.load("companies/1-A");

        // Edit the entity, the session will track this change
        entity.name = "NewCompanyName";

        await session.saveChanges();
        // At this point, the change made is persisted to the existing document in the database
        
        //endregion
    }
    {
        const session = documentStore.openSession();

        //region session_usage_3
        // A document is fetched from the server
        const entity1 = await session.load("companies/1-A");

        // Loading the same document will now retrieve its entity from the session's map
        const entity2 = await session.load("companies/1-A");

        // This command will Not throw an exception.
        assert.equal(entity1, entity2);
        //endregion
    }
}

function whatIsSessionUsingThen() {
    {
        //region session_usage_1_then
        // Obtain a session from your Document Store
        const session = documentStore.openSession();

        // Create a new company entity
        class Company {
            constructor(name) {
                this.name = name;
            }
        }

        const entity = new Company("CompanyName");

        // Store the entity in the Session's internal map
        session.store(entity)
            .then(() => {
                // From now on, any changes that will be made to the entity will be tracked by the session.
                // However, the changes will be persisted to the server only when 'saveChanges()' is called.
                
                session.saveChanges();
            })
            .then(() => {
                // At this point the entity is persisted to the database as a new document.
                // Since no database was specified when opening the session, the Default Database is used.
            });
        //endregion
    }
    {
        //region session_usage_2_then 
        // Open a session
        const session = documentStore.openSession();

        // Load an existing document to the session using its ID
        // The loaded entity will be added to the session's internal map
        session.load("companies/1-A")
            .then((company) => {
                // Edit the entity, the session will track this change
                company.name = "NewCompanyName";
            })
            .then(() => session.saveChanges())
            .then(() => {
                // At this point, the change made is persisted to the existing document in the database
            });
        //endregion
    }
}


