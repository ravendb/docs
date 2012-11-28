#Client integration

RavenDB's Client API is aware of the replication mechanism offered by server istances and is ready to support failover scenarios.

##Failover behavior

 By default the client will detect and respond appropriately whenever a server has the replication bundle enabled. This includes:

* Detecting that an instance is replicating to another set of instances.
* When that instance is down, will automatically shift to the other instances.

This is because a failover mechanism is turned on in a document store by default. Then the client attempts to gain a replication document from `/docs/Raven/Replication/Destinations` in order to know what replication instances it would have use if a need of the failover occurred.

{NOTE Note that the client by default creates requests for the replication document even if the server has not the replication bundle enabled. Then you might notice in server's logs that requests for `/docs/Raven/Replication/Destinations` result in `404`./}

You are able to turn off the failover by using conventions of the document store. In order to do that use `FailImmediately` option:

{CODE-START: csharp/}
documentStore.Conventions.FailoverBehavior = FailoverBehavior.FailImmediately;
{CODE-END /}

The remaining values of `FailoverBehavior` enumeration are:

* *AllowReadsFromSecondaries* (default),
* *AllowReadsFromSecondariesAndWritesToSecondaries*,
* *ReadFromAllServers*.

They determine the strategy of the failovers if the primary server is down and the environment is configured to replicate between sibling instances.

##Discovering destinations

Once the document store is configured to support failovers it checks if the database server is configured to replicate. It retrieves a list of replicated nodes and saves it in a local application storage. Even if the primary server could no be reached in the future the list still exists locally and the document store can try to work with secondary instances according to conventions.

The Client API also checks if the replication configuration has changed on the server. It does it at regular intervals of 5 minutes to make sure that if the failover occurs, documents will go to instances that are slaves for our primary server.

##Request redirection

The Raven Client API is quite intelligent in this regard, upon failure, it will:

* Assume that the failure is transient, and retry the request.
* If the second attempt fails as well, we record the failure and shift to a replicated node, if available.
* After ten consecutive failures, Raven will start replicating to this node less often
* * Once every 10 requests, until failure count reaches 100
* * Once every 100 requests, until failure count reaches 1,000
* * Once every 1,000 requests, when failure count is above 1,000
* On the first successful request, the failure count is reset.

If the second replicated node fails, the same logic applies to it as well, and we move to the third replicated node, and so on. If all nodes fail, an appropriate exception is thrown.

##Back to primary

The client that has been shifted to a replicated node will go back to its primary server 
as soon as the primary will become reachable (irrespective of the failure counter). In replication environment the nodes send heartbeat messages to notify destination instances that they are up again. Then the destination (which is the secondary server for our shifted client) will send a feedback message to client and then it tries to send request to the primary server again. If an operation succeeded the failure counter is reset and a communication starts to work normally.


##Replicated operations

At a lower level, those are the operations that support replication:

* Get - single document and multi documents
* Put
* Delete
* Query
* Rollback
* Commit

The following operation do not support replication in the Client API:

* PutIndex
* DeleteIndex