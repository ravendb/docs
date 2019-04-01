# Session: Querying: How to Stream Query Results

Query results can be streamed using the `stream` method from the `advanced` session operations. The query can be issued using either a static index, or it can be a dynamic one where it will be handled by an auto index.

## Syntax

{CODE:java stream_1@ClientApi\Session\Querying\HowToStream.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | Query to stream results for. |
| `Reference` **streamQueryStats** | StreamQueryStatistics | Information about performed query. |

| Return Value | |
| ------------- | ----- |
| CloseableIterator<StreamResult> | Iterator with entities. |

## Example I - Using Static Index

{CODE:java stream_2@ClientApi\Session\Querying\HowToStream.java /}

## Example II - Dynamic Document Query

{CODE:java stream_3@ClientApi\Session\Querying\HowToStream.java /}

## Example III - Dynamic Raw Query

{CODE:java stream_4@ClientApi\Session\Querying\HowToStream.java /}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)
