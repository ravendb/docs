# Session: Querying: How to stream query results?

Query results can be streamed using `Stream` method from `Advanced` session operations.

## Syntax

{CODE stream_1@ClientApi\Session\Querying\HowToStream.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IQueryable](../../../client-api/session/querying/how-to-query) or [IDocumentQuery](../../../client-api/session/querying/lucene/how-to-use-lucene-in-queries) | Query to stream results for. |
| **queryHeaderInformation** | [QueryHeaderInformation](../../../glossary/query-header-information) | Information about performed query. |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult](../../../glossary/stream-result)> | Enumerator with entities. |
| [QueryHeaderInformation](../../../glossary/query-header-information) | Information about performed query. |

## Example I

{CODE stream_2@ClientApi\Session\Querying\HowToStream.cs /}

## Example II

{CODE stream_3@ClientApi\Session\Querying\HowToStream.cs /}

## Remarks

Streaming results only works for queries that were made against a (predefined) static index.

## Related articles

- [Commands : Documents : Stream](../../commands/documents/stream)
