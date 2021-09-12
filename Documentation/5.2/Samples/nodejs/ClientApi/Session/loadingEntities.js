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
session.load(id, [documentType], [callback]);
//endregion

//region loading_entities_2_0
session.include(path);
//endregion

//region loading_entities_3_0
session.load(idsArray, [documentType], [callback]); 
session.load(idsArray, [options], [callback]);
//endregion

//region loading_entities_4_0
session.advanced.loadStartingWith(idPrefix, [options], [callback]);

session.advanced.loadStartingWithIntoStream(idPrefix, output, [options], [callback]);
//endregion

//region loading_entities_5_0
// stream query results
session.stream(query, [statsCallback], [callback]);          

// stream documents with ID starting with
session.stream(idPrefix, [options], [callback]);          
//endregion

//region loading_entities_6_0
session.advanced.isLoaded(id);
//endregion

class Employee {

}
class Supplier {

}
class Product {
    constructor({ supplier }) {
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

    //region loading_entities_7_0
    (object, changeVector)ConditionalLoad<object>(id, changeVector);
    //endregion

    //region loading_entities_7_1
    var session = store.openSession();
    session.store(user, "users/1");
    session.SaveChanges();

    const changeVector = session.Advanced.GetChangeVectorFor(user);

    // New session which does not track our User entity
    // The given change vector matches 
    // the server-side change vector
    // Does not load the document
    var session = store.openSession();
    var result1 = session.Advanced
        .ConditionalLoad<User>("users/1", changeVector);

    // Modify the document
    user.name = "Bob Smith";
    session.store(user);
    session.saveChanges();

    // Change vectors do not match
    // Loads the document
    var result2 = session.Advanced
        .ConditionalLoad < User > ("users/1", changeVector);
    //endregion

