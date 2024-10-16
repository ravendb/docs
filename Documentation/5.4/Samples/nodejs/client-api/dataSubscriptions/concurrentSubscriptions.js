import * as assert from "assert";
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

class InvalidOperationException extends Error {
    constructor(message) {
        super(message);
        this.name = "InvalidOperationException";
    }
}

async function concurrentSubscriptions() {
    
    {   
        //region concurrent_1
        // Define 2 concurrent subscription workers
        // ========================================
        
        const options = {
            // Set concurrent strategy
            strategy: "Concurrent", 
            subscriptionName: "Get all orders",
            maxDocsPerBatch: 20
        };

        const worker1 = documentStore.subscriptions.getSubscriptionWorker(options);
        const worker2 = documentStore.subscriptions.getSubscriptionWorker(options);
        //endregion
    }
    
    {
        //region concurrent_2
        worker1.on("batch", (batch, callback) => {
            try {
                for (const item of batch.items) {
                    // Process item
                }
                callback();

            } catch(err) {
                callback(err);
            }
        });

        worker2.on("batch", (batch, callback) => {
            try {
                for (const item of batch.items) {
                    // Process item
                }
                callback();

            } catch(err) {
                callback(err);
            }
        });
        //endregion
    }
    
    {        
        //region concurrent_3
        // Drop connection for worker2
        await documentStore.subscriptions.dropSubscriptionWorker(worker2);
        //endregion
    }
}

{
//region drop_syntax
// Available overloads:
dropConnection(options);
dropConnection(options, database);
dropSubscriptionWorker(worker);
dropSubscriptionWorker(worker, database);
//endregion
}
