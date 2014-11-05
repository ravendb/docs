#Conventions related to querying

###FindPropertyNameForIndex and FindPropertyNameForDynamicIndex

These two conventions specify functions that are used to find the indexed property name for a static index or a dynamic one. This can be useful when you are indexing nested properties 
of complex types. Their default implementations are:

{CODE find_prop_name@ClientApi\Configuration\Conventions\Querying.cs /}

The arguments in the order of appearance: an indexed document type, an index name, a current path and a property path.

###DefaultQueryingConsistency

The consistency options used when querying a database. Its default value is: 

{CODE querying_consistency@ClientApi\Configuration\Conventions\Querying.cs /}

This option ensures that after querying an index at time T, you will never see the results of the index at a time prior to T. This is ensured by the server and 
require no action from the client. Note that because of the indexing process done by the server in the background you might get [stale](../../../indexes/stale-indexes) index results. 

If you really need your query results to include documents you have just changed or added, you should use the option:

{CODE querying_consistency_2@ClientApi\Configuration\Conventions\Querying.cs /}

###ApplyReduceFunction

If you work in [a sharded environment](../../../server/scaling-out/sharding/how-to-setup-sharding) then `ShardedDocumentStore` needs to merge results of a map-reduce index that come
from different shards. So they need to be reduced on the client side. The behavior of the client side reduction can by overwritten by the following 
convention:

{CODE apply_reduce_func@ClientApi\Configuration\Conventions\Querying.cs /}


###AllowQueriesOnId 

It determines whether queries on document id are allowed. By default, queries on id are disabled, because it is far more efficient to do a `Load()` than a `Query()` if you already know the id.

{CODE allow_queries_on_id@ClientApi\Configuration\Conventions\Querying.cs /}

{DANGER:Backward compatibility only}
This convention is provided for backward compatibility purposes only. It is NOT recommended to set its value to `true`.
{DANGER/}