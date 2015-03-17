#Commands : UpdateMetadataAsync

**UpdateMetadataAsync** is used if you need to change just a file's metadata without any modification of its content.

## Syntax

{CODE update_metadata_1@FileSystem\ClientApi\Commands\Metadata.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The modified file name |
| **metadata** | RavenJObject | New file metadata |
| **etag** | Etag | The current file etag, used for concurrency checks (`null` skips check) |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous metadata update operation. |

## Example

{CODE update_metadata_2@FileSystem\ClientApi\Commands\Metadata.cs /}