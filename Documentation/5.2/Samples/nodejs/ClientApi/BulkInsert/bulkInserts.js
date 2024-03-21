import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let database;
    //region bulk_inserts_1
    documentStore.bulkInsert([database]);
    //endregion
}

class Employee {
    constructor(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
    }
}

async function examples() {

    {
        //region bulk_inserts_4
        {
            const bulkInsert = documentStore.bulkInsert();
            for (let i = 0; i < 1000000; i++) {
                const employee = new Employee("FirstName #" + i, "LastName #" + i);
                await bulkInsert.store(employee);
            }

            await bulkInsert.finish();
        }
        //endregion
    }
}
