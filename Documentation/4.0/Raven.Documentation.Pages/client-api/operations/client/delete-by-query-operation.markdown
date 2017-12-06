# Operations: How to delete documents by query

The `DeleteByQueryOperation` gives you the ability to delete a large amount of documents with single Quary.

### Syntax

{CODE delete_by_query1@ClientApi\Operations\DeleteByQuery.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index to perform a query on |
| **expression** | Expression<Func<T, bool>> | The linq expression (the query that will be performed) |
| **queryToDelete** | IndexQuery | Holds all the information required to query an index |
| **options** | QueryOperationOptions | Holds different setting options for base operations |



## Example
{CODE-TABS}
{CODE-TAB:csharp:Sync delete_by_query2@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB:csharp:Async delete_by_query3@ClientApi\Operations\DeleteByQuery.cs /}
{CODE-TAB-BLOCK/}
{CODE-TABS/}


#### Remarks
{NOTE: Importent} 
`DeleteByQueryOperation` can only be performed on map index. Executing it on map-reduce index will lead to an exception. 
{NOTE/}


## Related articles

- [What are **operations**?](../what-are-operations?)  
