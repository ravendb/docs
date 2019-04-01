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

Subscription worker generation is accessible through the `DocumentStore`'s `subscriptions` property, of type `DocumentSubscriptions`:
{CODE:nodejs subscriptionWorkerGeneration@client-api\dataSubscriptions\dataSubscriptions.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **subscriptionName** | string | The subscription's name. This parameter appears in more simple overloads allowing to start processing without creating a `SubscriptionCreationOptions` instance, relying on the default values |
| **options** | `SubscriptionWorkerOptions` | Contains subscription worker, affecting the interaction of the specific worker with the subscription, but does not affect the subscription's definition |
| *subscriptionName* | string | Returns the subscription name passed to the constructor. This name will be used by the server side to identify the subscription in question. |
| *timeToWaitBeforeConnectionRetry* | number | Time to wait before reconnecting, in the case of non-aborting failure during the subscription processing. Default: 5 seconds. |
| *ignoreSubscriberErrors* | boolean | If true, will not abort subscription processing if client code, passed to the `run` function, throws an unhandled exception. Default: false. |
| *strategy* | `SubscriptionOpeningStrategy`(enum) | Sets the way the server will treat current and/or other clients when they will try to connect. See [Workers interplay](how-to-consume-data-subscription#workers-interplay). Default: `OPEN_IF_FREE`. |
| *maxDocsPerBatch* | number | Maximum amount of documents that the server will try sending in a batch. If the server will not find "enough" documents, it won't wait and send the amount it found. Default: 4096. |
| *closeWhenNoDocsLeft* | boolean | If true, it performs an "ad-hoc" operation that processes all possible documents, until the server can't find any new documents to send. An error of type `SubscriptionClosedException` is going to be emitted. Default: false. |
| **database** | string | Name of the database to look for the data subscription. If `null`, the default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| `SubscriptionWorker` | A created data subscription worker.  |


{PANEL:Running subscription worker}

After obtaining a subscription worker instance, it's going to connect asynchronously when the `"batch"` event listener is registered. Once connected it's going to emit events allowing the user to process batches and handle errors.

{CODE:nodejs subscriptionWorkerRunning@client-api\dataSubscriptions\dataSubscriptions.js /}

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

{INFO/}

{PANEL/}

{PANEL:SubscriptionWorker}

{NOTE:Methods}

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **dispose()** | `void` | Aborts subscription worker operation. |

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

