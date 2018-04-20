# Session : How to Defer Operations

Operations can be deferred till `saveChanges` is called by using `defer` method in `advanced` session operations. There are three types of commands that can be deferred:

- [PutCommandData](../../../glossary/put-command-data)
- [DeleteCommandData](../../../glossary/delete-command-data)
- [PatchCommandData](../../../glossary/patch-command-data)

## Syntax

{CODE:java defer_1@ClientApi\Session\HowTo\Defer.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| `ICommandData` | Command to be executed. |
| `ICommandData[]` | Array of commands implementing `ICommandData` interface. |

## Example

{CODE:java defer_2@ClientApi\Session\HowTo\Defer.java /}
