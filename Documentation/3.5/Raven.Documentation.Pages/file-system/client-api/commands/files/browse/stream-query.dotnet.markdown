#Commands: StreamQueryAsync

**StreamQueryAsync** is used to Stream the query results to the client.

## Syntax

{CODE stream_query_1@FileSystem\ClientApi\Commands\Browse.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | string | The Lucene query |
| **sortFields** | string[] | The fields to sort by |
| **start** | int | The number of files that should be skipped |
| **pageSize** | int | The maximum number of file headers that will be retrieved |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;IAsyncEnumerator&lt;FileHeader&gt;&gt;** | A task that represents the asynchronous operation. The task result is the enumerator of [`FileHeaders`](../../../../../glossary/file-header) objects. |


## Example

{CODE stream_query_3@FileSystem\ClientApi\Commands\Browse.cs /}   

By using the following code **StreamQueryAsync** can sort the result for the query:
{CODE stream_query_2@FileSystem\ClientApi\Commands\Browse.cs /}
