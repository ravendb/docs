# Session: Opening a session

To open synchronous session use `OpenSession` method from `DocumentStore` or `OpenAsyncSession` if you prefer working in asynchronous manner.

## Syntax

There are three overloads of `OpenSession` method

{CODE open_session_1@ClientApi\Session\OpeningSession.cs /}

First method is a equivalent of doing

{CODE open_session_2@ClientApi\Session\OpeningSession.cs /}

Second method is a equivalent of doing

{CODE open_session_3@ClientApi\Session\OpeningSession.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | [OpenSessionOptions](../../glossary/open-session-options) | Options **containing** information such as **name of database** on which session will work and **credentials** that will be used. |

| Return Value | |
| ------------- | ----- |
| IDocumentSession | Instance of a session object that implements `IDocumentSession` interface. |

## Example I

{CODE open_session_4@ClientApi\Session\OpeningSession.cs /}

## Example II

{CODE open_session_5@ClientApi\Session\OpeningSession.cs /}

## Remarks

Always remember to release session allocated resources after usage by invoking `Dispose` method or wrapping session object in `using` statement.

## Related articles

- [What is a session and how does it work?](./what-is-a-session-and-how-does-it-work)  
