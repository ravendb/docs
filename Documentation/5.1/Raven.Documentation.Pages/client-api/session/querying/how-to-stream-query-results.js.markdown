# How to Stream Query Results

---

{NOTE: }

* RavenDB supports streaming queries for both dynamic queries and static index queries.  
  __Streaming query results__ is useful when processing a large number of results.

* Neither the client nor the server holds the full query response in memory.   
  Instead, as soon as the server has a __single result__, it sends it to the client,  
  Thus, your application can start processing results before the server sends them all.

* The results of a streaming query are __not tracked__ by the session.  
  Changes made to them will not be sent to the server when _saveChanges_ is called.

* The stream results are a __snapshot of the data__ at the time when the query is computed by the server.  
  Results that match the query after it was already processed are not streamed to the client.

* Streaming query results does not support using [include](../../../client-api/session/loading-entities#load-with-includes).  
  Learn how to __stream related documents__ here [below](../../../client-api/session/querying/how-to-stream-query-results#stream-related-documents).

* To stream query results, use the `stream` method from the `advanced` session operations.

* In this page:
    * [Stream a dynamic query](../../../client-api/session/querying/how-to-stream-query-results#stream-a-dynamic-query)
    * [Stream a dynamic raw query](../../../client-api/session/querying/how-to-stream-query-results#stream-a-dynamic-raw-query)
    * [Stream a projected query](../../../client-api/session/querying/how-to-stream-query-results#stream-a-projected-query)
    * [Stream an index query](../../../client-api/session/querying/how-to-stream-query-results#stream-an-index-query)
    * [Stream related documents](../../../client-api/session/querying/how-to-stream-query-results#stream-related-documents)
    * [Syntax](../../../client-api/session/querying/how-to-stream-query-results#syntax)

{NOTE/}

---

{PANEL: Stream a dynamic query}

{CODE:nodejs stream_1@client-api\session\querying\howToStream.js /}

{PANEL/}

{PANEL: Stream a dynamic raw query}

{CODE:nodejs stream_2@client-api\session\querying\howToStream.js /}

{PANEL/}

{PANEL: Stream a projected query}

{CODE:nodejs stream_3@client-api\session\querying\howToStream.js /}

{PANEL/}

{PANEL: Stream an index query}

{CODE-TABS}
{CODE-TAB:nodejs:Query stream_4@client-api\session\querying\howToStream.js /}
{CODE-TAB:nodejs:Index stream_4_index@client-api\session\querying\howToStream.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Stream related documents}

#### Why streaming query results does not support 'include':

* A document can reference [related documents](../../../indexes/indexing-related-documents#what-are-related-documents).
* An [include](../../../client-api/session/loading-entities#load-with-includes) clause in a non-streamed query loads these related documents to the session  
  so that they can be accessed without an additional query to the server.
* Those included documents are sent to the client at the end of the query results.  
  This does not mesh well with streaming, which is designed to allow transferring massive amounts of data,  
  possibly over a significant amount of time.

#### How to stream related documents:

* Instead of using _include_, define the query so that it will return a [projection](../../../indexes/querying/projections).
* The projected query results will not be just the documents from the queried collection.  
  Instead, each result will be an entity containing the related document entities in addition to the original queried document.
* On the client side, you need to define a class that matches the projected query result.

#### Example:

* The below example uses RawQuery.  
  However, the same logic can be applied to a Query, DocumentQuery, or when querying an index.
* Note:  
  The projected class in the example contains the full related documents.  
  However, you can project just the needed properties from the related documents.

{CODE:nodejs stream_5@client-api\session\querying\howToStream.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\session\querying\howToStream.js /}

| Parameters | | |
| - | - | - |
| **query** | `IDocumentQuery` or `IRawDocumentQuery` | The query for which to stream results |
| **statsCallback** | `(streamStats) => void` | <ul><li>An optional callback function with an output parameter.</li><li>The parameter passed to the callback will be filled with the `StreamQueryStatistics` object when query returns.</li></ul> |

| Return Value | |
| - | - |
| `Promise<DocumentResultStream>` | A `Promise` resolving to readable stream with query results |

| `StreamQueryStatistics` | | |
| - | - | - |
| __totalResults__ | `number` | Total number of results |
| __resultEtag__ | `number` | An Etag that is specific for the query results |
| __indexName__ | `string` | Name of index that was used for the query |
| __indexTimestamp__ | `object` | Time when index was last updated |
| __stale__ | `boolean` | `true` if index is stale |

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)


### Inside RavenDB

- [Streaming data](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/4-deep-dive-into-the-ravendb-client-api#streaming-data)

