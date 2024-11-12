import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getTrackedEntities() {
    {
        //region get_tracked_entities_1
        const session = documentStore.openSession();
        
        // Store entities within the session:
        let employee1 = new Employee();
        employee.firstName = "John";
        employee.lastName = "Doe";
        await session.store(employee, "employees/1-A");

        let employee2 = new Employee();
        employee.firstName = "David";
        employee.lastName = "Brown";
        await session.store(employee2, "employees/2-A");

        let employee3 = new Employee();
        employee.firstName = "Tom";
        employee.lastName = "Miller";
        await session.store(employee3, "employees/3-A");

        // Get tracked entities:
        let trackedEntities = session.advanced.getTrackedEntities();

        // The session tracks the new stored entities:
        const entityInfo = trackedEntities["employees/1-A"];
        assert.equal(entityInfo.id, "employees/1-A");
        assert.ok(entityInfo.entity instanceof Employee);

        // Save changes:
        await session.saveChanges();

        // The session keeps tracking the entities even after SaveChanges is called:
        trackedEntities = session.advanced.getTrackedEntities();
        //endregion
    }
    {
        //region get_tracked_entities_2
        const session = documentStore.openSession();

        // Load entity:
        const employee1 = await session.load("employees/1-A");

        // Delete entity:
        session.delete("employees/3-A");

        // Get tracked entities:
        const trackedEntities = session.advanced.getTrackedEntities();

        // The session tracks the 2 entities:

        // The loaded entity:
        const entityInfo1 = trackedEntities["employees/1-A"];
        assert.ok(!entityInfo1.isDeleted);

        // The deleted entity:
        const entityInfo2 = trackedEntities.get("employees/3-A");
        assert.ok(entityInfo2.isDeleted);

        // Save changes:
        await session.saveChanges();
        //endregion
    }
    {
        //region get_tracked_entities_3
        const session = documentStore.openSession();
        
        // Query for all employees:
        const employees = await session.query({ collection: "employees"}).all();

        // Get tracked entities:
        const trackedEntities = session.advanced.getTrackedEntities();

        // The session tracks the entities loaded via the query:
        const entityInfo1 = trackedEntities[employees[0].id];
        const entityInfo2 = trackedEntities[employees[1].id];
        //endregion
    }
}

{    
    //region syntax_1
    getTrackedEntities();
    //endregion
    
    //region syntax_2
    class EntityInfo {
        // The tracked entity id
        id;       // string
        
        // The tracked entity object
        entity;   // object
        
        // isDeleted is true if entity was deleted
        isDeleted // boolean
    }
    //endregion

}
