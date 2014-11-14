#Replication conventions

###FailoverBehavior

This conventions tells the client how it should behave in a replicated environment when the primary node is unreachable and need to failover to secondary node(s). Detailed description you will
find [here](../../bundles/how-client-integrates-with-replication-bundle).

{CODE:java failover_behavior@ClientApi\Configuration\Conventions\Replication.java /}

###ReplicationInformerFactory

This is called to provide replication behavior for the client. You can customize this to inject your own replication / failover logic by implementing `IDocumentStoreReplicationInformer`.

{CODE:java replication_informer@ClientApi\Configuration\Conventions\Replication.java /}


##IndexAndTransformerReplicationMode

It allows to change the replication mode for index and transformer definitions when they are created (or changed) by the client code (`AbstractIndexCreationTask.execute` / `AbstractTransformerCreationTask.execute`). The possible values are:

* `None` - neither indexes nor transformers are updated to replicated instances,
* `Indexes` - all created indexes are replicated.
* `Transformers` - all transformers are replicated.

By default both indexes and transformers are uploaded to the replication nodes:

{CODE:java index_and_transformer_replication_mode@ClientApi\Configuration\Conventions\Replication.java /}