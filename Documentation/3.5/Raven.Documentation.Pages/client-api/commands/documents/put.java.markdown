# Commands: Documents: Put

**Put** is used to insert or update a document in a database.

## Syntax

{CODE:java put_1@clientApi\commands\documents\Put.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | unique key under which document will be stored |
| **etag** | Etag | current document etag, used for concurrency checks (`null` to skip check) |
| **document** | RavenJObject | document data |
| **metadata** | RavenJObject | document metadata |

<hr />

{CODE:java putresult@Common.java /}

| Return Value | | |
| ------------- | ------------- | ----- |
| **Key** | String | unique key under which document was stored |
| **Etag** | Etag | stored document etag |

## Example

{CODE:java put_3@ClientApi\Commands\Documents\Put.java /}

## Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Delete](../../../client-api/commands/documents/delete)  
- [Revisions and concurrency with ETags](../../../client-api/concurrency/revisions-and-concurrency-with-etags)   
