#Downloading files

The session exposes `DownloadAsync` method in order to allow you to retrieve file content.


##Syntax

There are two overloads:

{CODE download_1@FileSystem\ClientApi\Session\DownloadingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | The full file path |
| **file** | [FileHeader](../../../glossary/file-header) | The file represented by `FileHeader` |
| **metadata** | Reference&lt;RavenJObject&gt; | Metadata of the downloaded file |


| Return Value | |
| ------------- | ------------- |
| **Task&lt;Stream&gt;** |  A task that represents the asynchronous download operation. The task result is a file's content represented by a readable stream. | 

| Exceptions | |
| ------------- | ------------- |
| FileNotFoundException | Thrown when the requested file does not exist in the file system. |

##Example

{CODE download_2@FileSystem\ClientApi\Session\DownloadingFiles.cs /}