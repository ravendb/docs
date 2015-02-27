#Registering deletions

In order to register a file delete operation you need to use `RegisterFileDeletion` method.

#Syntax

There are two overloads:

{CODE register_delete_1@FileSystem\ClientApi\Session\DeletingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | The full file path |
| **file** | [FileHeader](../../../glossary/file-header) | The file represented by `FileHeader` |
| **etag** | Etag | Current file ETag, used for concurrency checks (`null` will skip the check) |

{WARNING: FileNotFoundException}
If the requested file does not exist in the file system then `FileNotFoundException` will be thrown by `SaveChangesAsync`.
{WARNING/}

##Example

{CODE register_delete_2@FileSystem\ClientApi\Session\DeletingFiles.cs /}