# Subscription Consumption Examples
---

{NOTE: }

* In this page:
  * [Client with full exception handling and processing retries](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)
  * [Worker with a specified batch size](../../../client-api/data-subscriptions/consumption/examples#worker-with-a-specified-batch-size)
  * [Worker that operates with a session](../../../client-api/data-subscriptions/consumption/examples#worker-that-operates-with-a-session)
  * [Worker that processes dynamic objects](../../../client-api/data-subscriptions/consumption/examples#worker-that-processes-dynamic-objects)
  * [Subscription that ends when no documents are left](../../../client-api/data-subscriptions/consumption/examples#subscription-that-ends-when-no-documents-are-left)
  * [Subscription that uses included documents](../../../client-api/data-subscriptions/consumption/examples#subscription-that-uses-included-documents)
  * [Primary and secondary workers](../../../client-api/data-subscriptions/consumption/examples#primary-and-secondary-workers)

{NOTE/}

---

{PANEL: Client with full exception handling and processing retries}

Here we implement a client that handles exceptions thrown by the worker.  
If the exception is recoverable, the client retries creating the worker.

{CODE:nodejs consume_1@client-api\dataSubscriptions\consumption\examples.js /}

{PANEL/}

{PANEL: Worker with a specified batch size}

Here we create a worker and specify the maximum number of documents the server will send to the worker in each batch.

{CODE:nodejs consume_2@client-api\dataSubscriptions\consumption\examples.js /}

{PANEL/}

{PANEL: Worker that operates with a session}

Here we create a subscription that sends _Order_ documents that do not have a shipping date.  
The worker receiving these documents will update the `ShippedAt` field value and save the document back to the server via the session.

{INFO: }
Note:  
The session is opened with `batch.openSession` instead of with `documentStore.openSession`.
{INFO/}

{CODE:nodejs consume_3@client-api\dataSubscriptions\consumption\examples.js /}

{PANEL/}

{PANEL: Worker that processes dynamic objects}

Here we define a subscription that projects the _Order_ documents into a dynamic format.  
The worker processes the dynamic objects it receives.

{CODE:nodejs consume_4@client-api\dataSubscriptions\consumption\examples.js /}

{PANEL/}

{PANEL: Subscription that ends when no documents are left}

Here we create a subscription client that runs until there are no more new documents to process.  
This is useful for ad-hoc, single-use processing where the user needs to ensure that all documents are fully processed.

{CODE:nodejs consume_5@client-api\dataSubscriptions\consumption\examples.js /}

{PANEL/}

{PANEL: Subscription that uses included documents}

Here we create a subscription that, in addition to sending all the _Order_ documents to the worker,  
will include all the referenced _Product_ documents in the batch sent to the worker.

When the worker accesses these _Product_ documents, no additional requests will be made to the server.

{CODE:nodejs consume_6@client-api\dataSubscriptions\consumption\examples.js /}

{PANEL/}

{PANEL: Primary and secondary workers}

Here we create two workers:

* The primary worker, with a `TakeOver` strategy, will take over the other worker and establish the connection.  
* The secondary worker, with a `WaitForFree` strategy, will wait for the first worker to fail (due to machine failure, etc.).

The primary worker:

{CODE:nodejs consume_7@client-api\dataSubscriptions\consumption\examples.js /}

The secondary worker:

{CODE:nodejs consume_8@client-api\dataSubscriptions\consumption\examples.js /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
