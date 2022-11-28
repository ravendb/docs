# How to Defer Commands

---

{NOTE: }

* `Defer` allows you to register server commands via the session.

* All the deferred requests will be stored in the session and sent to the server in a single batch when SaveChanges is called,
  along with any other changes/operations made on the session.  
  Thus, all deferred commands are __executed as part of the session's SaveChanges transaction__.

* When SaveChanges is done, the session state will be updated appropriately for all actions.

* In this page:   
    * [Defer commands example](../../../client-api/session/how-to/defer-operations#defer-commands-example)  
    * [Commands that can be deferred](../../../client-api/session/how-to/defer-operations#commands-that-can-be-deferred)
    * [Syntax](../../../client-api/session/how-to/defer-operations#syntax)  
{NOTE/}

---

{PANEL: Defer commands example}

{CODE defer_1@ClientApi\Session\HowTo\Defer.cs /}

{PANEL/}

{PANEL: Commands that can be deferred}

The following commands implement the `ICommandData` interface and can be deferred:  

  - [PutCommandData](../../../glossary/put-command-data)
  - [DeleteCommandData](../../../glossary/delete-command-data)
  - DeletePrefixedCommandData
  - [PatchCommandData](../../../glossary/patch-command-data)
  - BatchPatchCommandData
  - PutAttachmentCommandData
  - DeleteAttachmentCommandData
  - [CopyAttachmentCommandData](../../../glossary/copy-attachment-command-data)
  - [MoveAttachmentCommandData](../../../glossary/move-attachment-command-data)
  - [CountersBatchCommandData](../../../glossary/counters-batch-command-data)
  - PutCompareExchangeCommandData
  - DeleteCompareExchangeCommandData
  - CopyTimeSeriesCommandData
  - TimeSeriesBatchCommandData
  - ForceRevisionCommandData
{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\HowTo\Defer.cs /}

| Parameter | Type | Description |
| - |-|-|
| **command** | A command that implements the `ICommandData` interface | The command to be executed |
| **commands** | `ICommandData[]` | Array of commands to be executed |

{PANEL/}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
