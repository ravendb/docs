# Data Subscriptions: Consumption API Overview

---

{NOTE: }

In this page:  

[Subscription worker generation](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-generation)  
[SubscriptionWorkerOptions](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions)  
[Running subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#running-subscription-worker)  
[SubscriptionBatch&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch<t>)  
[SubscriptionWorker&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker<t>)  

{NOTE/}

---

{PANEL:Subscription worker generation}

Subscription worker generation is accessible through the `DocumentStore`'s `Subscriptions` Property, of type `DocumentSubscriptions`:
{CODE subscriptionWorkerGeneration@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **subscriptionName** | `string` | The subscription's name. This parameter appears in more simple overloads allowing to start processing without creating a `SubscriptionCreationOptions` instance, relying on the default values |
| **options** | `SubscriptionWorkerOptions` | Contains subscription worker, affecting the interaction of the specific worker with the subscription, but does not affect the subscription's definition |
| **database** | `string` | Name of the database to look for the data subscription. If `null`, the default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| `SubscriptionWorker` | A created data subscription worker. When returned, the worker is Idle and it will start working only when the `Run` function is called. |


{PANEL/}

{PANEL:SubscriptionWorkerOptions}

{NOTE The only mandatory parameter for SubscriptionWorkerOptions creation is the subscription's name. /}

| Member | Type | Description |
|--------|:-----|-------------| 
| **SubscriptionName** | `string` | Returns the subscription name passed to the constructor. This name will be used by the server side to identify the subscription in question. |
| **TimeToWaitBeforeConnectionRetry** | `TimeSpan` | Time to wait before reconnecting, in the case of non-aborting failure during the subscription processing. Default: 5 seconds. |
| **IgnoreSubscriberErrors** | `bool` | If true, will not abort subscription processing if client code, passed to the `Run` function, throws an unhandled exception. Default: false. |
| **Strategy** | `SubscriptionOpeningStrategy`<br>(enum) | Sets the way the server will treat current and/or other clients when they will try to connect. See [Workers interplay](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay). Default: `OpenIfFree`. |
| **MaxDocsPerBatch** | `int` | Maximum amount of documents that the server will try sending in a batch. If the server will not find "enough" documents, it won't wait and send the amount it found. Default: 4096. |
| **CloseWhenNoDocsLeft** | `bool` | If true, it performs an "ad-hoc" operation that processes all possible documents, until the server can't find any new documents to send. At that moment, the task returned by the `Run` function will fail and throw a `SubscriptionClosedException` exception. Default: false. |
| **SendBufferSizeInBytes** | `int` | The size in bytes of the TCP socket buffer used for _sending_ data. <br>Default: 32,768 (32 KiB) |
| **ReceiveBufferSizeInBytes** | `int` | The size in bytes of the TCP socket buffer used for _receiving_ data. <br>Default: 32,768 (32 KiB) |

{PANEL/}

{PANEL:Running subscription worker}

After [generating](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-generation) a subscription worker, the subscription worker is still not processing any documents. SubscriptionWorker's `Run` function allows you to start processing worker operations.  
The `Run` function receives the client-side code as a delegate that will process the received batches:

{CODE subscriptionWorkerRunning@ClientApi\DataSubscriptions\DataSubscriptions.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **processDocuments** | `Action<SubscriptionBatch<T>>` | Delegate for sync batches processing |
| **processDocuments** | `Func<SubscriptionBatch<T>, Task>` | Delegate for async batches processing |
| **ct** | `CancellationToken` | Cancellation token used in order to halt the worker operation |

| Return value | |
| ------------- | ----- |
| `Task` | Task that is alive as long as the subscription worker is processing or tries processing. If the processing is aborted, the task exits with an exception | 

{PANEL/}


{PANEL:SubscriptionBatch&lt;T&gt;}

| Member | Type | Description |
|--------|:-----|-------------| 
| **Items** | `List<SubscriptionBatch&lt;T&gt;.Item>` | Batch's items list. |
| **NumberOfItemsInBatch** | `int` | Amount of items in the batch. |

| Method Signature | Return value | Description |
|--------|:-------------|-------------|
| **OpenSession()** | `IDocumentSession` | New document session, that tracks all items and included items of the current batch. |
| **OpenAsyncSession()** | `IDocumentSession` | New asynchronous document session, that tracks all items and included items of the current batch. |


{NOTE:Subscription Session characteristics}
Session will be created by the same document store that created the worker, therefore will receive the same configurations as any other session created by the store.  
However, in order to maintain consistency, the session will address the same server that the batch was received from.  
It won't try to failover to another server. It might also fail if the subscription worker changes the node it communicates with.  
Such event could happen if the subscription worker starts again to address its original node after a fallback occurrence.  
If such failure occurs, the subscription processing will be stopped, and will have to be restarted, as shown [here](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)  
{NOTE/}



{INFO:SubscriptionBatch&lt;T&gt;.Item}

{NOTE if T is `BlittableJsonReaderObject`, no deserialization will take place /}

| Member | Type | Description |
|--------|:-----|-------------| 
| **Result** | `T` | Current batch item. |
| **ExceptionMessage** | `string` | Message of the exception thrown during current document processing in the server side. |
| **Id** | `string` | Current batch item's underlying document ID. |
| **ChangeVector** | `string` | Current batch item's underlying document change vector of the current document. |
| **RawResult** | `BlittableJsonReaderObject` | Current batch item before serialization to `T`. |
| **RawMetadata** | `BlittableJsonReaderObject` | Current batch item's underlying document metadata. |
| **Metadata** | `IMetadataDictionary` | Current batch item's underlying metadata values. |


{WARNING Usage of `RawResult`, `RawMetadata`, and `Metadata` values outside of the document processing delegate are not supported /}


{INFO/}

{PANEL/}

{PANEL:SubscriptionWorker&lt;T&gt;}

{NOTE:Methods}

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **Dispose()** | `void` | Aborts subscription worker operation ungracefully by waiting for the task returned by the `Run` function to finish running. |
| **DisposeAsync()** | `Task` | Async version of `Dispose()`. |
| **Dispose(bool waitForSubscriptionTask)** | `void` | Aborts the subscription worker, but allows deciding whether to wait for the `Run` function task or not. |
| **DisposeAsync(bool waitForSubscriptionTask)** | `void` | Async version of `DisposeAsync(bool waitForSubscriptionTask)`. |
| **Run (multiple overloads)** | `Task` | Starts the subscription worker work of processing batches, receiving the batch processing delegates (see [above](../../../client-api/data-subscriptions/consumption/api-overview#running-subscription-worker)). |

{NOTE/}

{NOTE:Events}

| Event | Type\Return type | Description |
|--------|:-----|-------------| 
| **AfterAcknowledgment** | `AfterAcknowledgmentAction` (event) | Event that is risen after each time the server acknowledges batch processing progress. |
| **OnSubscriptionConnectionRetry** | `Action<Exception>` (event) | Event that is fired when the subscription worker tries to reconnect to the server after a failure. The event receives as a parameter the exception that interrupted the processing. |
| **OnDisposed** | `Action<SubscriptionWorker<T>>` (event) | Event that is fired after the subscription worker was disposed. |

{INFO:AfterAcknowledgmentAction}

| Parameters | | |
| ------------- | ------------- | ----- |
| **batch** | `SubscriptionBatch&lt;T&gt;` | The batch process which was acknowledged |

| Return value | |
| ------------- | ----- |
| `Task` | Task for which the worker will wait for the event processing to be finished (for async functions, etc.) | 

{INFO/}

{NOTE/}



{NOTE:Properties}

| Member | Type\Return type | Description |
|--------|:-----|-------------| 
| **CurrentNodeTag** | `string` | Returns current processing RavenDB server's node tag. |
| **SubscriptionName** | `string` | Returns processed subscription's name. |

{NOTE/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
