# Client API : Documents : How to get document metadata only?

**Head** is used to retrieve document metadata from a database.

## Syntax

{CODE head_1@ClientApi\Commands\Documents\HowTo\Head.cs /}

**Parameters**   

key
:   Type: string   
key of a document to get metadata for

**Return Value**

{CODE jsondocumentmetadata@Common.cs /}

Metadata
:   Type: RavenJObject   
Document metadata

Etag
:   Type: Etag   
current document ETag

Key
:   Type: string   
document key

Metadata
:   Type: DateTime?   
last modified date of document 

## Example

{CODE head_2@ClientApi\Commands\Documents\HowTo\Head.cs /}

#### Related articles

- [Get](../../../../client-api/commands/documents/get)  
- [Put](../../../../client-api/commands/documents/put)  
- [Delete](../../../../client-api/commands/documents/delete)  
- [Revisions and concurrency with ETags](../../../../client-api/concurrency/revisions-and-concurrency-with-etags)   