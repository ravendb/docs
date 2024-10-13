import * as assert from "assert";
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function revisionsSubscription() {

    {
        //region delete
        await documentStore.subscriptions.delete("subscriptionNameToDelete");
        //endregion
    }    
    {
        //region disable
        await documentStore.subscriptions.disable("subscriptionNameToDisable");
        //endregion
    }
    {
        //region enable
        await documentStore.subscriptions.enable("subscriptionNameToEnable");
        //endregion
    }
    {
        //region update
        const updateOptions = {
            id: "<theSubscriptionId>",
            query: "<theSubscriptionQuery>"
            // ...
        }
        await documentStore.subscriptions.update(updateOptions);
        //endregion
    }
    {
        //region drop_connection
        // Drop all connections to the subscription:
        // =========================================
        
        await documentStore.subscriptions.dropConnection("subscriptionName");
        
        // Drop specific worker connection:
        // ===============================
        
        const workerOptions = {
            subscriptionName: "subscriptionName",
            // ...
        };

        const worker = documentStore.subscriptions.getSubscriptionWorker(workerOptions);


        worker.on("batch", (batch, callback) => {
            // worker processing logic 
        });
        
        await documentStore.subscriptions.dropConnection(worker);
        //endregion
    }
    {
        //region get_subscriptions
        const subscriptions = await documentStore.subscriptions.getSubscriptions(0, 10);
        //endregion
    }
    {
        //region get_state
        const subscriptionState = 
            await documentStore.subscriptions.getSubscriptionState("subscriptionName");
        //endregion
    }
}

{
//region create_syntax
// Available overloads:
create(options);
create(options, database);
create(documentType);
createForRevisions(options)
createForRevisions(options, database)    
//endregion
}
{
//region delete_syntax
// Available overloads:
delete(name);
delete(name, database);
//endregion
}
{
//region disable_syntax
// Available overloads:
disable(name);
disable(name, database);
//endregion
}
{
//region enable_syntax
// Available overloads:
enable(name);
enable(name, database);
//endregion
}
{
//region update_syntax
// Available overloads:
update(options);
update(options, database);
//endregion
}
{
//region drop_connection_syntax
// Available overloads:
dropConnection(options);
dropConnection(options, database);
dropSubscriptionWorker(worker); 
dropSubscriptionWorker(worker, database); 
//endregion
}
{
//region get_subscriptions_syntax
// Available overloads:
getSubscriptions(start, take);
getSubscriptions(start, take, database);
//endregion
}
{
//region get_state_syntax
// Available overloads:
getSubscriptionState(subscriptionName);
getSubscriptionState(subscriptionName, database);
//endregion
}
