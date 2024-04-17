import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Employee {}

{
    let entity;
    //region evict_1
    session.advanced.evict(entity);
    //endregion
}

async function examples() {
    {
        //region evict_2
        const employee1 = new Employee();
        employee1.firstName = "John";
        employee1.lastName = "Doe";

        const employee2 = new Employee();
        employee2.firstName = "Joe";
        employee2.lastName = "Shmoe";

        await session.store(employee1);
        await session.store(employee2);

        session.advanced.evict(employee1);

        await session.saveChanges();        // only 'Joe Shmoe' will be saved
        //endregion
    }

    {
        //region evict_3
        let employee = await session.load("employees/1-A"); // loading from serer
        employee = await session.load("employees/1-A");     // no server call
        session.advanced.evict(employee);
        employee = await session.load("employees/1-A");     // loading from server
        //endregion
    }
}
