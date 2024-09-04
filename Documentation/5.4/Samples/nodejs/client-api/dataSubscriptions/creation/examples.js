import * as assert from "assert";
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = store.openSession();

async function createExamples() {

    {
        //region create_1
        const subscriptionName = await documentStore.subscriptions.create({
            // Optionally, provide a custom name for the subscription
            name: "OrdersProcessingSubscription",
            
            // You can provide the collection name in the RQL string in the 'query' param 
            query: "from Orders" 
        });
        //endregion
    }
    {
        //region create_1_1
        const subscriptionName = await documentStore.subscriptions.create({
            name: "OrdersProcessingSubscription",
            
            // Or, you can provide the document type for the collection in the 'documentType' param
            documentType: Order
        });
        //endregion
    }
    {
        //region create_1_2
        // Or, you can use the folllowing overload,
        // pass the document class type to the 'create' method
        const subscriptionName = await documentStore.subscriptions.create(Order);
        //endregion
    }
    {
        //region create_2
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
            where getOrderLinesSum(o) > 100`;
        
        // Create the subscription with the defined query
        const subscriptionName = await documentStore.subscriptions.create({ query });

        // In this case, the server will create a default name for the subscription
        // since no specific name was provided when creating the subscription.
        //endregion
    }     
    {
        //region create_3
        const query = `
            declare function getOrderLinesSum(doc) {
                var sum = 0; 
                for (var i in doc.Lines) {
                    sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                }
                return sum;
            }
            
            declare function projectOrder(doc) {
                 return {
                     Id: doc.Id,
                     Total: getOrderLinesSum(doc)
                 }
            }
             
            from order as o 
            where getOrderLinesSum(o) > 100 
            select projectOrder(o)`;
        
        const subscriptionName = await documentStore.subscriptions.create({ query });
        //endregion
    }
    {
        //region create_4
        const query = `
            declare function getOrderLinesSum(doc) {
                var sum = 0;
                for (var i in doc.Lines) {
                    sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                }
                return sum;
            }
            
            declare function projectOrder(doc) {
                var employee = load(doc.Employee);
                return {
                    Id: doc.Id,
                    Total: getOrderLinesSum(doc),
                    ShipTo: doc.ShipTo,
                    EmployeeName: employee.FirstName + ' ' + employee.LastName
                }
            }
             
            from order as o
            where getOrderLinesSum(o) > 100
            select projectOrder(o)`;
        
        const subscriptionName = await documentStore.subscriptions.create({ query });
        //endregion
    }
    {
        //region create_5
        const options = {
            // The documents whose IDs are specified in the 'Product' property
            // will be included in the batch
            includes: builder => builder.includeDocuments("Lines[].Product"),
            documentType: Order
        };
        
        const subscriptionName = await documentStore.subscriptions.create(options);
        //endregion
    }
    {
        //region create_5_1
        const query = `from Orders include Lines[].Product`;
        const subscriptionName = await documentStore.subscriptions.create({ query });
        //endregion
    }
    {
        //region create_5_2
        const query = `
            declare function includeProducts(doc) {
                let includedFields = 0;
                let linesCount = doc.Lines.length;

                for (let i = 0; i < linesCount; i++) {
                    includedFields++;
                    include(doc.Lines[i].Product);
                }

                return doc;
            }

            from Orders as o select includeProducts(o)`;
        
        const subscriptionName = await documentStore.subscriptions.create({ query });
        //endregion
    }
    {
        //region create_6
        todo...
        includeCounter(name: string): ISubscriptionIncludeBuilder;
        includeCounters(names: string[]): ISubscriptionIncludeBuilder;
        includeAllCounters(): ISubscriptionIncludeBuilder;
        //endregion
    }
    {
        //region create_7
        const options = {
            includes: builder => builder
                 // Values for the specified counters will be included in the batch
                .includeCounters(["Pros", "Cons"]),
            documentType: Order
        };

        const subscriptionName = await documentStore.subscriptions.create(options);
        //endregion
    }
    {
        //region create_7_1
        const options = {
            query: "from Orders include counters('Pros'), counters('Cons')" 
        };

        const subscriptionName = await documentStore.subscriptions.create(options);
        //endregion
    }
    {
        //region create_8
        const options = {
            includes: builder => builder
                .includeCounter("Likes")
                .includeCounters(["Pros", "Cons"])
                .includeDocuments("Employee"),
            documentType: Order
        };

        const subscriptionName = await documentStore.subscriptions.create(options);
        //endregion
    }
    {
        //region create_9
        const subscriptionName = await documentStore.subscriptions.createForRevisions({
            documentType: Order
        });
        //endregion
    }
    {
        //region create_9_1
        const subscriptionName = await documentStore.subscriptions.createForRevisions({
            query: "from Orders (Revisions = true)"
        });
        //endregion
    }
    {
        //region update_0
        const subscriptionName = await documentStore.subscriptions.update({
            name: "my subscription",
            query: "from Products where PricePerUnit > 20",

            // Set to true so that a new subscription will be created 
            // if a subscription with name "my subscription" does Not exist
            createNew: true
        });
        //endregion
    }
    {
        //region update_1
        const subscriptionName = await documentStore.subscriptions.update({
            // Specify the subscription you wish to modify
            name: "my subscription",
            
            // Provide a new query
            query: "from Products where PricePerUnit > 50" 
        });
        //endregion
    }
    {
        //region update_2
        // Get the subscription's ID
        const mySubscription = await documentStore.subscriptions.getSubscriptionState("my subscription");
        const subscriptionId = mySubscription.subscriptionId;

        // Update the subscription
        const subscriptionName = await documentStore.subscriptions.update({
            id: subscriptionId,
            query: "from Products where PricePerUnit > 50"
        });
        //endregion
    }

    {
        //region update_3
        // Get the subscription's ID
        const mySubscription = await documentStore.subscriptions.getSubscriptionState("my subscription");
        const subscriptionId = mySubscription.subscriptionId;

        // Update the subscription's name
        const subscriptionName = await documentStore.subscriptions.update({
            id: subscriptionId,
            name: "new name"
        });
        //endregion
    }

    class Order {}
}
