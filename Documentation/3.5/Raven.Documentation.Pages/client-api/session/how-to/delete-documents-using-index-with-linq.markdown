# Session: How to Delete Documents Using Index with LINQ Statment

To delete a large amount of documents answering certain criteria, we can use the `DeleteByIndex` method from `session.Advanced`

### Syntax

{CODE delete_by_index_LINQ1@ClientApi\Session\HowTo\DeleteByIndexWithLinq.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index to perform a query on |
| **expression** | Expression<Func<T, bool>> | The LINQ expression (the query that will be performed) |

| Return Value | |
| ------------- | ----- |
| [Operation](../../../glossary/operation) | Object that allows waiting for an operation to complete. |

### Remarks
{NOTE: Note} 
`DeleteByIndex` can only be performed on a map index. Executing it on map-reduce index will lead to an exception. 
The document will be removed from the server after the method is called and not after `SaveChanges`.
{NOTE/}

### Example
{CODE delete_by_index_LINQ2@ClientApi\Session\HowTo\DeleteByIndexWithLinq.cs /}

## Related Articles
[Delete](../deleting-entities)   
[Commands: DeleteByIndex](../../commands/documents/how-to/delete-or-update-documents-using-index)
