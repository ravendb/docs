# Data Subscriptions: Subscription Consumption Examples

---

{NOTE: }

In this page:  

[Worker with a specified batch size](../../../client-api/data-subscriptions/consumption/examples#worker-with-a-specified-batch-size)  
[Client with full exception handling and processing retries](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)  
[Subscription that ends when no documents left](../../../client-api/data-subscriptions/consumption/examples#subscription-that-ends-when-no-documents-left)  
[Worker that processes dynamic objects](../../../client-api/data-subscriptions/consumption/examples#worker-that-processes-dynamic-objects)  
[Subscription that works with lowest level API](../../../client-api/data-subscriptions/consumption/examples#subscription-that-works-with-lowest-level-api)  
[Two subscription workers that are waiting for each other](../../../client-api/data-subscriptions/consumption/examples#two-subscription-workers-that-are-waiting-for-each-other)  

{NOTE/}

---

{PANEL:Worker with a specified batch size}

Here we create a worker, specifying the maximum batch size we want to receive.

{CODE:nodejs subscription_worker_with_batch_size@client-api\dataSubscriptions\dataSubscriptions.js /}

{PANEL/}

{PANEL:Client with full exception handling and processing retries}

Here we implement a client that treats exceptions thrown by worker, and retries creating the worker if an exception is recoverable.

{CODE:nodejs reconnecting_client@client-api\dataSubscriptions\dataSubscriptions.js /}

{PANEL/}

{PANEL:Subscription that ends when no documents left}

Here we create a subscription client that runs only up to the point there are no more new documents left to process.  

This is useful for an ad-hoc single use processing that the user wants to be sure is performed completely. 

{CODE:nodejs single_run@client-api\dataSubscriptions\dataSubscriptions.js /}

{PANEL/}


{PANEL:Two subscription workers that are waiting for each other}

Here we create two workers:

* The main worker with the `TakeOver` strategy that will take over the other one and will take the lead  

* The secondary worker that will wait for the first one fail (due to machine failure etc.)

The main worker:

{CODE:nodejs waiting_subscription_1@client-api\dataSubscriptions\dataSubscriptions.js /}

The secondary worker:

{CODE:nodejs waiting_subscription_2@client-api\dataSubscriptions\dataSubscriptions.js /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
