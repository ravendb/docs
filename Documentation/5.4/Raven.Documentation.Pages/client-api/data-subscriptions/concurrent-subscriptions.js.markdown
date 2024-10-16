# Concurrent Subscriptions
---

{NOTE: }

* With **Concurrent Subscriptions**, multiple data subscription workers can connect to the same subscription task simultaneously.

* Each worker is assigned a different batch of documents to process.

* By processing different batches in parallel, multiple workers can significantly accelerate the consumption of the subscription's contents.

* Documents that were assigned to workers whose connection has ended unexpectedly,  
  can be reassigned by the server to available workers. 
  See [connection failure](../../client-api/data-subscriptions/concurrent-subscriptions#connection-failure) below.

* In this page:  
   * [Defining concurrent workers](../../client-api/data-subscriptions/concurrent-subscriptions#defining-concurrent-workers)  
   * [Dropping a connection](../../client-api/data-subscriptions/concurrent-subscriptions#dropping-a-connection)  
   * [Connection failure](../../client-api/data-subscriptions/concurrent-subscriptions#connection-failure)  

{NOTE/}

---

{PANEL: Defining concurrent workers}

Concurrent workers are defined similarly to other workers, except their 
[strategy](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-strategies) 
is set to [Concurrent](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#multiple-workers-per-subscription-strategy).  

* To define a concurrent worker:  
   * Create the worker using [getSubscriptionWorker](../../client-api/data-subscriptions/consumption/api-overview#create-the-subscription-worker).  
   * Pass it a [subscription worker options](../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-options) object.  
   * Set the strategy to `Concurrent`  

* Usage:  
   * Define two concurrent workers  
     {CODE:nodejs concurrent_1@client-api\dataSubscriptions\concurrentSubscriptions.js /}
   * Run both workers  
     {CODE:nodejs concurrent_2@client-api\dataSubscriptions\concurrentSubscriptions.js /}

{PANEL/}

{PANEL: Dropping a connection}

* Use `dropSubscriptionWorker` to **forcefully disconnect**
  the specified worker from the subscription it is connected to.

* Use `dropConnection` to disconnect ALL workers connected to the specified subscription.

{CODE:nodejs concurrent_3@client-api\dataSubscriptions\concurrentSubscriptions.js /}

{CODE:nodejs drop_syntax@client-api\dataSubscriptions\concurrentSubscriptions.js /}

{PANEL/}

{PANEL: Connection failure}

* When a concurrent worker's connection ends unexpectedly, 
  the server may reassign the documents this worker has been processing to any other concurrent worker that is available.  
* A worker that reconnects after a connection failure will be assigned a **new** batch of documents.  
  It is **not** guaranteed that the new batch will contain the same documents this worker was processing before the disconnection.
* As a result, documents may be processed more than once:  
  - first by a worker that disconnected unexpectedly without acknowledging the completion of its assigned documents, 
  - and later by other workers the documents are reassigned to.

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [How to Create a Data Subscription](../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
- [Maintenance Operations](../../client-api/data-subscriptions/advanced-topics/maintenance-operations)
