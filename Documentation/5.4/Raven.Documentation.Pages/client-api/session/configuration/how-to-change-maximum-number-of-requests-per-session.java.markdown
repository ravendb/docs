# Session: How to Change Maximum Number of Requests per Session

By default, the maximum number of requests that a session can send the server is **30**.  
This number, if everything is done correctly, should never be reached since remote 
calls are expensive and the number of remote calls per "session" should be as close 
to **1** as possible.  
If the limit is reached, it may indicate a `Select N+1` problem or some other misuse 
of the session object.

Nevertheless, if needed, this number can be changed for a single session or for all sessions.

## Single session

To change the maximum number of requests in a single session, modify the `maxNumberOfRequestsPerSession` 
field value using the `advanced` session operations.

{CODE:java max_requests_1@ClientApi\Session\Configuration\MaxRequests.java /}

## All sessions

To change the maximum number of requests for **all** sessions (on a particular store), 
the `maxNumberOfRequestsPerSession` field from DocumentStore `conventions` must be changed.

{CODE:java max_requests_2@ClientApi\Session\Configuration\MaxRequests.java /}

{INFO: Injecting maxNumberOfRequestsPerSession from the Server}
The maximum number of requests for all sessions can also be configured via injected client 
configuration from the Server. Read more about this [here](../../../studio/server/client-configuration).
{INFO/}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
