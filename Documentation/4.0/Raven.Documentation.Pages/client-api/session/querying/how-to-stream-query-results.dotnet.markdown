# Session : Querying : How to stream query results?

Query results can be streamed using `Stream` method from `Advanced` session operations. The query can be issued using either a static index or be dynamic one then it will be handled by auto index.

## Syntax

{CODE stream_1@ClientApi\Session\Querying\HowToStream.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IQueryable](../../../client-api/session/querying/how-to-query#session.query), [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | Query to stream results for. |
| `out` **streamQueryStats** | [StreamQueryStatistics](../../../glossary/stream-query-statistics) | Information about performed query. |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult](../../../glossary/stream-result)> | Enumerator with entities. |

## Example I - Using static index

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_2@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_2_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

## Example II - Dynamic document query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_3@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_3_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

## Example III - Dynamic raw query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_4@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_4_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}
