# Session: Storing Entities

To store entities inside the **session** object, use `store()` method.

## Syntax

You can pass the following arguments to the `store()` method:

- entity only: Stores the entity in the session, then extracts the ID from the entity or generates a new one if it's not available.

{CODE:nodejs store_entities_1@ClientApi\Session\storingEntities.js /}

- entity and an id: Stores the entity in a session with given ID.

{CODE:nodejs store_entities_2@ClientApi\Session\storingEntities.js /}

- entity, an id and store options: Stores the entity in a session with given ID, forces concurrency check with given change vector.

{CODE:nodejs store_entities_3@ClientApi\Session\storingEntities.js /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity that will be stored |
| **id** | string | Entity will be stored under this ID, (`null` to generate automatically) |
| **documentType** | class | class used to determine collection of the entity (extracted from entity by default)|
| **options** | object | Options object with the below properties: |
| **changeVector** | string | entity *change vector* used for concurrency checks (`null` to skip check) |
| **documentType** | class | class used to determine collection of the entity (extracted from entity by default)|

| Return value | |
| ------------- | ----- |
| Promise | A promise resolved once entity obtained an ID and is stored in Unit of Work |

{WARNING: Asynchronous call }
`store()` method is asynchronous (since it reaches out to server to get a new ID) and returns a `Promise`, so don't forget to use either `await`, `.then()` *before* saving changes. 
{WARNING/}

{INFO: On collection name when storing object literals }
In order to comfortably use object literals as entities set the function getting collection name based on the content of the object - `store.conventions.findCollectionNameForObjectLiteral()`

{CODE:nodejs storing_literals_1@ClientApi\Session\storingEntities.js /}

This needs to be done before an `initialize()` call on `DocumentStore` instance. If you fail to do so, your entites will land up in *@empty* collection having an *UUID* for an ID.

{INFO/}

## Example

{CODE:nodejs store_entities_5@ClientApi\Session\storingEntities.js /}

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
