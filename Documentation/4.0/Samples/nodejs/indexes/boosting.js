import { 
    DocumentStore, AbstractIndexCreationTask, IndexDefinition, PutIndexesOperation
} from "ravendb";

const store = new DocumentStore();

//region boosting_2
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
    constructor() {
        super();
        this.map = "docs.Employees.Select(employee => new {" +
            "    firstName = employee.FirstName.Boost(10)," +
            "    lastName = employee.LastName" +
            "})";
    }
}
//endregion

class Employee { }

async function boosting() {
    
    {
        const session = store.openSession();

        //region boosting_3
        // employees with 'FirstName' equal to 'Bob'
        // will be higher in results
        // than the ones with 'LastName' match
        const results = await session.query({ indexName: "Employees/ByFirstAndLastName" })
            .whereEquals("FirstName", "Bob")
            .whereEquals("LastName", "Bob")
            .all();
        //endregion
    }

    //region boosting_4
    const indexDefinition = new IndexDefinition();
    indexDefinition.name = "Employees/ByFirstAndLastName";
    indexDefinition.maps = new Set([ 
        "docs.Employees.Select(employee => new {" +
        "    FirstName = employee.FirstName.Boost(10)," +
        "    LastName = employee.LastName" +
        "})" 
    ]);

    await store.maintenance.send(new PutIndexesOperation(indexDefinition));
    //endregion
}
