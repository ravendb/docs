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

async function consumeExamples() {

    {
        // Example: Client with full exception handling and processing retries
        // ===================================================================

        //region consume_1
        // Create the subscription task on the server:
        // ===========================================
        
        const subscriptionName = await documentStore.subscriptions.create({
            name: "ProcessOrdersWithLowFreight",
            query: "from Orders where Freight < 0.5"
        });

        // Create the subscription worker that will consume the documents:
        // ===============================================================
        
        await setupReconnectingWorker(subscriptionName);
        
        async function setupReconnectingWorker(subscriptionName) {
            let subscriptionWorker;

            await reconnect();

            function closeWorker(worker) {
                worker.dispose();
            }

            async function reconnect() {
                if (subscriptionWorker) {
                    closeWorker(subscriptionWorker);
                }

                // Configure the worker:
                const subscriptionWorkerOptions = {
                    subscriptionName: subscriptionName,
                    // Allow a downtime of up to 2 hours
                    maxErroneousPeriod: 2 * 3600 * 1000,
                    // Wait 2 minutes before reconnecting
                    timeToWaitBeforeConnectionRetry: 2 * 60 * 1000
                };

                subscriptionWorker = 
                    store.subscriptions.getSubscriptionWorker(subscriptionWorkerOptions);

                // Subscribe to connection retry events,
                // and log any exceptions that occur during processing
                subscriptionWorker.on("connectionRetry", error => {
                    console.error(
                        "Error during subscription processing: " + subscriptionName, error);
                });

                // Run the worker:
                // =============== 
                subscriptionWorker.on("batch", (batch, callback) => {
                    try {
                        for (const item of batch.items) {
                            const orderDocument = item.result;
                            
                            // Forcefully stop subscription processing if the ID is "companies/46-A"
                            // and throw an exception to let external logic handle the specific case
                            if (orderDocument.Company && orderDocument.Company === "companies/46-A") {
                                // 'The InvalidOperationException' thrown from here
                                // will be wrapped by `SubscriberErrorException`
                                callback(new InvalidOperationException(
                                    "Company ID can't be 'companies/46-A', pleases fix"));
                                return;
                            }

                            // Process the order document - provide your own logic
                            processOrder(orderDocument);
                        }
                        // Call 'callback' once you're done
                        // The worker will send an acknowledgement to the server,
                        // so that server can send next batch
                        callback();
                    } 
                    catch(err) {
                        callback(err);
                    }
                });

                // Handle errors:
                // ============== 
                subscriptionWorker.on("error", error => {
                    console.error("Failure in subscription: " + subscriptionName, error);

                    // The following exceptions are Not recoverable
                    if (error.name === "DatabaseDoesNotExistException" ||
                        error.name === "SubscriptionDoesNotExistException" ||
                        error.name === "SubscriptionInvalidStateException" ||
                        error.name === "AuthorizationException") {
                        throw error;
                    }

                    if (error.name === "SubscriptionClosedException") {
                        // Subscription probably closed explicitly by admin
                        return closeWorker(subscriptionWorker);
                    }

                    if (error.name === "SubscriberErrorException") {
                        // For the InvalidOperationException we want to throw an exception,
                        // otherwise, continue processing
                        if (error.cause && error.cause.name === "InvalidOperationException") {
                            throw error;
                        }

                        setTimeout(reconnect, 1000);
                        return;
                    }

                    // Handle this depending on the subscription opening strategy
                    if (error.name === "SubscriptionInUseException") {
                        setTimeout(reconnect, 1000);
                        return;
                    }

                    setTimeout(reconnect, 1000);
                    return;
                });

                // Handle worker end event:
                // ========================
                subscriptionWorker.on("end", () => {
                    closeWorker(subscriptionWorker);
                });
            }
        }
        //endregion

        function processOrder(item) { }
    }
    
    {   
        // Example: Worker with a specified batch size
        // ===========================================
        
        //region consume_2
        // Create the subscription task on the server:
        // ===========================================
        
        const subscriptionName = await documentStore.subscriptions.create({
            name: "ProcessOrders",
            query: "from Orders"
        });

        // Create the subscription worker that will consume the documents:
        // ===============================================================
        
        const workerOptions = {
            subscriptionName: subscriptionName,
            maxDocsPerBatch: 20 // Set the maximum number of documents per batch
        };

        const worker = documentStore.subscriptions.getSubscriptionWorker(workerOptions);

        worker.on("batch", (batch, callback) => {
            try {
                // Add your logic for processing the incoming batch items here...   

                // Call 'callback' once you're done
                // The worker will send an acknowledgement to the server,
                // so that server can send next batch
                callback();

            } catch(err) {
                callback(err);
            }
        });
        //endregion
    }
    
    {
        // Example:Worker that operates with a session 
        // ===========================================
        
        //region consume_3
        // Create the subscription task on the server:
        // ===========================================
        
        const subscriptionName = await documentStore.subscriptions.create({
            name: "ProcessOrdersThatWereNotShipped",
            query: "from Orders as o where o.ShippedAt = null"
        });

        // Create the subscription worker that will consume the documents:
        // ===============================================================

        const workerOptions = { subscriptionName };
        const worker = documentStore.subscriptions.getSubscriptionWorker(workerOptions);

        worker.on("batch", async (batch, callback)
            try {
                // Open a session with 'batch.openSession'
                const session = batch.openSession();
                
                for (const item of batch.items) {
                    orderDocument = item.result;

                    transferOrderToShipmentCompany(orderDocument); // call your custom method 
                    orderDocument.ShippedAt = new Date();  // update the document field
                }

                // Save the updated Order documents
                await session.saveChanges();
                callback();
                
            } catch(err) {
                callback(err);
            }
        });
        //endregion
        
        function transferOrderToShipmentCompany(item) { }
    }
    
    {
        // Example: Worker that processes dynamic objects
        // ==============================================
        
        //region consume_4
        // Create the subscription task on the server:
        // ===========================================
        
        const subscriptionName = await documentStore.subscriptions.create({
            name: "ProcessDynamicFields",
            query: `From Orders as o
                    Select {
                        dynamicField: "Company: " + o.Company + " Employee: " + o.Employee,
                    }`
        });

        // Create the subscription worker that will consume the documents:
        // ===============================================================

        const workerOptions = { subscriptionName };
        const worker =  documentStore.subscriptions.getSubscriptionWorker(workerOptions);

        worker.on("batch", (batch, callback) => {
            for (const item of batch.items) {
                
                // Access the dynamic field in the document
                const field = item.result.dynamicField;

                // Call your custom method
                processItem(field);
            }

            callback();
        });
        //endregion

        function processItem(field) { }
    }
    
    { 
        // Example: Subscription that ends when no documents are left
        // ==========================================================
        
        async function closeWhenNoDocsAreLeft() {
            //region consume_5
            // Create the subscription task on the server:
            // ===========================================

            // Define the filtering criteria
            const query = `
                            declare function getOrderLinesSum(doc) {
                                var sum = 0;
                                for (var i in doc.Lines) {
                                    sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                                }
                                return sum;
                            }
                            
                            from Orders as o 
                            where getOrderLinesSum(o) > 10_000`;

            // Create the subscription with the defined query
            const subscriptionName = await documentStore.subscriptions.create({ query });

            // Create the subscription worker that will consume the documents:
            // ===============================================================
            
            const workerOptions = {
                subscriptionName: subscriptionName,
                // Here we set the worker to stop when there are no more documents left to send 
                // Will throw SubscriptionClosedException when it finishes it's job
                closeWhenNoDocsLeft: true
            };

            const highValueOrdersWorker = 
                documentStore.subscriptions.getSubscriptionWorker(workerOptions);

            highValueOrdersWorker.on("batch", (batch, callback) => {
                for (const item of batch.items) {
                    sendThankYouNoteToEmployee(item.result); // call your custom method 
                }

                callback();
            });

            highValueOrdersWorker.on("error", err => {
                if (err.name === "SubscriptionClosedException") {
                    // That's expected, no more documents to process
                }
            });
            //endregion
        }

        function sendThankYouNoteToEmployee(item) { }
    }
    
    {
        // Example: Subscription that uses included documents
        // ==================================================
        
        //region consume_6
        // Create the subscription task on the server:
        // ===========================================

        const subscriptionName = await documentStore.subscriptions.create({
            name: "ProcessIncludedDocuments",
            query: `from Orders include Lines[].Product`
        });

        // Create the subscription worker that will consume the documents:
        // ===============================================================

        const workerOptions = { subscriptionName };
        const worker = documentStore.subscriptions.getSubscriptionWorker(workerOptions);

        worker.on("batch", async (batch, callback) => {
            // Open a session via 'batch.openSession'
            // in order to access the Product documents
            const session = batch.openSession();
            
            for (const item of batch.items) {
                const orderDocument = item.result;
                
                for (const orderLine of orderDocument.Lines)
                {
                    // Calling 'load' will Not generate a request to the server,
                    // because orderLine.Product was included in the batch
                    const product = await session.load(orderLine.Product);
                    const productName = product.Name;

                    // Call your custom method
                    processOrderAndProduct(order, product);
                }
            }
            
            callback();
        });
        //endregion
        
        function processOrderAndProduct(order, product) { }
    }
    
    {
        // Example: Primary and secondary workers
        // ======================================
        
        //region consume_7
        const workerOptions1 = {
            subscriptionName,
            strategy: "TakeOver",
            documentType: Order
        };

        const worker1 = documentStore.subscriptions.getSubscriptionWorker(workerOptions1);

        worker1.on("batch", (batch, callback) => {
            // your logic
            callback();
        });

        worker1.on("error", err => {
            // retry
        });
        //endregion
    }
    
    {
        // Example: Primary and secondary workers
        // ======================================
        
        //region consume_8
        const workerOptions2 = {
            subscriptionName,
            strategy: "WaitForFree",
            documentType: Order
        };

        const worker2 = documentStore.subscriptions.getSubscriptionWorker(workerOptions2);

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
