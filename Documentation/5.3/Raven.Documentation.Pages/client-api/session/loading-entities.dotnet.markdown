# Session: Loading Entities
---

{NOTE: }

There are several methods with many overloads that allow users to download documents 
from the database and convert them to entities. This article will cover the following 
methods:  

- [Load](../../client-api/session/loading-entities#load)
- [Load with Includes](../../client-api/session/loading-entities#load-with-includes)
- [Load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
- [LoadStartingWith](../../client-api/session/loading-entities#loadstartingwith)
- [ConditionalLoad](../../client-api/session/loading-entities#conditionalload)
- [Stream](../../client-api/session/loading-entities#stream)
- [IsLoaded](../../client-api/session/loading-entities#isloaded)

{NOTE/}

---

{PANEL:Load}

The most basic way to load a single entity is to use one of the `Load` methods.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_1_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_1_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | Identifier of a document that will be loaded. |

| Return Type | Description |
| ------------- | ----- |
| `TResult` | Instance of `TResult` or `null` if a document with a given ID does not exist. |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_1_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_1_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

{NOTE From RavenDB version 4.x onwards, only string identifiers are supported. If you are upgrading from 3.x, this is a major change, because in 3.x non-string identifiers are supported. /}

{PANEL/}

{PANEL:Load with Includes}

When there is a 'relationship' between documents, those documents can be loaded in a 
single request call using the `Include + Load` methods. Learn more in 
[How To Handle Document Relationships](../../client-api/how-to/handle-document-relationships).  

{NOTE: }
Also see:  

* [Including Counters](../../document-extensions/counters/counters-and-other-features#including-counters)  
* [Including Time Series](../../document-extensions/timeseries/client-api/session/include/overview)  
* [Including Compare Exchange Values](../../client-api/operations/compare-exchange/include-compare-exchange)  
* [Including Document Revisions](../../document-extensions/revisions/client-api/session/including)  
{NOTE/}

{CODE loading_entities_2_0@ClientApi\Session\LoadingEntities.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **path** | `string` or Expression | Path in documents in which the server should look for 'referenced' documents. |

| Return Type | Description |
| ------------- | ----- |
| `ILoaderWithInclude` | The `Include` method by itself does not materialize any requests but returns loader containing methods such as `Load`. |

### Example I

We can use this code to also load an employee which made the order.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_2_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_2_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

### Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_2_2@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_2_2_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

{PANEL/}

{PANEL:Load - multiple entities}

To load multiple entities at once, use one of the following `Load` overloads.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_3_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_3_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **ids** | `IEnumerable<string>` | Multiple document identifiers to load |

| Return Type | Description |
| ------------- | ----- |
| `Dictionary<string, TResult>` | Instance of Dictionary which maps document identifiers to `TResult` or `null` if a document with given ID doesn't exist. |

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_3_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_3_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

{PANEL/}

{PANEL:LoadStartingWith}

To load multiple entities that contain a common prefix, use the `LoadStartingWith` method from the `Advanced` session operations.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_4_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_4_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

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
| `Stream` | Output entities matching given parameters as a stream. |

### Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_4_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_4_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

### Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_4_2@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_4_2_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: ConditionalLoad}

This method can be used to check whether a document has been modified 
since the last time its change vector was recorded, so that the cost of loading it 
can be saved if it has not been modified.  

The `ConditionalLoad` method takes a document's [change vector](../../server/clustering/replication/change-vector). 
If the entity is tracked by the session, this method returns the entity. If the entity 
is not tracked, it checks if the provided change vector matches the document's 
current change vector on the server side. If they match, the entity is not loaded. 
If the change vectors _do not_ match, the document is loaded.  

The method is accessible from the `session.Advanced` operations.  

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_7_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_7_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | The identifier of a document to be loaded. |
| **changeVector** | `string` | The change vector you want to compare with the server-side change vector. If the change vectors match, the document is not loaded. |

| Return Type | Description |
| ------------- | ----- |
| ValueTuple `(T Entity, string ChangeVector)` | If the given change vector and the server side change vector do not match, the method returns the requested entity and its current change vector.<br/>If the change vectors match, the method returns `default` as the entity, and the current change vector.<br/>If the specified document, the method returns only `default` without a change vector. |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_7_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_7_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

{PANEL/}

{PANEL:Stream}

Entities can be streamed from the server using one of the following `Stream` methods from the `Advanced` session operations.

Streaming query results does not support the [`include` feature](../../client-api/how-to/handle-document-relationships#includes). 
Learn more in [How to Stream Query Results](../../client-api/session/querying/how-to-stream-query-results).  

{INFO Entities loaded using `Stream` will be transient (not attached to session). /}

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_5_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_5_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

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

Stream documents for a ID prefix:

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_5_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_5_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

## Example 2

Fetch documents for a ID prefix directly into a stream:

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_5_2@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_5_2_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:IsLoaded}

To check if an entity is attached to a session, e.g. it has been loaded previously, use the `IsLoaded` method from the **Advanced** session operations.  
  
`IsLoaded` checks if you've already tried to load a document during the current session.  
If you try to load a document that no longer exists with the `Load` method (perhaps it has been deleted),  
`IsLoaded` will then return `true` because `IsLoaded` shows that you've already tried to load the non-existent document.  

{CODE loading_entities_6_0@ClientApi\Session\LoadingEntities.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | Entity ID for which the check should be performed. |

| Return Type | Description |
| ------------- | ----- |
| `bool` | Indicates if an entity with a given ID is loaded. |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_6_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_6_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Deleting Entities](../../client-api/session/deleting-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Basics](../../indexes/querying/basics)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
