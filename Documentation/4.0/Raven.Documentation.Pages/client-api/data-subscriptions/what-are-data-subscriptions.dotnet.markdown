#What are Data Subscriptions?

Data subscriptions provide a reliable and handy way to perform batch processing on documents.

To start a job with this feature you need to create a subscription in the database along with a set of conditions with which a document has to comply in order to be sent through subscription channel i.e. collection name, filtering.

When you open the subscription it will send you all documents matching the specified creation options, it may be a raw query, or lambdas describing the filtering and transformation. Documents are sent in batches and once you process them the whole batch is marked as processed. Documents are always sent in Change Vector and Etag order which means that data that already been processed and acknowledged won't be sent again.

This also ensures that you never miss any document even in the presence of failure - subscription will retry to send documents from the last acknowledged and processed document (by tracking its Change Vector). Subscriptions progress is stored in the cluster level, which means that in the case of server failure, the process can continue using another node. The usage of Change Vectors allows us to continue from a point that is close to the last point reached before failure, rather than starting the process from scratch.

Every time you open the subscription you receive all new or changed documents since the last time you pulled. After you download and process all documents you can still keep the subscription open to get new or modified documents. Under the hood, the data subscription uses a continuous TCP connection that lets the client to be notified about any document changes, so it is able to immediately provide subscribers with new documents that match the subscription criteria.

The subscriptions are persistent and long lived. A subscription created by one client can be accessed by another client by using a unique subscription name that user can provide, or let the server generate one. However, there can be only one client connected at any time. 

##Data subscriptions usage example
Data subscriptions are accessible by a document store. Here's an example of an ad-hod creation and usage of data subscriptions:

{CODE subscriptions_example@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
