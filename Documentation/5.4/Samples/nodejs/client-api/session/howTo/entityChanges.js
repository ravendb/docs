import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function example() {
    {
        //region changes_1
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
}

{
    let entity;
    //region syntax_1
    session.advanced.hasChanged(entity);
    //endregion
}
