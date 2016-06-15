#Opening a session

The `OpenAsyncSession` method is the only way to retrieve the `IAsyncFilesSession` object from  the `FilesStore`.

##Syntax

There are three overloads of the `OpenAsyncSession` method:

{CODE open_session_1@FileSystem\ClientApi\Session\OpeningSession.cs /}

The first method is an equivalent of:

{CODE open_session_2@FileSystem\ClientApi\Session\OpeningSession.cs /}

The second overload is an equivalent of:

{CODE open_session_3@FileSystem\ClientApi\Session\OpeningSession.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **sessionOptions** | OpenFilesSessionOptions | Options containing information such as the name of a file system on which the session will work and the credentials to use. |

| Return Value | |
| ------------- | ------------- |
| **IAsyncFilesSession** | The session object that implements the `IAsyncFilesSession` interface. |

##Example

{CODE open_session_4@FileSystem\ClientApi\Session\OpeningSession.cs /}

{INFO: Always dispose the session}
The session needs to allocate additional resources internally. You need to invoke the `Dispose` method on it or wrap the session object into the `using` statement.
{INFO/}