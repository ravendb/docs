#Conventions related to querying

###FindPropertyNameForIndex and FindPropertyNameForDynamicIndex

These two conventions specify functions that are used to find the indexed property name for a static index or a dynamic one. This can be useful when you are indexing nested properties 
of complex types. Their default implementations are:

{CODE:java find_prop_name@ClientApi\Configuration\Conventions\Querying.java /}

The arguments in the order of appearance: an indexed document type, an index name, a current path and a property path.

###DefaultQueryingConsistency

The consistency options used when querying a database. Its default value is: 

{CODE:java querying_consistency@ClientApi\Configuration\Conventions\Querying.java /}

This option ensures that after querying an index at time T, you will never see the results of the index at a time prior to T. This is ensured by the server and 
require no action from the client. Note that because of the indexing process done by the server in the background you might get [stale](../../../indexes/stale-indexes) index results. 

If you really need your query results to include documents you have just changed or added, you can use the option:

{CODE:java querying_consistency_2@ClientApi\Configuration\Conventions\Querying.java /}

Then the client will wait 15 seconds for non-stale results, using an etag of a last written document within a session as a cut-off point. If after that time the results are still stale, then `TimeoutException` will be thrown. 

{DANGER:Beware of `AlwaysWaitForNonStaleResultsAsOfLastWrite` overuse}
The indexing mechanism in RavenDB is built on [a BASE model](../../faq/transaction-support#base-for-query-operations). In order to avoid querying consistency pitfalls in the future you need to consider this at the data modeling phase.

The usage of [`WaitForNonStaleResultsAsOfLastWrite` at a query level](../../session/querying/how-to-customize-query#waitfornonstaleresultsasoflastwrite) is usually reasonable on only very rare occassions. The need of taking advantage of `AlwaysWaitForNonStaleResultsAsOfLastWrite` convention is very often a symptom of deeper issues in an application model and misunderstanding of querying concepts in RavenDB. 
{DANGER/}

###AllowQueriesOnId 

It determines whether queries on document id are allowed. By default, queries on id are disabled, because it is far more efficient to do a `load()` than a `query()` if you already know the id.

{CODE:java allow_queries_on_id@ClientApi\Configuration\Conventions\Querying.java /}

{DANGER:Backward compatibility only}
This convention is provided for backward compatibility purposes only. It is NOT recommended to set its value to `true`.
{DANGER/}