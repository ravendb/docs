# Commands: Documents: Stream

**StreamDocs** is used to stream documents which match chosen criteria from a database.

## Syntax

{CODE stream_1@ClientApi\Commands\Documents\Stream.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fromEtag** | Etag | ETag of a document from which stream should start (mutually exclusive with 'startsWith') |
| **startsWith** | string | prefix for which documents should be streamed (mutually exclusive with 'fromEtag') |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **exclude** | int | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on a server side |
| **skipAfter** | String | skip document fetching until given key is found and return documents after that key (default: `null`) |

## Example

{CODE stream_2@ClientApi\Commands\Documents\Stream.cs /}

## Related articles

- [How to use **startsWith**, **matches** and **exclude**?](../../../client-api/commands/documents/get#startswith)  
- [Get](../../../client-api/commands/documents/get)  
