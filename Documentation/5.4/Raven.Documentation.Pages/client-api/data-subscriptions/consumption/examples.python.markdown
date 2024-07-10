# Data Subscriptions: Subscription Consumption Examples

---

{NOTE: }

* In this page:  
   * [Subscription workers with failover on other nodes](../../../client-api/data-subscriptions/consumption/examples#subscription-workers-with-failover-on-other-nodes)  
   * [Worker with a specified batch size](../../../client-api/data-subscriptions/consumption/examples#worker-with-a-specified-batch-size)  
   * [Client with full exception handling and processing retries](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)  
   * [Subscription that ends when no documents are left](../../../client-api/data-subscriptions/consumption/examples#subscription-that-ends-when-no-documents-are-left)  
   * [Worker that processes dynamic objects](../../../client-api/data-subscriptions/consumption/examples#worker-that-processes-dynamic-objects)  
   * [Subscription that uses included documents](../../../client-api/data-subscriptions/consumption/examples#subscription-that-uses-included-documents)  
   * [Subscription that works with a session](../../../client-api/data-subscriptions/consumption/examples#subscription-that-works-with-a-session)  
   * [Subscription workers with a primary and a secondary node](../../../client-api/data-subscriptions/consumption/examples#subscription-workers-with-a-primary-and-a-secondary-node)  

{NOTE/}

---

{PANEL:Subscription workers with failover on other nodes}

In this configuration, any available node will create a worker.  
If the worker fails, another available node will take over.

{CODE:python waitforfree@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL:Worker with a specified batch size}

Here we create a worker, specifying the maximum batch size we want to receive.

{CODE:python subscription_worker_with_batch_size@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL:Client with full exception handling and processing retries}

Here we implement a client that treats exceptions thrown by a worker, and retries creating the worker if an exception is recoverable.

{CODE:python reconnecting_client@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL:Subscription that ends when no documents are left}

Here we create a subscription client that runs only up to the point there are no more new documents left to process.  

This is useful for an ad-hoc single-use processing that the user wants to be sure is performed completely. 

{CODE:python single_run@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}


{PANEL:Worker that processes dynamic objects}

Here we create a worker that processes received data as dynamic objects.

{CODE:python dynamic_worker@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL:Subscription that works with a session}

Here we create a worker that receives all orders without a shipping date, lets the shipment mechanism handle it, and updates the `ShippedAt` field value.

{CODE:python subscription_with_open_session_usage@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL:Subscription that uses included documents}

Here we create a subscription utilizing the includes feature, by processing `Order` documents and including all `Product`s of each order.  
When processing the subscription, we create a session using the [SubscriptionBatch[\_T]](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch<t>) object, 
and for each order line, we obtain the `Product` document and process it alongside the `Order`.

{CODE subscription_with_includes_path_usage@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL:Subscription workers with a primary and a secondary node}

Here we create two workers:  

* The primary worker, set with a `TAKE_OVER` strategy, will take the lead over the secondary worker.  
* The secondary worker, set with a `WAIT_FOR_FREE` strategy, will take over if the primary worker fails (e.g. due to a machine failure).  

The primary worker:  
{CODE:python waiting_subscription_1@ClientApi\DataSubscriptions\DataSubscriptions.py /}

The secondary worker:  
{CODE:python waiting_subscription_2@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
