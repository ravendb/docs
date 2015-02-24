#Commands : RenameAsync

**RenameAsync** is used to change a file name.

## Syntax

{CODE rename_1@FileSystem\ClientApi\Commands\Files.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **currentName** | string | The name of the file that you want to change |
| **newName** | string | The new file name |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous rename operation |

## Example

{CODE rename_2@FileSystem\ClientApi\Commands\Files.cs /}

Note that a file move operation is basically the rename too (directories are just a virtual concept). In order to move `intro.avi` file from `/movies` folder
to `/movies/examples` we need to use the following code:

{CODE rename_3@FileSystem\ClientApi\Commands\Files.cs /}

{INFO: Rename on server side}
If the file rename is requested then RavenFS needs to update collection of usages of this files's [pages](../../../files#pages). In order to make RavenFS resistant to restarts 
in the middle of the rename operation there is [a periodic task](../../../server/background-tasks), which ensures that all requested renames will be finished after restart.
{INFO/}