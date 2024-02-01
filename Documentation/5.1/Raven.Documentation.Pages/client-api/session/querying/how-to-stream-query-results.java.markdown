# Stream Query Results

RavenDB supports __streaming data__ from the server to the client.  
Streaming is useful when processing a large number of results.

The data streamed can be a result of a dynamic query, a static index query, or just filtered by a prefix.

To stream results, use the `stream` method from the `advanced` session operations.

---

{PANEL: Streaming overview}

* __Immediate processing__:  
  Neither the client nor the server holds the full response in memory.   
  Instead, as soon as the server has a single result, it sends it to the client.  
  Thus, your application can start processing results before the server sends them all.

* __No tracking__:  
  The stream results are Not tracked by the session.  
  Changes made to the resulting entities will not be sent to the server when _saveChanges_ is called.

* __A snapshot of the data__:  
  The stream results are a snapshot of the data at the time when the query is computed by the server.  
  Results that match the query after it was already processed are Not streamed to the client.

* __Query limitations:__:

    * A streaming query does not wait for indexing by design.  
      So calling [waitForNonStaleResults](../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults) is Not supported and will result in an exception.

    * Using [include](../../../client-api/session/loading-entities#load-with-includes) to load a related document to the session in a streaming query is Not supported.  

{PANEL/}

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
