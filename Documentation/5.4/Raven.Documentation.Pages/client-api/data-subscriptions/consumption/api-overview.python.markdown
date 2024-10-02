# Consume Subscriptions API
---

{NOTE: }

* In this page:  
   * [Create the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker)
   * [`SubscriptionWorkerOptions`](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions)  
   * [Run the subscription worker](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker)  
   * [`SubscriptionBatch[_T]`](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionbatch[_t])  
   * [`SubscriptionWorker[_T]`](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker[_t])  

{NOTE/}

---

{PANEL: Create the subscription worker}

Create a subscription worker using `get_subscription_worker` or `get_subscription_worker_by_name`.  

* Use the `get_subscription_worker` method to specify the subscription options while creating the worker.  
* Use the `get_subscription_worker_by_name` method to create the worker using the default options.  

{CODE:python subscriptionWorkerGeneration@ClientApi\DataSubscriptions\DataSubscriptions.py /}

| Parameter                        |                             |                                                                                                                                                      |
|----------------------------------|-----------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------|
| **options**                      | `SubscriptionWorkerOptions` | Options that affect how the worker interacts with the subscription. These options do not alter the definition of the subscription itself.            |
| **object_type** (Optional)       | `Type[_T]`                  | Defines the object type (class) for the items that will be included in the received `SubscriptionBatch` object.                                      |
| **database** (Optional)          | `str`                       | The name of the database where the subscription task resides. If `None`, the default database configured in DocumentStore will be used.              |
| **subscription_name** (Optional) | `str`                       | The subscription's name. Used when the worker is generated without creating a `SubscriptionCreationOptions` instance, relying on the default values. |

| Return value         |                                                                                                                                                     |
|----------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------|
| `SubscriptionWorker` | The subscription worker that has been created.<br>Initially, it is idle and will only start processing documents when the `run` function is called. |

{PANEL/}

{PANEL: `SubscriptionWorkerOptions`}

When creating a worker with `SubscriptionWorkerOptions`, the only mandatory property is `subscription_name`.  
All other parameters are optional and will default to their respective default values if not specified.

| Member                                   | Type                                    | Description                                                                                                                                                                                                                                                                                                                                                                                                       |
|------------------------------------------|-----------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **subscription_name**                    | `str`                                   | The name of the subscription to which the worker will connect.                                                                                                                                                                                                                                                                                                                                                    |
| **time_to_wait_before_connection_retry** | `timedelta`                             | The time to wait before attempting to reconnect after a non-aborting failure during subscription processing. Default: 5 seconds.                                                                                                                                                                                                                                                                                  |
| **ignore_subscriber_errors**             | `bool`                                  | Determines if subscription processing is aborted when the worker's batch-handling code throws an unhandled exception.<br><br>`True` – subscription processing will continue.<br><br>`False` (Default) – subscription processing will be aborted.                                                                                                                                                                  |
| **max_docs_per_batch**                   | `int`                                   | The maximum number of documents that the server will try to retrieve and send to the client in a batch. If the server doesn't find as many documents as specified, it will send the documents it has found without waiting. Default: 4096.                                                                                                                                                                        |
| **close_when_no_docs_left**              | `bool`                                  | Determines whether the subscription connection closes when no new documents are available.<br><br>`True` – The subscription worker processes all available documents and stops when none remain, at which point the `run` method throws a `SubscriptionClosedException`.<br>Useful for ad-hoc, one-time processing.<br><br>`False` (Default) – The subscription worker remains active, waiting for new documents. |
| **send_buffer_size_in_bytes**            | `int`                                   | The size in bytes of the TCP socket buffer used for _sending_ data.<br>Default: 32,768 bytes (32 KiB).                                                                                                                                                                                                                                                                                                            |
| **receive_buffer_size_in_bytes**         | `int`                                   | The size in bytes of the TCP socket buffer used for _receiving_ data.<br>Default: 4096 (4 KiB).                                                                                                                                                                                                                                                                                                                   |
| **strategy**                             | `SubscriptionOpeningStrategy`<br>(enum) | Configures how the server handles connection attempts from workers to a specific subscription task.<br>Learn more in [worker strategies](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-strategies).<br>Default: `OPEN_IF_FREE`.                                                                                                                                      |

{NOTE: }

Learn more about `SubscriptionOpeningStrategy` in [worker strategies](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-strategies).

| `SubscriptionOpeningStrategy`   |                                                   |
|---------------------------------|---------------------------------------------------|
| `OPEN_IF_FREE`                  | Connect if no other worker is connected           |
| `WAIT_FOR_FREE`                 | Wait for currently connected worker to disconnect |
| `TAKE_OVER`                     | Take over the connection                          |
| `CONCURRENT`                    | Connect concurrently                              |

