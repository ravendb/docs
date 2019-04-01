# Session: How to Defer Operations

Operations can be deferred till `saveChanges` is called by using `defer` method in `advanced` session operations. All of the operations will update session state appropriately after `saveChanges` is called.

Types of commands that can be deferred:

- [PutCommandData](../../../glossary/put-command-data)
- [DeleteCommandData](../../../glossary/delete-command-data)
- DeletePrefixedCommandData
- [PatchCommandData](../../../glossary/patch-command-data)
- PutAttachmentCommandData
- DeleteAttachmentCommandData
- [CopyAttachmentCommandData](../../../glossary/copy-attachment-command-data)
- [MoveAttachmentCommandData](../../../glossary/move-attachment-command-data)
- [CountersBatchCommandData](../../../glossary/counters-batch-command-data)

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
