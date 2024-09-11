# Subscription Consumption Examples
---

{NOTE: }

*  this page:  
   * [Worker with a specified batch size](../../../client-api/data-subscriptions/consumption/examples#worker-with-a-specified-batch-size)  
   * [Client with full exception handling and processing retries](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)  
   * [Subscription that ends when no documents left](../../../client-api/data-subscriptions/consumption/examples#subscription-that-ends-when-no-documents-left)  
   * [Worker that processes raw objects](../../../client-api/data-subscriptions/consumption/examples#sworker-that-processes-raw-objects)  
   * [Worker that operates with a session](../../../client-api/data-subscriptions/consumption/examples#worker-that-operates-with-a-session)
   * [Subscription that uses included documents](../../../client-api/data-subscriptions/consumption/examples#subscription-that-uses-included-documents)  
   * [Primary and secondary workers](../../../client-api/data-subscriptions/consumption/examples#primary-and-secondary-workers)  

{NOTE/}

---

{PANEL: Worker with a specified batch size}

Here we create a worker and specify the maximum number of documents the server will send to the worker in each batch.

{CODE:java subscription_worker_with_batch_size@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Client with full exception handling and processing retries}

Here we implement a client that handles exceptions thrown by a worker.  
If the exception is recoverable, the client retries creating the worker.

{CODE:java reconnecting_client@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Subscription that ends when no documents left}

Here we create a subscription client that runs only up to the point there are no more new documents left to process.  

This is useful for ad-hoc, single-use processing where the user needs to ensure that all documents are fully processed.

{CODE:java single_run@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Worker that processes raw objects}

Here we create a worker that processes received data as ObjectNode objects.

{CODE:java dynamic_worker@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Worker that operates with a session}

Here we create a subscription that sends Order documents that do not have a shipping date.  
The worker receiving these documents will update the `ShippedAt` field value and save the document back to the server via the session.

{CODE:java subscription_with_open_session_usage@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Subscription that uses included documents}

Here we create a subscription that, in addition to sending all the _Order_ documents to the worker,  
will include all the referenced _Product_ documents in the batch sent to the worker.

When the worker accesses these _Product_ documents, no additional requests will be made to the server.

{CODE:java subscription_with_includes_path_usage@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Primary and secondary workers}

Here we create two workers:  

* The primary worker, with a `TAKE_OVER` strategy, will take over the other worker and establish the connection.
* The secondary worker, with a `WAIT_FOR_FREE` strategy, will wait for the first worker to fail (due to machine failure, etc.).

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
