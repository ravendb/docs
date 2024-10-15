# How to Get Entity Counters

---

{NOTE: }

* When a document is loaded to the session, the loaded entity will include 
  various metadata details such as ID, current change-vector, etc.

* If the document has **Counters**, the document metadata will also contain 
  its counter names.  
  The counter names are available for each entity using the `getCountersFor()` 
  method from the `advanced` session operations.

* In this page:
    * [Get entity counters](../../../client-api/session/how-to/get-entity-counters#get-entity-counters)
    * [Syntax](../../../client-api/session/how-to/get-entity-counters#syntax)
{NOTE/}

---

{PANEL: Get entity counters}

{CODE:php example@ClientApi\Session\HowTo\GetCountersFor.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Session\HowTo\GetCountersFor.php /}


| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **$instance** | `mixed` | An instance of an entity for which counter names will be returned. |

| Return Type | Description |
| ----------- | ----------- |
| `?StringList` | Returns the counter names for the specified entity, or `None` if the entity has no counters.<br>An exception is thrown if the entity is not tracked by the session. |

{PANEL/}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)

### Counters

- [Counters Overview](../../../document-extensions/counters/overview)
