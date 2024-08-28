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
            const name = await store.subscriptions.create({ 
                name: "OrdersProcessingSubscription",
                documentType: Order
            });
            //endregion
        }

        {
            //region create_whole_collection_generic_with_mentor_node
            const name = await store.subscriptions.create({
                documentType: Order,
                mentorNode: "D"
            });
            //endregion
        }

        {
            //region create_whole_collection_generic1
            // With the following subscription definition, the server will send all documents
            // from the 'Orders' collection to a client that connects to this subscription.
            store.subscriptions.create(Order);
            //endregion
        }

        {
            //region create_whole_collection_RQL
            store.subscriptions.create({ query: "from Orders" });
            //endregion
        }

        {
            //region create_filter_only_RQL
            const query = `declare function getOrderLinesSum(doc) {
                    var sum = 0;
                    for (var i in doc.Lines) { sum += doc.Lines[i]; }
                    return sum;
                }
                from Orders as o 
                where getOrderLinesSum(o) > 100`;

            const name = await store.subscriptions.create({ query });
            //endregion
        }

        {
            //region create_filter_and_projection_RQL
            const query = 
                `declare function getOrderLinesSum(doc) {
                    var sum = 0; 
                    for (var i in doc.Lines) { sum += doc.Lines[i]; }
                    return sum;
                }

                declare function projectOrder(doc) {
                     return {
                         Id: order.Id,
                         Total: getOrderLinesSum(order)
                     }
                 }
                 from order as o 
                 where getOrderLinesSum(o) > 100 
                 select projectOrder(o)`;

            const name = await store.subscriptions.create({ query });
            //endregion
        }

        {
            //region create_filter_and_load_document_RQL
            const query =
                `declare function getOrderLinesSum(doc) {
                    var sum = 0;
                    for (var i in doc.Lines) { sum += doc.Lines[i]; }
                    return sum;
                }

                declare function projectOrder(doc) {
                     var employee = LoadDocument(doc.Employee);
                     return {
                         Id: order.Id,
                         Total: getOrderLinesSum(order),
                         ShipTo: order.ShipTo,
                         EmployeeName: employee.FirstName + ' ' + employee.LastName
                     }
                 }
                 from order as o
                 where getOrderLinesSum(o) > 100
                 select projectOrder(o)`;

            const name = await store.subscriptions.create({ query });
            //endregion
        }

        {
            //region create_simple_revisions_subscription_generic
            const name = await store.subscriptions.createForRevisions(Order);
            //endregion
        }

        {
            //region create_simple_revisions_subscription_RQL
            const name = await store.subscriptions.createForRevisions({
                query: "from orders (Revisions = true)"
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
            //region subscription_worker_with_batch_size
            const options = { 
                subscriptionName,
                maxDocsPerBatch: 20
            };

            const workerWBatch = store.subscriptions.getSubscriptionWorker(options);
            workerWBatch.on("batch", (batch, callback) => { /* custom logic */});
            //endregion
        }

        {
            let workerWBatch = null;
            //region throw_during_user_logic
            workerWBatch.on("batch", (batch, callback) => {
                callback(new Error("Error during processing batch."));
            });
            //endregion
        }

        async function reconnecting() {
            //region reconnecting_client

            // here we configure that we allow a down time of up to 2 hours,
            // and will wait for 2 minutes for reconnecting
            const options = {
                subscriptionName,
                maxErroneousPeriod: 2 * 3600 * 1000,
                timeToWaitBeforeConnectionRetry: 2 * 60 * 1000
            };

            setupReconnectingSubscription(options);

            function setupReconnectingSubscription(subscriptionWorkerOptions) {
                let subscriptionWorker;

                reconnect();

                function closeWorker(worker) {
                    worker.removeAllListeners();
                    worker.on("error", () => {}); // ignore errors from old connection
                    worker.dispose();
                }

                function reconnect() {
                    if (subscriptionWorker) {
                        closeWorker();
                    }

                    subscriptionWorker = store.subscriptions.getSubscriptionWorker(subscriptionWorkerOptions);

                    // here we are able to be informed of any exception that happens during processing
                    subscriptionWorker.on("connectionRetry", error => {
                        console.error(
                            "Error during subscription processing: " + subscriptionName, error);
                    });

                    subscriptionWorker.on("batch", (batch, callback) => {
                        for (const item of batch.items) {
                            // we want to force close the subscription processing in that case
                            // and let the external code decide what to do with that
                            if (item.result.shipVia 
                                && "Europe" === item.result.shipVia) {
                                callback(new InvalidOperationException("We cannot ship via Europe."));
                                return;
                            }

                            processOrder(item.result);
                        }
                    });

                    subscriptionWorker.on("error", error => {
                        console.error("Failure in subscription: " + subscriptionName, error);

                        if (error.name === "DatabaseDoesNotExistException" ||
                            error.name === "SubscriptionDoesNotExistException" ||
                            error.name === "SubscriptionInvalidStateException" ||
                            error.name === "AuthorizationException") {
                            throw error; 
                        }

                        if (error.name === "SubscriptionClosedException") {
                            // closed explicitly by admin, probably
                            return closeWorker(subscriptionWorker);
                        }

                        if (error.name === "SubscriberErrorException") {
                            // for InvalidOperationException type, we want to throw an exception, otherwise
                            // we continue processing
                            // RavenDB client uses VError - it can nest errors and keep track of inner errors
                            // under cause property
                            if (error.cause && error.cause.name === "InvalidOperationException") {
                                throw error;
                            }

                            return reconnect();
                        }

                        // handle this depending on subscription
                        // open strategy (discussed later)
                        if (error.name === "SubscriptionInUseException") {
                            return reconnect();
                        }

                        return reconnect();
                    });

                    subscriptionWorker.on("end", () => {
                        closeWorker(subscriptionWorker);
                    });
                }
            }
            //endregion
    }

    function processOrder(result) { }

    function singleRun() {
        let subsId;
        //region single_run
        const options = { 
            subscriptionName: subsId,
            // Here we ask the worker to stop when there are no documents left to send.
            // Will throw SubscriptionClosedException when it finishes it's job
            closeWhenNoDocsLeft: true
        };

        const highValueOrdersWorker = store
            .subscriptions.getSubscriptionWorker(options);

        highValueOrdersWorker.on("batch", async (batch, callback) => {
            for (const item of batch.items) {
                await sendThankYouNoteToEmployee(item.result);
            }

            callback();
        });

        highValueOrdersWorker.on("error", err => {
            if (err.name === "SubscriptionClosedException"){
                //that's expected
            }
        });
        //endregion
    }

    async function sendThankYouNoteToEmployee(oac) { }

    class Order {}

    async function twoSubscription1() {
        //region waiting_subscription_1
        const options1 = {
            subscriptionName,
            strategy: "TakeOver",
            documentType: Order
        };

        const worker1 = store.subscriptions.getSubscriptionWorker(options1);

        worker1.on("batch", (batch, callback) => {
            // your logic
            callback();
        });

        worker1.on("error", err => {
            // retry
        });
        //endregion
    }
    
    async function twoSubscription2() {
        //region waiting_subscription_2
        const options2 = {
            subscriptionName,
            strategy: "WaitForFree",
            documentType: Order
        };

        const worker2 = store.subscriptions.getSubscriptionWorker(options2);

        worker2.on("batch", (batch, callback) => {
            // your logic
            callback();
        });

        worker2.on("error", err => {
            // retry
        });
        //endregion
    }
}
