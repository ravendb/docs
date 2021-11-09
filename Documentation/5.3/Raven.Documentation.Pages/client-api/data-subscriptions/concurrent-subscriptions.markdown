# Data Subscriptions: Concurrent Subscriptions

---

{NOTE: }

* With **Concurrent Subscriptions**, multiple data subscription workers 
  are able to connect a common subscription task simultaneously.  
* Different concurrent workers are given different document batches 
  to process.  
* Documents will never be processed concurrently by multiple workers.  
* With multiple concurrent workers that process different document 
  batches in parallel, the consumption of a subscription's contents 
  can be greatly accelerated.  
* Documents that were assigned to workers whose connection has ended 
  unexpectedly, can be [reassigned](../../client-api/data-subscriptions/concurrent-subscriptions#connection-failure) 
  by the server to available workers.  

* In this page:  
   * [Defining a Concurrent Workers](../../client-api/data-subscriptions/concurrent-subscriptions#defining-concurrent-workers)  
   * [Dropping a Connection](../../client-api/data-subscriptions/concurrent-subscriptions#dropping-a-connection)  
   * [Connection Failure](../../client-api/data-subscriptions/concurrent-subscriptions#connection-failure)  

{NOTE/}

---

{PANEL: Defining Concurrent Workers}

Subscription workers are defined similarly to other workers, except for their 
[strategy](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay) 
that is set as [SubscriptionOpeningStrategy.Concurrent](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#concurrent-subscription-strategy).  

* To define a concurrent worker:  
   * Create the worker using [GetSubscriptionWorker](../../client-api/data-subscriptions/consumption/api-overview#subscription-worker-generation).  
   * Pass it a [SubscriptionWorkerOptions](../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions) instance.  
   * Set the strategy to `SubscriptionOpeningStrategy.Concurrent`.  

* Usage:  
   * Define two concurrent workers  
     {CODE conSub_defineWorkers@ClientApi\DataSubscriptions\ConcurrentSubscriptions.cs /}
   * Run both workers  
     {CODE conSub_runWorkers@ClientApi\DataSubscriptions\ConcurrentSubscriptions.cs /}

{PANEL/}


{PANEL: Dropping a Connection}

* Use `Subscriptions.DropSubscriptionWorker` to **forcefully disconnect** 
  any concurrent worker from the subscription it is connected to.  
    {CODE-BLOCK: csharp}
    public void DropSubscriptionWorker<T>(SubscriptionWorker<T> worker, string database = null)
    {CODE-BLOCK/}

* Usage:  
  {CODE conSub_dropWorker@ClientApi\DataSubscriptions\ConcurrentSubscriptions.cs /}


{PANEL/}

{PANEL: Connection Failure}

* When a concurrent worker's connection ends unexpectedly, the 
  server may assign the documents this worker has been processing 
  to any other concurrent worker that is available.  
* A worker that reconnects after a connection failure will be assigned 
  a **new** batch of documents.  
  It is **not** guaranteed that the new batch will contain the same 
  documents this worker had been processing before disconnecting.  
* It may therefore happen that documents will be processed more 
  than once:  
   - First by a worker that disconnected unexpectedly, without acknowledging the procession of its documents, 
   - and then by workers the documents have been reassigned to.  

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [How to Create a Data Subscription](../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
- [Maintenance Operations](../../client-api/data-subscriptions/advanced-topics/maintenance-operations)
