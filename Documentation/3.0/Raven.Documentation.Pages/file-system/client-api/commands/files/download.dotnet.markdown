#Commands: DownloadAsync

The **DownloadAsync** method is used to retrieve the file's content and metadata.

## Syntax

{CODE download_1@FileSystem\ClientApi\Commands\Files.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The name of a downloaded file |
| **metadata** | Reference&lt;RavenJObject&gt; | Reference of metadata object where downloaded file metadata will be placed (if not `null`, default: `null`)  |
| **from** | long? | The number of the first byte in a range when a partial download is requested |
| **to** | long? | The number of the last byte in a range when a partial download is requested|

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;Stream&gt;** | A task that represents the asynchronous download operation. The task result is a file's content represented by a readable stream. |

## Example

{CODE download_2@FileSystem\ClientApi\Commands\Files.cs /}
