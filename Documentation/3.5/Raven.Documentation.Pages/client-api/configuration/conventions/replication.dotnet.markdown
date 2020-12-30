#Replication Conventions

###FailoverBehavior

This convention tells the client how it should behave in a replicated environment when the primary node is unreachable and needs a failover to a secondary node or nodes.  

{CODE failover_behavior@ClientApi\Configuration\Conventions\Replication.cs /}

In a cluster environment, this convention lets the client know how to behave when a request is made.  

{CODE cluster_failover_behavior@ClientApi\Configuration\Conventions\Replication.cs /}

You will find a detailed description [here](../../bundles/how-client-integrates-with-replication-bundle).  

###ReplicationInformerFactory

This convention determines replication behavior for the client. You can customize this to inject your own replication / failover logic by implementing `IDocumentStoreReplicationInformer`.

{CODE replication_informer@ClientApi\Configuration\Conventions\Replication.cs /}

###IndexAndTransformerReplicationMode

This convention allows you to change the replication mode for index and transformer definitions when they are created (or changed) by the client code (`AbstractIndexCreationTask.Execute` / `AbstractTransformerCreationTask.Execute`). It is an enum type with `[Flags]` attributes applied. The possible values are:

* `None` - Neither indexes nor transformers are updated to replicated instances.
* `Indexes` - All created indexes are replicated.
* `Transformers` - All transformers are replicated.

By default, both indexes and transformers are uploaded to the replication nodes:

{CODE index_and_transformer_replication_mode@ClientApi\Configuration\Conventions\Replication.cs /}
