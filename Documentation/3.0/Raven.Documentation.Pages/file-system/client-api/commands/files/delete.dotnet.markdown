#Commands : DeleteAsync

The **DeleteAsync** method is used to delete a file.

## Syntax

{CODE delete_1@FileSystem\ClientApi\Commands\Files.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The name of a file to be deleted |
| **etag** | Etag | The current file Etag, used for concurrency checks (`null` skips check) |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous delete operation. |

## Example

{CODE delete_2@FileSystem\ClientApi\Commands\Files.cs /}

{INFO: Delete on the server side}
To delete a file, RavenFS needs to remove a lot of information related to this file. In order to respond to the user quickly, the file is just renamed and a delete marker is added to its metadata. The actual delete is performed by [a periodic task](../../../server/background-tasks), which ensures that all the requested deletes will be accomplished even if the presence of a server restarts in the middle.
{INFO/}