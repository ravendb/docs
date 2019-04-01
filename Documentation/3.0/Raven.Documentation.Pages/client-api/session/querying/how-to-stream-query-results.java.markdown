# Session: Querying: How to stream query results?

Query results can be streamed using `stream` method from `advanced()` session operations.

## Syntax

{CODE:java stream_1@ClientApi\Session\Querying\HowToStream.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IQueryable](../../../client-api/session/querying/how-to-query) or [IDocumentQuery](../../../client-api/session/querying/lucene/how-to-use-lucene-in-queries) | Query to stream results for. |
| **queryHeaderInformation** | [QueryHeaderInformation](../../../glossary/query-header-information) | Information about performed query. |

| Return Value | |
| ------------- | ----- |
| CloseableIterator&lt;StreamResult&lt;T&gt;&gt; | Iterator with entities. |
| [QueryHeaderInformation](../../../glossary/query-header-information) | Information about performed query. |

## Example I

{CODE:java stream_2@ClientApi\Session\Querying\HowToStream.java /}

## Example II

{CODE:java stream_3@ClientApi\Session\Querying\HowToStream.java /}

## Related articles

- [Commands : Documents : Stream](../../commands/documents/stream)
