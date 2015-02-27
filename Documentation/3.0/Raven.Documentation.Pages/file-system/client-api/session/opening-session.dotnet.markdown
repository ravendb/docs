#Opening a session

`OpenAsyncSession` method is the only way to retrieve `IAsyncFilesSession` object from `FilesStore`.

##Syntax

There are three overloads of `OpenAsyncSession` method:

{CODE open_session_1@FileSystem\ClientApi\Session\OpeningSession.cs /}

The first method is a equivalent of doing:

{CODE open_session_2@FileSystem\ClientApi\Session\OpeningSession.cs /}

The second overload is a equivalent of doing

{CODE open_session_3@FileSystem\ClientApi\Session\OpeningSession.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **sessionOptions** | OpenFilesSessionOptions | Options containing information such as a file system name on which a session will work and credentials to use. |

| Return Value | |
| ------------- | ------------- |
| **IAsyncFilesSession** | The session object that implements `IAsyncFilesSession` interface. |

##Example

{CODE open_session_4@FileSystem\ClientApi\Session\OpeningSession.cs /}

{INFO: Always dispose the session}
The session needs to allocate additional resources internally. You need to invoke `Dispose` method on it or wrap the session object into `using` statement.
{INFO/}