# Client API : Documents : Put

**Put** is used to insert or update a document in a database.

## Syntax

{CODE put_1@ClientApi\Commands\Documents\Put.cs /}

**Parameters**   

key
:   Type: string   
unique key under which document will be stored

etag
:   Type: Etag   
current document etag, used for concurrency checks (`null` to skip check) 

document
:   Type: RavenJObject   
document data

metadata
:   Type: RavenJObject   
document metadata

**Return Value**

{CODE putresult@Common.cs /}

Key
:   Type: string   
unique key under which document was stored  

ETag
:   Type: Etag   
stored document etag 

## Example

{CODE put_3@ClientApi\Commands\Documents\Put.cs /}

#### Related articles

- [Get](../../../client-api/commands/documents/get)  
- [Delete](../../../client-api/commands/documents/delete)  
- [Revisions and concurrency with ETags](../../../client-api/concurrency/revisions-and-concurrency-with-etags)   