# Data Subscriptions: Subscription Consumption Examples

---

{NOTE: }

In this page:  

[Worker with a specified batch size](../../../client-api/data-subscriptions/consumption/examples#worker-with-a-specified-batch-size)  
[Client with full exception handling and processing retries](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)  
[Subscription that ends when no documents left](../../../client-api/data-subscriptions/consumption/examples#subscription-that-ends-when-no-documents-left)  
[Worker that processes dynamic objects](../../../client-api/data-subscriptions/consumption/examples#worker-that-processes-dynamic-objects)  
[Subscription that works with lowest level API](../../../client-api/data-subscriptions/consumption/examples#subscription-that-works-with-lowest-level-api)  
[Subscription workers with failover on other nodes](../../../client-api/data-subscriptions/consumption/examples#subscription-with-failover-on-other-nodes)  
[Subscription workers with a primary and secondary node](../../../client-api/data-subscriptions/consumption/examples#subscription-workers-with-a-primary-and-secondary-node)
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


{PANEL:Subscription workers with failover on other nodes}

In this configuration, any available node will create a worker. If that worker fails, another available node will take over.

{CODE waitforfree@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL:Subscription workers with a primary and secondary node}

Here we create two workers:  
* The primary worker with the `TakeOver` strategy will take the lead over the secondary worker
* The secondary worker will takeover if the primary fails (due to machine failure etc.)

The primary worker:

{CODE:java waiting_subscription_1@ClientApi\DataSubscriptions\DataSubscriptions.java /}

The secondary worker:

{CODE:java waiting_subscription_2@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
