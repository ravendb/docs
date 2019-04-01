# Commands: Documents: Delete

**Delete** is used to remove a document from a database.

## Syntax

{CODE:java delete_1@ClientApi\commands\documents\delete.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | key of a document to be deleted |
| **etag** | Etag | current document etag, used for concurrency checks (`null` to skip check) |

## Example

{CODE:java delete_2@ClientApi\commands\documents\delete.java /}

## Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [Revisions and concurrency with ETags](../../../client-api/concurrency/revisions-and-concurrency-with-etags)   
