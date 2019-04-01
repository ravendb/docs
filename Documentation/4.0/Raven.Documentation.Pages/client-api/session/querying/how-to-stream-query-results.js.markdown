# Session: Querying: How to Stream Query Results

Query results can be streamed using the `stream()` method from the `advanced` session operations. The query can be issued using either a static index, or it can be a dynamic one where it will be handled by an auto index.

## Syntax

{CODE:nodejs stream_1@client-api\session\querying\howToStream.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | Query to stream results for. |
| **statsCallback** | function | callback returning information about the streaming query (amount of results, which index was queried, etc.) |
| **callback** | function | returns a readable stream with query results (same as Return Value result below) |

| Return Value | |
| ------------- | ----- |
| `Promise<Readable>` | A `Promise` resolving to readable stream with query results |

## Example I - Using Static Index

{CODE:nodejs stream_2@client-api\session\querying\howToStream.js /}

## Example II - Dynamic Document Query

{CODE:nodejs stream_3@client-api\session\querying\howToStream.js /}

## Example III - Dynamic Raw Query

{CODE:nodejs stream_4@client-api\session\querying\howToStream.js /}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)
