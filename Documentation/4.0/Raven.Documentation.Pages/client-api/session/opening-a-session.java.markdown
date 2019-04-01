# Session: Opening a Session

To open session use the `openSession` method from `DocumentStore`.

## Syntax

There are three overloads of `openSession` methods

{CODE:java open_session_1@ClientApi\Session\OpeningSession.java /}

The first method is an equivalent of doing

{CODE:java open_session_2@ClientApi\Session\OpeningSession.java /}

The second method is an equivalent of doing

{CODE:java open_session_3@ClientApi\Session\OpeningSession.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `OpenSessionOptions` | Options **containing** information such as **name of database** and **RequestExecutor**. |

| Return Value | |
| ------------- | ----- |
| IDocumentSession | Instance of a session object. |

## Example

{CODE:java open_session_4@ClientApi\Session\OpeningSession.java /}

{DANGER:Important}
**Always remember to release session allocated resources after usage by invoking the `close` method or wrapping the session object in the `try` statement.**
{DANGER/}

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
