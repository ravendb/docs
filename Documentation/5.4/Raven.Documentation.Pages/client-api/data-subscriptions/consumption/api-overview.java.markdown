# Consume Subscriptions API
---

{NOTE: }

* In this page:  
   * [Create the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker)
   * [SubscriptionWorkerOptions](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions)  
   * [Run the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker)  
   * [SubscriptionBatch&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch<t>)  
   * [SubscriptionWorker&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker<t>)  

{NOTE/}

---

{PANEL: Create the subscription worker}

Subscription worker generation is accessible through the `DocumentStore`'s `subscriptions()` method, of type `DocumentSubscriptions`:
{CODE:java subscriptionWorkerGeneration@ClientApi\DataSubscriptions\DataSubscriptions.java /}

| Parameter            |                             |                                                                                                                                                                                                |
|----------------------|-----------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **subscriptionName** | `String`                    | The subscription's name. This parameter appears in more simple overloads allowing to start processing without creating a `SubscriptionCreationOptions` instance, relying on the default values |
| **options**          | `SubscriptionWorkerOptions` | Options that affect how the worker interacts with the subscription. These options do not alter the definition of the subscription itself.                                                      |
| **database**         | `String`                    | The name of the database where the subscription task resides. If `null`, the default database configured in DocumentStore will be used.                                                        |

| Return value         |                                                                                                                                                     |
|----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------|
| `SubscriptionWorker` | The subscription worker that has been created.<br>Initially, it is idle and will only start processing documents when the `run` function is called. |


{PANEL/}

{PANEL: SubscriptionWorkerOptions}

When creating a worker with `SubscriptionWorkerOptions`, the only mandatory property is `subscriptionName`.  
All other parameters are optional and will default to their respective default values if not specified.


| Member                              | Type                                    | Description                                                                                                                                                                                                                                                                                                                                                                                                       |
|-------------------------------------|-----------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **subscriptionName**                | `String`                                | The name of the subscription to which the worker will connect.                                                                                                                                                                                                                                                                                                                                                    |
| **timeToWaitBeforeConnectionRetry** | `Duration`                              | The time to wait before attempting to reconnect after a non-aborting failure during subscription processing. Default: 5 seconds.                                                                                                                                                                                                                                                                                  |
| **ignoreSubscriberErrors**          | `boolean`                               | Determines if subscription processing is aborted when the worker's batch-handling code throws an unhandled exception.<br><br>`true` – subscription processing will continue.<br><br>`false` (Default) – subscription processing will be aborted.                                                                                                                                                                  |
| **strategy**                        | `SubscriptionOpeningStrategy`<br>(enum) | Configures how the server handles connection attempts from workers to a specific subscription task.<br>Learn more in [worker strategies](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-strategies).<br>Default: `OPEN_IF_FREE`.                                                                                                                                      |
| **maxDocsPerBatch**                 | `int`                                   | The maximum number of documents that the server will try to retrieve and send to the client in a batch. If the server doesn't find as many documents as specified, it will send the documents it has found without waiting. Default: 4096.                                                                                                                                                                        |
| **closeWhenNoDocsLeft**             | `boolean`                               | Determines whether the subscription connection closes when no new documents are available.<br><br>`true` – The subscription worker processes all available documents and stops when none remain, at which point the `run` method throws a `SubscriptionClosedException`.<br>Useful for ad-hoc, one-time processing.<br><br>`false` (Default) – The subscription worker remains active, waiting for new documents. |
| **sendBufferSizeInBytes**           | `int`                                   | The size in bytes of the TCP socket buffer used for _sending_ data.<br>Default: 32,768 bytes (32 KiB).                                                                                                                                                                                                                                                                                                            |
| **receiveBufferSizeInBytes**        | `int`                                   | The size in bytes of the TCP socket buffer used for _receiving_ data.<br>Default: 4096 (4 KiB).                                                                                                                                                                                                                                                                                                                   |

{PANEL/}

{PANEL: Run the subscription worker}

After [creating](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker) a subscription worker, the subscription worker is still not processing any documents.  
To start processing, you need to call the `run` method of the [SubscriptionWorker](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker<t>).

