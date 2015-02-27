#What is a file session?

After you created a files store you can start to use your file system. There are two ways to work with files on the client side. The first one 
is the usage of low level [commands](../commands/what-are-commands) that directly make calls to RavenFS server. However the primary way to interact
with RavenFS is a session. In order to perform any file operation by using this mechanism you need to obtain `IAsyncFilesSession` object from
the files store. The work model with the session usage is straightforward. You need to open the session, register some file operations and finally
apply changes to a remote RavenFS server.

{CODE session_usage_1@FileSystem\ClientApi\Session\WhatIsFilesSession.cs /}

All registered modifications are applied when `SaveChangesAsync` is called. It means that a stream object passed to `RegisterUpload` needs to be
available to read at the moment of `SaveChangesAsync` call (cannot be disposed).

Note that the session does not load files with content. It just retrieves and operates on file headers. If you want to get a file's content you have to
explicitly download it.

##Unit of Work

The session implements the Unit of Work pattern. That has the following implications:

* a single file (identified by its full path) always resolves to the same instance in the context of the same session

{CODE unit_of_work_1@FileSystem\ClientApi\Session\WhatIsFilesSession.cs /}

* the session keeps track of all files you have loaded and when you call `SaveChangesAsync`, then all changes made to those files are sent
to the server

{CODE unit_of_work_2@FileSystem\ClientApi\Session\WhatIsFilesSession.cs /}

{INFO: Applying changes}
In order to apply changes to the file system, the session uses [commands](../commands/what-are-commands) under the hood. In contrast to [`IDocumentSession`](../../../client-api/session/what-is-a-session-and-how-does-it-work)
modification operations are not batched. Each registered change is completed as a separate command, realized as a remote call.
{INFO/}

{SAFE:MaxNumberOfRequestsPerSession}
By default the session does not allow to execute more than 30 calls to RavenFS server. More details in [session configuration](TODO arek) options article.
{SAFE/}