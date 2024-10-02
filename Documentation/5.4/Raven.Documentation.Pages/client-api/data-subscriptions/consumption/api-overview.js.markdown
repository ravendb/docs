# Data Subscriptions: Consumption API Overview
---

{NOTE: }

* In this page:  
  * [Create the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker)  
  * [Subscription worker options](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-options)  
  * [Run the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker)  
  * [Subscription batch](../../../client-api/data-subscriptions/consumption/api-overview#subscription-batch)  
  * [Subscription batch item](../../../client-api/data-subscriptions/consumption/api-overview#subscription-batch-item)  
  * [Subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker)  

{NOTE/}

---

{PANEL: Create the subscription worker}

A subscription worker can be created using the following `getSubscriptionWorker` methods available through the `subscriptions` property of the `documentStore`.

Note: Simply creating the worker is insufficient;  
after creating the worker, you need to [run the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker) to initiate document processing.

{CODE:nodejs consume_syntax_1@client-api\dataSubscriptions\consumption\consumptionApi.js /}

| Parameter            | Type     | Description                                                                                                                                                                                                                                                         |
|----------------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **subscriptionName** | `string` | The name of the subscription to which the worker will connect.                                                                                                                                                                                                      |
| **database**         | `string` | The name of the database where the subscription task resides.<br>If `null`, the default database configured in DocumentStore will be used.                                                                                                                          |
| **options**          | `object` | [Subscription worker options](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-options) object that affect how the worker interacts with the subscription. These options do not alter the definition of the subscription itself. |

| Return value         |                                                                                                                                                                                                                                                                       |
|----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `SubscriptionWorker` | The [subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker) that has been created.<br>The worker will start processing documents once you define the worker's `on` method,<br> which listens to the `batch` event. |

{PANEL/}

{PANEL: Subscription worker options}

{CODE:nodejs consume_syntax_2@client-api\dataSubscriptions\consumption\consumptionApi.js /}

When creating a worker with subscription worker options, the only mandatory property is `subscriptionName`.  
All other parameters are optional and will default to their respective default values if not specified.

| Member                              | Type      | Description                                                                                                                                                                                                                                                                                                                                                                                                |
|-------------------------------------|-----------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **subscriptionName**                | `string`  | The name of the subscription to which the worker will connect.                                                                                                                                                                                                                                                                                                                                             |
| **documentType**                    | `object`  | The class type of the subscription documents.                                                                                                                                                                                                                                                                                                                                                              |
| **ignoreSubscriberErrors**          | `boolean` | Determines if subscription processing is aborted when the worker's batch-handling code throws an unhandled exception.<br><br>`true` – subscription processing will continue.<br><br>`false` (default) – subscription processing will be aborted.                                                                                                                                                           |
| **closeWhenNoDocsLeft**             | `boolean` | Determines whether the subscription connection closes when no new documents are available.<br><br>`true` – The subscription worker processes all available documents and stops when none remain, at which point the `SubscriptionClosedException` will be thrown.<br>Useful for ad-hoc, one-time processing.<br><br>`false` (default) – The subscription worker remains active, waiting for new documents. |
| **maxDocsPerBatch**                 | `number`  | The maximum number of documents that the server will try to retrieve and send to the client in a batch. If the server doesn't find as many documents as specified, it will send the documents it has found without waiting. Default: 4096.                                                                                                                                                                 |
| **timeToWaitBeforeConnectionRetry** | `number`  | The time (in ms) to wait before attempting to reconnect after a non-aborting failure during subscription processing. Default: 5 seconds.                                                                                                                                                                                                                                                                   |
| **maxErroneousPeriod**              | `number`  | The maximum amount of time (in ms) a subscription connection can remain in an erroneous state before it is terminated. Default: 5 minutes.                                                                                                                                                                                                                                                                 |
| **strategy**                        | `string`  | The strategy configures how the server handles connection attempts from workers to a specific subscription task.<br><br>Available options:<br>`OpenIfFree` (default), `TakeOver`, `WaitForFree`, or `Concurrent`.<br><br>Learn more in [worker strategies](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-strategies).                                         |

{PANEL/}

{PANEL: Run the subscription worker}

After [creating](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker) a subscription worker, the subscription worker is still not processing any documents.  
To initiate processing, you need to define an event handler and attach it to the worker's `batch` event listener.  

This handler contains your client-side code responsible for processing the document batches received from the server. 
Whenever a new batch of documents is ready, the provided handler will be triggered.

{CODE:nodejs consume_syntax_3@client-api\dataSubscriptions\consumption\consumptionApi.js /}

{PANEL/}

{PANEL: Subscription batch}

The subscription batch class contains the following public properties & methods:

| Property                      | Type       | Description                                                                                                                                            |
|-------------------------------|------------|--------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **items**                     | `Item[]`   | List of items in the batch.<br>See [subscription batch item](../../../client-api/data-subscriptions/consumption/api-overview#subscription-batch-item). |

| Method                        | Return type | Description                                                                                                              |
|-------------------------------|-------------|--------------------------------------------------------------------------------------------------------------------------| 
| **getNumberOfItemsInBatch()** | `number`    | Get the number of items in the batch.                                                                                    |
| **getNumberOfIncludes()**     | `number`    | Get the number of included documents in the batch.                                                                       |
| **openSession()**             | `object`    | Open a new document session that tracks all items and their included items within the current batch.                     |
| **openSession(options)**      | `object`    | Open a new document session - can pass [session options](../../../client-api/session/opening-a-session#session-options). |

{INFO: }

##### Subscription worker connectivity

As long as there is no exception, the worker will continue addressing the same server that the first batch was received from.  
If the worker fails to reach that node, it will try to [failover](../../../client-api/configuration/load-balance/overview) to another node from the session's topology list.  
The node that the worker succeeded connecting to, will inform the worker which node is currently responsible for data subscriptions.

{INFO/}

{PANEL/}

{PANEL: Subscription batch item}

This class represents a single item in a subscription batch result.

{CODE:nodejs consume_syntax_4@client-api\dataSubscriptions\consumption\consumptionApi.js /}

| Member               | Type      | Description                                                                         |
|----------------------|-----------|-------------------------------------------------------------------------------------| 
| **result**           | `object`  | The current batch item.                                                             |
| **exceptionMessage** | `string`  | The exception message thrown during current document processing in the server side. |
| **id**               | `string`  | The document ID of the underlying document for the current batch item.              |
| **changeVector**     | `string`  | The change vector of the underlying document for the current batch item.            |
| **rawResult**        | `object`  | Current batch item - no types reconstructed.                                        |
| **rawMetadata**      | `object`  | Current batch item's underlying document metadata.                                  |
| **metadata**         | `object`  | Current batch item's underlying metadata values.                                    |

{PANEL/}

{PANEL: Subscription worker}

{NOTE: }

##### Methods

| Method            | Return type   | Description                                       |
|-------------------|---------------|---------------------------------------------------| 
| **dispose()**     | `void`        | Aborts subscription worker operation.             |
| **on()**          | `object`      | Method used to set up event listeners & handlers. |
| **getWorkerId()** | `string`      | Get the worker ID.                                |

{NOTE/}

{NOTE: }

##### Events

| Event                             | Listener signature          | Description                                                                                                                                                                   |
|-----------------------------------|-----------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **"batch"**                       | `(batch, callback) => void` | Emitted when a batch of documents is sent from the server to the client.<br><br>Once processing is done, `callback` *must  be called* in order to continue batches' emission. |
| **"afterAcknowledgment"**         | `(batch, callback) => void` | Emitted after each time the server acknowledges the progress of batch processing.                                                                                             |
| **"connectionRetry"**             | `(error) => void`           | Emitted when the worker attempts to reconnect to the server after a failure.                                                                                                  |
| **"error"**                       | `(error) => void`           | Emitted on subscription errors.                                                                                                                                               |
| **"end"**                         | `(error) => void`          | Emitted when subscription is finished.<br>No more batches are going to be emitted.                                                                                            |

{NOTE/}

{NOTE: }

##### Properties

| Member               | Type     | Description                                                           |
|----------------------|----------|-----------------------------------------------------------------------| 
| **currentNodeTag**   | `string` | The node tag of the current RavenDB server handling the subscription. |
| **subscriptionName** | `string` | The name of the currently processed subscription.                     |

{NOTE/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
