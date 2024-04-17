# How to Get Entity Counters

---

{NOTE: }

* When a document is loaded to the session,  
  the loaded entity will contain various metadata information such as ID, current change-vector, and more.

* If the document has __Counters__, the document metadata will also contain its counter names.  
  The counter names are available for each entity using the `GetCountersFor()` method from the `Advanced` session operations.

* In this page:
    * [Get entity counters](../../../client-api/session/how-to/get-entity-counters#get-entity-counters)
    * [Syntax](../../../client-api/session/how-to/get-entity-counters#syntax)
{NOTE/}

---

{PANEL: Get entity counters}

{CODE example@ClientApi\Session\HowTo\GetCountersFor.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\HowTo\GetCountersFor.cs /}


| Parameters | | |
| - | - | - |
| **instance** | T | Instance of an entity for which counter names will be returned. |

| Return Value | |
| - | - |
| `List<string>` | Returns the counter names for the specified entity, or `null` if the entity has no counters.<br>An exception is thrown if the `instance` is not tracked by the session. |

{PANEL/}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)

### Counters

- [Counters Overview](../../../document-extensions/counters/overview)
