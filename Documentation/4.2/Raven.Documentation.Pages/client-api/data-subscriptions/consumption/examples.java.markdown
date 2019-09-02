# Data Subscriptions: Subscription Consumption Examples

---

{NOTE: }

In this page:  

[Worker with a specified batch size](../../../client-api/data-subscriptions/consumption/examples#worker-with-a-specified-batch-size)  
[Client with full exception handling and processing retries](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)  
[Subscription that ends when no documents left](../../../client-api/data-subscriptions/consumption/examples#subscription-that-ends-when-no-documents-left)  
[Subscription that processes ObjectNode objects](../../../client-api/data-subscriptions/consumption/examples#subscription-that-processes-objectnode-objects)  
[Subscription that works with a session](../../../client-api/data-subscriptions/consumption/examples#subscription-that-works-with-a-session)  
[Subscription that uses included documents](../../../client-api/data-subscriptions/consumption/examples#subscription-that-uses-included-documents)  
[Two subscription workers that are waiting for each other](../../../client-api/data-subscriptions/consumption/examples#two-subscription-workers-that-are-waiting-for-each-other)  

{NOTE/}

---

{PANEL:Worker with a specified batch size}

Here we create a worker, specifying the maximum batch size we want to receive.

{CODE:java subscription_worker_with_batch_size@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL:Client with full exception handling and processing retries}

Here we implement a client that treats exceptions thrown by worker, and retries creating the worker if an exception is recoverable.

{CODE:java reconnecting_client@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL:Subscription that ends when no documents left}

Here we create a subscription client that runs only up to the point there are no more new documents left to process.  

This is useful for an ad-hoc single use processing that the user wants to be sure is performed completely. 

{CODE:java single_run@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL:Worker that processes raw objects}

Here we create a worker that processes received data as ObjectNode objects.

{CODE:java dynamic_worker@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL:Subscription that works with a session}

Here we create a worker that receives all orders without a shipping date, lets the shipment mechanism to handle it and updates the `ShippedAt` field value.

{CODE:java subscription_with_open_session_usage@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL:Subscription that uses included documents}

Here we create a subscription utilizing the includes feature, by processing `Order` documents and including all `Product`s of each order.  
When processing the subscription, we create a session using the [SubscriptionBatch&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch<t>) object, 
and for each order line, we obtain the `Product` document and process it alongside with the `Order`.

{CODE:java subscription_with_includes_path_usage@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}


{PANEL:Two subscription workers that are waiting for each other}

Here we create two workers:  
* The main worker with the `TAKE_OVER` strategy that will take over the other one and will take the lead  
* The secondary worker that will wait for the first one fail (due to machine failure etc.)

The main worker:

{CODE:java waiting_subscription_1@ClientApi\DataSubscriptions\DataSubscriptions.java /}

The secondary worker:

{CODE:java waiting_subscription_2@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
