import * as assert from "assert";
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = store.openSession();

async function consumptionApi() {

    {
        //region consume_syntax_1
        await documentStore.subscriptions.getSubscriptionWorker(subscriptionName);
        await documentStore.subscriptions.getSubscriptionWorker(subscriptionName, database);
        
        await documentStore.subscriptions.getSubscriptionWorker(options);
        await documentStore.subscriptions.getSubscriptionWorker(options, database);
        //endregion
    }
    {
        //region consume_syntax_2
        // The SubscriptionWorkerOptions object:
        // =====================================
        {
            subscriptionName;
            documentType;
            ignoreSubscriberErrors;
            closeWhenNoDocsLeft;
            maxDocsPerBatch;
            timeToWaitBeforeConnectionRetry;
            maxErroneousPeriod;            
            strategy;
        }
        //endregion
    }
    {
        let subscriptionWorker;
        //region consume_syntax_3
        subscriptionWorker.on("batch", (batch, callback) => {
            // Process incoming items:
            // =======================
            
            // 'batch': 
            // Contains the documents to be processed.
            
            // callback(): 
            // Needs to be called after processing the batch 
            // to notify the worker that you're done processing.
        });
        //endregion
    }
    {
        //region consume_syntax_4
        class Item
        {
            result;
            exceptionMessage;
            id;
            changeVector;
            projection;
            revision;
            rawResult;
            rawMetadata;
            metadata;
        }
        //endregion
    }

    {
        // for later
        subscriptionWorker.on("error", (error) => {});
        subscriptionWorker.on("end", () => {});
    }
    
}
