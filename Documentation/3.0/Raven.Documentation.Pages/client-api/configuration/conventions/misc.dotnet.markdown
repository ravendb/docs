#Miscellaneous conventions

###DisableProfiling

Disable all profiling support which has been enabled by the `DocumentStore.InitializeProfiling()` call.

{CODE disable_profiling@ClientApi\Configuration\Conventions\Misc.cs /}

###EnlistInDistributedTransactions

It determines whether RavenDB client should automatically enlist in distributed transactions or not. Default: `true`.

{CODE enlist_in_dist_tx@ClientApi\Configuration\Conventions\Misc.cs /}

###FailoverBehavior

This conventions tells the client how it should behave in a replicated environment when the primary node is unreachable and need to failover to secondary node(s). Detailed description you will
find [here](../../bundles/how-client-integrates-with-replication-bundle).

{CODE failover_behavior@ClientApi\Configuration\Conventions\Misc.cs /}

###ReplicationInformerFactory

This is called to provide replication behavior for the client. You can customize this to inject your own replication / failover logic by implementing `IDocumentStoreReplicationInformer`.

{CODE replication_informer@ClientApi\Configuration\Conventions\Misc.cs /}


###MaxNumberOfRequestsPerSession

The max number of requests per session. See the [related article](../../session/configuration/how-to-change-maximum-number-of-requests-per-session) for details.

{CODE max_number_of_requests_per_session@ClientApi\Configuration\Conventions\Misc.cs /}

###SaveEnumsAsIntegers

It determines if C# `enum` types should be saved as integers or strings and instruct the Linq provider to query enums as integer values. Default: `false`.

{CODE save_enums_as_integers@ClientApi\Configuration\Conventions\Misc.cs /}

###DefaultUseOptimisticConcurrency

This convention allows to enable optimistic concurrency for all opened sessions. More about optimistic concurrency you will find [here](../../session/configuration/how-to-enable-optimistic-concurrency).
By default concurrency checks are turned off:

{CODE use_optimistic_concurrency_by_default@ClientApi\Configuration\Conventions\Misc.cs /}
