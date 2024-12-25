import {
    DocumentStore,
    AbstractJavaScriptIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

//region indexes_1
// Define the index:
// =================

class Employees_ByNameAndCountry extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map("Employees", employee => {
            return {
                // Define the content for each index-field:
                // ========================================
                LastName: employee.LastName,
                FullName: employee.FirstName + " " + employee.LastName,
                Country: employee.Address.Country
            };
        });
    }
}
//endregion

class Employee { }

async function whatAreIndexes() {

    //region indexes_2
    // Deploy the index to the server:
    // ===============================

    const employeesIndex = new Employees_ByNameAndCountry();
    await employeesIndex.execute(store);
    //endregion

    {
        const session = store.openSession();
        //region indexes_3
        // Query the database using the index: 
        // ===================================

        const employeesFromUK = await session
            .query({ indexName: employeesIndex.getIndexName()  })
            // Here we query for all Employee documents that are from the UK
            // and have 'King' in their LastName field:
            .whereEquals("LastName", "King")
            .whereEquals("Country", "UK")
            .all();
        //endregion
    }

}

