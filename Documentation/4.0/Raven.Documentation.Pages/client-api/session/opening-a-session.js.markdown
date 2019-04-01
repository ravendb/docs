# Session: Opening a Session

To open session use the `openSession()` method from `DocumentStore` instance.

## Syntax

There are three overloads of `openSession()` methods

{CODE:nodejs open_session_1@client-api\session\openingSession.js /}

The first method is an equivalent of doing:

{CODE:nodejs open_session_2@client-api\session\openingSession.js /}

The second method is an equivalent of doing:

{CODE:nodejs open_session_3@client-api\session\openingSession.js /}

Here is the list of available SessionOption object properties:

| Options | | |
| ------------- | ------------- | ----- |
| **database** | `string` | name of database |
| **requestExecutor** | `RequestExecutor` | RequestExecutor instance |


| Return Value | |
| ---------- | --------- |
|`IDocumentSession` | session instance |

## Example

{CODE:nodejs open_session_4@client-api\session\openingSession.js /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Basics](../../indexes/querying/basics)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
