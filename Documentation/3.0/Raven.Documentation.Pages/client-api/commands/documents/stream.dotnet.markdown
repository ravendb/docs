# Client API : Documents : Stream

**StreamDocs** is used to stream documents from a database that match given criteria.

## Syntax

{CODE stream_1@ClientApi\Commands\Documents\Stream.cs /}

**Parameters**   

fromEtag
:   Type: Etag   
ETag of a document from which stream should start (mutually exclusive with 'startsWith')   

startsWith
:   Type: string   
prefix for which documents should be streamed (mutually exclusive with 'fromEtag')   

matches
:   Type: string   
pipe ('|') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters)

start
:   Type: int   
number of documents that should be skipped   

pageSize
:   Type: int   
maximum number of documents that will be retrieved

exclude
:   Type: string   
pipe ('|') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters)       

pagingInformation
:   Type: RavenPagingInformation   
used to perform rapid pagination on server side      

## Example

{CODE stream_2@ClientApi\Commands\Documents\Stream.cs /}

#### Related articles

- [How to use **startsWith**, **matches** and **exclude**?](../../../client-api/commands/documents/get#startswith)  
- [Get](../../../client-api/commands/documents/get)  