{NOTE/}

{PANEL/}

{PANEL: Run the subscription worker}

After [creating](../../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker) a subscription worker, the subscription worker is still not processing any documents.  
To start processing, you need to call the `run` function of the [SubscriptionWorker](../../../client-api/data-subscriptions/consumption/api-overview#subscriptionworker[_t]).

The `run` function receives the client-side code as a function that will process the received document batches.

{CODE:python subscriptionWorkerRunning@ClientApi\DataSubscriptions\DataSubscriptions.py /}

| Parameter                        |                                            |                                   |
|----------------------------------|--------------------------------------------|-----------------------------------|
| **process_documents** (Optional) | `[Callable[[SubscriptionBatch[_T]], Any]]` | Delegate to sync batch processing |

{PANEL/}


{PANEL: `SubscriptionBatch[_T]`}

| Member                       | Type                               | Description                  |
|------------------------------|------------------------------------|------------------------------| 
| **items**                    | `SubscriptionBatch[_T].Item` array | List of items in the batch   |
| **number_of_items_in_batch** | `int`                              | Number of items in the batch |

{CODE:python number_of_items_in_batch_definition@ClientApi\DataSubscriptions\DataSubscriptions.py /}


{NOTE: Subscription worker connectivity}
As long as there is no exception, the worker will continue addressing the same 
server that the first batch was received from.  
If the worker fails to reach that node, it will try to 
[failover](../../../client-api/configuration/load-balance/overview) to another node 
from the session's topology list.  
The node that the worker succeeds connecting to, will inform the worker which 
node is currently responsible for data subscriptions.  
{NOTE/}

{CODE:python Item_definition@ClientApi\DataSubscriptions\DataSubscriptions.py /}
{CODE:python SubscriptionBatch_definition@ClientApi\DataSubscriptions\DataSubscriptions.py /}

| `SubscriptionBatch[_T].item` Member   | Type                   | Description                                                                           |
|---------------------------------------|------------------------|---------------------------------------------------------------------------------------| 
| **\_result** (Optional)               | `_T_Item`              | Current batch item                                                                    |
| **\_exception_message** (Optional)    | `str`                  | Message of the exception thrown during current document processing in the server side |
| **\_key** (Optional)                  | `str`                  | Current batch item underlying document ID                                             |
| **\_change_vector** (Optional)        | `str`                  | Current batch item underlying document change vector of the current document          |
| **\_projection** (Optional)           | `bool`                 | indicates whether the value id a projection                                           |
| **raw_result** (Optional)             | `Dict`                 | Current batch item before serialization to `T`                                        |
| **raw_metadata** (Optional)           | `Dict`                 | Current batch item underlying document metadata                                       |
| **\_metadata** (Optional)             | `MetadataAsDictionary` | Current batch item underlying metadata values                                         |

{WARNING: }
Usage of `raw_result`, `raw_metadata`, and `_metadata` values outside of the document processing delegate 
is not supported.
{WARNING/}

{PANEL/}

{PANEL: `SubscriptionWorker[_T]`}

---

### Methods:

| Method                                          | Return Type    | Description                                                                                                                                                                                                             |
|-------------------------------------------------|----------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| `close(bool wait_for_subscription_task = True)` | `void`         | Aborts subscription worker operation ungracefully by waiting for the task returned by the `run` function to finish running.                                                                                             |
| `run`                                           | `Future[None]` | Call `run` to begin the worker's batch processing.<br>Pass the batch processing delegates to this method<br>(see [above](../../../client-api/data-subscriptions/consumption/api-overview#run-the-subscription-worker)). |

---

### Events:

| Event                     | Type\Return type                          | Description                                                                      |
|---------------------------|-------------------------------------------|----------------------------------------------------------------------------------| 
| **after\_acknowledgment** | `Callable[[SubscriptionBatch[_T]], None]` | Event invoked after each time the server acknowledges batch processing progress. |

| `after_acknowledgment` Parameters  |                         |                                          |
|------------------------------------|-------------------------|------------------------------------------|
| **batch**                          | `SubscriptionBatch[_T]` | The batch process which was acknowledged |

| Return value   |                                                              |
|----------------|--------------------------------------------------------------|
| `Future[None]` | The worker waits for the task to finish the event processing |

### Properties:

| Member                | Type    | Description                                                           |
|-----------------------|---------|-----------------------------------------------------------------------| 
| **current_node_tag**  | `str`   | The node tag of the current RavenDB server handling the subscription. |
| **subscription_name** | `str`   | The name of the currently processed subscription.                     |

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
