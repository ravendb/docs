# Session: Loading Entities

There are various methods with many overloads that allow users to download documents from a database and convert them to entities. This article will cover the following methods:

- [Load](../../client-api/session/loading-entities#load)
- [Load with Includes](../../client-api/session/loading-entities#load-with-includes)
- [Load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
- [LoadStartingWith](../../client-api/session/loading-entities#loadstartingwith)
- [IsLoaded](../../client-api/session/loading-entities#isloaded)
- [Stream](../../client-api/session/loading-entities#stream)

{PANEL:Load}

The most basic way to load a single entity is to use one of the `load` methods.

{CODE:java loading_entities_1_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | Identifier of a document that will be loaded. |

| Return Value | |
| ------------- | ----- |
| T | Instance of `T` or `null` if a document with a given ID does not exist. |

### Example

{CODE:java loading_entities_1_1@ClientApi\Session\LoadingEntities.java /}

{NOTE In 4.x RavenDB, only string identifiers are supported. If you are upgrading from 3.x, this is a major change, because in 3.x non-string identifiers are supported. /}

{PANEL/}

{PANEL:Load with Includes}

When there is a 'relationship' between documents, those documents can be loaded in a single request call using the `include + load` methods.

{CODE:java loading_entities_2_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | String | Path in documents in which the server should look for 'referenced' documents. |

| Return Value | |
| ------------- | ----- |
| ILoaderWithInclude | The `include` method by itself does not materialize any requests but returns loader containing methods such as `load`. |

### Example I

We can use this code to also load an employee which made the order.

{CODE:java loading_entities_2_1@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

{PANEL:Load - multiple entities}

To load multiple entities at once, use one of the following `load` overloads.

{CODE:java loading_entities_3_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **ids** | Collection&lt;String&gt; or String... | Multiple document identifiers to load |

| Return Value | |
| ------------- | ----- |
| Map<String, T> | Instance of Map which maps document identifiers to `T` or `null` if a document with given ID doesn't exist. |

{CODE:java loading_entities_3_1@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

{PANEL:LoadStartingWith}

To load multiple entities that contain a common prefix, use the `loadStartingWith` method from the `advanced` session operations.

{CODE:java loading_entities_4_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **idPrefix** | String |  prefix for which the documents should be returned  |
| **matches** | String | pipe ('&#124;') separated values for which document IDs (after 'idPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **exclude** | String | pipe ('&#124;') separated values for which document IDs (after 'idPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **skipAfter** | String | skip document fetching until given ID is found and return documents after that ID (default: `null`) |

| Return Value | |
| ------------- | ----- |
| T[] | Array of entities matching given parameters. |

### Example I

{CODE:java loading_entities_4_1@ClientApi\Session\LoadingEntities.java /}

### Example II

{CODE:java loading_entities_4_2@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

{PANEL:Stream}

Entities can be streamed from the server using one of the following `stream` methods from the `advanced` session operations.

{CODE:java loading_entities_5_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **startsWith** | String | prefix for which documents should be streamed |
| **matches** | String | pipe ('&#124;') separated values for which document IDs should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **skipAfter** | String | skip document fetching until a given ID is found and returns documents after that ID (default: `null`) |
| Reference streamQueryStats (out parameter) | Information about the streaming query (amount of results, which index was queried, etc.) |

| Return Value | |
| ------------- | ----- |
| CloseableIterator<StreamResult<T>> | Iterator with entities. |
| streamQueryStats (out parameter) | Information about the streaming query (amount of results, which index was queried, etc.) |


### Example I

Stream documents for a ID prefix:

{CODE:java loading_entities_5_1@ClientApi\Session\LoadingEntities.java /}

## Example 2

Fetch documents for a ID prefix directly into a stream:

{CODE:java loading_entities_5_2@ClientApi\Session\LoadingEntities.java /}

### Remarks

{INFO Entities loaded using `stream` will be transient (not attached to session). /}

{PANEL/}

{PANEL:IsLoaded}

To check if an entity is attached to a session, e.g. it has been loaded previously, use the `isLoaded` method from the `advanced` session operations.

{CODE:java loading_entities_6_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | Entity ID for which the check should be performed. |

| Return Value | |
| ------------- | ----- |
| boolean | Indicates if an entity with a given ID is loaded. |

### Example

{CODE:java loading_entities_6_1@ClientApi\Session\LoadingEntities.java /}

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
