import * as assert from "assert";
import { 
    DocumentStore,
    PutIndexesOperation,
    AbstractIndexCreationTask,
    IndexDefinition
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region storing_1
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
    constructor() {
        super();
        
        this.map = `docs.Employees.Select(employee => new {    
            FirstName = employee.FirstName,    
            LastName = employee.LastName
        })`;

        this.store("FirstName", "Yes");
        this.store("LastName", "Yes");
    }
}
//endregion

async function storing() {
    const session = store.openSession();
    
    //region storing_2
    const indexDefinition = new IndexDefinition();
    indexDefinition.name = "Employees_ByFirstAndLastName";
    indexDefinition.maps = new Set([
        "docs.Employees.Select(employee => new {" +
        "    FirstName = employee.FirstName," +
        "    LastName = employee.LastName" +
        "})"
    ]);
    indexDefinition.fields = {
        "FirstName": { storage: "Yes" },
        "LastName": { storage: "Yes" }
    };

    await store.maintenance.send(new PutIndexesOperation(indexDefinition));
    //endregion
}

