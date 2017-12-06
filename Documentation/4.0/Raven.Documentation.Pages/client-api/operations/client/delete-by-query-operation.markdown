﻿# Operations: How to delete documents by query

The `DeleteByQueryOperation` gives you the ability to delete a large amount of documents with single quary.
The operation is performed in the background on the server 

### Syntax

{CODE delete_by_query1@ClientApi\Operations\DeleteByQuery.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index to perform a query on |
| **expression** | Expression<Func<T, bool>> | The linq expression (the query that will be performed) |
| **queryToDelete** | IndexQuery | Holds all the information required to query an index |
| **options** | QueryOperationOptions | Holds different setting options for base operations |



## Example I
{CODE-TABS}
{CODE-TAB:csharp:Sync delete_by_query1@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB:csharp:Async delete_by_query1_async@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Person/ByName' where Name = 'Bob' 
{CODE-TAB-BLOCK/}
{CODE-TABS/}


## Example II
{CODE-TABS}
{CODE-TAB:csharp:Sync delete_by_query2@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB:csharp:Async delete_by_query2_async@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Person/ByName' where Age < 35
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III
{CODE-TABS}
{CODE-TAB:csharp:Sync delete_by_query3@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB:csharp:Async delete_by_query3_async@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from People
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE: WaitForCompletion}
`DeleteByQueryOperation` is performed in the background on the server.    
You have the option to **wait** for it using `WaitForCompletion`.

{CODE-TABS}
{CODE-TAB:csharp:Sync delete_by_query_wait_for_completion@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB:csharp:Async delete_by_query_wait_for_completion_async@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from People where Name = 'Bob' and Age >= 29
{CODE-TAB-BLOCK/}
{CODE-TABS/}
{NOTE /}

#### Remarks
{WARNING: important} 
`DeleteByQueryOperation` can only be performed on map index. Executing it on map-reduce index will lead to an exception. 
{WARNING/}


## Related articles

- [What are **operations**?](../what-are-operations?)  
