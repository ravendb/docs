#Registering an upload

In order to register a file upload within the session you can use one of four overloads of `RegisterUpload` method.

##Syntax

First two overloads allow to register an upload by specifying a file's full path and its content. The content can be represented either by 
a stream object or a write action that is allowed to directly write to a HTTP request stream:

{CODE register_upload_1@FileSystem\ClientApi\Session\UploadingFiles.cs /}

Next two overloads instead of a full path accept `FileHeader` object which represents a file in the session.

{CODE register_upload_2@FileSystem\ClientApi\Session\UploadingFiles.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | The full file path |
| **file** | [FileHeader](../../../glossary/file-header) | The file represented by `FileHeader` |
| **stream** | Stream | The file content that will be copied to HTTP request |
| **fileSize** | long | The declared number of bytes to write in **write** action |
| **write** | Action&lt;Stream&gt; | The action which writes file content bytes directly to HTTP request stream |
| **metadata** | RavenJObject | The file metadata |
| **etag** | Etag | Current file ETag, used for concurrency checks (`null` will skip the check) |

##Example I

Below code will upload an entire file stored on a local disk to RavenFS:

{CODE register_upload_3@FileSystem\ClientApi\Session\UploadingFiles.cs /}

{INFO: File content}
The actual upload is made when `SaveChangesAsync` is run. The stream which is file content needs to be available to read at that moment.
The stream value must not be disposed _before_ the save changes call.
{INFO/}

##Example II

You can also dynamically generate file content:

{CODE register_upload_4@FileSystem\ClientApi\Session\UploadingFiles.cs /}

{WARNING: Possible `BadRequestException`}
If the write action will put fewer bytes than declared then RavenFS will detect this and cancel the upload by throwing `BadRequestException`
when `SaveChangesAsync` is called.
{WARNING/}

##Example III

This example uploads a file only if it doesn't exits in RavenFS or a local file was modified at least one hour later than the remote one:

{CODE register_upload_5@FileSystem\ClientApi\Session\UploadingFiles.cs /}
