# Data Subscriptions: Consumption API Overview

---

{NOTE: }

* In this page:  
   * [Subscription worker generation](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-generation)  
   * [`SubscriptionWorkerOptions`](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions)  
   * [Running subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#running-subscription-worker)  
   * [`SubscriptionBatch[_T]`](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch[_t])  
   * [`SubscriptionWorker[_T]`](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker[_t])  

{NOTE/}

---

{PANEL:Subscription worker generation}

Create a subscription worker using `get_subscription_worker` or `get_subscription_worker_by_name`.  

* Use the `get_subscription_worker` method to specify the subscription options while creating the worker.  
* Use the `get_subscription_worker_by_name` method to create the worker using the default options.  

{CODE:python subscriptionWorkerGeneration@ClientApi\DataSubscriptions\DataSubscriptions.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `SubscriptionWorkerOptions` | Subscription worker options (affecting the interaction of the specific worker with the subscription, but not the subscription's definition) |
| **object_type** (Optional) | `Type[_T]` | Defines the object type (class) for the items that will be included in the received `SubscriptionBatch` object. |
| **database** (Optional) | `str` | Name of the database to look for the data subscription. If `None`, the default database configured in DocumentStore will be used. |
| **subscription_name** (Optional) | `str` | The subscription's name. Used when the worker is generated without creating a `SubscriptionCreationOptions` instance, relying on the default values. |

| Return value | |
| ------------- | ----- |
| `SubscriptionWorker` | A created data subscription worker. When returned, the worker is Idle. It will start working only when its `run` function is called. |

{PANEL/}

{PANEL:`SubscriptionWorkerOptions`}

{NOTE The only mandatory parameter for the ceation of `SubscriptionWorkerOptions` is the subscription **name**. /}

| Member | Type | Description |
|--------|:-----|-------------| 
| **subscription_name** | `str` | Returns the subscription name passed to the constructor. This name will be used by the server side to identify the subscription. |
| **time_to_wait_before_connection_retry** | `DateTime` | Time to wait before reconnecting, in the case of non-aborting failure during the subscription processing.<br>Default: 5 seconds. |
| **ignore_subscriber_errors** | `bool` | If `True`, will not abort subscription processing if client code, passed to the `run` function, throws an unhandled exception.<br>Default: `False` |
| **strategy** | `SubscriptionOpeningStrategy`<br>(enum) | Sets the way the server will treat current and/or other clients when they try to connect. See [Workers interplay](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay). <br>Default: `OPEN_IF_FREE`. |
| **max_docs_per_batch** | `int` | Max number of documents the server will try to send in a batch. If the server doesn't find as many documents as specified, it will not wait and send those it did find.<br>Default: 4096. |
| **close_when_no_docs_left** | `bool` | If `True`, an "ad-hoc" operation processes all possible documents, until the server finds no new documents to send. When this happens, the task returned by the `run` function will fail and throw a `SubscriptionClosedException` exception.<br>Default: `False` |
| **send_buffer_size_in_bytes** | `int` | Size, in bytes, of the TCP socket buffer used for _sending_ data.<br>Default: 32,768 (32 KiB) |
| **receive_buffer_size_in_bytes** | `int` | Size, in bytes, of the TCP socket buffer used for _receiving_ data.<br>Default: 32,768 (32 KiB) |

{PANEL/}

{PANEL:Running subscription worker}

After [generating](../../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-generation) 
a subscription worker, the worker doesn't process any documents yet.  
Processing starts when the `run` function is called.  
The `run` function receives the client-side code as a delegate that will process the retrieved batches:
{CODE:python subscriptionWorkerRunning@ClientApi\DataSubscriptions\DataSubscriptions.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **process_documents** (Optional) | `[Callable[[SubscriptionBatch[_T]], Any]]` | Delegate to sync batch processing |

{PANEL/}


{PANEL:`SubscriptionBatch[_T]`}

| Member | Type | Description |
|--------|:-----|-------------| 
| **items** | `SubscriptionBatch[_T].item` array | Batch items list |
| **number_of_items_in_batch** | `int` | Number of items in the batch |

{NOTE:Subscription Worker Connectivity}
As long as there is no exception, the worker will continue addressing the same 
server that the first batch was received from.  
If the worker fails to reach that node, it will try to 
[failover](../../../client-api/configuration/load-balance/overview) to another node 
from the session's topology list.  
The node that the worker succeeds connecting to, will inform the worker which 
node is currently responsible for data subscriptions.  
{NOTE/}

| `SubscriptionBatch[_T].item` Member | Type | Description |
|--------|:-----|-------------| 
| **result** | `T` | Current batch item |
| **exception_message** | `str` | Message of the exception thrown during current document processing in the server side |
| **id** | `str` | Current batch item's underlying document ID |
| **change_vector** | `str` | Current batch item's underlying document change vector of the current document |
| **raw_result** |  | Current batch item before serialization to `T` |
| **raw_metadata** |  | Current batch item's underlying document metadata |
| **metadata** |  | Current batch item's underlying metadata values |

{WARNING: }
Usage of `raw_result`, `raw_metadata`, and `metadata` values outside of the document processing delegate 
is not supported.
{WARNING/}

{PANEL/}

{PANEL:`SubscriptionWorker[_T]`}

---

### Methods:

| Method | Return Type | Description |
|--------|:-----|-------------| 
| `dispose` | `void` | Aborts subscription worker operation ungracefully by waiting for the task returned by the `run` function to finish running. |
| `dispose(bool waitForSubscriptionTask)` | `void` | Aborts the subscription worker, but allows deciding whether to wait for the `run` function task or not. |
| `run` | `Task` | Starts the subscription worker work of processing batches, receiving the batch processing delegates (see [above](../../../client-api/data-subscriptions/consumption/api-overview#running-subscription-worker)). |

---

### Events:

| Event | Type\Return type | Description |
|--------|:-----|-------------| 
| **after\_acknowledgment** | `AfterAcknowledgmentAction` (event) | Event invoked after each time the server acknowledges batch processing progress. |

| `AfterAcknowledgmentAction` Parameters | | |
| ------------- | ------------- | ----- |
| **batch** | `SubscriptionBatch[_T]` | The batch process which was acknowledged |

| Return value | |
| ------------- | ----- |
| `Task` | The worker waits for the task to finish the event processing |

### Properties:

| Member | Type\Return type | Description |
|--------|:-----|-------------| 
| **current_node_tag** | `str` | Returns the node tag of the current processing RavenDB server |
| **subscription_name** | `str` | Returns processed subscription name |

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
