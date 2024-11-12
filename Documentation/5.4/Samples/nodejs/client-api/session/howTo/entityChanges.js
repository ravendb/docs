import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function example() {
    {
        //region changes_1
        const session = documentStore.openSession();
        
        // Store a new entity within the session
        // =====================================

        let employee = new Employee();
        employee.firstName = "John";
        employee.lastName = "Doe";
        await session.store(employee, "employees/1-A");

        // 'hasChanged' will be TRUE 
        assert.ok(session.advanced.hasChanged(employee));

        // 'hasChanged' will reset to FALSE after saving changes 
        await session.saveChanges();
        assert.ok(!session.advanced.hasChanged(employee));

        // Load & modify entity within the session
        // =======================================

        employee = await session.load("employees/1-A");
        assert.ok(!session.advanced.hasChanged(employee));   // FALSE

        employee.lastName = "Brown";
        assert.ok(session.advanced.hasChanged(employee));    // TRUE

        await session.saveChanges();
        assert.ok(!session.advanced.hasChanged(employee));   // FALSE
        //endregion
    }
    {
        //region changes_2
        const session = documentStore.openSession();
        
        // Store (add) a new entity, it will be tracked by the session
        let employee = new Employee();
        employee.firstName = "John";
        employee.lastName = "Doe";
        await session.store(employee, "employees/1-A");

        // Get the changes for the entity in the session
        // Call 'whatChangedFor', pass the entity object in the param
        const changesForEmployee = session.advanced.whatChangedFor(employee);
        
        // Assert there was a single change for this entity
        assert.equal(changesForEmployee.length, 1);

        // Get the change type
        const changeType = changesForEmployee[0].change;
        assert.equal(changeType, "DocumentAdded");
        
        await session.saveChanges();
        //endregion
    }
    {
        //region changes_3
        const session = documentStore.openSession();
        
        // Load the entity, it will be tracked by the session
        const employee = await session.load("employees/1-A");

        // Modify the entity
        employee.firstName = "Jim";
        employee.lastName = "Brown";

        // Get the changes for the entity in the session
        // Call 'whatChangedFor', pass the entity object in the param
        const changesForEmployee = session.advanced.whatChangedFor(employee);

        assert.equal(changesForEmployee[0].fieldName, "firstName");
        assert.equal(changesForEmployee[0].fieldNewValue, "Jim");
        assert.equal(changesForEmployee[0].change, "FieldChanged");

        assert.equal(changesForEmployee[1].fieldName, "lastName");
        assert.equal(changesForEmployee[1].fieldNewValue, "Brown");
        assert.equal(changesForEmployee[1].change, "FieldChanged");
        //endregion
    }
}

{
    let entity;
    
    //region syntax_1
    session.advanced.hasChanged(entity);
    //endregion
    
    //region syntax_2
    session.advanced.whatChangedFor(entity);
    //endregion

    //region syntax_3
    class DocumentsChanges {
        // Previous field value
        fieldOldValue; // object

        // Current field value
        fieldNewValue; // object        
        
        // Name of field on which the change occurred
        fieldName; // string

        // Path of field on which the change occurred
        fieldPath; // string

        // Path + Name of field on which the change occurred
        fieldFullName; // string

        // Type of change that occurred, can be: 
        //     "DocumentDeleted"
        //     "DocumentAdded"
        //     "FieldChanged"
        //     "NewField"
        //     "RemovedField"
        //     "ArrayValueChanged"
        //     "ArrayValueAdded"
        //     "ArrayValueRemoved"
        change; // string
    }
    //endregion
}
