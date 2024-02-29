# Session: Loading Entities
---

{NOTE: }

* There are several methods that allow users to load documents from the database and convert them to entities.  

* This article covers the following methods:  

  - [load](../../client-api/session/loading-entities#load)
  - [load with includes](../../client-api/session/loading-entities#load-with-includes)
  - [load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
  - [load_starting_with](../../client-api/session/loading-entities#load_starting_with)
  - [conditional_load](../../client-api/session/loading-entities#conditional_load)
  - [stream](../../client-api/session/loading-entities#stream)
  - [is_loaded](../../client-api/session/loading-entities#is_loaded)

* For loading entities lazily see [perform requests lazily](../../client-api/session/how-to/perform-operations-lazily).

{NOTE/}

---

{PANEL:load}

The most basic way to load a single entity is to use one of the `load` methods.

{CODE:python loading_entities_1_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | Identifier of a document that will be loaded. |

| Return Type | Description |
| ------------- | ----- |
| `TResult` | Instance of `TResult` or `null` if a document with a given ID does not exist. |

### Example

{CODE:python loading_entities_1_1@ClientApi\Session\LoadingEntities.py /}

{NOTE From RavenDB version 4.x onwards, only string identifiers are supported. If you are upgrading from 3.x, this is a major change, because in 3.x non-string identifiers are supported. /}

{PANEL/}

{PANEL:load with includes}

When there is a 'relationship' between documents, those documents can be loaded in a 
single request call using the `include + load` methods. Learn more in 
[How To Handle Document Relationships](../../client-api/how-to/handle-document-relationships).  

{NOTE: }
Also see:  

* [Including Counters](../../document-extensions/counters/counters-and-other-features#including-counters)  
* [Including Time Series](../../document-extensions/timeseries/client-api/session/include/overview)  
* [Including Compare Exchange Values](../../client-api/operations/compare-exchange/include-compare-exchange)  
* [Including Document Revisions](../../client-api/session/revisions/including)  
{NOTE/}

{CODE:python loading_entities_2_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **path** | `string` or Expression | Path in documents in which the server should look for 'referenced' documents. |

| Return Type | Description |
| ------------- | ----- |
| `ILoaderWithInclude` | The `include` method by itself does not materialize any requests but returns loader containing methods such as `load`. |

### Example I

We can use this code to also load an employee which made the order.

{CODE:python loading_entities_2_1@ClientApi\Session\LoadingEntities.py /}

### Example II

{CODE:python loading_entities_2_2@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

{PANEL:load - multiple entities}

To load multiple entities at once, use one of the following `load` overloads.

{CODE:python loading_entities_3_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **ids** | `IEnumerable<string>` | Multiple document identifiers to load |

| Return Type | Description |
| ------------- | ----- |
| `Dictionary<string, TResult>` | Instance of Dictionary which maps document identifiers to `TResult` or `null` if a document with given ID doesn't exist. |

{CODE:python loading_entities_3_1@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

{PANEL:load_starting_with}

To load multiple entities that contain a common prefix, use the `load_starting_with` method from the `advanced` session operations.

{CODE:python loading_entities_4_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **idPrefix** | `string` |  prefix for which the documents should be returned  |
| **matches** | `string` | pipe ('&#124;') separated values for which document IDs (after 'idPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | `int` | number of documents that should be skipped  |
| **pageSize** | `int` | maximum number of documents that will be retrieved |
| **exclude** | `string` | pipe ('&#124;') separated values for which document IDs (after 'idPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **skipAfter** | `string` | skip document fetching until given ID is found and return documents after that ID (default: `null`) |

| Return Type | Description |
| ------------- | ----- |
| `TResult[]` | Array of entities matching given parameters. |
| `stream` | Output entities matching given parameters as a stream. |

### Example I

{CODE:python loading_entities_4_1@ClientApi\Session\LoadingEntities.py /}

### Example II

{CODE:python loading_entities_4_2@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

{PANEL: conditional_load}

This method can be used to check whether a document has been modified 
since the last time its change vector was recorded, so that the cost of loading it 
can be saved if it has not been modified.  

The `conditional_load` method takes a document's [change vector](../../server/clustering/replication/change-vector). 
If the entity is tracked by the session, this method returns the entity. If the entity 
is not tracked, it checks if the provided change vector matches the document's 
current change vector on the server side. If they match, the entity is not loaded. 
If the change vectors _do not_ match, the document is loaded.  

The method is accessible from the `session.advanced` operations.  

{CODE:python loading_entities_7_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | The identifier of a document to be loaded. |
| **changeVector** | `string` | The change vector you want to compare with the server-side change vector. If the change vectors match, the document is not loaded. |

| Return Type | Description |
| ------------- | ----- |
| ValueTuple `(T Entity, string ChangeVector)` | If the given change vector and the server side change vector do not match, the method returns the requested entity and its current change vector.<br/>If the change vectors match, the method returns `default` as the entity, and the current change vector.<br/>If the specified document, the method returns only `default` without a change vector. |

### Example

{CODE:python loading_entities_7_1@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

{PANEL:stream}

Entities can be streamed from the server using one of the following `stream` methods from the `advanced` session operations.

Streaming query results does not support the [`include` feature](../../client-api/how-to/handle-document-relationships#includes). 
Learn more in [How to Stream Query Results](../../client-api/session/querying/how-to-stream-query-results).  

{INFO Entities loaded using `stream` will be transient (not attached to session). /}

{CODE:python loading_entities_5_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **startsWith** | `string` | prefix for which documents should be streamed |
| **matches** | `string` | pipe ('&#124;') separated values for which document IDs should be matched ('?' any single character, '*' any characters) |
| **start** | `int` | number of documents that should be skipped  |
| **pageSize** | `int` | maximum number of documents that will be retrieved |
| **skipAfter** | `string` | skip document fetching until a given ID is found and returns documents after that ID (default: `null`) |
| **StreamQueryStats** | `streamQueryStats` (out parameter) | Information about the streaming query (amount of results, which index was queried, etc.) |

| Return Type | Description |
| ------------- | ----- |
| `IEnumerator<`[StreamResult](../../glossary/stream-result)`>` | Enumerator with entities. |
| `streamQueryStats` (out parameter) | Information about the streaming query (amount of results, which index was queried, etc.) |


### Example I

Stream documents for an ID prefix:

{CODE:python loading_entities_5_1@ClientApi\Session\LoadingEntities.py /}

## Example 2

Fetch documents for a ID prefix directly into a stream:

{CODE:python loading_entities_5_2@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

{PANEL:is_loaded}

To check if an entity is attached to a session, e.g. it has been loaded previously, use the `is_loaded` method from the **advanced** session operations.  
  
`is_loaded` checks if you've already tried to load a document during the current session.  
If you try to load a document that no longer exists with the `load` method (perhaps it has been deleted),  
`is_loaded` will then return `true` because `is_loaded` shows that you've already tried to load the non-existent document.  

{CODE:python loading_entities_6_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | Entity ID for which the check should be performed. |

| Return Type | Description |
| ------------- | ----- |
| `bool` | Indicates if an entity with a given ID is loaded. |

### Example

{CODE:python loading_entities_6_1@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Deleting Entities](../../client-api/session/deleting-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Query Overview](../../client-api/session/querying/how-to-query)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
