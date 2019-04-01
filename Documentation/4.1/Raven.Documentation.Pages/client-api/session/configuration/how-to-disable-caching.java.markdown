# Session: How to Disable Caching per Session

To reduce the overhead of sending the documents over the network, client library is caching the HTTP responses and sends only ETags to Server. If the request was previously cached giving the Server an opportunity to send back `304 Not Modified` without any content data or sending the up-to-date results, this will update the cache. This behavior can be changed globally by disabling the HTTP Cache size (more [here](../../../client-api/configuration/conventions#maxhttpcachesize)), but can also be changed per session basing using the `sessionOptions.noCaching` property.

## Example

{CODE:java open_session_caching_1@ClientApi\Session\OpeningSession.java /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../../client-api/session/opening-a-session)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Loading Entities](../../../client-api/session/loading-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
