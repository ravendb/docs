# Session : Loading entities

There are various methods with many overloads that allow user to download a documents from database and convert them to entities. This article will cover following methods:

- [Load](../../client-api/session/loading-entities#load)
- [Load with Includes](../../client-api/session/loading-entities#load-with-includes)
- [Load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
- [LoadStartingWith](../../client-api/session/loading-entities#loadstartingwith)
- [Stream](../../client-api/session/loading-entities#stream)
- [IsLoaded](../../client-api/session/loading-entities#isloaded)

{PANEL:Load}

The most basic way to load single entity is to use one of `Load` methods.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_1_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_1_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string or ValueType | Identifier of a document that will be loaded. |

| Return Value | |
| ------------- | ----- |
| TResult | Instance of `TResult` or `null` if document with given Id does not exist. |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_1_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_1_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

{NOTE In 4.x RavenDB, only string identifiers are supported. If you are upgrading from 3.x, this is a major change, because in 3.x non-string identifiers are supported. /}

{PANEL/}

{PANEL:Load with Includes}

When there is a 'relationship' between documents, then those documents can be loaded in a single request call using `Include + Load` methods.

{CODE loading_entities_2_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string or Expression | Path in documents in which server should look for a 'referenced' documents. |

| Return Value | |
| ------------- | ----- |
| ILoaderWithInclude | `Include` method by itself does not materialize any requests, but returns loader containing methods such as `Load`. |

### Example I

We can use this code to load also an employee which made the order.

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

To load multiple entities at once use one of the following `Load` overloads.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_3_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_3_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

| Parameters | | |
| ------------- | ------------- | ----- |
| **ids** | IEnumerable<string> | Multiple document identifiers to load |

| Return Value | |
| ------------- | ----- |
| Dictionary<string, TResult> | Instance of Dictionary which maps document identifiers to `TResult` or `null` if document with given Id does not exist. |

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_3_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_3_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/} 

{PANEL/}

{PANEL:LoadStartingWith}

To load multiple entities that contain common prefix use `LoadStartingWith` method from `Advanced` session operations.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_4_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_4_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keyPrefix** | string |  prefix for which documents should be returned  |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **exclude** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **skipAfter** | string | skip document fetching until given key is found and return documents after that key (default: `null`) |

| Return Value | |
| ------------- | ----- |
| TResult[] | Array of entities matching given parameters. |
| Stream | Output entities matching given parameters as a stream. |

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

{PANEL:Stream}

Entities can be streamed from server using one of the following `Stream` methods from `Advanced` session operations.

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_5_0@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_5_0_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

| Parameters | | |
| ------------- | ------------- | ----- |
| **startsWith** | string | prefix for which documents should be streamed (mutually exclusive with 'fromEtag') |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **skipAfter** | string | skip document fetching until given key is found and return documents after that key (default: `null`) |
| streamQueryStats (out parameter) | Information about the streaming query (amount of results, which index was queried, etc. |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult](../../glossary/stream-result)> | Enumerator with entities. |
| streamQueryStats (out parameter) | Information about the streaming query (amount of results, which index was queried, etc. |


### Example I

Stream documents for a Id prefix

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_5_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_5_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

## Example 2
Fetch documents for a Id prefix directly into a stream
{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_5_2@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_5_2_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

### Remarks

{INFO Entities loaded using `Stream` will be transient (not attached to session). /}

{PANEL/}

{PANEL:IsLoaded}

To check if entity is attached to session, e.g. has been loaded previously, use `IsLoaded` method from `Advanced` session operations.

{CODE loading_entities_6_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | Entity Id for which check should be performed. |

| Return Value | |
| ------------- | ----- |
| bool | Indicates if entity with given Id is loaded. |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync loading_entities_6_1@ClientApi\Session\LoadingEntities.cs /}
{CODE-TAB:csharp:Async loading_entities_6_1_async@ClientApi\Session\LoadingEntities.cs /}
{CODE-TABS/}

{PANEL/}

## Related articles

- [Opening a session](./opening-a-session)  
