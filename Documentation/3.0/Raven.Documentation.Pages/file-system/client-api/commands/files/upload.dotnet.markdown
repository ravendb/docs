#Commands: UploadAsync

**UploadAsync** is used to insert a new file or update the content of an existing one in a file system.

## Syntax

{CODE upload_1@FileSystem\ClientApi\Commands\Files.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The name of the uploaded file (full path) |
| **source** | Stream | The file content |
| **metadata** | RavenJObject | The file metadata (default: `null`) |
| **etag** | Etag | The current file etag used for concurrency checks (`null` skips check) |

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous upload operation |

{CODE upload_3@FileSystem\ClientApi\Commands\Files.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The name of the uploaded file (full path) |
| **source** | Action&lt;Stream&gt; | The action which writes file content to the network stream |
| **prepareStream** | Action | The action executed before the content is being written (`null` means no action to perform) |
| **size** | long | The file size. It is sent in `RavenFS-Size` header to validate the number of bytes received on the server side. If there is a mismatch between the size reported in the header and the number of the bytes read on the server side, then `BadRequestException` is thrown |
| **metadata** | RavenJObject | The file metadata (default: `null`) |
| **etag** | Etag | The current file etag used for concurrency checks (`null` skips check) |

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous upload operation |

## Example I

{CODE upload_2@FileSystem\ClientApi\Commands\Files.cs /}

## Example II

{CODE upload_4@FileSystem\ClientApi\Commands\Files.cs /}
