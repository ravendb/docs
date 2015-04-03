#Registering deletions

{PANEL: Deleting a single file}

In order to register a file delete operation you need to use the `RegisterFileDeletion` method.

##Syntax

There are two overloads:

{CODE register_delete_1@FileSystem\ClientApi\Session\DeletingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | The full file path |
| **file** | [FileHeader](../../../glossary/file-header) | The file represented by `FileHeader` |
| **etag** | Etag | Current file Etag, used for concurrency checks (`null` will skip the check) |

{WARNING: FileNotFoundException}
If the requested file does not exist in the file system, the `FileNotFoundException` will be thrown by the `SaveChangesAsync`.
{WARNING/}

##Example

{CODE register_delete_2@FileSystem\ClientApi\Session\DeletingFiles.cs /}

{PANEL/}

{PANEL: Deleting multiple files based on search results}

You can also register deletion of multiple files that match certain criteria. Then the actual delete operation is performed based on 
results of the specified query. There are two methods accessible from the session that allows you to pass the query that will be used to determine
which files have to be deleted. 

##RegisterDeletionQuery

The first one is `RegisterDeletionQuery`, which has the following signature:

###Syntax

{CODE register_deletion_query_1@FileSystem\ClientApi\Session\DeletingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | string | The Lucene query |

You need to specify the valid Lucene query as a string value. 

###Example

The below code deletes all files which names end with `.draft` (note the usage of the built-in `__rfileName` term, click [here](../../indexing) to see more built-in index fields)

{CODE register_deletion_query_2@FileSystem\ClientApi\Session\DeletingFiles.cs /}

##RegisterResultsForDeletion

Deleting multiple files by querying is even more convenient when you can take advantage of the session's [querying support](./querying/basics).

###Example

The below code registers exactly the same files for deletion, in this case however the query is specified in strongly typed manner:

{CODE register_deletion_query_3@FileSystem\ClientApi\Session\DeletingFiles.cs /}


{PANEL/}