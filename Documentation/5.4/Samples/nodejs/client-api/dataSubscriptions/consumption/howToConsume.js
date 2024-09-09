import * as assert from "assert";
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = store.openSession();

async function howToConsume() {
    {
        //region consume_1
        const worker = documentStore.subscriptions.getSubscriptionWorker({
            subscriptionName: "your subscription name"
        });
        //endregion
    }
    {
        //region consume_2
        worker.on("batch", (batch, callback) => {
            try {
                // Add your logic for processing the incoming batch items here...               

                // Call 'callback' once you're done
                // The worker will send an acknowledgement to the server,
                // allowing the server to send the next batch
                callback();

            } catch(err) {
                // If processing fails for a particular batch then pass the error to the callback
                callback(err);
            }
        });
        //endregion
    }
    {
        //region consume_3
        worker.on("batch", (batch, callback) => {
            try {
                throw new Error("Exception occurred");
            } catch (err) {
                callback(err); // Pass the error to the callback to signal failure
            }
        });
        //endregion
    }
}
