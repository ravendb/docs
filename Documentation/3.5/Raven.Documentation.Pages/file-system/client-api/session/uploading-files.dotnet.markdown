#Registering an upload

In order to register a file upload within the session, you can use one of four overloads of the `RegisterUpload` method.

##Syntax

First two overloads allow you to register an upload by specifying the file's full path and content. The content can be represented either by the stream object or the write action that is allowed to the directly write to the HTTP request stream:

{CODE register_upload_1@FileSystem\ClientApi\Session\UploadingFiles.cs /}

Next two overloads accept the `FileHeader` object, which represents the file in the session, instead of the file's full path.

{CODE register_upload_2@FileSystem\ClientApi\Session\UploadingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | The full path of the file |
| **file** | [FileHeader](../../../glossary/file-header) | The file represented by the `FileHeader` |
| **stream** | Stream | The file content that will be copied to the HTTP request |
| **fileSize** | long | The declared number of bytes to write in **write** action |
| **write** | Action&lt;Stream&gt; | The action which writes file content bytes directly to the HTTP request stream |
| **metadata** | RavenJObject | The file's metadata |
| **etag** | Etag | Current file Etag, used for concurrency checks (`null` will skip the check) |

##Example I

The below code will upload an entire file stored on a local disk to RavenFS:

{CODE register_upload_3@FileSystem\ClientApi\Session\UploadingFiles.cs /}

{INFO: File content}
The actual upload is made when the `SaveChangesAsync` is run. The stream, which is file content, needs to be available to read at that moment.
The stream value must not be disposed _before_ the save changes call.
{INFO/}

##Example II

You can also generate the file content dynamically:

{CODE register_upload_4@FileSystem\ClientApi\Session\UploadingFiles.cs /}

{WARNING: Possible `BadRequestException`}
If the write action puts fewer bytes than declared, RavenFS will detect this and cancel the upload by throwing the `BadRequestException` when the `SaveChangesAsync` is called.
{WARNING/}

##Example III

This example uploads a file only if it doesn't exits in RavenFS or if a local file was modified at least one hour later than the remote one:

{CODE register_upload_5@FileSystem\ClientApi\Session\UploadingFiles.cs /}
