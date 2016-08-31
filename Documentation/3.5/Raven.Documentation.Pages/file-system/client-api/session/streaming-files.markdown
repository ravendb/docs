#Session : Querying : StreamFileHeadersAsync

File Header can be streamed With **StreamFileHeadersAsync** method from Advanced session operations  .

## Syntax

{CODE streaming_files_1@FileSystem\ClientApi\Session\StreamingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fromEtag** | Etag | ETag of a file from which stream should start |
| **pageSize** | int | Maximum number of file headers that will be retrieved |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;IAsyncEnumerator&lt;FileHeader&gt;&gt;** | A task that represents the asynchronous operation. The task result is the enumerator of [`FileHeaders`](../../../../../glossary/file-header) objects. |

## Example

{CODE streaming_files_2@FileSystem\ClientApi\Session\StreamingFiles.cs /}   

{INFO: Track entities}
**StreamFileHeadersAsync** Does NOT track the entities in the session, and will not includes changes there when **SaveChangesasync()** is called.
{INFO/}

## Related articles

- [Commands : StreamFileHeadersAsync](../commands/files/browse/stream-file-headers)