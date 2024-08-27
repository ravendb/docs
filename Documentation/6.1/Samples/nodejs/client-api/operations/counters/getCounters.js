import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getCounters() {
    {
        //region example_1
        // Define the get counters operation
        const getCountersOp = new GetCountersOperation("users/1", "Likes");

        // Execute the operation by passing it to operations.send
        const result = await documentStore.operations.send(getCountersOp);
        const counters = result.counters;
        //endregion
    }
    {
        //region example_2
        const getCountersOp = new GetCountersOperation("users/1", ["Likes", "Dislikes"]);

        const result = await documentStore.operations.send(getCountersOp);
        const counters = result.counters;
        //endregion
    }
    {
        //region example_3
        const getCountersOp = new GetCountersOperation("users/1");

        const result = await documentStore.operations.send(getCountersOp);
        const counters = result.counters;
        //endregion
    }
    {
        //region example_4
        const getCountersOp = new GetCountersOperation("users/1", "Likes", true);

        const result = await documentStore.operations.send(getCountersOp);
        const counters = result.counters;
        //endregion
    }
}

//region syntax

//region syntax_1 
// Get single counter
const getCountersOp = new GetCountersOperation(docId, counter);
const getCountersOp = new GetCountersOperation(docId, counter, returnFullResults = false);
//endregion

//region syntax_2 
// Get multiple counters
const getCountersOp = new GetCountersOperation(docId, counters);
const getCountersOp = new GetCountersOperation(docId, counters, returnFullResults = false);
//endregion

//region syntax_3 
// Get all counters of a document
const getCountersOp = new GetCountersOperation(docId);
//endregion

//region syntax_4
// The CounterDetails object:
// ==========================
{
    // A list of "CounterDetail" objects;
    counters;
}
//endregion

//region syntax_5
// The CounterDetail object:
// =========================
{
    // ID of the document that holds the counter;
    documentId; // string
    
    // The counter name
    counterName; //string

    // Total counter value
    totalValue; // number
    
    // A dictionary of counter values per database node
    counterValues?; 
    
    // Etag of counter
    etag?; // number;

    // Change vector of counter
    changeVector?; // string
}
//endregion

//endregion
