# How to Defer Commands

---

{NOTE: }

* The `defer` method allows you to register server commands via the session.

* All the deferred requests will be stored in the session and sent to the server 
  in a single batch when `saveChanges` is called, along with any other changes/operations 
  made on the session.  
  Thus, all deferred commands are **executed as part of the session's `saveChanges` transaction**.

* In this page:   
    * [Defer commands example](../../../client-api/session/how-to/defer-operations#defer-commands-example)  
    * [Commands that can be deferred](../../../client-api/session/how-to/defer-operations#commands-that-can-be-deferred)
    * [Syntax](../../../client-api/session/how-to/defer-operations#syntax)  
{NOTE/}

---

{PANEL: Defer commands example}

{CODE:php defer_1@ClientApi\Session\HowTo\Defer.php /}

{PANEL/}

{PANEL: Commands that can be deferred}

The following commands implement the `CommandDataInterface` interface and can be deferred:  

  - [putCommandData](../../../glossary/put-command-data)
  - [deleteCommandData](../../../glossary/delete-command-data)
  - deletePrefixedCommandData
  - [patchCommandData](../../../glossary/patch-command-data)
  - batchPatchCommandData
  - putAttachmentCommandData
  - deleteAttachmentCommandData
  - [copyAttachmentCommandData](../../../glossary/copy-attachment-command-data)
  - [moveAttachmentCommandData](../../../glossary/move-attachment-command-data)
  - [countersBatchCommandData](../../../glossary/counters-batch-command-data)
  - putCompareExchangeCommandData
  - deleteCompareExchangeCommandData
  - copyTimeSeriesCommandData
  - timeSeriesBatchCommandData
  - forceRevisionCommandData
{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Session\HowTo\Defer.php /}

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **$command** | `CommandDataInterface` | A command to be executed |
| **$commands** | `array<CommandDataInterface>` | An array of commands to be executed |
| **$commands** | `array` | An array of commands to be executed |

{PANEL/}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
