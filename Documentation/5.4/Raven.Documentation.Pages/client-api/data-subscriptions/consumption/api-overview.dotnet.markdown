# Consume Subscriptions API 
---

{NOTE: }

* In this page:  
   * [Create the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker)  
   * [SubscriptionWorkerOptions](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions)  
   * [Run the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker)  
   * [SubscriptionBatch&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch<t>)  
   * [SubscriptionBatch&lt;T&gt;.Item](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch<t>.item)  
   * [SubscriptionWorker&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker<t>)  

{NOTE/}

---

{PANEL: Create the subscription worker}

A subscription worker can be created using the following `GetSubscriptionWorker` methods available through the `Subscriptions` property of the `DocumentStore`.

Note: Simply creating the worker is insufficient;  
after creating the worker, you need to [run the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker) to initiate document processing.

{CODE subscriptionWorkerGeneration@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameter            | Type                        | Description                                                                                                                                |
|----------------------|-----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------|
| **subscriptionName** | `string`                    | The name of the subscription to which the worker will connect.                                                                             |
| **options**          | `SubscriptionWorkerOptions` | Options that affect how the worker interacts with the subscription. These options do not alter the definition of the subscription itself.  |
| **database**         | `string`                    | The name of the database where the subscription task resides.<br>If `null`, the default database configured in DocumentStore will be used. |

| Return value         |                                                                                                                                                     |
|----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------|
| `SubscriptionWorker` | The subscription worker that has been created.<br>Initially, it is idle and will only start processing documents when the `Run` function is called. |

{PANEL/}

{PANEL: SubscriptionWorkerOptions}

{CODE worker_options@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

When creating a worker with `SubscriptionWorkerOptions`, the only mandatory property is `SubscriptionName`.  
All other parameters are optional and will default to their respective default values if not specified.

| Member                              | Type                          | Description                                                                                                                                                                                                                                                                                                                                                                                                       |
|-------------------------------------|-------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **SubscriptionName**                | `string`                      | The name of the subscription to which the worker will connect.                                                                                                                                                                                                                                                                                                                                                    |
| **MaxDocsPerBatch**                 | `int`                         | The maximum number of documents that the server will try to retrieve and send to the client in a batch. If the server doesn't find as many documents as specified, it will send the documents it has found without waiting. Default: 4096.                                                                                                                                                                        |
| **SendBufferSizeInBytes**           | `int`                         | The size in bytes of the TCP socket buffer used for _sending_ data.<br>Default: 32,768 bytes (32 KiB).                                                                                                                                                                                                                                                                                                            |
| **ReceiveBufferSizeInBytes**        | `int`                         | The size in bytes of the TCP socket buffer used for _receiving_ data.<br>Default: 4096 (4 KiB).                                                                                                                                                                                                                                                                                                                   |
| **IgnoreSubscriberErrors**          | `bool`                        | Determines if subscription processing is aborted when the worker's batch-handling code throws an unhandled exception.<br><br>`true` – subscription processing will continue.<br><br>`false` (Default) – subscription processing will be aborted.                                                                                                                                                                  |
| **CloseWhenNoDocsLeft**             | `bool`                        | Determines whether the subscription connection closes when no new documents are available.<br><br>`true` – The subscription worker processes all available documents and stops when none remain, at which point the `Run` method throws a `SubscriptionClosedException`.<br>Useful for ad-hoc, one-time processing.<br><br>`false` (Default) – The subscription worker remains active, waiting for new documents. |
| **TimeToWaitBeforeConnectionRetry** | `TimeSpan`                    | The time to wait before attempting to reconnect after a non-aborting failure during subscription processing. Default: 5 seconds.                                                                                                                                                                                                                                                                                  |
| **MaxErroneousPeriod**              | `TimeSpan`                    | The maximum amount of time a subscription connection can remain in an erroneous state before it is terminated. Default: 5 minutes.                                                                                                                                                                                                                                                                                |
| **Strategy**                        | `SubscriptionOpeningStrategy` | This enum configures how the server handles connection attempts from workers to a specific subscription task.<br>Default: `OpenIfFree`.                                                                                                                             |

Learn more about `SubscriptionOpeningStrategy` in [worker strategies](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-strategies).

{CODE strategy_enum@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Run the subscription worker}

After [creating](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker) a subscription worker, the subscription worker is still not processing any documents.  
To start processing, you need to call the `Run` method of the [SubscriptionWorker](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker<t>).  

The `Run` function takes a delegate, which is your client-side code responsible for processing the received document batches.

{CODE subscriptionWorkerRunning@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameter            | Type                               | Description                                                    |
|----------------------|------------------------------------|----------------------------------------------------------------|
| **processDocuments** | `Action<SubscriptionBatch<T>>`     | Delegate for sync batches processing.                          |
| **processDocuments** | `Func<SubscriptionBatch<T>, Task>` | Delegate for async batches processing.                         |
| **ct**               | `CancellationToken`                | Cancellation token used in order to halt the worker operation. |

| Return value  |                                                                                                                                                             |
|---------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Task`        | Task that is alive as long as the subscription worker is processing or tries processing.<br>If the processing is aborted, the task exits with an exception. |

{PANEL/}

{PANEL: SubscriptionBatch&lt;T&gt;}

| Member                   | Type                              | Description                                                                                                                                                            |
|--------------------------|-----------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Items**                | `List<SubscriptionBatch<T>.Item>` | List of items in the batch.<br>See [SubscriptionBatch&lt;T&gt;.Item](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch<t>.item) below. |
| **NumberOfItemsInBatch** | `int`                             | Number of items in the batch.                                                                                                                                          |

| Method Signature       | Return value            | Description                                                                                                       |
|------------------------|-------------------------|-------------------------------------------------------------------------------------------------------------------|
| **OpenSession()**      | `IDocumentSession`      | Open a new document session that tracks all items and their included items within the current batch.              |
| **OpenAsyncSession()** | `IAsyncDocumentSession` | Open a new asynchronous document session that tracks all items and their included items within the current batch. |

{INFO: }

##### Subscription worker connectivity

As long as there is no exception, the worker will continue addressing the same server that the first batch was received from.  
If the worker fails to reach that node, it will try to [failover](../../../client-api/configuration/load-balance/overview) to another node from the session's topology list.  
The node that the worker succeeded connecting to, will inform the worker which node is currently responsible for data subscriptions.  

{INFO/}

{PANEL/}

{PANEL: SubscriptionBatch&lt;T&gt;.Item}

This class represents a single item in a subscription batch results.

{CODE batch_item@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Member               | Type                        | Description                                                                                           |
|----------------------|-----------------------------|-------------------------------------------------------------------------------------------------------| 
| **Result**           | `T`                         | The current batch item.<br>If `T` is `BlittableJsonReaderObject`, no deserialization will take place. |
| **ExceptionMessage** | `string`                    | The exception message thrown during current document processing in the server side.                   |
| **Id**               | `string`                    | The document ID of the underlying document for the current batch item.                                |
| **ChangeVector**     | `string`                    | The change vector of the underlying document for the current batch item.                              |
| **RawResult**        | `BlittableJsonReaderObject` | Current batch item before serialization to `T`.                                                       |
| **RawMetadata**      | `BlittableJsonReaderObject` | Current batch item's underlying document metadata.                                                    |
| **Metadata**         | `IMetadataDictionary`       | Current batch item's underlying metadata values.                                                      |

{WARNING: }
This class should only be used within the subscription's `Run` delegate.  
Using it outside this scope may cause unexpected behavior.
{WARNING/}

{PANEL/}

{PANEL: SubscriptionWorker&lt;T&gt;}

{NOTE: }

##### Methods

| Method Signature                               | Return Type   | Description                                                                                                                                                                                                             |
|------------------------------------------------|---------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **Dispose()**                                  | `void`        | Aborts subscription worker operation ungracefully by waiting for the task returned by the `Run` function to finish running.                                                                                             |
| **DisposeAsync()**                             | `Task`        | Async version of `Dispose()`.                                                                                                                                                                                           |
| **Dispose(bool waitForSubscriptionTask)**      | `void`        | Aborts the subscription worker, but allows deciding whether to wait for the `Run` function task or not.                                                                                                                 |
| **DisposeAsync(bool waitForSubscriptionTask)** | `Task`        | Async version of `DisposeAsync(bool waitForSubscriptionTask)`.                                                                                                                                                          |
| **Run (multiple overloads)**                   | `Task`        | Call `Run` to begin the worker's batch processing.<br>Pass the batch processing delegates to this method<br>(see [above](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker)). |

{NOTE/}

{NOTE: }

##### Events

| Event                             | Event type                      | Description                                                                                                                                                                     |
|-----------------------------------|:--------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **AfterAcknowledgment**           | `AfterAcknowledgmentAction`     | Triggered after each time the server acknowledges the progress of batch processing.                                                                                             |
| **OnSubscriptionConnectionRetry** | `Action<Exception>`             | Triggered when the subscription worker attempts to reconnect to the server after a failure.<br>The event receives as a parameter the exception that interrupted the processing. |
| **OnDisposed**                    | `Action<SubscriptionWorker<T>>` | Triggered after the subscription worker is disposed.                                                                                                                            |

{INFO: }

##### AfterAcknowledgmentAction

| Parameter   |                        |                                          |
|-------------|------------------------|------------------------------------------|
| **batch**   | `SubscriptionBatch<T>` | The batch process which was acknowledged |

| Return value   |                                                                                                         |
|----------------|---------------------------------------------------------------------------------------------------------|
| `Task`         | Task for which the worker will wait for the event processing to be finished (for async functions, etc.) |

{INFO/}

{NOTE/}

{NOTE: }

##### Properties

| Member                        | Type     | Description                                                           |
|-------------------------------|----------|-----------------------------------------------------------------------| 
| **CurrentNodeTag**            | `string` | The node tag of the current RavenDB server handling the subscription. |
| **SubscriptionName**          | `string` | The name of the currently processed subscription.                     |
| **WorkerId**                  | `string` | The worker ID.                                                        |

{NOTE/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
