# Session: Opening a session

To open synchronous session use `openSession` method from `DocumentStore`.

## Syntax

There are three overloads of `openSession` method

{CODE:java open_session_1@ClientApi\Session\OpeningSession.java /}

First method is a equivalent of doing

{CODE:java open_session_2@ClientApi\Session\OpeningSession.java /}

Second method is a equivalent of doing

{CODE:java open_session_3@ClientApi\Session\OpeningSession.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | [OpenSessionOptions](../../glossary/open-session-options) | Options **containing** information such as **name of database** on which session will work and **credentials** that will be used. |

| Return Value | |
| ------------- | ----- |
| IDocumentSession | Instance of a session object that implements `IDocumentSession` interface. |

## Example I

{CODE:java open_session_4@ClientApi\Session\OpeningSession.java /}

## Remarks

Always remember to release session allocated resources after usage by invoking `close` method or wrapping session object in `try` statement.

## Related articles

- [What is a session and how does it work?](./what-is-a-session-and-how-does-it-work)  
