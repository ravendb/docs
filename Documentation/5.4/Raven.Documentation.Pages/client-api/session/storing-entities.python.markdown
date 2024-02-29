# Session: Storing Entities

To store entities inside the **session** object, use the `store` method.

## Syntax

Stores the entity in a session, then extracts the ID from the entity or if not available generates a new one.

{CODE:python store_entities_1@ClientApi\Session\StoringEntities.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity that will be stored |
| **change_vector** | string | Entity change vector, used for concurrency checks (`null` to skip check) |
| **id** | string | Entity will be stored under this ID, (`null` to generate automatically) |

## Example

{CODE:python store_entities_5@ClientApi\Session\StoringEntities.py /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Query Overview](../../client-api/session/querying/how-to-query)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
