#What are Data Subscriptions?

Data subscriptions provide a reliable and handy way to perform batch processing on documents.

To start a batch processing worker with this feature you need to create a subscription in the database along with a set of conditions with which a document has to comply in order to be sent through subscription channel i.e. collection name, filtering.

When you run a subscription worker it will send you all documents matching the specified creation options, it may be a raw query, or lambdas describing the filtering and transformation. Processing progress will be persisted and therefore it can be paused and resumed from the last point it was stopped. Documents are sent in batches and progress will be registered only after the whole batch is processed and acknowledged. Documents are always sent in Etag order which means that data that already been processed and acknowledged won't be sent twice. Nevertheless, there are conditions in which documents could be sent twice: 

1. If the document was changed more than one time

2. If data was received but not acknowledged, due to some client side error

3. In case of subscription failover, when there is a chance that documents will be processed again, because it's not always possible to find the same starting point on a different machine.

The persistence mechanism also ensures that you never miss any document even in the presence of failure, whether it's client side related, communication or any other disaster. Subscription worker will retry to send documents from the last acknowledged and processed document (by tracking its Change Vector). Subscriptions progress is stored in the cluster level, which means that in the case of node failure, the process can continue using another node. The usage of Change Vectors allows us to continue from a point that is close to the last point reached before failure, rather than starting the process from scratch.

Every time you open the subscription you receive all new or changed documents since the last time you pulled. After you download and process all documents you can still keep the subscription open to get new or modified documents. Under the hood, the data subscription uses a continuous TCP connection that lets the client to be notified about any document changes, so it is able to immediately provide subscribers with new documents that match the subscription criteria.

The subscriptions are persistent and long lived. A subscription created by one client can be accessed by another client by using a unique subscription name that user can provide, or let the server generate one. However, there can be only one client connected at any time. 

##Data subscriptions usage example
Data subscriptions are accessible by a document store. Here's an example of an ad-hod creation and usage of data subscriptions:

{CODE subscriptions_example@ClientApi\DataSubscriptions\DataSubscriptions.cs /}


## Related articles

- [How to create a data subscription?](../../client-api/data-subscriptions/how-to-create-data-subscription)
