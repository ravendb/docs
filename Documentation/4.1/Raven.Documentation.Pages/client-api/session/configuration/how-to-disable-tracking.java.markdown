# Session: How to Disable Entities Tracking per Session

By default, session is tracking the changes. This means that for each document retrieved, session will start tracking it's changes allowing you to make the modifications and save the entities using the `saveChanges` method.

Sometimes it might be considered an unnecessary overhead (e.g. when session does not plan to make any changes, just retrieve the documents). This is why tracking can be turned off using `sessionOptions.noTracking` property. When turned off, the `store` and `saveChanges` operations will no longer be available (exception will be throw when used) and each call to methods such as `load` will create a new instance of a class that is being returned.

## Example

{CODE:java open_session_tracking_1@ClientApi\Session\OpeningSession.java /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../../client-api/session/opening-a-session)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Loading Entities](../../../client-api/session/loading-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
