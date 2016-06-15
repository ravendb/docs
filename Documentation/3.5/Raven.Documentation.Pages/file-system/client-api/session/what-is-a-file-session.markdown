#What is a file session?

After you created the files store, you can start using your file system. There are two ways to work with files on the client side. The first one is to use the low level [commands](../commands/what-are-commands) that make calls directly to the RavenFS server. However, the session is the primary way to interact with RavenFS. In order to perform any file operation by using this mechanism, you need to obtain the `IAsyncFilesSession` object from the files store. The work model with the session usage is straightforward. You need to open the session, register some file operations, and finally apply changes to the remote RavenFS server.

{CODE session_usage_1@FileSystem\ClientApi\Session\WhatIsFilesSession.cs /}

All registered modifications are applied when the `SaveChangesAsync` is called. It means that the stream object passed to the `RegisterUpload` needs to be available to read at the moment of the `SaveChangesAsync` call (cannot be disposed).

Note that the session does not load the files with content. It simply retrieves and operates on  the file headers. If you want to get the file's content, you have to download it explicitly.

##Unit of Work

The session implements the Unit of Work pattern. That has the following implications:

* a single file (identified by its full path) always resolves to the same instance in the context of the same session

{CODE unit_of_work_1@FileSystem\ClientApi\Session\WhatIsFilesSession.cs /}

* the session keeps track of all files you have loaded and when you call the `SaveChangesAsync`, all the changes made to those files are sent to the server.

{CODE unit_of_work_2@FileSystem\ClientApi\Session\WhatIsFilesSession.cs /}

{INFO: Applying changes}
In order to apply changes to the file system, the session uses the [commands](../commands/what-are-commands) under the hood. In contrast to the [`IDocumentSession`](../../../client-api/session/what-is-a-session-and-how-does-it-work), the modification operations are not batched. Each registered change is completed as a separate command and realized as a remote call.
{INFO/}

{SAFE:MaxNumberOfRequestsPerSession}
By default, the session does not allow to execute more than 30 calls to RavenFS server. For more details, see  the [session configuration](./configuration/how-to-change-maximum-number-of-requests-per-session) options article.
{SAFE/}