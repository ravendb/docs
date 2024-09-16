# Data Subscriptions

---

{NOTE: }
Data subscriptions provide a reliable and handy way to perform document processing on the client side.  
The server sends batches of documents to the client.  
The client then processes the batch and will receive the next one only after it acknowledges the batch was processed.  
The server persists the processing progress, allowing you to pause and continue the processing.  

* In this page:  
   * [Data subscription consumption](../../client-api/data-subscriptions/what-are-data-subscriptions#data-subscription-consumption)  
   * [What defines a data subscription](../../client-api/data-subscriptions/what-are-data-subscriptions#what-defines-a-data-subscription)  
   * [Documents processing](../../client-api/data-subscriptions/what-are-data-subscriptions#documents-processing)  
   * [Progress Persistence](../../client-api/data-subscriptions/what-are-data-subscriptions#progress-persistence)  
   * [How the worker communicates with the server](../../client-api/data-subscriptions/what-are-data-subscriptions#how-the-worker-communicates-with-the-server)  
   * [Working with multiple clients](../../client-api/data-subscriptions/what-are-data-subscriptions#working-with-multiple-clients)  
   * [Data subscriptions usage example](../../client-api/data-subscriptions/what-are-data-subscriptions#data-subscriptions-usage-example)  


{NOTE/}

---

{PANEL:Data subscription consumption}

* Data subscriptions are consumed by clients, called **Subscription Workers**.  
* You can determine whether workers would be able to connect a subscription 
  [concurrently, or only one at a time](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay).  
* A worker that connects to a data subscription receives a batch of documents, and gets to process it.  
  Depending on the code that the client provided the worker with, processing can take from seconds to hours.  
  When all documents are processed, the worker informs the server of its progress and the server can send it the next batch.  

{PANEL/}

{PANEL:What defines a data subscription}

Data subscriptions are defined by the server-side definition and by the worker connecting to it:

1. [Subscription Creation Options](../../client-api/data-subscriptions/creation/api-overview#subscriptioncreationoptions): The documents that will be sent to the worker, it's filtering and projection.

2. [Subscription Worker Options](../../client-api/data-subscriptions/consumption/api-overview#subscriptionworkeroptions): Worker batch processing logic, batch size, interaction with other connections.

{PANEL/}

{PANEL:Documents processing}

Documents are sent in batches and progress will be registered only after the whole batch is processed and acknowledged. 
Documents are always sent in Etag order which means that data that has already been processed and acknowledged won't be sent twice, except for the following scenarios:

1. If the document was changed after it was already sent.

2. If data was received but not acknowledged.

3. In case of subscription failover (`Enterprise feature`), when there is a chance that documents will be processed again, because it's not always possible to find the same starting point on a different machine.

{NOTE: }
If the database has Revisions defined, the subscription can be configured to process pairs 
of subsequent document revisions.  
Read more here: [revisions support](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)
{NOTE/}

{PANEL/}

{PANEL:Progress Persistence}

* The processing progress is persisted on the server and therefore the subscription 
  task can be paused and resumed from the last point it was stopped.  
* The persistence mechanism also ensures that no documents are missed even in the 
  presence of failure, whether it's client-side related, communication, or any other disaster.  
* Subscriptions progress is stored in the cluster level, in the `Enterprise edition`.  
  In the case of a node failure, the processing can be automatically failed over to another node.  
* The usage of **Change Vectors** allows us to continue from a point that is close to 
  the last point reached before failure rather than starting the process from scratch.  
{PANEL/}

{PANEL:How the worker communicates with the server}

A worker communicates with the data subscription using a custom protocol on top of a long-lived TCP connection. Each successful batch processing consists of these stages:

1. The server sends documents in a batch.

2. Worker sends acknowledgment message after it finishes processing the batch.

3. The server returns the client a notification that the acknowledgment persistence is done and it is ready to send the next batch.

{INFO: Failover}
When the responsible node handling the subscription is down, the subscription task can be manually reassigned to another node in the cluster.  
With the Enterprise license, the cluster will automatically reassign the work to another node.
{INFO/}

* The status of the TCP connection is also used to determine the "state" of the worker process.  
  If the subscription and its workers implement a 
  [One Worker Per Subscription](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay) 
  strategy, as long as the connection is alive the server will not allow 
  other clients to consume the subscription. 
* The TCP connection is kept alive and monitored using "heartbeat" messages.  
  If the connection is found nonfunctional, the current batch progress will be restarted.

See the sequence diagram below that summarizes the lifespan of a subscription connection.

![Subscription document processing](images/SubscriptionsDocumentProcessing.png)

{PANEL/}

{PANEL:Working with multiple clients}

You can use a **Subscription Worker Strategy** to determine whether multiple 
workers of the same subscription can connect to it one by one, or **concurrently**.  

* **One Worker Per Subscription Strategies**  
  The one-worker-per-subscription strategies allow workers of the same subscription 
  to connect to it **one worker at a time**, with different strategies to support various 
  inter-worker scenarios.  
   * One worker is allowed to take the place of another in the processing of a subscription.  
     Thanks to subscriptions persistence, the worker will be able to continue the work 
     starting at the point its predecessor got to.  
   * You can also configure a worker to wait for an existing connection to fail and take 
     its place, or to force an existing connection to close.  
   * Read more about these strategies [here](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#one-worker-per-subscription-strategies).  

* **Concurrent Subscription Strategy**  
  Using the concurrent subscription strategy, multiple workers of the same subscription can 
  connect to it simultaneously and divide the documents processing load between them to speed it up.  
   * Batch processing is divided between the multiple workers.  
   * Connection failure is handled by assigning batches of failing workers to 
     active available workers.  
   * Read more about this strategy [here](../../client-api/data-subscriptions/concurrent-subscriptions).  

{PANEL/}

{PANEL:Data subscriptions usage example}

Data subscriptions are accessible by a document store.  
Here's an example of creating and using a data subscription:

{CODE subscriptions_example@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [How to Create a Data Subscription](../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
- [Maintenance Operations](../../client-api/data-subscriptions/advanced-topics/maintenance-operations)
