import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region clear_1
    session.advanced.clear();
    //endregion
}

async function example() {
    class Employee {}

    {
        //region clear_2
        const employee = new Employee();
        employee.firstName = "John";
        employee.lastName = "Doe";
        await session.store(employee);

        session.advanced.clear();

        await session.saveChanges(); // nothing will hapen
        //endregion
    }
}
