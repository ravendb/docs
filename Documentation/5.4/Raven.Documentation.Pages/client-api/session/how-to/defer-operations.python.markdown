# How to Defer Commands

---

{NOTE: }

* The `defer` method allows you to register server commands via the session.

* All the deferred requests will be stored in the session and sent to the server 
  in a single batch when `save_changes` is called, along with any other changes/operations 
  made on the session.  
  Thus, all deferred commands are **executed as part of the session's `save_changes` transaction**.

* In this page:   
    * [Defer commands example](../../../client-api/session/how-to/defer-operations#defer-commands-example)  
    * [Commands that can be deferred](../../../client-api/session/how-to/defer-operations#commands-that-can-be-deferred)
    * [Syntax](../../../client-api/session/how-to/defer-operations#syntax)  
{NOTE/}

---

{PANEL: Defer commands example}

{CODE:python defer_1@ClientApi\Session\HowTo\Defer.py /}

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

{CODE:python syntax@ClientApi\Session\HowTo\Defer.py /}

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **\*commands** | `CommandData` | An array of commands to be executed |

{PANEL/}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
