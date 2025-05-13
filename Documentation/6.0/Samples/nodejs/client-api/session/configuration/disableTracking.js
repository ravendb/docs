import { DocumentStore } from "ravendb";
import assert from "assert";

async function whatIsSessionUsingAwait() {
    {
        const documentStore = new DocumentStore();
        //region disable_tracking_1
        const session = documentStore.openSession();        

        // Load a product entity - the session will track the entity by default
        const product = await session.load("products/1-A");

        // Call 'ignoreChangesFor' to instruct the session to ignore changes made to this entity
        session.advanded.ignoreChangesFor(product);

        // The following change will be ignored by SaveChanges - it will not be persisted
        product.unitsInStock += 1;
        
        await session.saveChanges();
        //endregion
    }
    {
        const documentStore = new DocumentStore();
        //region disable_tracking_2
        const session = documentStore.openSession({
            // Disable tracking for all entities in the session's options
            noTracking: true
        });

        // Load any entity, it will Not be tracked by the session
        const employee1 = await session.load("employees/1-A");

        // Loading again from same document will result in a new entity instance
        const employee2 = await session.load("employees/1-A");

        // Entities instances are not the same
        assert.notStrictEqual(company1, company2);
        
        // Calling saveChanges will throw an exception
        await session.saveChanges();
        //endregion
    }
    {
        const documentStore = new DocumentStore();
        //region disable_tracking_3
        const session = documentStore.openSession();

        // Define a query
        const employeesResults = await session.query({ collection: "employees" })
            .whereEquals("FirstName", "Robert")
             // Set noTracking, all resulting entities will not be tracked
            .noTracking()
            .all();

        // The following modification will not be tracked for saveChanges
        const firstEmployee = employeesResults[0];
        firstEmployee.lastName = "NewName";

        // Change to 'firstEmployee' will not be persisted
        session.saveChanges();
        //endregion
    }
    {
        //region disable_tracking_4
        const customStore = new DocumentStore();
        
        // Define the 'ignore' convention on your document store
        customStore.conventions.shouldIgnoreEntityChanges =
            (sessionOperations, entity, documentId) => {
                // Define for which entities tracking should be disabled 
                // Tracking will be disabled ONLY for entities of type Employee whose firstName is Bob
                return entity instanceof Employee && entity.firstName === "Bob";
            };
        customStore.initialize();

        const session = customStore.openSession();

        const employee1 = new Employee();
        employee1.firstName = "Alice";

        const employee2 = new Employee();
        employee2.firstName = "Bob";

        await session.store(employee1, "employees/1-A"); // This entity will be tracked
        await session.store(employee2, "employees/2-A"); // Changes to this entity will be ignored

        await session.saveChanges();   // Only employee1 will be persisted

        employee1.firstName = "Bob";   // Changes to this entity will now be ignored
        employee2.firstName = "Alice"; // This entity will now be tracked

        session.saveChanges();         // Only employee2 is persisted
        //endregion
    }
    {
        const documentStore = new DocumentStore();
        //region disable_tracking_5
        const session = documentStore.openSession({
            // Working with a non-tracking session
            noTracking: true
        });

        try {
            // Trying to include a related document when loading a document will throw
            const product = await session
                .include("supplier")
                .load("products/1-A");
        }
        catch (error) {
            // An exception is expected here
        }
        //endregion
    }
    {
        const documentStore = new DocumentStore();
        //region disable_tracking_6
        const session = documentStore.openSession({
            // Working with a non-tracking session
            noTracking: true
        });

        try {
            // Trying to include related documents in a query will throw
            const products = await session
                .query({ collection: 'products' })
                .include("supplier")
                .all();
        }
        catch (error) {
            // An exception is expected here
        }
        //endregion
    }
    {
        //region syntax_1
        session.advanced.ignoreChangesFor(entity);
        //endregion
    }
    {
        //region syntax_2
        store.conventions.shouldIgnoreEntityChanges = (sessionOperations, entity, documentId) => {
            // Write your logic
            // return value:
            //     true - entity will not be tracked 
            //     false - entity will be tracked
        }
        //endregion
    }
}

