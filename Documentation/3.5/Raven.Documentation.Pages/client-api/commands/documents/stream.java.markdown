# Commands: Documents: Stream

**StreamDocs** is used to stream documents which match chosen criteria from a database.

## Syntax

{CODE:java stream_1@ClientApi\commands\documents\Stream.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fromEtag** | Etag | ETag of a document from which stream should start (mutually exclusive with 'startsWith') (default: `null`) |
| **startsWith** | String | prefix for which documents should be streamed (mutually exclusive with 'fromEtag') (default: `null`) |
| **matches** | String | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) (default: `null`) |
| **start** | int | number of documents that should be skipped (default: `0`) |
| **pageSize** | int | maximum number of documents that will be retrieved (default: `Integer.MAX_VALUE`) |
| **exclude** | int | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) (default: `null`) |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on a server side (default: `null`) |
| **skipAfter** | String | skip document fetching until given key is found and return documents after that key (default: `null`) |

## Example

{CODE:java stream_2@ClientApi\commands\documents\Stream.java /}

## Related articles

- [How to use **startsWith**, **matches** and **exclude**?](../../../client-api/commands/documents/get#startswith)  
- [Get](../../../client-api/commands/documents/get)  
