import { GetDocumentsCommand, DocumentStore } from "ravendb";

//region get_interface_single
new GetDocumentsCommand({
    id: "users/1",                 // string
    includes: [ "kids" ],          // string[]
    metadataOnly: false,           // boolean
    conventions                    // DocumentConventions object
});
//endregion

//region get_interface_multiple
new GetDocumentsCommand({
    id: [ "users/1", "users/2" ],      // string[]
    includes: [ "field1", "field2" ],  // string[]
    metadataOnly: false,               // boolean
    conventions                        // DocumentConventions object
});
//endregion

//region get_interface_paged
new GetDocumentsCommand({
    start: 0,                // number
    pageSize: 100,           // number
    conventions              // DocumentConventions object
});
//endregion

//region get_interface_startswith
new GetDocumentsCommand({
    start: 0,                                   // number
    pageSize: 100,                              // number
    startsWith: "orchestra/",                   // string
    startsAfter: "orchestra/ny-symphony/flute", // string
    matches: "*/violin",                        // string
    exclude: "orchestra/paris/*",               // string
    metadataOnly: false,                        // boolean
    conventions                                 // DocumentConventions object
});
//endregion


function single() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_single
    const command = new GetDocumentsCommand({ id: "orders/1-A" });
    await session.advanced.getRequestExecutor().execute(command);
    const order = command.result.results[0];
    //endregion
}

function multiple() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_multiple
    const command = new GetDocumentsCommand({
        ids: [ "orders/1-A", "employees/3-A" ]
    });
    await session.advanced.getRequestExecutor().execute(command);
    const order = command.result.results[0];
    const employee = command.result.results[1];
    //endregion
}

function includes() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_includes
    // Fetch emploees/5-A and his boss.
    const command = new GetDocumentsCommand({
        id: "employees/5-A", 
        includes: [ "ReportsTo" ]
    });
    await session.advanced.getRequestExecutor().execute(command);

    const employee = command.result.results[0];
    const bossId = employee.ReportsTo;
    const boss = command.result.includes[bossId];
    //endregion
}

function missing() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_missing
    // Assuming that products/9999-A doesn't exist.
    const command = new GetDocumentsCommand({
        ids: [ "products/1-A", "products/9999-A", "products/3-A" ]
    });
    await session.advanced.getRequestExecutor().execute(command);
    const products = command.result.results; // products/1-A, null, products/3-A
    //endregion
}

function paged() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_paged
    const command = new GetDocumentsCommand({
        start: 0, 
        pageSize: 128
    });
    await session.advanced.getRequestExecutor().execute(command);
    const firstDocs = command.result.results;
    //endregion
}

function startsWith() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_startswith
    const command = new GetDocumentsCommand({
        startsWith: "products",  
        start: 0, 
        pageSize: 128
    });

    await session.advanced.getRequestExecutor().execute(command);
    const products = command.result.results;
    //endregion
}

function startsWithMatches() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_startswith_matches
    // return up to 128 documents  with key that starts with 'products/'
    // and rest of the key begins with "1" or "2", eg. products/10, products/25
    const command = new GetDocumentsCommand({
        startsWith: "products", 
        matches: "1*|2*", 
        start: 0, 
        pageSize: 128
    });
    //endregion
}

function startsWithMatchesEnd() {
    const store = new DocumentStore();
    const session = store.openSession();
    //region get_sample_startswith_matches_end
    // return up to 128 documents with key that starts with 'products/'
    // and rest of the key have length of 3, begins and ends with "1"
    // and contains any character at 2nd position e.g. products/101, products/1B1
    const command = new GetDocumentsCommand({
        startsWith: "products",
        matches: "1?1",
        start: 0,
        pageSize: 128
    });
    await session.advanced.getRequestExecutor().execute(command);
    const products = command.result.results;
    //endregion
}
