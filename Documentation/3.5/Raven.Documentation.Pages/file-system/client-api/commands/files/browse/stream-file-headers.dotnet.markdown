#Commands: StreamFileHeadersAsync

**StreamFileHeadersAsync** is used to stream the headers of files which match the criteria chosen from a file system.

## Syntax

{CODE stream_file_headers_1@FileSystem\ClientApi\Commands\Browse.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fromEtag** | Etag | ETag of a file from which the stream should start |
| **pageSize** | int | The maximum number of file headers that will be retrieved |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;IAsyncEnumerator&lt;FileHeader&gt;&gt;** | A task that represents the asynchronous operation. The task result is the enumerator of [`FileHeaders`](../../../../../glossary/file-header) objects. |


## Example

{CODE stream_file_headers_2@FileSystem\ClientApi\Commands\Browse.cs /}
