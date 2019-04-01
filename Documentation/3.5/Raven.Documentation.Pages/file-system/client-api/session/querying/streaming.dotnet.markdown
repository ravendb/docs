#Session: Querying: StreamQueryAsync

Query results can be streamed using **StreamQueryAsync** method from Advanced session operations.

## Syntax

{CODE streaming_1@FileSystem\ClientApi\Session\Querying\Streaming.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | IAsyncFilesQuery&lt;FileHeader&gt; | Query to stream results for |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;IAsyncEnumerator&lt;FileHeader&gt;&gt;** | A task that represents the asynchronous operation. The task result is the enumerator of [`FileHeaders`](../../../../../glossary/file-header) objects. |

## Example

{CODE streaming_2@FileSystem\ClientApi\Session\Querying\Streaming.cs /}   

{INFO: Information}
Entities loaded using Stream will be transient (not attached to session).
{INFO/}

## Related articles

- [Commands : StreamQueryAsync](../../commands/files/browse/stream-query)
