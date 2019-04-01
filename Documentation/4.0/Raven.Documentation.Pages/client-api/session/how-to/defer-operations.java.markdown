# Session: How to Defer Operations

Operations can be deferred till `saveChanges` is called by using `defer` method in `advanced` session operations. There are three types of commands that can be deferred:

- PutCommandData
- DeleteCommandData
- DeletePrefixedCommandData
- PatchCommandData
- PutAttachmentCommandData
- DeleteAttachmentCommandData

## Syntax

{CODE:java defer_1@ClientApi\Session\HowTo\Defer.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| `ICommandData` | Command to be executed. |
| `ICommandData[]` | Array of commands implementing `ICommandData` interface. |

## Example

{CODE:java defer_2@ClientApi\Session\HowTo\Defer.java /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