The `run` function takes a delegate, which is your client-side code responsible for processing the received document batches.

{CODE:java subscriptionWorkerRunning@ClientApi\DataSubscriptions\DataSubscriptions.java /}

| Parameter            |                                  |                                      |
|----------------------|----------------------------------|--------------------------------------|
| **processDocuments** | `Consumer<SubscriptionBatch<T>>` | Delegate for sync batches processing |

| Return value              |                                                                                                                                                           |
|---------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------|
| `CompletableFuture<Void>` | Task that is alive as long as the subscription worker is processing or tries processing. If the processing is aborted, the future exits with an exception | 

{PANEL/}


{PANEL: SubscriptionBatch&lt;T&gt;}

| Member                   | Type                              | Description                   |
|--------------------------|-----------------------------------|-------------------------------| 
| **items**                | `List<SubscriptionBatch<T>.Item>` | List of items in the batch.   |
| **numberOfItemsInBatch** | `int`                             | Number of items in the batch. |

| Method Signature   | Return value       | Description                                                                          |
|--------------------|--------------------|--------------------------------------------------------------------------------------|
| **openSession()**  | `IDocumentSession` | New document session, that tracks all items and included items of the current batch. |


{NOTE: Subscription worker connectivity}

As long as there is no exception, the worker will continue addressing the same 
server that the first batch was received from.  
If the worker fails to reach that node, it will try to failover to another node 
from the session's topology list.  
The node that the worker succeeded connecting to, will inform the worker which 
node is currently responsible for data subscriptions.  

{NOTE/}


{INFO: SubscriptionBatch&lt;T&gt;.Item}

{NOTE if T is `ObjectNode`, no deserialization will take place /}

| Member               | Type                  | Description                                                                            |
|----------------------|-----------------------|----------------------------------------------------------------------------------------| 
| **result**           | `T`                   | Current batch item.                                                                    |
| **exceptionMessage** | `String`              | Message of the exception thrown during current document processing in the server side. |
| **id**               | `String`              | Current batch item's underlying document ID.                                           |
| **changeVector**     | `String`              | Current batch item's underlying document change vector of the current document.        |
| **rawResult**        | `ObjectNode`          | Current batch item before serialization to `T`.                                        |
| **rawMetadata**      | `ObjectNode`          | Current batch item's underlying document metadata.                                     |
| **metadata**         | `IMetadataDictionary` | Current batch item's underlying metadata values.                                       |

{INFO/}

{PANEL/}

{PANEL: SubscriptionWorker&lt;T&gt;}

{NOTE: Methods}

| Method Signature             | Return Type               | Description                                                                                                                                                                                                             |
|------------------------------|---------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **close()**                  | `void`                    | Aborts subscription worker operation ungracefully by waiting for the task returned by the `run` function to finish running.                                                                                             |
| **run (multiple overloads)** | `CompletableFuture<Void>` | Call `run` to begin the worker's batch processing.<br>Pass the batch processing delegates to this method<br>(see [above](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker)). |

{NOTE/}

{NOTE: Events}

| Event                              | Type\Return type                          | Description                                                                                                                                                                         |
|------------------------------------|-------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **addAfterAcknowledgmentListener** | `Consumer<SubscriptionBatch<T>>` (event)  | Event that is risen after each the server acknowledges batch processing progress.                                                                                                   |
| **onSubscriptionConnectionRetry**  | `Consumer<Exception>` (event)             | Event that is fired when the subscription worker tries to reconnect to the server after a failure. The event receives as a parameter the exception that interrupted the processing. |
| **onClosed**                       | `Consumer<SubscriptionWorker<T>>` (event) | Event that is fired after the subscription worker was disposed.                                                                                                                     |

{NOTE/}

{NOTE: Properties}

| Member               | Type\Return type   | Description                                                           |
|----------------------|--------------------|-----------------------------------------------------------------------| 
| **currentNodeTag**   | `String`           | The node tag of the current RavenDB server handling the subscription. |
| **subscriptionName** | `String`           | The name of the currently processed subscription.                     |

{NOTE/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

