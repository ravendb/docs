# Data Subscriptions

---

{NOTE: }
Data subscriptions provide a reliable and handy way to perform document processing on the client side.  
The server sends batches of documents to the client.  
The client then processes the batch and will receive the next one only after it acknowledges the batch was processed.  
The server persists the processing progress, allowing to pause and continue the processing.  

In this page:  
[Data subscription consumption](../../client-api/data-subscriptions/what-are-data-subscriptions#data-subscription-consumption)  
[What defines a data subscription](../../client-api/data-subscriptions/what-are-data-subscriptions#what-defines-a-data-subscription)  
[Documents processing](../../client-api/data-subscriptions/what-are-data-subscriptions#documents-processing)  
[Progress Persistence](../../client-api/data-subscriptions/what-are-data-subscriptions#progress-persistence)  
[How the worker communicates with the server](../../client-api/data-subscriptions/what-are-data-subscriptions#how-the-worker-communicates-with-the-server)  
[Working with multiple clients](../../client-api/data-subscriptions/what-are-data-subscriptions#working-with-multiple-clients)  
[Data subscriptions usage example](../../client-api/data-subscriptions/what-are-data-subscriptions#data-subscriptions-usage-example)  


{NOTE/}

---

{PANEL:Data subscription consumption}

Data subscriptions are consumed by clients, called subscription workers. In any given moment, only one worker can be connected to a data subscription. 
A worker connected to a data subscription receives a batch of documents and gets to process it. 
When it's done, depending on the code that the client gave the worker, it can take from seconds to hours. It informs the server about the progress, and the server is ready to send the next batch.

{PANEL/}

{PANEL:What defines a data subscription}

Data subscriptions are defined by the server side definition and by the worker connecting to it:

1. [Subscription Creation Options](../../client-api/data-subscriptions/subscription-creation/api-overview#subscriptioncreationoptions): The documents that will be received, it's filtering and projection.

2. [Subscription Worker Options](../../client-api/data-subscriptions/subscription-consumption/api-overview#subscriptionworkeroptions): Worker batch processing logic, batch size, interaction with other connections.

{PANEL/}

{PANEL:Documents processing}

Documents are sent in batches and progress will be registered only after the whole batch is processed and acknowledged. 
Documents are always sent in Etag order which means that data that already been processed and acknowledged won't be sent twice, except for the following scenarios:

1. If the document was changed after it was already sent.

2. If data was received but not acknowledged.

3. In case of subscription failover (`Enterprise feature`), when there is a chance that documents will be processed again, because it's not always possible to find the same starting point on a different machine.

A subscription worker will retry processing documents from the last acknowledged and processed document (by tracking its Change Vector).

* If the database has Revisions defined, the subscription can be configured to process pairs of subsequent document revisions. Read more in [revisions support](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning).

{PANEL/}

{PANEL:Progress Persistence}

Processing progress is persisted and therefore it can be paused and resumed from the last point it was stopped. 
The persistence mechanism also ensures that no documents are missed even in the presence of failure, whether it's client side related, communication, or any other disaster. 
Subscriptions progress is stored in the cluster level, in `Enterprise edition`. In the case of node failure, the processing can be automatically failed over to another node.
The usage of Change Vectors allows us to continue from a point that is close to the last point reached before failure, rather than starting the process from scratch.
{PANEL/}

{PANEL:How the worker communicates with the server}

A worker communicates with the data subscription using a custom protocol, on top of a long-lived TCP connection. Each successful batch processing consists of these stages:

1. Server sends documents a batch.

2. Worker sends acknowledgment message after it finishes processing the batch.

3. Server returns the client a notification that the acknowledgment persistence is done and it is ready to send the next batch.

{INFO: Failover}
When the responsible node handling the subscription is down, the subscription task can be manually reassigned to another node in the cluster.  
With the Enterprise license the cluster will automatically reassign the work to another node.
{INFO/}


The TCP connection is also used as the "state" of the worker process and as long as it's alive, the server will not allow other clients to consume the subscription. 
The TCP connection is kept alive and monitored using "heartbeat" messages. If it's found nonfunctional, the current batch progress will be restarted.

See the sequence diagram below that summarizes the lifetime of a subscription connection.

![Subscription document processing](images/SubscriptionsDocumentProcessing.png)

{PANEL/}

{PANEL:Working with multiple clients}

In order to support various inter-worker scenarios, one worker is allowed to take the place of another in the processing of a subscription. 
Thanks to subscriptions persistence, the worker will be able to continue the work from the point it's predecessor stopped. 

It's possible to configure that a worker will wait for an existing connection to fail, and take it's place, or we can configure it to force close an existing connection etc. See more in [Workers interplay](../../client-api/data-subscriptions/subscription-consumption/how-to-consume-data-subscription#workers-interplay).

{PANEL/}

{PANEL:Data subscriptions usage example}

Data subscriptions are accessible by a document store. Here's an example of an ad-hoc creation and usage of data subscriptions:

{CODE subscriptions_example@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

## Related Articles

### Data Subscriptions

- [How to Create a Data Subscription](../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
- [Maintenance Operations](../../client-api/data-subscriptions/advanced-topics/maintenance-operations)
