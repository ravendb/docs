# Subscription Consumption Examples
---

{NOTE: }

* In this page:  
  * [Client with full exception handling and processing retries](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)
  * [Worker with a specified batch size](../../../client-api/data-subscriptions/consumption/examples#worker-with-a-specified-batch-size)
  * [Worker that operates with a session](../../../client-api/data-subscriptions/consumption/examples#worker-that-operates-with-a-session)
  * [Worker that processes dynamic objects](../../../client-api/data-subscriptions/consumption/examples#worker-that-processes-dynamic-objects)
  * [Worker that processes a blittable object](../../../client-api/data-subscriptions/consumption/examples#worker-that-processes-a-blittable-object)
  * [Subscription that ends when no documents are left](../../../client-api/data-subscriptions/consumption/examples#subscription-that-ends-when-no-documents-are-left)
  * [Subscription that uses included documents](../../../client-api/data-subscriptions/consumption/examples#subscription-that-uses-included-documents)
  * [Subscription workers with failover on other nodes](../../../client-api/data-subscriptions/consumption/examples#subscription-workers-with-failover-on-other-nodes)
  * [Primary and secondary workers](../../../client-api/data-subscriptions/consumption/examples#primary-and-secondary-workers)

{NOTE/}

---


{PANEL: Client with full exception handling and processing retries}

Here we implement a client that handles exceptions thrown by the worker.  
If the exception is recoverable, the client retries creating the worker.

{CODE reconnecting_client@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Worker with a specified batch size}

Here we create a worker and specify the maximum number of documents the server will send to the worker in each batch.

{CODE subscription_worker_with_batch_size@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Worker that operates with a session}

Here we create a subscription that sends Order documents that do not have a shipping date.  
The worker receiving these documents will update the `ShippedAt` field value and save the document back to the server via the session.

{CODE subscription_with_open_session_usage@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Worker that processes dynamic objects}

Here we define a subscription that projects the Order documents into a dynamic format.  
The worker processes the dynamic objects it receives.

{CODE dynamic_worker@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Worker that processes a blittable object}

Create a worker that processes documents as low level blittable objects.  
This can be useful in extreme high-performance scenarios, but may be dangerous due to the direct usage of unmanaged memory.

{CODE blittable_worker@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Subscription that ends when no documents are left}

Here we create a subscription client that runs until there are no more new documents to process.  
This is useful for ad-hoc, single-use processing where the user needs to ensure that all documents are fully processed.

{CODE single_run@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Subscription that uses included documents}

Here we create a subscription that, in addition to sending all the _Order_ documents to the worker,  
will include all the referenced _Product_ documents in the batch sent to the worker.

When the worker accesses these _Product_ documents, no additional requests will be made to the server.

{CODE subscription_with_includes_path_usage@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Subscription workers with failover on other nodes}

In this configuration, any available node will create a worker.  
If the worker fails, another available node will take over.

{CODE waitforfree@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Primary and secondary workers}

Here we create two workers:  

* The primary worker, set with a `TakeOver` strategy, will take the lead over the secondary worker.  
* The secondary worker, set with a `WaitForFree` strategy, will take over if the primary worker fails  
  (e.g. due to a machine failure).  

The primary worker:  
{CODE waiting_subscription_1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

The secondary worker:  
{CODE waiting_subscription_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
