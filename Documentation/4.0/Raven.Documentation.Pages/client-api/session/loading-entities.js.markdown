# Session: Loading Entities

There are various methods with many overloads that allow users to download documents from a database and convert them to entities. This article will cover the following methods:

- [Load](../../client-api/session/loading-entities#load)
- [Load with Includes](../../client-api/session/loading-entities#load-with-includes)
- [Load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
- [LoadStartingWith](../../client-api/session/loading-entities#loadstartingwith)
- [IsLoaded](../../client-api/session/loading-entities#isloaded)
- [Stream](../../client-api/session/loading-entities#stream)

{PANEL:Load}

The most basic way to load a single entity is to use session's `load()` method.

{CODE:nodejs loading_entities_1_0@client-api\session\loadingEntities.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | Identifier of a document that will be loaded. |
| **documentType** | function | A class constructor used for reviving the results' entities |
| **callback** | function | error-first callback, returns loaded document |

| Return Value | |
| ------------- | ----- |
| `Promise<object>` | A `Promise` returning `object` or `null` if a document with a given ID does not exist. |

### Example

{CODE:nodejs loading_entities_1_1@client-api\session\loadingEntities.js /}

{NOTE In 4.x RavenDB, only string identifiers are supported. If you are upgrading from 3.x, this is a major change, because in 3.x non-string identifiers are supported. /}

{PANEL/}

{PANEL:Load with Includes}

When there is a *relationship* between documents, those documents can be loaded in a single request call using the `include()` and `load()` methods.

{CODE:nodejs loading_entities_2_0@client-api\session\loadingEntities.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | Field path in documents in which the server should look for 'referenced' documents. |

| Return Value | |
| ------------- | ----- |
| `object { load() }` | The `include()` method by itself does not materialize any requests but returns loader containing methods such as `load()`. |

### Example I

We can use this code to also load an employee which made the order.

{CODE:nodejs loading_entities_2_1@client-api\session\loadingEntities.js /}

{PANEL/}

{PANEL:Load - multiple entities}

To load multiple entities at once, use one of the following ways to call `load()`.

{CODE:nodejs loading_entities_3_0@client-api\session\loadingEntities.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **idsArray** | string[] | Multiple document identifiers to load |
| **documentType** | function | A class constructor used for reviving the results' entities |
| **options** | string | Options with the following properties |
| &nbsp;&nbsp;*documentType* | function | A class construcor used for reviving the results' entities |
| &nbsp;&nbsp;*includes* | string[] | Field paths in documents in which the server should look for 'referenced' documents. |
| **callback** | function | error-first callback, returns an object mapping document identifiers to `object` or `null` if a document with given ID doesn't exist (see Return Value below) |

| Return Value | |
| ------------- | ----- |
| `Promise<{ [id]: object }>` | A `Promise` resolving to an object mapping document identifiers to `object` or `null` if a document with given ID doesn't exist |

{CODE:nodejs loading_entities_3_1@client-api\session\loadingEntities.js /}

{PANEL/}

{PANEL:LoadStartingWith}

To load multiple entities that contain a common prefix, use the `loadStartingWith()` method from the `advanced` session operations.

{CODE:nodejs loading_entities_4_0@client-api\session\loadingEntities.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **idPrefix** | string | prefix for which the documents should be returned  |
| **options** | string | Options with the following properties |
| &nbsp;&nbsp;*matches* | string | pipe ('&#124;') separated values for which document IDs (after 'idPrefix') should be matched ('?' any single character, '*' any characters) |
| &nbsp;&nbsp;*start* | number | number of documents that should be skipped  |
| &nbsp;&nbsp;*pageSize* | number | maximum number of documents that will be retrieved |
| &nbsp;&nbsp;*exclude* | string | pipe ('&#124;') separated values for which document IDs (after 'idPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| &nbsp;&nbsp;*skipAfter* | string | skip document fetching until given ID is found and return documents after that ID (default: `null`) |
| &nbsp;&nbsp;*documentType* | function | A class constructor used for reviving the results' entities |
| **callback** | function | error-first callback, returns an array of entities matching given parameters (see Return Value below) |

| Return Value | |
| ------------- | ----- |
| `Promise<object[]>` | A `Promise` resolving to an array of entities matching given parameters |

### Example I

{CODE:nodejs loading_entities_4_1@client-api\session\loadingEntities.js /}

### Example II

{CODE:nodejs loading_entities_4_2@client-api\session\loadingEntities.js /}

{PANEL/}

{PANEL:Stream}

Entities can be streamed from the server using the `stream()` method from the `advanced` session operations.

{CODE:nodejs loading_entities_5_0@client-api\session\loadingEntities.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **idPrefix** | string | prefix for which the documents should be returned  |
| **query** | query object | a query obtained from a call to `session.query()` or `session.advanced.rawQuery()` |
| **options** | string | Options with the following properties |
| &nbsp;&nbsp;*startsWith* | string | prefix for which documents should be streamed |
| &nbsp;&nbsp;*matches* | string | pipe ('&#124;') separated values for which document IDs should be matched ('?' any single character, '*' any characters) |
| &nbsp;&nbsp;*start* | number | number of documents that should be skipped  |
| &nbsp;&nbsp;*pageSize* | number | maximum number of documents that will be retrieved |
| &nbsp;&nbsp;*skipAfter* | string | skip document fetching until a given ID is found and returns documents after that ID (default: `null`) |
| &nbsp;&nbsp;*documentType* | function | A class constructor used for reviving the results' entities |
| **statsCallback** | function | callback returning information about the streaming query (amount of results, which index was queried, etc.) |
| **callback** | function | returns a readable stream with query results (same as Return Value result below) |

| Return Value | |
| ------------- | ----- |
| `Promise<Readable>` | A `Promise` resolving to readable stream with query results |


### Example I

Stream documents for a ID prefix:

{CODE:nodejs loading_entities_5_1@client-api\session\loadingEntities.js /}

### Example 2

Fetch documents for a ID prefix directly into a writable stream:

{CODE:nodejs loading_entities_5_2@client-api\session\loadingEntities.js /}

{INFO Entities loaded using `stream()` will be transient (not attached to session). /}

{PANEL/}

{PANEL:IsLoaded}

To check if an entity is attached to a session, e.g. it has been loaded previously, use the `isLoaded()` method from the `advanced` session operations.

{CODE:nodejs loading_entities_6_0@client-api\session\loadingEntities.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | Entity ID for which the check should be performed. |

| Return Value | |
| ------------- | ----- |
| boolean | Indicates if an entity with a given ID is loaded. |

### Example

{CODE:nodejs loading_entities_6_1@client-api\session\loadingEntities.js /}

{PANEL/}

### On entities loading, JS classes and the&nbsp;*documentType*&nbsp;parameter

Type information about the entity and its contents is by default stored in the document metadata. Based on that its types are revived when loaded from the server.

{INFO: Entity type registration }
In order to avoid passing **documentType** argument every time, you can register the type in the document conventions using the `registerEntityType()` method before calling DocumentStore's `initialize()` like so:

{CODE:nodejs query_1_8@client-api\session\querying\howToQuery.js /}

{INFO/}

If you fail to do so, entities (and all subobjects) loaded from the server are going to be plain object literals and not instances of the original type they were stored with.

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
