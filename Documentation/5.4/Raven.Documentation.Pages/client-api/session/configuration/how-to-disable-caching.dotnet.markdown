# Disable Caching per Session

To reduce the overhead of sending the documents over the network, 
client library is caching the HTTP responses and sends only ETags to Server. 

If the request was previously cached giving the Server an opportunity to send back `304 Not Modified` 
without any content data or sending the up-to-date results, this will update the cache. 

This behavior can be changed globally by disabling the HTTP Cache size (more [here](../../../client-api/configuration/conventions#maxhttpcachesize)), 
but can also be changed per session using the `SessionOptions.NoCaching` property.

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_caching@ClientApi\Session\Configuration\DisableCaching.cs /}
{CODE-TAB:csharp:Async disable_caching_async@ClientApi\Session\Configuration\DisableCaching.cs /}
{CODE-TABS/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a session](../../../client-api/session/opening-a-session)
- [Storing entities](../../../client-api/session/storing-entities)
- [Loading entities](../../../client-api/session/loading-entities)
- [Saving changes](../../../client-api/session/saving-changes)
