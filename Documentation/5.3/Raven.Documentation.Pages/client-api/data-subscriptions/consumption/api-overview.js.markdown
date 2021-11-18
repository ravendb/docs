# Data Subscriptions: Consumption API Overview

---

{NOTE: }

In this page:  

[Subscription worker generation](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-generation)  
[SubscriptionWorkerOptions](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions)  
[SubscriptionBatch](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch)  
[SubscriptionWorker&lt;T&gt;](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker<t>)  

{NOTE/}

---

{PANEL:Subscription worker generation}

Subscription worker generation is accessible through the `DocumentStore`'s `subscriptions` property, of type `DocumentSubscriptions`:
{CODE:nodejs subscriptionWorkerGeneration@client-api\dataSubscriptions\dataSubscriptions.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **subscriptionName** | `string` | The subscription's name. This parameter appears in more simple overloads allowing to start processing without creating a `SubscriptionCreationOptions` instance, relying on the default values |
| **options** | `SubscriptionWorkerOptions<T>` | Contains subscription worker, affecting the interaction of the specific worker with the subscription, but does not affect the subscription's definition |
| **database** | `string` | Name of the database to look for the data subscription. If `null`, the default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| `SubscriptionWorkerOptions<T>` | A created data subscription worker.  |


{PANEL/}

{PANEL:SubscriptionWorkerOptions}

{NOTE The only mandatory parameter for SubscriptionWorkerOptions creation is the subscription's name. /}

| Member | Type | Description |
|--------|:-----|-------------| 
| **subscriptionName** | `string` | Returns the subscription name passed to the constructor. This name will be used by the server side to identify the subscription in question. |
| **timeToWaitBeforeConnectionRetry** | `number` | Time to wait before reconnecting, in the case of non-aborting failure during the subscription processing. Default: 5 seconds. |
| **ignoreSubscriberErrors** | `boolean` | If true, will not abort subscription processing if client code, passed to the `Run` function, throws an unhandled exception. Default: false. |
| **strategy** | `SubscriptionOpeningStrategy`<br>(enum) | Sets the way the server will treat current and/or other clients when they will try to connect. See [Workers interplay](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay). Default: `OpenIfFree`. |
| **maxDocsPerBatch** | `number` | Maximum amount of documents that the server will try sending in a batch. If the server will not find "enough" documents, it won't wait and send the amount it found. Default: 4096. |
| **closeWhenNoDocsLeft** | `boolean` | If true, it performs an "ad-hoc" operation that processes all possible documents, until the server can't find any new documents to send. At that moment, the task returned by the `Run` function will fail and throw a `SubscriptionClosedException` exception. Default: false. |
| **sendBufferSizeInBytes** | `number` | The size in bytes of the TCP socket buffer used for _sending_ data. <br>Default: 32,768 (32 KiB) |
| **ReceiveBufferSizeInBytes** | `number` | The size in bytes of the TCP socket buffer used for _receiving_ data. <br>Default: 32,768 (32 KiB) |

{PANEL/}

{PANEL:SubscriptionBatch}

| Member | Type | Description |
|--------|:-----|-------------| 
| **items** | `SubscriptionBatchItem` | Batch's items list. |
| **numberOfItemsInBatch** | `number` | Amount of items in the batch. |

{INFO:SubscriptionBatchItem}

| Member | Type | Description |
|--------|:-----|-------------| 
| **result** | object | Current batch item. |
| **exceptionMessage** | string | Message of the exception thrown during current document processing in the server side. |
| **id** | string | Current batch item's underlying document ID. |
| **changeVector** | string | Current batch item's underlying document change vector of the current document. |
| **rawResult** | object | Current batch item - no types revived. |
| **rawMetadata** | object | Current batch item's underlying document metadata. |
| **metadata** | object | Current batch item's underlying metadata values. |

{WARNING Usage of `RawResult`, `RawMetadata`, and `Metadata` values outside of the document processing delegate are not supported /}


{INFO/}

{PANEL/}

{PANEL:SubscriptionWorker}

{NOTE:Methods}

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **dispose()** | `void` | Aborts subscription worker operation. |
| **disposeAsync()** | `Promise<void>` | Async version of `Dispose()`. |
| **dispose(boolean waitForSubscriptionTask)** | `void` | Aborts the subscription worker, but allows deciding whether to wait for the `Run` function task or not. |
| **disposeAsync(boolean waitForSubscriptionTask)** | `void` | Async version of `DisposeAsync(boolean waitForSubscriptionTask)`. |

{NOTE/}

{NOTE:Events}

| Events | Listener signature | |
| ------------- | ------------- | ----- |
| **"batch"** | `(batch, callback) => void` | Client-side batch processing. Once processing is done, `callback` *must  be called* in order to continue batches' emission. |
| **"error"** | `(error) => void` | Emitted on subscription errors |
| **"connectionRetry"** | `(error) => void` | Emitted on connection retry. |
| **"end"** | `(error) => void` | Emitted when subscription is finished. No more batches are going to be emitted. |

{NOTE/}

{NOTE:Properties}

| Member | Type\Return type | Description |
|--------|:-----|-------------| 
| **currentNodeTag** | string | Returns current processing RavenDB server's node tag. |
| **subscriptionName** | string | Returns processed subscription's name. |

{NOTE/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

