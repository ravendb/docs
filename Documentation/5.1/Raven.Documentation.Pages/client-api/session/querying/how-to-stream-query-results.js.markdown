# Stream Query Results

---

{NOTE: }

* RavenDB supports __streaming data__ from the server to the client.  
  Streaming is useful when processing a large number of results.

* The data streamed can be a result of a dynamic query, a static index query,  
  or just filtered by a prefix.

* Neither the client nor the server holds the full response in memory.   
  Instead, as soon as the server has a __single result__, it sends it to the client,  
  Thus, your application can start processing results before the server sends them all.

* The stream results are __not tracked__ by the session.  
  Changes made to them will not be sent to the server when _saveChanges_ is called.

* The stream results are a __snapshot of the data__ at the time when the query is computed by the server.  
  Results that match the query after it was already processed are not streamed to the client.

* Streaming query results does not support using [include](../../../client-api/session/loading-entities#load-with-includes).  
  Learn how to __stream related documents__ here [below](../../../client-api/session/querying/how-to-stream-query-results#stream-related-documents).

* To stream results, use the `stream` method from the `advanced` session operations.

* In this page:
    * [Stream by query](../../../client-api/session/querying/how-to-stream-query-results#stream-by-query)
        * [Stream a dynamic query](../../../client-api/session/querying/how-to-stream-query-results#stream-a-dynamic-query)
        * [Stream a dynamic raw query](../../../client-api/session/querying/how-to-stream-query-results#stream-a-dynamic-raw-query)
        * [Stream a projected query](../../../client-api/session/querying/how-to-stream-query-results#stream-a-projected-query)
        * [Stream an index query](../../../client-api/session/querying/how-to-stream-query-results#stream-an-index-query)
        * [Stream related documents](../../../client-api/session/querying/how-to-stream-query-results#stream-related-documents)
        * [By query syntax](../../../client-api/session/querying/how-to-stream-query-results#by-query-syntax)
    * [Stream by prefix](../../../client-api/session/querying/how-to-stream-query-results#stream-by-prefix)
        * [Stream results by prefix](../../../client-api/session/querying/how-to-stream-query-results#stream-results-by-prefix)
        * [By prefix syntax](../../../client-api/session/querying/how-to-stream-query-results#by-prefix-syntax)

{NOTE/}

---

{PANEL: Stream by query}

{NOTE: }
#### Stream a dynamic query

{CODE:nodejs stream_1@client-api\session\querying\howToStream.js /}

{NOTE/}

{NOTE: }
#### Stream a dynamic raw query

{CODE:nodejs stream_2@client-api\session\querying\howToStream.js /}

{NOTE/}

{NOTE: }
#### Stream a projected query

{CODE:nodejs stream_3@client-api\session\querying\howToStream.js /}

{NOTE/}

{NOTE: }
#### Stream an index query

{CODE-TABS}
{CODE-TAB:nodejs:Query stream_4@client-api\session\querying\howToStream.js /}
{CODE-TAB:nodejs:Index stream_4_index@client-api\session\querying\howToStream.js /}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### Stream related documents

---

__Why streaming query results does not support 'include'__:

* A document can reference [related documents](../../../indexes/indexing-related-documents#what-are-related-documents).
* An [include](../../../client-api/session/loading-entities#load-with-includes) clause in a non-streamed query loads these related documents to the session  
  so that they can be accessed without an additional query to the server.
* Those included documents are sent to the client at the end of the query results.  
  This does not mesh well with streaming, which is designed to allow transferring massive amounts of data,  
  possibly over a significant amount of time.

__How to stream related documents__:

* Instead of using _include_, define the query so that it will return a [projection](../../../indexes/querying/projections).
* The projected query results will not be just the documents from the queried collection.  
  Instead, each result will be an entity containing the related document entities in addition to the original queried document.
* On the client side, you need to define a class that matches the projected query result.

__Example__:

* The below example uses RawQuery.  
  However, the same logic can be applied to a Query, DocumentQuery, or when querying an index.
* Note:  
  The projected class in the example contains the full related documents.  
  However, you can project just the needed properties from the related documents.

{CODE:nodejs stream_5@client-api\session\querying\howToStream.js /}

{NOTE/}

{NOTE: }
#### By query syntax

{CODE:nodejs syntax_1@client-api\session\querying\howToStream.js /}

| Parameters | type | description |
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

{NOTE/}

{PANEL/}

{PANEL: Stream by prefix}

{NOTE: }
#### Stream results by prefix

---

* Streamed data can also be filtered by an __ID prefix__ and by some __filtering options__, see below.
* Note: No auto-index is created when streaming results by a prefix.

{CODE:nodejs stream_6@client-api\session\querying\howToStream.js /}
{CODE:nodejs stream_7@client-api\session\querying\howToStream.js /}

{NOTE/}

{NOTE: }
#### By prefix syntax

{CODE:nodejs syntax_2@client-api\session\querying\howToStream.js /}

| Parameters | type | description |
| - | - | - |
| **idPrefix** | `string` | Stream documents with this ID prefix |
| **options** | `StartingWithOptions` | More filtering options, see description below |

| Return Value | |
| - | - |
| `Promise<DocumentResultStream>` | A `Promise` resolving to readable stream with query results |

| `StartingWithOptions` | | |
| - | - | - |
| __matches__ | `number` | Filter the ID part that comes after the specified prefix.<br>Use '?' for any character, '*' any characters.<br>Use '&#124;' to separate rules. |
| __start__ | `number` | Number of documents to skip |
| __pageSize__ | `number` | Maximum number of documents to retrieve |
| __exclude__ | `strring` | Maximum number of documents to retrieve |
| __startAfter__ | `string` | Skip fetching documents until this ID is found.<br>Only return documents after this ID (default: null). |

{NOTE/}
{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)


### Inside RavenDB

- [Streaming data](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/4-deep-dive-into-the-ravendb-client-api#streaming-data)

