import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    //region what_changed_1
    session.advanced.hasChanges();
    //endregion

    //region what_changed_3
    session.advanced.whatChanged();
    //endregion
}

class Employee {}

async function example() {
    //region what_changed_2
    {
        const session = store.openSession();

        assert.ok(!session.advanced.hasChanges());

        const employee = new Employee();
        employee.firstName = "John";
        employee.lastName = "Doe";

        await session.store(employee);

        assert.ok(session.advanced.hasChanges());
    }
    //endregion

    //region what_changed_4
    {
        const session = store.openSession();

        const employee = new Employee();
        employee.firstName = "John";
        employee.lastName = "Doe";

        await session.store(employee);

        const changes = session.advanced.whatChanged();
        const employeeChanges = changes["employees/1-A"];
        const change =
            employeeChanges[0].change; // "DocumentAdded"

    }
    //endregion

    {
        //region what_changed_5
        const session = store.openSession();
        const employee = await session.load("employees/1-A"); // 'Joe Doe'
        employee.firstName = "John";
        employee.lastName = "Shmoe";

        const changes = session.advanced.whatChanged();
        const employeeChanges = changes["employees/1-A"];
        const change1 = employeeChanges[0]; // "FieldChanged"
        const change2 = employeeChanges[1]; // "FieldChanged"
        //endregion
    }
}
