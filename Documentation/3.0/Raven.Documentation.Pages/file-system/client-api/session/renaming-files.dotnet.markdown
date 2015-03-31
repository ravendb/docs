#Registering renames

Use the `RegisterRename` method to rename a file.

##Syntax

{CODE rename_1@FileSystem\ClientApi\Session\RenamingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **sourceFile** | string | The full file path to change|
| **sourceFile** | [FileHeader](../../../glossary/file-header) | The file that you want to rename represented by the `FileHeader` |
| **destinationFile** | string | The new file path |
| **etag** | Etag | The current file Etag, used for concurrency checks (`null` will skip the check) |

{WARNING: FileNotFoundException}
If the requested file does not exist in the file system, the `FileNotFoundException` will be thrown by the `SaveChangesAsync`.
{WARNING/}

{NOTE: Rename and move}
Rename and move is basically the same operation. Directories in RavenFS are a virtual concept, which relies on the file paths. So if you want to move a file into a different "directory", simply rename it.
{NOTE/}

##Example

{CODE rename_2@FileSystem\ClientApi\Session\RenamingFiles.cs /}
