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

Subscription worker generation is accessible through the `DocumentStore`'s `subscriptions()` method, of type `DocumentSubscriptions`:
{CODE:java subscriptionWorkerGeneration@ClientApi\DataSubscriptions\DataSubscriptions.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **subscriptionName** | `String` | The subscription's name. This parameter appears in more simple overloads allowing to start processing without creating a `SubscriptionCreationOptions` instance, relying on the default values |
| **options** | `SubscriptionWorkerOptions` | Contains subscription worker, affecting the interaction of the specific worker with the subscription, but does not affect the subscription's definition |
| **database** | `String` | Name of the database to look for the data subscription. If `null`, the default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| `SubscriptionWorker` | A created data subscription worker. When returned, the worker is Idle and it will start working only when the `run` function is called. |


{PANEL/}

{PANEL:SubscriptionWorkerOptions}

{NOTE The only mandatory parameter for SubscriptionWorkerOptions creation is the subscription's name. /}

| Member | Type | Description |
|--------|:-----|-------------| 
| **subscriptionName** | `String` | Returns the subscription name passed to the constructor. This name will be used by the server side to identify the subscription in question. |
| **timeToWaitBeforeConnectionRetry** | `Duration` | Time to wait before reconnecting, in the case of non-aborting failure during the subscription processing. Default: 5 seconds. |
| **ignoreSubscriberErrors** | `boolean` | If true, will not abort subscription processing if client code, passed to the `run` function, throws an unhandled exception. Default: false. |
| **strategy** | `SubscriptionOpeningStrategy`<br>(enum) | Sets the way the server will treat current and/or other clients when they will try to connect. See [Workers interplay](how-to-consume-data-subscription#workers-interplay). Default: `OPEN_IF_FREE`. |
| **maxDocsPerBatch** | `int` | Maximum amount of documents that the server will try sending in a batch. If the server will not find "enough" documents, it won't wait and send the amount it found. Default: 4096. |
| **closeWhenNoDocsLeft** | `boolean` | If true, it performs an "ad-hoc" operation that processes all possible documents, until the server can't find any new documents to send. At that moment, the task returned by the `run` function will fail and throw a `SubscriptionClosedException` exception. Default: false. |
| **sendBufferSizeInBytes** | `int` | The size in bytes of the TCP socket buffer used for _sending_ data. <br>Default: 32,768 (32 KiB) |
| **receiveBufferSizeInBytes** | `int` | The size in bytes of the TCP socket buffer used for _receiving_ data. <br>Default: 32,768 (32 KiB) |

{PANEL/}

{PANEL:Running subscription worker}

After [generating](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-generation) a subscription worker, the subscription worker is still not processing any documents. SubscriptionWorker's `run` function allows you to start processing worker operations.  
The `run` function receives the client-side code as a delegate that will process the received batches:

{CODE:java subscriptionWorkerRunning@ClientApi\DataSubscriptions\DataSubscriptions.java /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **processDocuments** | `Consumer<SubscriptionBatch<T>>` | Delegate for sync batches processing |
| **processDocuments** | `Consumer<SubscriptionBatch<T>, Task>` | Delegate for async batches processing |
| **ct** | `CancellationToken` | Cancellation token used in order to halt the worker operation |

| Return value | |
| ------------- | ----- |
| `Task` | Task that is alive as long as the subscription worker is processing or tries processing. If the processing is aborted, the task exits with an exception | 

{PANEL/}


{PANEL:SubscriptionBatch<T>}

| Member | Type | Description |
|--------|:-----|-------------| 
| **items** | `List<SubscriptionBatch<T>.Item>` | Batch's items list. |
| **numberOfItemsInBatch** | `int` | Amount of items in the batch. |

| Method Signature | Return value | Description |
|--------|:-------------|-------------|
| **openSession()** | `IDocumentSession` | New document session, that tracks all items and included items of the current batch. |
| **openAsyncSession()** | `IDocumentSession` | New asynchronous document session, that tracks all items and included items of the current batch. |


{NOTE:Subscription Session characteristics}
Session will be created by the same document store that created the worker, therefore will receive the same configurations as any other session created by the store.  
However, in order to maintain consistency, the session will address the same server that the batch was received from.  
It won't try to fail over to another server. It might also fail if the subscription worker changes the node it communicates with.  
Such event could happen if the subscription worker starts again to address its original node after a fallback occurrence.  
If such failure occurs, the subscription processing will be stopped, and will have to be restarted, as shown [here](../../../client-api/data-subscriptions/consumption/examples#client-with-full-exception-handling-and-processing-retries)  
{NOTE/}


{INFO:SubscriptionBatch<T>.Item}

{NOTE if T is `ObjectNode`, no deserialization will take place /}

| Member | Type | Description |
|--------|:-----|-------------| 
| **result** | `T` | Current batch item. |
| **exceptionMessage** | `String` | Message of the exception thrown during current document processing in the server side. |
| **id** | `String` | Current batch item's underlying document ID. |
| **changeVector** | `String` | Current batch item's underlying document change vector of the current document. |
| **rawResult** | `ObjectNode` | Current batch item before serialization to `T`. |
| **rawMetadata** | `ObjectNode` | Current batch item's underlying document metadata. |
| **metadata** | `IMetadataDictionary` | Current batch item's underlying metadata values. |


{WARNING Usage of `rawResult`, `rawMetadata`, and `metadata` values outside of the document processing delegate are not supported /}


{INFO/}

{PANEL/}

{PANEL:SubscriptionWorker<T>}

{NOTE:Methods}

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **dispose()** | `void` | Aborts subscription worker operation ungracefully by waiting for the task returned by the `run` function to finish running. |
| **disposeAsync()** | `Task` | Async version of `dispose()`. |
| **dispose(boolean waitForSubscriptionTask)** | `void` | Aborts the subscription worker, but allows deciding whether to wait for the `run` function task or not. |
| **disposeAsync(boolean waitForSubscriptionTask)** | `void` | Async version of `disposeAsync(bool waitForSubscriptionTask)`. |
| **run (multiple overloads)** | `Task` | Starts the subscription worker work of processing batches, receiving the batch processing delegates (see [above](../../../client-api/data-subscriptions/consumption/api-overview#running-subscription-worker)). |

{NOTE/}

{NOTE:Events}

| Event | Type\Return type | Description |
|--------|:-----|-------------| 
| **addAfterAcknowledgmentListener** | `Consumer<SubscriptionBatch<T>>` (event) | Event that is risen after each time the server acknowledges batch processing progress. |
| **onSubscriptionConnectionRetry** | `Consumer<Exception>` (event) | Event that is fired when the subscription worker tries to reconnect to the server after a failure. The event receives as a parameter the exception that interrupted the processing. |
| **onDisposed** | `Consumer<SubscriptionWorker<T>>` (event) | Event that is fired after the subscription worker was disposed. |

{INFO:AfterAcknowledgmentAction}

| Parameters | | |
| ------------- | ------------- | ----- |
| **batch** | `SubscriptionBatch<T>` | The batch process which was acknowledged |

| Return value | |
| ------------- | ----- |
| `Task` | Task for which the worker will wait for the event processing to be finished (for async functions, etc.) | 

{INFO/}

{NOTE/}



{NOTE:Properties}

| Member | Type\Return type | Description |
|--------|:-----|-------------| 
| **currentNodeTag** | `String` | Returns current processing RavenDB server's node tag. |
| **subscriptionName** | `String` | Returns processed subscription's name. |

{NOTE/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

