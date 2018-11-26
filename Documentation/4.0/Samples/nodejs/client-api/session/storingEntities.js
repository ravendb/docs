import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region store_entities_1
session.store(entity); 
//endregion

//region store_entities_2
session.store(entity, id);
//endregion

//region store_entities_3
session.store(entity, id, options);
//endregion

class Employee {
    constructor(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
    }
}

const session = store.openSession();
//region store_entities_5
const employee = new Employee("John", "Doe");

// generate Id automatically
await session.store(employee);

// send all pending operations to server, in this case only `Put` operation
await session.saveChanges();
//endregion

//region storing_literals_1
const store = new DocumentStore(urls, database);
store.conventions.findCollectionNameForObjectLiteral = entity => entity["collection"];
// ...
store.initialize();
//endregion
