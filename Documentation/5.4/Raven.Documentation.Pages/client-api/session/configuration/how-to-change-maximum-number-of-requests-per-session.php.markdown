# Session: How to Change Maximum Number of Requests per Session

By default, the maximum number of requests that a session can send the server is **30**.  
This number, if everything is done correctly, should never be reached since remote 
calls are expensive and the number of remote calls per "session" should be as close 
to **1** as possible.  
If the limit is reached, it may indicate a `Select N+1` problem or some other misuse 
of the session object.

Nevertheless, if needed, this number can be changed for a single session or for all sessions.

## Single session

To modify the maximum number of requests for a **single** session, 
set the value of the session `maxNumberOfRequestsPerSession` property.

You can do this using the **advanced session** `setMaxNumberOfRequestsPerSession` method.

{CODE:php max_requests_1@ClientApi\Session\Configuration\MaxRequests.php /}

## All sessions

To modify the maximum number of requests for **all** sessions (on a particular store), 
set the value of the DocumentStore conventions `maxNumberOfRequestsPerSession` property.

You can do this using the **store** `setMaxNumberOfRequestsPerSession` method.

{CODE:php max_requests_2@ClientApi\Session\Configuration\MaxRequests.php /}

{INFO: }
The maximum number of requests for all sessions can also be configured by via injected client 
configuration from the Server. Read more about this [here](../../../studio/server/client-configuration).
{INFO/}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
