import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let entity;
    //region has_changed_1
    session.advanced.hasChanged(entity);
    //endregion
}

async function example() {
    {
        //region has_changed_2
        const employee = await session.load("employees/1-A");
        let hasChanged = session.advanced.hasChanged(employee); // false
        employee.lastName = "Shmoe";
        hasChanged = session.advanced.hasChanged(employee);       // true
        //endregion
    }
}
