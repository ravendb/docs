# Session: Storing Entities

To store entities inside the **session** object, use one of the three `store` methods.

## Syntax

First overload: Stores the entity in a session, then extracts the ID from the entity or generates a new one if it's not available.

{CODE:java store_entities_1@ClientApi\Session\StoringEntities.java /}

Second overload: Stores the entity in a session with given ID.

{CODE:java store_entities_2@ClientApi\Session\StoringEntities.java /}

Third overload: Stores the entity in a session with given ID, forces concurrency check with given change vector.

{CODE:java store_entities_3@ClientApi\Session\StoringEntities.java /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | Object | Entity that will be stored |
| **changeVector** | String | Entity changeVector, used for concurrency checks (`null` to skip check) |
| **id** | String | Entity will be stored under this ID, (`null` to generate automatically) |

## Example

{CODE:java store_entities_5@ClientApi\Session\StoringEntities.java /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Basics](../../indexes/querying/basics)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
