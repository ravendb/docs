# Session: Loading Entities
---

{NOTE: }

* There are several methods that allow users to load documents from the database and convert them to entities.

* This article covers the following methods:

  - [`load`](../../client-api/session/loading-entities#load)
  - [`load` with `include`](../../client-api/session/loading-entities#load-with-include)
  - [`load` - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
  - [`loadStartingWith`](../../client-api/session/loading-entities#loadstartingwith)
  - [`conditionalLoad`](../../client-api/session/loading-entities#conditionalload)
  - [`isLoaded`](../../client-api/session/loading-entities#isloaded)

* For loading entities lazily see [perform requests lazily](../../client-api/session/how-to/perform-operations-lazily).

{NOTE/}

---

{PANEL:`load`}

The most basic way to load a single entity is to use the `load` method.

{CODE:php loading_entities_1_0@ClientApi\Session\LoadingEntities.php /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | An ID to load a single entity by |
| **className** | `string` | What entity type to load |

| Return Type | Description |
| ------------- | ----- |
| `?object` | The loaded entity, or `null` if an entity with the given ID doesn't exist |

### Example

{CODE:php loading_entities_1_1@ClientApi\Session\LoadingEntities.php /}

{NOTE: }
Starting with RavenDB version 4.x, only string identifiers are supported.  
If you are upgrading from 3.x, this is a major change since in `3.x`` non-string 
identifiers were supported. 
{NOTE/}

{PANEL/}

{PANEL:`load` with `include`}

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

{CODE:php loading_entities_2_0@ClientApi\Session\LoadingEntities.php /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **path** | `string` | A path that the server should search the referenced documents by |
| **className** | `string` | What entity type to load |
| **id** | `string` | An ID to load a single entity by |
| **ids** | `array`/`StringArray` | An array of IDs to load entities by |

| Return Type | Description |
| ------------- | ----- |
| `?object` | The loaded entity, or `null` if an entity with the given ID doesn't exist |
| `ObjectArray` | An array of loaded entities |
| `LoaderWithIncludeInterface` | The `include` method doesn't satidfy requests directly but returns an interface that can be used |

### Example I

We can use this code to also load an employee which made the order.

{CODE:php loading_entities_2_1@ClientApi\Session\LoadingEntities.php /}

### Example II

{CODE:php loading_entities_2_2@ClientApi\Session\LoadingEntities.php /}

{PANEL/}

{PANEL:`load` - multiple entities}

To load multiple entities at once, use one of the following `load` overloads.

{CODE:php loading_entities_3_0@ClientApi\Session\LoadingEntities.php /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **className** | `string` | What entity type to load |
| **ids** | `array`/`StringArray` | An array of IDs to load entities by |

| Return Type | Description |
| ------------- | ----- |
| `ObjectArray` | An array of loaded entities |

{CODE:php loading_entities_3_1@ClientApi\Session\LoadingEntities.php /}

{PANEL/}

{PANEL:`loadStartingWith`}

To load multiple entities that contain a common prefix, use the `loadStartingWith` method from the `advanced` session operations.

{CODE:php loading_entities_4_0@ClientApi\Session\LoadingEntities.php /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **className** | `string` | What entity type to load |
| **idPrefix** | `string` | ID prefix: documents will be retrieved if their ID starts with the given prefix |
| **matches** | `string` | pipe (\|) separated values, that document IDs (after 'idPrefix') should match.<br>`?` - any single character<br>`*` - any string of characters |
| **start** | `int` | number of documents to skip |
| **pageSize** | `int` | maximum number of documents to retrieve |
| **exclude** | `string` | pipe (\|) separated values, that document IDs (after 'idPrefix') should **not** match.<br>`?` - any single character<br>`*` - any string of characters |
| **startAfter** | `string` | skip fetching document until the given ID is found, and return documents after this ID (default: `null`) |

| Return Type | Description |
| ------------- | ----- |
| `ObjectArray` | an array of entities matching the given parameters |

### Example I

{CODE:php loading_entities_4_1@ClientApi\Session\LoadingEntities.php /}

### Example II

{CODE:php loading_entities_4_2@ClientApi\Session\LoadingEntities.php /}

{PANEL/}

{PANEL: `conditionalLoad`}

This method can be used to check whether a document has been modified 
since the last time its change vector was recorded, so that the cost of loading it 
can be saved if it has not been modified.  

The `conditionalLoad` method takes a document's [change vector](../../server/clustering/replication/change-vector). 
If the entity is tracked by the session, this method returns the entity. If the entity 
is not tracked, it checks if the provided change vector matches the document's 
current change vector on the server side. If they match, the entity is not loaded. 
If the change vectors _do not_ match, the document is loaded.  

The method is accessible from the session `advanced` operations.  

{CODE:php loading_entities_7_0@ClientApi\Session\LoadingEntities.php /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **className** | `string` | What entity type to load |
| **id** | `string` | The identifier of a document to load |
| **changeVector** | `string` | The change vector you want to compare with the server-side change vector. If the change vectors match, the document is not loaded. |

| Return Type | Description |
| ------------- | ----- |
| `ConditionalLoadResult` | If the given change vector and the server side change vector do not match, the method returns the requested entity and its current change vector.<br/>If the change vectors match, the method returns `default` as the entity, and the current change vector.<br/>If the specified document, the method returns only `default` without a change vector. |

### Example

{CODE:php loading_entities_7_1@ClientApi\Session\LoadingEntities.php /}

{PANEL/}

{PANEL:isLoaded}

Use the `isLoaded` method from the `advanced` session operations
To check if an entity is attached to a session (e.g. because it's been 
previously loaded).  

{NOTE: }
`isLoaded` checks if an attempt to load a document has been already made 
during the current session, and returns `true` even if such an attemp was 
made and failed.  
If, for example, the `load` method was used to load `employees/3` during 
this session and failed because the document has been previously deleted, 
`isLoaded` will still return `true` for `employees/3` for the remainder 
of the session just because of the attempt to load it.  
{NOTE/}

{CODE:php loading_entities_6_0@ClientApi\Session\LoadingEntities.php /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **id** | `string` | The ID of the entity to perform the check for |

| Return Type | Description |
| ------------- | ----- |
| `bool` | Indicates whether an entity with a given ID is loaded |

### Example

{CODE:php loading_entities_6_1@ClientApi\Session\LoadingEntities.php /}

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
