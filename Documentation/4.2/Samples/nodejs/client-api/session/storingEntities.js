import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

let id, entity, callback, documentType, options, urls, database;

//region store_entities_1
session.store(entity, [documentType], [callback]); 
//endregion

//region store_entities_2
session.store(entity, id, [callback]);
//endregion

//region store_entities_3
session.store(entity, id, [options], [callback]);
session.store(entity, id, [documentType], [callback]);
//endregion

class Employee {
    constructor(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
    }
}

async function store_entities_5() {
    //region store_entities_5
    const employee = new Employee("John", "Doe");

    // generate Id automatically
    await session.store(employee);

    // send all pending operations to server, in this case only `Put` operation
    await session.saveChanges();
    //endregion
}

async function storing_literals_1() {
    //region storing_literals_1
    const store = new DocumentStore(urls, database);
    store.conventions.findCollectionNameForObjectLiteral = entity => entity["collection"];
    // ...
    store.initialize();
    //endregion
}
