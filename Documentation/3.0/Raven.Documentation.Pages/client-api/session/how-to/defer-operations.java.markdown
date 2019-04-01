# Session: How to defer operations?

Operations can be deferred till `saveChanges` is called by using `defer` method in `advanced()` session operations. There are four types of commands that can be deferred:

- [PutCommandData](../../../glossary/put-command-data)
- [DeleteCommandData](../../../glossary/delete-command-data)
- [PatchCommandData](../../../glossary/patch-command-data)
- [ScriptedPatchCommandData](../../../glossary/scripted-patch-command-data)

## Syntax

{CODE:java defer_1@ClientApi\Session\HowTo\Defer.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| ICommandData[] | Array of commands implementing `ICommandData` interface. |

## Example

{CODE:java defer_2@ClientApi\Session\HowTo\Defer.java /}
