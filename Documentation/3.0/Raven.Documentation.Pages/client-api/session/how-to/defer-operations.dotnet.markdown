# Client API : Session : How to defer operations?

Operations can be deferred till `SaveChanges` is called by using `Defer` method in `Advanced` session operations. There are four types of commands that can be deferred:

- [PutCommandData]()
- [DeleteCommandData]()
- [PatchCommandData]()
- [ScriptedPatchCommandData]()

## Syntax

{CODE defer_1@ClientApi\Session\HowTo\Defer.cs /}

**Parameters**

commands
:   Type: [ICommandData]()[]  
Array of commands implementing `ICommandData` interface.

## Example

{CODE defer_2@ClientApi\Session\HowTo\Defer.cs /}

#### Related articles

TODO