# How to Get Entity Counters

---

{NOTE: }

* When a document is loaded to the session,  
  the loaded entity will contain various metadata information such as ID, current change-vector, and more.

* If the document has __Counters__, the document metadata will also contain its counter names.  
  The counter names are available for each entity using the `getCountersFor()` method from the `advanced` session operations.

* In this page:
    * [Get entity counters](../../../client-api/session/how-to/get-entity-counters#get-entity-counters)
    * [Syntax](../../../client-api/session/how-to/get-entity-counters#syntax)
{NOTE/}

---

{PANEL: Get entity counters}

{CODE:nodejs example@client-api\session\HowTo\getCountersFor.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\session\HowTo\getCountersFor.js /}

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **entity** | T | The entity for which counter names will be returned. |

| Return Type | Description |
| ----------- | ----------- |
| `string[]` | Returns the counter names for the specified entity, or `null` if the entity has no counters.<br>An exception is thrown if the entity is not tracked by the session. |

{PANEL/}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)

### Counters

- [Counters Overview](../../../document-extensions/counters/overview)
