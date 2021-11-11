# Session: Querying: How to Stream Query Results

Query results can be streamed using the `Stream` method from the `Advanced` session operations. The query can be issued using either a static index, or it can be a dynamic one where it will be handled by an auto index.

Streaming query results does not support the [`include` feature](../../../client-api/how-to/handle-document-relationships#includes). 
Instead, the query should rely on the [`load` clause](../../../indexes/querying/what-is-rql#load). See 
[example IV](../../../client-api/session/querying/how-to-stream-query-results#example-iv) below.  

{INFO Entities loaded using `Stream` will be transient (not attached to session). /}

## Syntax

{CODE stream_1@ClientApi\Session\Querying\HowToStream.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IQueryable](../../../client-api/session/querying/how-to-query#session.query), [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | Query to stream results for. |
| `out` **streamQueryStats** | [StreamQueryStatistics](../../../glossary/stream-query-statistics) | Information about performed query. |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult](../../../glossary/stream-result)> | Enumerator with entities. |

### Example I - Using Static Index

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_2@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_2_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

### Example II - Dynamic Document Query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_3@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_3_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

### Example III - Dynamic Raw Query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_4@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_4_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

## Alternative to Using Includes

Streaming does not support the [`include` feature](../../../client-api/how-to/handle-document-relationships#includes). 
An `include` clause in a query loads additional documents related to the primary target of the query. 
These are stored in the session on the client side so that they can be accessed without an additional 
query.  

In a normal non-streamed query, included documents are sent at the end of results, after the 
main targets and documents added with `load`. This does not mesh well with streaming, which is 
designed to allow transferring massive amounts of data, possibly over a significant amount of time. 
Instead of getting related documents at the end of the stream, it is better to get them interspersed 
with the other results.  

### Example IV

To include related documents in your query, add them using `load`, then use `select` to 
retrieve the documents.  

Because we used `select`, the query results are now a [projection](../../../indexes/querying/projections) containing more than 
one entity. So on the client side, you need a projection class that matches the query result.  

In this example, we query the `Orders` collection and also load related `Company` and 
`Employee` documents. With the select clause, we return all three objects. We have a class 
called `MyProjection` that has the three entity types as properties. We use this class to 
store the results.  

{CODE-TABS}
{CODE-TAB:csharp:Sync includes@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async includes_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Class class@ClientApi\Session\Querying\HowToStream.cs /}

{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)
