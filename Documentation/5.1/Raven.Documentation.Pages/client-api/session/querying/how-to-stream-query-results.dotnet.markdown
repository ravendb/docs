# Stream Query Results
---

{NOTE: }

* RavenDB supports __streaming data__ from the server to the client.  
  Streaming is useful when processing a large number of results.

* The data streamed can be a result of a dynamic query, a static index query, or just filtered by a prefix.  

* To stream results, use the `Stream` method from the `Advanced` session operations.

* In this page:

    * [Streaming overview](../../../client-api/session/querying/how-to-stream-query-results#streaming-overview)
     
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

{PANEL: Streaming overview}

* __Immediate processing__:  
  Neither the client nor the server holds the full response in memory.   
  Instead, as soon as the server has a single result, it sends it to the client.  
  Thus, your application can start processing results before the server sends them all.

* __No tracking__:  
  The stream results are Not tracked by the session.  
  Changes made to the resulting entities will not be sent to the server when _SaveChanges_ is called.

* __A snapshot of the data__:  
  The stream results are a snapshot of the data at the time when the query is computed by the server.  
  Results that match the query after it was already processed are Not streamed to the client.

* __Query limitations:__:  

  * A streaming query does not wait for indexing by design.  
    So calling [WaitForNonStaleResults](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults) is Not supported and will result in an exception.   
  
  * Using [Include](../../../client-api/how-to/handle-document-relationships#includes) to load a related document to the session in a streaming query is Not supported.  
    Learn how to __stream related documents__ here [below](../../../client-api/session/querying/how-to-stream-query-results#stream-related-documents).

{PANEL/}

{PANEL: Stream by query}

{NOTE: }
#### Stream a dynamic query

{CODE-TABS}
{CODE-TAB:csharp:Query-Sync stream_1@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Query-Async stream_1_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:DocumentQuery-Sync stream_2@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:DocumentQuery-Async stream_2_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### Stream a dynamic raw query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_3@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_3_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### Stream a projected query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_4@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_4_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:ProjectedClass stream_4_class@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### Stream an index query

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_5@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_5_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Index stream_5_index@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### Stream related documents

---

__Why streaming query results does not support 'include'__:

* A document can reference [related documents](../../../indexes/indexing-related-documents#what-are-related-documents).
* An [Include](../../../client-api/how-to/handle-document-relationships#includes) clause in a non-streamed query loads these related documents to the session  
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

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_6@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_6_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:ProjectedClass stream_6_class@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{NOTE: }
#### By query syntax

{CODE syntax@ClientApi\Session\Querying\HowToStream.cs /}

| Parameters | type | description |
| - | - | - |
| **query** | [IQueryable](../../../client-api/session/querying/how-to-query#session.query), [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | The query for which to stream results |
| `out` **streamQueryStats** | [StreamQueryStatistics](../../../glossary/stream-query-statistics) | Information about performed query |

| Return Value | |
| - | - |
| IEnumerator<[StreamResult&lt;T&gt;](../../../glossary/stream-result)> | Enumerator with resulting entities |

{NOTE/}

{PANEL: Stream by prefix}

{NOTE: }
####  Stream results by prefix

---

* Streamed data can also be filtered by an __ID prefix__ and by other __filtering options__, see syntax below.
* Note: No auto-index is created when streaming results by a prefix.

{CODE-TABS}
{CODE-TAB:csharp:Sync stream_7@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TAB:csharp:Async stream_7_async@ClientApi\Session\Querying\HowToStream.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### By prefix syntax

{CODE syntax_2@ClientApi\Session\Querying\HowToStream.cs /}

| Parameters | type | description |
| - | - | - |
| **startsWith** | `string` | Stream documents with this ID prefix |
| **matches** | `string` | Filter the ID part that comes after the specified prefix.<br>Use '?' for any character, '*' any characters.<br>Use '&#124;' to separate rules. |
| **start** | `int` | Number of documents to skip |
| **pageSize** | `int` | Maximum number of documents to retrieve |
| **startAfter** | `string` | Skip fetching documents until this ID is found.<br>Only return documents after this ID (default: null). |

| Return Value | |
| - | - |
| IEnumerator<[StreamResult&lt;T&gt;](../../../glossary/stream-result)> | Enumerator with resulting entities |

{NOTE/}
{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)


### Inside RavenDB

- [Streaming data](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/4-deep-dive-into-the-ravendb-client-api#streaming-data)
