import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

async function example() {    
    {
        //region changes_1
        const session = store.openSession();

        // No changes made yet - 'hasChanges' will be FALSE
        assert.ok(!session.advanced.hasChanges());

        // Store a new entity within the session
        const employee = new Employee();
        employee.firstName = "John";
        employee.lastName = "Doe";

        await session.store(employee);

        // 'hasChanges' will now be TRUE
        assert.ok(session.advanced.hasChanges());

        // 'hasChanges' will reset to FALSE after saving changes 
        await session.saveChanges();
        assert.ok(!session.advanced.hasChanges());
        //endregion
    }
    {
        //region changes_2
        const session = store.openSession();

        // Store (add) new entities, they will be tracked by the session
        const employee1 = new Employee();
        employee1.firstName = "John";
        employee1.lastName = "Doe";

        const employee2 = new Employee();
        employee2.firstName = "Jane";
        employee2.lastName = "Doe";

        await session.store(employee1, "employees/1-A");
        await session.store(employee2, "employees/2-A");

        // Call 'WhatChanged' to get all changes in the session
        const changes = session.advanced.whatChanged();
        assert.equal(Object.keys(changes).length, 2);  // 2 entities were added

        // Get the change details for an entity, specify the entity ID
        const changesForEmployee = changes["employees/1-A"];
        assert.equal(Object.keys(changesForEmployee).length, 1); // a single change for this entity (adding)

        // Get the change type
        const changeType = changesForEmployee[0].change;
        assert.equal(changeType, "DocumentAdded");

        await session.saveChanges();
        //endregion
    }
    {
        //region changes_3
        const session = store.openSession();

        // Load the entities, they will be tracked by the session
        const employee1 = await session.load("employees/1-A");
        const employee2 = await session.load("employees/2-A");

        // Modify entities
        employee1.firstName = "Jim";
        employee1.lastName = "Brown";
        employee2.lastName = "Smith";

        // Delete an entity
        session.delete(employee2);

        // Call 'WhatChanged' to get all changes in the session
        const changes = session.advanced.whatChanged();

        // Get the change details for an entity, specify the entity ID
        let changesForEmployee = changes["employees/1-A"];

        assert.equal(changesForEmployee[0].fieldName, "firstName");  // Field name
        assert.equal(changesForEmployee[0].fieldNewValue, "Jim");    // New value
        assert.equal(changesForEmployee[0].change, "FieldChanged");  // Change type

        assert.equal(changesForEmployee[1].fieldName, "lastName");
        assert.equal(changesForEmployee[1].fieldNewValue, "Brown");
        assert.equal(changesForEmployee[1].change, "FieldChanged");

        // Note: for employee2 - even though the LastName was changed to 'Smith',
        // the only reported change is the latest modification, which is the delete action. 
        changesForEmployee = changes["employees/2-A"];
        assert.equal(changesForEmployee[0].change, "DocumentDeleted");

        await session.saveChanges();
        //endregion
    }
}

{
    //region syntax_1
    // HasChanges
    session.advanced.hasChanges();
    //endregion

    //region syntax_2
    // WhatChanged
    session.advanced.whatChanged();
    //endregion
}
