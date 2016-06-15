#Downloading files

The session exposes the `DownloadAsync` method to allow you to retrieve the content of a file.


##Syntax

There are two overloads:

{CODE download_1@FileSystem\ClientApi\Session\DownloadingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | The full file path |
| **file** | [FileHeader](../../../glossary/file-header) | The file represented by the `FileHeader` |
| **metadata** | Reference&lt;RavenJObject&gt; | Metadata of the downloaded file |


| Return Value | |
| ------------- | ------------- |
| **Task&lt;Stream&gt;** |  A task that represents the asynchronous download operation. The task result is the file's content represented by a readable stream. | 

| Exceptions | |
| ------------- | ------------- |
| FileNotFoundException | It is thrown when the requested file does not exist in the file system. |

##Example

{CODE download_2@FileSystem\ClientApi\Session\DownloadingFiles.cs /}