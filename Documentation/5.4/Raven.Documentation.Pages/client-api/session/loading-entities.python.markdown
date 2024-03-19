# Session: Loading Entities
---

{NOTE: }

* There are several methods that allow users to load documents from the database and convert them to entities.

* This article covers the following methods:

  - [load](../../client-api/session/loading-entities#load)
  - [load with includes](../../client-api/session/loading-entities#load-with-includes)
  - [load_starting_with](../../client-api/session/loading-entities#load_starting_with)
  - [load_starting_with_into_stream](../../client-api/session/loading-entities#load_starting_with_into_stream)
  - [conditional_load](../../client-api/session/loading-entities#conditional_load)
  - [stream](../../client-api/session/loading-entities#stream)
  - [is_loaded](../../client-api/session/loading-entities#is_loaded)

* For loading entities lazily see [perform requests lazily](../../client-api/session/how-to/perform-operations-lazily).

{WARNING: }
From RavenDB version 4.x onward, only string identifiers are supported.  
If you are upgrading from 3.x, this is a major change, because in 3.x non-string identifiers are supported as well.  
{WARNING/}

{NOTE/}
---

{PANEL:load}

Use the `load` method to load **an entity** or **multiple entities**.  

{CODE:python loading_entities_1_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **key_or_keys** | `str` or `List[str]` | Identifier or a list of identifiers of entities to load |
| **object_type**<br>(optional) | `[Type[_T]` | Entity type to load (optional) |
| **includes**<br>(optional) | `Callable[[IncludeBuilder], None]` | A **consumer function** that takes an [include builder](../../client-api/how-to/handle-document-relationships#includes) argument.<br>The user should use the builder inside this function to _include_ all the data needed within a load. |

| Return Type | Description |
| ------------- | ----- |
| `_T` | If a single document was requested, return an instance of the document or `None` if no document was found |
| `Dict[str, _T]` | If multiple documents were requested, return a dictionary of document instances or `None` if no documents were found |


### Examples

* Load an entiry
  {CODE:python loading_entities_1_1@ClientApi\Session\LoadingEntities.py /}

* Load multiple entities:
  {CODE:python loading_entities_3_1@ClientApi\Session\LoadingEntities.py /}

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
* [Including Document Revisions](../../document-extensions/revisions/client-api/session/including)  
{NOTE/}

{CODE:python loading_entities_2_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **path** | `str` | Search path that the server will use to look for the 'referenced' documents |

| Return Type | Description |
| ------------- | ----- |
| `LoaderWithInclude` | The ` include` method by itself does not materialize any requests but returns loader containing methods such as `load`. |

### Example I

We can use this code to also load an employee which made the order.

{CODE:python loading_entities_2_1@ClientApi\Session\LoadingEntities.py /}

### Example II

{CODE:python loading_entities_2_2@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

{PANEL:load_starting_with}

To load multiple entities with a common prefix, use the `advanced` session operation `load_starting_with`.  
{CODE:python load_starting_with@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id_prefix** | `str` |  Prefix to return the documents to |
| **object_type**<br>(optional) | `Type[_T]` | The object type |
| **matches**<br> | `str` | Pipe ('&#124;') separated values for which document IDs (after 'id_prefix') should be matched ('?' any single character, '*' any characters) |
| **start**<br>(optional) | `int` | Number of documents that should be skipped  |
| **page_size**<br>(optional) | `int` | Maximum number of documents that will be retrieved |
| **exclude**<br>(optional) | `str` | Pipe ('&#124;') separated values for which document IDs (after 'id_prefix') should **not** be matched ('?' any single character, '*' any characters) |
| **start_after**<br>(optional) | `str` | Skip document fetching until given ID is found and return documents after that ID (default: `None`) |

| Return Type | Description |
| ----------- | ----------- |
| `List[_T]` | An array of entities matching the given parameters |

### Example I

{CODE:python loading_entities_4_1@ClientApi\Session\LoadingEntities.py /}

### Example II

{CODE:python loading_entities_4_2@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

{PANEL:load_starting_with_into_stream}

To output multiple entities with a common prefix into a stream, use the `advanced` session operation `load_starting_with_into_stream`.  
{CODE:python load_starting_with_into_stream@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id_prefix** | `str` |  Prefix to return the documents to |
| **matches**<br> | `str` | Pipe ('&#124;') separated values for which document IDs (after 'id_prefix') should be matched ('?' any single character, '*' any characters) |
| **start** | `int` | Number of documents that should be skipped  |
| **page_size** | `int` | Maximum number of documents that will be retrieved |
| **exclude** | `str` | Pipe ('&#124;') separated values for which document IDs (after 'id_prefix') should **not** be matched ('?' any single character, '*' any characters) |
| **start_after** | `str` | Skip document fetching until given ID is found and return documents after that ID (default: `None`) |

| Return Type | Description |
| ----------- | ----------- |
| `bytes` | The retrieved entities, returned as a stream of bytes |

### Example

To stream entities from the `employees` collection, use:  
{CODE:python loading_entities_5_2@ClientApi\Session\LoadingEntities.py /}

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
| **key** | `str` | The identifier of a document to be loaded |
| **change_vector** | `str` | The change vector you want to compare with the server-side change vector. If the change vectors match, the document is not loaded. |
| **object_type**<br>(optional) | `Type[_T]` | Object type |

| Return Type | Description |
| ------------- | ----- |
| `ConditionalLoadResult[_T]` | If the given change vector and the server side change vector do not match, the method returns the requested entity and its current change vector.<br/>If the change vectors match, the method returns `default` as the entity, and the current change vector.<br/>If the specified document, the method returns only `default` without a change vector. |

### Example

{CODE:python loading_entities_7_1@ClientApi\Session\LoadingEntities.py /}

{PANEL/}

<!-- 
{PANEL:stream}

Entities can be streamed from the server using one of the following `stream` methods from the `advanced` session operations.

Streaming query results does not support the [`include` feature](../../client-api/how-to/handle-document-relationships#includes). 
Learn more in [How to Stream Query Results](../../client-api/session/querying/how-to-stream-query-results).  

{INFO Entities loaded using `stream` will be transient (not attached to session). /}

{CODE:python loading_entities_5_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **starts_with** | `str` | prefix for which documents should be streamed |
| **matches** | `str` | pipe ('&#124;') separated values for which document IDs should be matched ('?' any single character, '*' any characters) |
| **start** | `int` | number of documents that should be skipped  |
| **page_size** | `int` | maximum number of documents that will be retrieved |
| **start_after** | `str` | skip document fetching until a given ID is found and returns documents after that ID (default: `None`) |
| **stream_query_stats** | `streamQueryStats` (out parameter) | Information about the streaming query (amount of results, which index was queried, etc.) |

| Return Type | Description |
| ------------- | ----- |
| `IEnumerator<`[StreamResult](../../glossary/stream-result)`>` | Enumerator with entities. |
| `streamQueryStats` (out parameter) | Information about the streaming query (amount of results, which index was queried, etc.) |


### Example I

Stream documents for a ID prefix:

{CODE:python loading_entities_5_1@ClientApi\Session\LoadingEntities.py /}

## Example 2

Fetch documents for a ID prefix directly into a stream:

{CODE:python loading_entities_5_2@ClientApi\Session\LoadingEntities.py /}

{PANEL/}
-->

{PANEL:is_loaded}

Use the `is_loaded` method from the `advanced` session operations
To check if an entity is attached to a session (e.g. because it's been 
previously loaded).  
  
{NOTE: }
`is_loaded` checks if an attempt to load a document has been already made 
during the current session, and returns `True` even if such an attemp was 
made and failed.  
If, for example, the `load` method was used to load `employees/3` during 
this session and failed because the document has been previously deleted, 
`is_loaded` will still return `True` for `employees/3` for the remainder 
of the session just because of the attempt to load it.  
{NOTE/}

{CODE:python loading_entities_6_0@ClientApi\Session\LoadingEntities.py /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **key** | `str` | ID of the entity whose status is checked |

| Return Type | Description |
| ------------- | ----- |
| `bool` | Indicates if the given entity is loaded |

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
- [Querying an Index](../../indexes/querying/query-index)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
