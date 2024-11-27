import { GetDocumentsCommand, DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function single() {
    //region get_1_storeContext
    // Define the 'GetDocumentsCommand'
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        id: "orders/1-A"
    });

    // Call 'execute' on the Store's Request Executor to send the command to the server
    await documentStore.getRequestExecutor().execute(command);

    // Access the results
    const order = command.result.results[0];
    const orderedAt = order.OrderedAt;
    //endregion

    //region get_1_sessionContext
    const session = documentStore.openSession();

    // Define the 'GetDocumentsCommand'
    const command = new GetDocumentsCommand({ 
          conventions: documentStore.conventions, 
          id: "orders/1-A"
    });

    // Call 'execute' on the Session's Request Executor to send the command to the server
    await session.advanced.requestExecutor.execute(command);

    // Access the results
    const order = command.result.results[0];
    const orderedAt = order.OrderedAt;
    //endregion
}

async function multiple() {
    //region get_2
    // Pass a list of document IDs to the get command
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        ids: ["orders/1-A", "employees/2-A", "products/1-A"]
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access results
    const order = command.result.results[0];
    const employee = command.result.results[1];
    const product = command.result.results[2];
    //endregion

    //region get_3
    // Assuming that employees/9999-A doesn't exist
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        ids: [ "orders/1-A", "employees/9999-A", "products/3-A" ]
    });

    await documentStore.getRequestExecutor().execute(command);

    // Results will contain 'null' for any missing document
    const results = command.result.results; // orders/1-A, null, products/3-A
    assert.equal(results[1], null);
    //endregion
}

async function metadataOnly() {
    //region get_4
    // Pass 'true' in the 'metadataOnly' param to retrieve only the document METADATA
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        id: "orders/1-A",
        metadataOnly: true
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access results
    const results = command.result.results[0];
    const metadata = results["@metadata"];

    // Print out all metadata properties
    for (const propertyName in metadata) {
        console.log(`${propertyName} = ${metadata[propertyName]}`);
    }
    //endregion
}

async function paged() {
    //region get_5
    // Specify the number of documents to skip (start)
    // and the number of documents to get (pageSize)
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        start: 0,
        pageSize: 128
    });

    await documentStore.getRequestExecutor().execute(command);

    // The documents are sorted by the last modified date,
    // with the most recent modifications appearing first.
    const firstDocs = command.result.results;
    //endregion
}

async function startsWith() {
    //region get_6
    // Return up to 50 documents with ID that starts with 'products/'
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        startsWith: "products/",  
        start: 0, 
        pageSize: 50
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access a Product document
    const product = command.result.results[0];
    //endregion
}

async function startsWithMatches() {
    //region get_7
    // Return up to 50 documents with IDs that start with 'orders/'
    // and the rest of the ID either begins with '23',
    // or contains any character at the 1st position and ends with '10-A'
    // e.g. orders/234-A, orders/810-A
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        startsWith: "orders/", 
        matches: "23*|?10-A", 
        start: 0, 
        pageSize: 50
    });

    await documentStore.getRequestExecutor().execute(command);
    
    // Access an Order document
    const order = command.result.results[0];
    
    const orderId = order["@metadata"]["@id"];
    assert.ok(orderId.startsWith("orders/23") || /^orders\/.{1}10-A$/.test(orderId));
    //endregion
}

async function startsWithExclude() {
    //region get_8
    // Return up to 50 documents with IDs that start with 'orders/'
    // and the rest of the ID excludes documents ending with '10-A',
    // e.g. will return orders/820-A, but not orders/810-A
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        startsWith: "orders/",
        exclude: "*10-A",
        start: 0,
        pageSize: 50
    });
    
    await documentStore.getRequestExecutor().execute(command);

    // Access an Order document
    const order = command.result.results[0];
    
    const orderId = order["@metadata"]["@id"];
    assert.ok(orderId.startsWith("orders/") && !orderId.endsWith("10-A"));
    //endregion
}

