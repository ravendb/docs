#What are Data Subscriptions?

{PANEL: General}

Data subscriptions provide a reliable and handy way to perform batch processing on documents. Data subscriptions stores the progress in the server, allowing reliably to continue processing from the point it was stopped. In addition to that, data subscription allows performing filtering on data and returning projections, instead of plain documents and even to return revisions of data (if versioning is activated). Finally, for Entrerprise license, subscriptions allows failover between servers, by storing progress on the cluster level.

{PANEL/}

{PANEL:Data subscription consumption}

Data subscriptions consumed by clients, called subscription workers. In any given moment, only one worker can be connected to a data subscription. A worker connected to a data subscription receives a batch of documents and gets to process it. When it's done (depends on the code that the client gave the worker, can take from seconds to hours), it informs the server about the progress and the server is ready to send the next batch.

{PANEL/}

{PANEL:What defines a data subscription}

Data subscription defined both by it's server side definition, and by the worker connecting to it:

1. [Subscription Creation Options](how-to-create-data-subscription#subscriptioncreationoptions): The documents that will be recieved, it's filtering and projection.

2. [Subscription Worker Options](../../glossary/subscription-worker-options): Worker batch processing logic, batch size, interaction with other connections

For the common cases, RavenDB's API gives default values to many of the parameters, but they may be very usefull for non-trivial usage, please see usage examples in articles below.

{PANEL/}

{PANEL:Documents processing}

Documents are sent in batches and progress will be registered only after the whole batch is processed and acknowledged. Documents are always sent in Etag order which means that data that already been processed and acknowledged won't be sent twice, except for these scenarios:

1. If the document was changed after it was already sent

2. If data was received but not acknowledged

3. In case of subscription failover (`Enterprise feature`), when there is a chance that documents will be processed again, because it's not always possible to find the same starting point on a different machine.

Subscription worker will retry processing documents from the last acknowledged and processed document (by tracking its Change Vector).

{PANEL/}

{PANEL:Progress Persistance}

Processing progress will be persisted and therefore it can be paused and resumed from the last point it was stopped. 
The persistence mechanism also ensures that no documents are missed even in the presence of failure, whether it's client side related, 
communication or any other disaster. 
Subscriptions progress is stored in the cluster level, in `Enterprise edition`, in the case of node failure, 
the processing can be automatically failed over to another node.
The usage of Change Vectors allows us to continue from a point that is close to the last point reached before failure, rather than starting the process 
from scratch.
{PANEL/}

{PANEL:How the worker communicates with the subscription}

A worker communicates with the data subscription using a custom protocol, on top of a long-lived TCP connection. Each successfull batch processing consists of those stages:

1. Server sends documents batch

2. Worker send acknowledgement message after it finishes processing batch.

3. Server returns the client a notification that the acknowledgement persistance is done and it is ready to send the next batch.

The TCP connection also used as the "state" of the worker process and as long as it's alive, 
the server will not allow other clients to consume the subscription. 
The TCP connection is kept alive and monitored using "heartbeat" messages, if it's found unfunctional, current batch progress will be restarted.

See the sequence diagram below that summarizes the lifetime of a subscription connection.

![Subscription document processing](images\SubscriptionsDocumentProcessing.png)

{PANEL/}

{PANEL:Working with multiple clients}

In order to support various inter-worker scenarios, one worker is allowed to take the place of another in the processing of a subscription. 
Thanks to subscriptions persistenance, the worker will be able to continue the work from the point it's predcessor stopped. 

It is possible to configure that a worker will wait for an existing connection to fail, 
and takes it's place, or we can configure it to force close an existing connection etc. See more in [Subscription Opening Strategy](../../../glossary/Subscription-Opening-Strategy)

{PANEL/}

{PANEL:Data subscriptions usage example}

Data subscriptions are accessible by a document store. Here's an example of an ad-hod creation and usage of data subscriptions:

{CODE subscriptions_example@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

## Related articles

- [How to **create** a data subscription?](../../client-api/data-subscriptions/how-to-create-data-subscription)
- [How to **consume** a data subscription?](../../client-api/data-subscriptions/how-to-consume-data-subscription)
