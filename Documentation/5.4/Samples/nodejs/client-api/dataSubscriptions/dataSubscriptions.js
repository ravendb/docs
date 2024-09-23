import * as assert from "assert";
import { DocumentStore } from "ravendb";

let database, options, clazz, subscriptionName;

class InvalidOperationException extends Error {}

const store = new DocumentStore();
const session = store.openSession();

{
    //region subscriptionCreationOverloads
    store.subscriptions.create(clazz, [database]);
    store.subscriptions.create(options, [database]);

    store.subscriptions.createForRevisions(clazz, [database]);
    store.subscriptions.createForRevisions(options, [database]);
    //endregion
}

    async function example() {
        const subscription = await store.subscriptions.create({});
        //region subscriptionWorkerGeneration
        store.subscriptions.getSubscriptionWorker(options, [database]);
        store.subscriptions.getSubscriptionWorker(subscriptionName, [database]);

        store.subscriptions.getSubscriptionWorkerForRevisions(options, [database]);
        store.subscriptions.getSubscriptionWorkerForRevisions(subscriptionName, [database]);
        //endregion
    }

    {
        let subscriptionWorker;
        //region subscriptionWorkerRunning
        subscriptionWorker.on("batch", (batch, callback) => { });
        subscriptionWorker.on("error", (error) => {});
        subscriptionWorker.on("end", () => {});
        //endregion
    }

    //region subscriptions_example
    async function worker() {
    
        // Create the ongoing subscription task on the server
        const subscriptionName = await store.subscriptions.create({ 
            query: "from Orders where Company = 'companies/11'" 
        });
        
        // Create a worker on the client that will consume the subscription
        const worker = store.subscriptions.getSubscriptionWorker(subscriptionName);

        // Listen for and process data received in batches from the subscription
        worker.on("batch", (batch, callback) => {
            for (const item of batch.items) {
                console.log(`Order #${item.result.Id} will be shipped via: ${item.result.ShipVia}`);
            }

            callback();
        });
    }
    //endregion

    async function createExamples() {

        class Order {}

        {
            //region create_whole_collection_generic_with_name
            const subscriptionName = await store.subscriptions.create({
                query: "from Orders",
                // Set a custom name for the subscription 
                name: "OrdersProcessingSubscription"
            });
            //endregion
        }

        {
            //region create_whole_collection_generic_with_mentor_node
            const subscriptionName = await store.subscriptions.create({
                query: "from Orders",
                // Set a responsible node for the subscritpion 
                mentorNode: "D"
            });
            //endregion
        }

        {
            //region create_whole_collection_generic1
            // With the following subscription definition, the server will send ALL documents
            // from the 'Orders' collection to a client that connects to this subscription.
            const subscriptionName = await documentStore.subscriptions.create({
                query: "from Orders"
            });
            //endregion
        }

        {
            //region use_simple_revision_subscription_generic
            const revisionWorker = store.subscriptions
                .getSubscriptionWorkerForRevisions({
                    documentType: Order,
                    name
                });
            revisionWorker.on("batch", (batch, callback) => {
                for (const documentsPair of batch.items) {
                    const prev = documentsPair.result.previous;
                    const current = documentsPair.result.current;
                    processOrderChanges(prev, current);
                    callback();
                }
            });
            //endregion
        }

        {
            //region create_projected_revisions_subscription_RQL
            const query =
                `declare function getOrderLinesSum(doc) {
                    var sum = 0;
                    for (var i in doc.Lines) { sum += doc.Lines[i]; } 
                    return sum;
                }
                
                from orders(Revisions = true)
                where getOrderLinesSum(this.Current) > getOrderLinesSum(this.Previous)
                select {
                    PreviousRevenue: getOrderLinesSum(this.Previous),
                    CurrentRevenue: getOrderLinesSum(this.Current)
                }`;

            const name = await store.subscriptions.create({ query });
            //endregion
        }

        {
            //region consume_revisions_subscription_generic
            const revenuesComparisonWorker = store
                .subscriptions.getSubscriptionWorker({
                    subscriptionName: name
                });

            revenuesComparisonWorker.on("batch", (batch, callback) => {
                for (const item of batch.items) {
                    console.log("Revenue for order with Id: "
                        + item.id + " grown from "
                        + item.result.PreviousRevenue
                        + " to " + item.result.CurrentRevenue);
                }

                callback();
            });
            //endregion
        }
    }

    function processOrderChanges() { }

    {
        //region interface_subscription_deletion
        store.subscriptions.delete(name, [database]);
        //endregion

        //region interface_subscription_dropping
        store.subscriptions.dropConnection(name, [database]);
        //endregion

        //region interface_subscription_state
        store.subscriptions.getSubscriptionState(subscriptionName, [database]);
        //endregion
    }

    {
        const subscriptionName = "";
        //region subscription_deletion
        store.subscriptions.delete(subscriptionName);
        //endregion

        //region connection_dropping
        store.subscriptions.dropConnection(subscriptionName);
        //endregion

        //region subscription_state
        store.subscriptions.getSubscriptionState(subscriptionName);
        //endregion
    }

    class OrderRevenues { }

    class OrderAndCompany { }

    class Company { }

    async function openingExamples() {
        let name;
        let subscriptionName;

        //region subscription_open_simple
        const subscriptionWorker = store.subscriptions.getSubscriptionWorker({ subscriptionName });
        //endregion

        //region subscription_run_simple
        subscriptionWorker.on("batch", (batch, callback) => {
            // your logic here
            
            // report batch processing error passing it to callback
            // callback(err)
            callback();
        });

        subscriptionWorker.on("error", error => {
            // handle errors
        });
        //endregion

        {
            let workerWBatch = null;
            //region throw_during_user_logic
            workerWBatch.on("batch", (batch, callback) => {
                callback(new Error("Error during processing batch."));
            });
            //endregion
        }

    class Order {}
}
