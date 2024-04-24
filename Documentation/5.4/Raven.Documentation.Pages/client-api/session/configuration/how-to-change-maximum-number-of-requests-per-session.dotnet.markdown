# Session: How to Change Maximum Number of Requests per Session

By default, maximum number of requests that session can send to server is **30**. This number, if everything is done correctly, should never be reached. Remote calls are expensive, and the number of remote calls per "session" should be as close to **1** as possible. If the limit is reached, it is a sure sign of either a `Select N+1` problem or other misuse of the session object.

Nevertheless, if needed, this number can be changed for single session or for all sessions.

## Single session

To change maximum number of requests in a single session just manipulate `MaxNumberOfRequestsPerSession` property value from `Advanced` session operations.

{CODE max_requests_1@ClientApi\Session\Configuration\MaxRequests.cs /}

## All sessions

To change maximum number of requests for all sessions (on particular store) the `MaxNumberOfRequestsPerSession` property from DocumentStore `Conventions` must be changed.

{CODE max_requests_2@ClientApi\Session\Configuration\MaxRequests.cs /}

{INFO: Injecting MaxNumberOfRequestsPerSession from the Server}
The maximum number of requests for all sessions can also be configured via injected client configuration from the Server. You can read more about this [here](../../../studio/server/client-configuration). 
{INFO/}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