async function includes() {
    //region get_9
    // Fetch document products/77-A and include its related Supplier document
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        id: "products/77-A",
        includes: [ "Supplier" ]
    });
    
    await documentStore.getRequestExecutor().execute(command);

    // Access the related document that was included
    const product = command.result.results[0];
    const supplierId = product["Supplier"];
    const supplier = command.result.includes[supplierId];
    //endregion

    //region get_10
    // Fetch document products/77-A and include the specified counters
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        id: "products/77-A",
        // Pass the names of the counters to include. In this example,
        // the counter names in RavenDB's sample data are stars... 
        counterIncludes: ["⭐", "⭐⭐"]
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access the included counters results
    const counters = command.result.counterIncludes["products/77-A"]
    const counter = counters[0];

    const counterName = counter["counterName"];
    const counterValue = counter["totalValue"];
    //endregion

    //region get_11
    // Fetch document employees/1-A and include the specified time series
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        ids: ["employees/1-A"],
        // Specify the time series name and the time range
        timeSeriesIncludes: [
            {
                name: "HeartRates",
                from: new Date("2020-04-01T00:00:00.000Z"),
                to: new Date("2024-12-31T00:00:00.000Z")
            }
        ]
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access the included time series results
    const timeseries = command.result.timeSeriesIncludes["employees/1-A"];
    const tsEntries = timeseries["HeartRates"][0].entries;

    const entryTimeStamp = tsEntries[0].timestamp;
    const entryValues = tsEntries[0].values;
    //endregion

    //region get_12
    // Fetch document orders/826-A and include the specified revisions
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        ids: ["orders/826-A"],
        // Specify list of document fields (part of document orders/826-A),
        // where each field is expected to contain the change-vector
        // of the revision you wish to include.
        revisionsIncludesByChangeVector: [
            "RevisionChangeVectorField1",
            "RevisionChangeVectorField2"
        ]
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access the included revisions
    const revisionObj = command.result.revisionIncludes[0];
    const revision = revisionObj.Revision;    
    //endregion

    //region get_13
    // Fetch document orders/826-A and include the specified revisions
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        ids: ["orders/826-A"],
        // Another option is to specify a single document field (part of document orders/826-A).
        // This field is expected to contain a list of all the change-vectors
        // for the revisions you wish to include.
        revisionsIncludesByChangeVector: [
            "RevisionsChangeVectors"
        ]
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access the included revisions
    const revisionObj = command.result.revisionIncludes[0];
    const revision = revisionObj.Revision;
    //endregion

    //region get_14
    // Fetch document orders/826-A and include the specified compare-exchange
    const command = new GetDocumentsCommand({
        conventions: documentStore.conventions,
        ids: ["orders/826-A"],
        // Similar to the previous "include revisions" examples,
        // EITHER:
        // Specify a list of document fields (part of document orders/826-A),
        // where each field is expected to contain a compare-exchange KEY
        // for the compare-exchange item you wish to include
        // OR:
        // Specify a single document field that contains a list of all keys to include.
        compareExchangeValueIncludes: [
            "CmpXchgItemField1",
            "CmpXchgItemField2"
        ]
    });

    await documentStore.getRequestExecutor().execute(command);

    // Access the included compare-exchange items
    const cmpXchgItems = command.result.compareExchangeValueIncludes;
    
    const cmpXchgItemKey = Object.keys(cmpXchgItems)[0];
    const cmpXchgItemValue = cmpXchgItem[cmpXchgItemKey].value.Object;
    //endregion
}

//region syntax_1
// Available overloads:
// ====================

new GetDocumentsCommand({
    conventions, id, 
    includes?, counterIncludes?, includeAllCounters?, metadataOnly? 
});

new GetDocumentsCommand({
    conventions, ids,
    includes?, timeSeriesIncludes?, compareExchangeValueIncludes?,
    revisionsIncludesByChangeVector?, revisionIncludeByDateTimeBefore?, 
    counterIncludes?, includeAllCounters?, metadataOnly?
});

new GetDocumentsCommand({
    conventions, start, pageSize, 
    startsWith?, startsAfter?, matches?, exclude?,
    counterIncludes?, includeAllCounters?, metadataOnly?
});
//endregion

//region syntax_2
// The `GetDocumentCommand` result object:
// =======================================

{
    includes;                     // object
    results;                      // any[]
    counterIncludes;              // object
    revisionIncludes;             // any[]
    timeSeriesIncludes;           // object
    compareExchangeValueIncludes; // object
}

//endregion
