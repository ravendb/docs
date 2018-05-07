# Operations: How to Delete Documents by Query

`DeleteByQueryOperation` gives you the ability to delete a large number of documents with a single query.
This operation is performed in the background on the server. 

## Syntax

{CODE:java delete_by_query@ClientApi\Operations\DeleteByQuery.java /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index to perform a query on |
| **queryToDelete** | IndexQuery | Holds all the information required to query an index |
| **options** | QueryOperationOptions | Holds different setting options for base operations |

## Example I

{CODE-TABS}
{CODE-TAB:java:Java delete_by_query1@ClientApi\Operations\DeleteByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Person/ByName' where Name = 'Bob' 
{CODE-TAB-BLOCK/}
{CODE-TABS/}


## Example II

{CODE-TABS}
{CODE-TAB:java:Java delete_by_query2@ClientApi\Operations\DeleteByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Person/ByName' where Age < 35
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III

{CODE-TABS}
{CODE-TAB:java:Java delete_by_query3@ClientApi\Operations\DeleteByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from People u where id(u) in ('people/1-A', 'people/3-A')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE: WaitForCompletion}
`DeleteByQueryOperation` is performed in the background on the server.    
You have the option to **wait** for it using `waitForCompletion`.

{CODE-TABS}
{CODE-TAB:java:Java delete_by_query_wait_for_completion@ClientApi\Operations\DeleteByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from People where Name = 'Bob' and Age >= 29
{CODE-TAB-BLOCK/}
{CODE-TABS/}
{NOTE /}

## Remarks

{WARNING: Map only indexes} 
`DeleteByQueryOperation` can only be performed on a map index. Executing it on map-reduce index will lead to an exception. 
{WARNING/}

{WARNING: Batching and Concurrency} 

The deletion of documents matching a specified query is run in batches of size 1024. RavenDB doesn't do concurrency checks during the operation
so it can happen than a document has been updated or deleted meanwhile.

{WARNING/}


## Related Articles

### Operations

- [What are Operations](../../client-api/operations/what-are-operations)

### Client API

- [How to Query](../../client-api/session/querying/how-to-query)

### Querying

- [What is RQL](../../indexes/querying/what-is-rql)
- [Basics](../../indexes/querying/basics)
