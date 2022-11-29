# How to Defer Commands

---

{NOTE: }

* `Defer` allows you to register server commands via the session.

* All the deferred requests will be stored in the session and sent to the server in a single batch when saveChanges is called,
  along with any other changes/operations made on the session.  
  Thus, all deferred commands are __executed as part of the session's saveChanges transaction__.

* In this page:
    * [Defer commands example](../../../client-api/session/how-to/defer-operations#defer-commands-example)
    * [Commands that can be deferred](../../../client-api/session/how-to/defer-operations#commands-that-can-be-deferred)
    * [Syntax](../../../client-api/session/how-to/defer-operations#syntax)  
      {NOTE/}

---

{PANEL: Defer commands example}

{CODE:nodejs defer_1@ClientApi\Session\HowTo\defer.js /}

{PANEL/}

{PANEL: Commands that can be deferred}

The following commands implement the `ICommandData` interface and can be deferred:

- PutCommandDataBase
- DeleteCommandData
- DeletePrefixedCommandData
- PatchCommandData
- BatchPatchCommandData
- PutAttachmentCommandData
- DeleteAttachmentCommandData
- CopyAttachmentCommandData
- MoveAttachmentCommandData
- CountersBatchCommandData
- PutCompareExchangeCommandData
- DeleteCompareExchangeCommandData
- CopyTimeSeriesCommandData
- TimeSeriesBatchCommandData
- ForceRevisionCommandData
  {PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\HowTo\defer.js /}

| Parameter | Type | Description |
| - |-|-|
| **commands** | `ICommandData[]` | Commands to be executed |

{PANEL/}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
