import * as fs from "fs";
import { DocumentStore } from "ravendb";

let query, 
    id, 
    callback, 
    statsCallback, 
    documentType, 
    idsArray, 
    includes, 
    path,
    idPrefix,           
    matches,
    start,
    pageSize,
    exclude,
    startAfter,
    options;

const store = new DocumentStore();
const session = store.openSession();

//region loading_entities_1_0
await session.load(id, [documentType]);
//endregion

//region loading_entities_2_0
await session.include(path);
//endregion

//region loading_entities_3_0
await session.load(idsArray, [documentType]);
await session.load(idsArray, [options]);
//endregion

//region loading_entities_4_0
await session.advanced.loadStartingWith(idPrefix, [options]);

await session.advanced.loadStartingWithIntoStream(idPrefix, output, [options]);
//endregion

//region loading_entities_5_0
// stream query results
await session.stream(query, [statsCallback]);

// stream documents with ID starting with
await session.stream(idPrefix, [options]);
//endregion

//region loading_entities_6_0
await session.advanced.isLoaded(id);
//endregion

class Employee {

}
class Supplier {

}
class Product {
    constructor(supplier) {
        this.supplier = supplier;
    }
}

async function examples() {

    //region loading_entities_1_1
    const employee = await session.load("employees/1");
    //endregion

    //region loading_entities_2_1
    // loading 'products/1'
    // including document found in 'supplier' property
    const product = await session
        .include("supplier")
        .load("products/1");

    const supplier = await session.load(product.supplier); // this will *not* make a server call
    //endregion

    {
        //region loading_entities_2_2
        // loading 'products/1'
        // including document found in 'supplier' property
        const product = await session
            .include("supplier")
            .load("products/1");

        const supplier = await session.load(product.supplier); // this will *not* make a server call
        //endregion
    }

    //region loading_entities_3_1
    const employees = await session.load(
        [ "employees/1", "employees/2", "employees/3" ]);
    // {
    //     "employees/1": { ... },
    //     "employees/2": { ... }
    //     "employees/3": { ... }
    // }
    //endregion

    //region loading_entities_4_1
    // return up to 128 entities with Id that starts with 'employees'
    const result = await session
        .advanced
        .loadStartingWith("employees/", {
            start: 0, 
            pageSize: 128
        });
    //endregion

    {
        //region loading_entities_4_2
        // return up to 128 entities with Id that starts with 'employees/'
        // and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
        const result = await session
            .advanced
            .loadStartingWith("employees/", {
                matches: "1*|2*",
                start: 0, 
                pageSize: 128
            });
        //endregion
    }

    //region loading_entities_5_1
    // stream() returns a Node.js Readable
    const stream = await session.advanced.stream("employees/");

    stream.on("data", data => {
        // Employee { name: 'Anna', id: 'employees/1-A' }
    });

    stream.on("error", err => {
        // handle errors
    });

    stream.on("end", () => {
        // stream ended
    });
    //endregion

    //region loading_entities_5_2
    const employeesFile = fs.createWriteStream("employees.json");
    await session.advanced.loadStartingWithIntoStream("employees/", employeesFile);
    //endregion

    {
        //region loading_entities_6_1
        session.advanced.isLoaded("employees/1"); // false
        const employee = await session.load("employees/1");
        session.advanced.isLoaded("employees/1"); // true
        //endregion
    }
}

{
    let id , object, changeVector;
    class User {}
    //region loading_entities_7_0
    await session.advanced.conditionalLoad(id, changeVector, object);
    //endregion

    //region loading_entities_7_1
    let session = store.openSession();
    await session.store(User, "users/1");
    await session.saveChanges();

    const _changeVector = session.advanced.getChangeVectorFor(User);

    // New session which does not track our User entity
    // The given change vector matches 
    // the server-side change vector
    // Does not load the document
    let session_2 = store.openSession();
    let user = await session.advanced
        .conditionalLoad("users/1", _changeVector,User);

    // Modify the document
    user.name = "Bob Smith";
    await session_2.store(User);
    await session_2.saveChanges();

    // Change vectors do not match
    // Loads the document
    let user2 = await session_2.advanced
        .conditionalLoad("users/1", _changeVector,User);
    //endregion
}

