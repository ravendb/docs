import * as assert from "assert";
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function revisionsSubscription() {

    {
        //region revisions_1
        const subscriptionName = await documentStore.subscriptions.create({
            // Add (Revisions = true) to your subscription RQL
            query: "From Orders (Revisions = true)"
        });
        //endregion
    }
    
    {
        //region revisions_2
        const workerOptions = { subscriptionName };

        const worker = 
            // Use method `getSubscriptionWorkerForRevisions`
            documentStore.subscriptions.getSubscriptionWorkerForRevisions(workerOptions);

        worker.on("batch", (batch, callback) => {
            try {
                for (const item of batch.items) {

                    // Access the previous revision via 'result.previous'
                    const previousRevision = item.result.previous;

                    // Access the current revision via 'result.current'
                    const currentRevision = item.result.current;
                }
                callback();
                
            } catch (err) {
                callback(err);
            }
        });
        //endregion
    }
    
    {
        //region revisions_3
        const subscriptionName = await documentStore.subscriptions.create({
            // Provide filtering logic
            // Only revisions that where shipped to Mexico will be sent to subscribed clients
            query: `declare function isSentToMexico(doc) { 
                        return doc.Current.ShipTo.Country == 'Mexico'
                    }

                    from 'Orders' (Revisions = true) as doc
                    where isSentToMexico(doc) == true`
        });
        //endregion
    }
    
    {
        //region revisions_4
        const workerOptions = { subscriptionName };

        const worker =
            documentStore.subscriptions.getSubscriptionWorkerForRevisions(workerOptions);

        worker.on("batch", (batch, callback) => {
            try {
                for (const item of batch.items) {
                    console.log(`
                        This is a revision of document ${item.id}.
                        The order in this revision was shipped at ${item.result.current.ShippedAt}.
                    `);
                }
                callback();

            } catch (err) {
                callback(err);
            }
        });
        //endregion
    }
    
    {
        //region revisions_5
        const subscriptionName = await documentStore.subscriptions.create({
            // Filter revisions by the revenue delta.
            // The subscription will only process revisions where the revenue
            // is higher than in the preceding revision by 2500.
            
            query: `declare function isRevenueDeltaAboveThreshold(doc, threshold) { 
                        return doc.Previous !== null && doc.Current.Lines.map(function(x) {
                            return x.PricePerUnit * x.Quantity;
                        }).reduce((a, b) => a + b, 0) > doc.Previous.Lines.map(function(x) { 
                            return x.PricePerUnit * x.Quantity;
                        }).reduce((a, b) => a + b, 0) + threshold
                    }

                    from 'Orders' (Revisions = true) as doc
                    where isRevenueDeltaAboveThreshold(doc, 2500)

                    // Define the projected fields that will be sent to the client:
                    select {
                        previousRevenue: doc.Previous.Lines.map(function(x) {
                            return x.PricePerUnit * x.Quantity;
                        }).reduce((a, b) => a + b, 0),
      
                        currentRevenue: doc.Current.Lines.map(function(x) {
                            return x.PricePerUnit * x.Quantity;
                        }).reduce((a, b) => a + b, 0)
                    }`
        });
        //endregion
    }
    
    {        
        //region revisions_6
        const workerOptions = { 
            subscriptionName: subscriptionName,
            documentType: OrderRevenues
        };

        const worker =
            // Note: in this case, where each resulting item in the batch is a projected object
            // and not the revision itself, we use method `getSubscriptionWorker`
            documentStore.subscriptions.getSubscriptionWorker(workerOptions);

        worker.on("batch", (batch, callback) => {
            try {
                for (const item of batch.items) {
                    // Access the projected content:
                    console.log(`
                        Revenue for order with ID: ${item.id}
                        has grown from ${item.result.previousRevenue}
                        to ${item.result.currentRevenue}
                    `);
                }
                callback();

            } catch (err) {
                callback(err);
            }
        });
        //endregion
    }
}

//region projection_class
class OrderRevenues {
    constructor() {
        this.previousRevenue;
        this.currentRevenue;
    }
}
//endregion
