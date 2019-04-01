#Commands: GetMetadataForAsync

**GetMetadataForAsync** is used to retrieve the file's metadata.

## Syntax

{CODE get_metadata_1@FileSystem\ClientApi\Commands\Metadata.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The name of a file |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;RavenJObject&gt;** |  A task that represents the asynchronous metadata download operation. The task result is the file's metadata. |

## Example

{CODE get_metadata_2@FileSystem\ClientApi\Commands\Metadata.cs /}
