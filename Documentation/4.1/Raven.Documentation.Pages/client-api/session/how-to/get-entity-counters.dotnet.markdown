# Session: How to Get Entity Counters

When a document is downloaded from the server, it contains various metadata information like ID or current change-vector.  
  
 If the document has *Counters*, all its counter names are also stored within the metadata in session.  
 They are available for each entity using the `GetCountersFor` method from the `Advanced` session operations.

## Syntax

{CODE syntax@ClientApi\Session\HowTo\GetCountersFor.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which counter names will be returned. |

| Return Value | |
| ------------- | ----- |
| List < string > | Returns the counter names for the specified entity, or `null` if the entity has no counters. Throws an exception if the `instance` is not tracked by the session. |

### Example

{CODE example@ClientApi\Session\HowTo\GetCountersFor.cs /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)

### Counters

- [Counters Overview](../../../client-api/session/counters/overview)
