#How to change maximum number of requests per session?

By default, the maximum number of requests that session can send to server is **30**. This number, if everything is done correctly, should never be reached. Remote calls are expensive, and the number of remote calls per "session" should be as small as possible. If the limit is reached, make sure that you don't misuse the session object.

Nevertheless, if needed, this number can be changed for a single session or for all the sessions.

## Single session

To change the maximum number of requests in a single session just manipulate `MaxNumberOfRequestsPerSession` property value from the `Advanced` session operations.

{CODE max_requests_1@FileSystem\ClientApi\Session\Configuration\MaxRequests.cs /}

## All sessions

To change the maximum number of requests for all the sessions (on the particular store), change the `MaxNumberOfRequestsPerSession` property from the `FilesStore.Conventions`.

{CODE max_requests_2@FileSystem\ClientApi\Session\Configuration\MaxRequests.cs /}
