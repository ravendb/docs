import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function countersBatch() {
    {
        //region example_1
        // Define the counter actions you want to make per document:
        // =========================================================
        
        const counterActions1 = new DocumentCountersOperation();
        counterActions1.documentId = "users/1";
        counterActions1.operations = [
            CounterOperation.create("Likes", "Increment", 5), // Increment "Likes" by 5
            CounterOperation.create("Dislikes", "Increment")  // No delta specified, value will stay the same
        ];

        const counterActions2 = new DocumentCountersOperation();
        counterActions2.documentId = "users/2";
        counterActions2.operations = [
            CounterOperation.create("Likes", "Increment", 100), // Increment "Likes" by 100
            CounterOperation.create("Score", "Increment", 50)   // Create a new counter "Score" with value 50
        ];
        
        // Define the batch:
        // =================
        const batch = new CounterBatch();
        batch.documents = [counterActions1, counterActions2];
        
        // Define the counter batch operation, pass the batch:
        // ===================================================
        const counterBatchOp = new CounterBatchOperation(batch);

        // Execute the operation by passing it to operations.send:
        // =======================================================
        const result = await documentStore.operations.send(counterBatchOp);
        const counters = result.counters;
        //endregion
    }
    {
        //region example_2
        // Define the counter actions you want to make per document:
        // =========================================================

        const counterActions1 = new DocumentCountersOperation();
        counterActions1.documentId = "users/1";
        counterActions1.operations = [
            CounterOperation.create("Likes", "Get"),
            CounterOperation.create("Downloads", "Get")
        ];

        const counterActions2 = new DocumentCountersOperation();
        counterActions2.documentId = "users/2";
        counterActions2.operations = [
            CounterOperation.create("Likes", "Get"),
            CounterOperation.create("Score", "Get")
        ];

        // Define the batch:
        // =================
        const batch = new CounterBatch();
        batch.documents = [counterActions1, counterActions2];

        // Define the counter batch operation, pass the batch:
        // ===================================================
        const counterBatchOp = new CounterBatchOperation(batch);

        // Execute the operation by passing it to operations.send:
        // =======================================================
        const result = await documentStore.operations.send(counterBatchOp);
        const counters = result.counters;
        //endregion
    }
    {
        //region example_3
        // Define the counter actions you want to make per document:
        // =========================================================

        const counterActions1 = new DocumentCountersOperation();
        counterActions1.documentId = "users/1";
        counterActions1.operations = [
            // "Likes" and "Dislikes" will be removed from counter-names in "users/1" metadata
            CounterOperation.create("Likes", "Delete"),
            CounterOperation.create("Dislikes", "Delete")
        ];

        const counterActions2 = new DocumentCountersOperation();
        counterActions2.documentId = "users/2";
        counterActions2.operations = [
            // "Downloads" will be removed from counter-names in "users/2" metadata
            CounterOperation.create("Downloads", "Delete")
        ];

        // Define the batch:
        // =================
        const batch = new CounterBatch();
        batch.documents = [counterActions1, counterActions2];

        // Define the counter batch operation, pass the batch:
        // ===================================================
        const counterBatchOp = new CounterBatchOperation(batch);

        // Execute the operation by passing it to operations.send:
        // =======================================================
        const result = await documentStore.operations.send(counterBatchOp);
        const counters = result.counters;
        //endregion
    }
    {
        //region example_4
        // Define the counter actions you want to make per document:
        // =========================================================

        const counterActions1 = new DocumentCountersOperation();
        counterActions1.documentId = "users/1";
        counterActions1.operations = [
            CounterOperation.create("Likes", "Increment", 30),
            // The results will include null for this 'Get' 
            // since we deleted the "Dislikes" counter in the previous example flow
            CounterOperation.create("Dislikes", "Get"), 
            CounterOperation.create("Downloads", "Delete")
        ];

        const counterActions2 = new DocumentCountersOperation();
        counterActions2.documentId = "users/2";
        counterActions2.operations = [
            CounterOperation.create("Likes", "Get"),
            CounterOperation.create("Dislikes", "Delete")
        ];

        // Define the batch:
        // =================
        const batch = new CounterBatch();
        batch.documents = [counterActions1, counterActions2];

        // Define the counter batch operation, pass the batch:
        // ===================================================
        const counterBatchOp = new CounterBatchOperation(batch);

        // Execute the operation by passing it to operations.send:
        // =======================================================
        const result = await documentStore.operations.send(counterBatchOp);
        const counters = result.counters;
        //endregion
    }
}

//region syntax

//region syntax_1 
const counterBatchOp = new CounterBatchOperation(counterBatch);
//endregion

//region syntax_2 
// The CounterBatch object:
// ========================
{
    // A list of "DocumentCountersOperation" objects
    documents;
    // A flag indicating if results should include a dictionary of counter values per database node
    replyWithAllNodesValues;
}
//endregion

//region syntax_3 
// The DocumentCountersOperation object:
// =====================================
{
    // Id of the document that holds the counters
    documentId;
    // A list of "CounterOperation" objects to perform
    operations;
}
//endregion

//region syntax_4 
// The CounterOperation object:
// ============================
{
    // The operation type: "Increment" | "Delete" | "Get" 
    type;
    // The counter name
    counterName;
    // The value to increment by
    delta;
}
//endregion

//region syntax_5
// The CounterDetails object:
// ==========================
{
    // A list of "CounterDetail" objects;
    counters;
}
//endregion

//region syntax_6
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
