# Commands : Documents : Delete

**Delete** is used to remove a document from a database.

## Syntax

{CODE delete_1@ClientApi\Commands\Documents\Delete.cs /}

**Parameters**   

key
:   Type: string   
Key of a document to delete

etag
:   Type: Etag   
current document etag, used for concurrency checks (`null` to skip check)   

## Example

{CODE delete_2@ClientApi\Commands\Documents\Delete.cs /}

#### Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Put](../../../client-api/commands/documents/put)  
- [Revisions and concurrency with ETags](../../../client-api/concurrency/revisions-and-concurrency-with-etags)   