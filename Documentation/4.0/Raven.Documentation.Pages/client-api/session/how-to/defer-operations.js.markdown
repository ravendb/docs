# Session: How to Defer Operations

Operations can be deferred till `saveChanges()` is called by using `defer()` method in `advanced` session operations. There are three types of commands that can be deferred:

- PutCommandData
- DeleteCommandData
- DeletePrefixedCommandData
- PatchCommandData
- PutAttachmentCommandData
- DeleteAttachmentCommandData

## Syntax

{CODE:nodejs defer_1@client-api\session\howTo\defer.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| commands | `ICommandData[]` | Command objects implementing `ICommandData` interface. |

## Example

{CODE:nodejs defer_2@client-api\session\howTo\defer.js /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
