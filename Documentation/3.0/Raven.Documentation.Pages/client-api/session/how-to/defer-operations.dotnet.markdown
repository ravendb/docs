# Session: How to defer operations?

Operations can be deferred till `SaveChanges` is called by using `Defer` method in `Advanced` session operations. There are four types of commands that can be deferred:

- [PutCommandData](../../../glossary/put-command-data)
- [DeleteCommandData](../../../glossary/delete-command-data)
- [PatchCommandData](../../../glossary/patch-command-data)
- [ScriptedPatchCommandData](../../../glossary/scripted-patch-command-data)

## Syntax

{CODE defer_1@ClientApi\Session\HowTo\Defer.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| ICommandData[] | Array of commands implementing `ICommandData` interface. |

## Example

{CODE defer_2@ClientApi\Session\HowTo\Defer.cs /}
