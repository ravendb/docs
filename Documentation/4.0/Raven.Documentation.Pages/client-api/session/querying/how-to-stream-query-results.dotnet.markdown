# Session: Querying: How to Stream Query Results

Query results can be streamed using the `Stream` method from the `Advanced` session operations. The query can be issued using either a static index, or it can be a dynamic one where it will be handled by an auto index.

## Syntax

{CODE stream_1@ClientApi\Session\Querying\HowToStream.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IQueryable](../../../client-api/session/querying/how-to-query#session.query), [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | Query to stream results for. |
| `out` **streamQueryStats** | [StreamQueryStatistics](../../../glossary/stream-query-statistics) | Information about performed query. |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult](../../../glossary/stream-result)> | Enumerator with entities. |

## Example I - Using Static Index

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_2@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_2_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

## Example II - Dynamic Document Query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_3@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_3_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

## Example III - Dynamic Raw Query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_4@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_4_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)
