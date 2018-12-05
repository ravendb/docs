import { DocumentStore } from "ravendb";

let callback;

const store = new DocumentStore();
const session = store.openSession();

//region saving_changes_1
session.saveChanges([callback]);
//endregion

class Employee {
    constructor(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
    }
}

async function examples() {
    {
        //region saving_changes_2
        const employee = new Employee("John", "Doe");
        await session.store(employee);
        await session.saveChanges();
        //endregion
    }

    {
        //region saving_changes_3
        session.advanced.waitForIndexesAfterSaveChanges({
            indexes: ["index/1", "index/2"],
            throwOnTimeout: true,
            timeout: 30 * 1000 // 30 seconds in ms
        });

        const employee = new Employee("John", "Doe");
        await session.store(employee);
        await session.saveChanges();
        //endregion
    }

    {
        //region saving_changes_4
        session.advanced
            .waitForReplicationAfterSaveChanges({
                throwOnTimeout: false, // default true
                timeout: 30000,
                replicas: 2, // minimum replicas to replicate
                majority: false
            });

        const employee = new Employee("John", "Doe");
        await session.store(employee);
        await session.saveChanges();
        //endregion
    }
}
