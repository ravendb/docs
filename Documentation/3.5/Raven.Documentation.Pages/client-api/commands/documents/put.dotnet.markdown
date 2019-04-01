# Commands: Documents: Put

**Put** is used to insert or update a document in a database.

## Syntax

{CODE put_1@ClientApi\Commands\Documents\Put.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | unique key under which document will be stored |
| **etag** | Etag | current document etag, used for concurrency checks (`null` to skip check) |
| **document** | RavenJObject | document data |
| **metadata** | RavenJObject | document metadata |

<hr />

{CODE putresult@Common.cs /}

| Return Value | | |
| ------------- | ------------- | ----- |
| **Key** | string | unique key under which document was stored |
| **Etag** | Etag | stored document etag |

## Example

{CODE put_3@ClientApi\Commands\Documents\Put.cs /}

## Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Delete](../../../client-api/commands/documents/delete)  
- [Revisions and concurrency with ETags](../../../client-api/concurrency/revisions-and-concurrency-with-etags)   
