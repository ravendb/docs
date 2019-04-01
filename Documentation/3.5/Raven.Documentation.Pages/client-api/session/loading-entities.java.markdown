# Session: Loading entities

There are various methods with many overloads that allow user to download a documents from database and convert them to entities. This article will cover following methods:

- [load](../../client-api/session/loading-entities#load)
- [load with Includes](../../client-api/session/loading-entities#load-with-includes)
- [load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
- [loadStartingWith](../../client-api/session/loading-entities#loadstartingwith)
- [stream](../../client-api/session/loading-entities#stream)
- [isLoaded](../../client-api/session/loading-entities#isloaded)

{PANEL:Load}

The most basic way to load single entity is to use one of `load` methods.

{CODE:java loading_entities_1_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String/UUID/Number | Identifier of a document that will be loaded. |
| **transformer or transformerType** | String or Class | Name or class of a transformer to use on a loaded document. |
| **configure** | LoadConfigurationFactory  | Additional configuration that should be used during operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult | Instance of `TResult` or `null` if document with given Id does not exist. |

### Example I

{CODE:java loading_entities_1_1@ClientApi\Session\LoadingEntities.java /}

### Example II

{CODE:java loading_entities_1_2@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

{PANEL:Load with includes}

When there is a 'relationship' between documents, then those documents can be loaded in a single request call using `include + load` methods.

{CODE:java loading_entities_2_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | String or Path | Path in documents in which server should look for a 'referenced' documents. |

| Return Value | |
| ------------- | ----- |
| ILoaderWithInclude | `include` method by itself does not materialize any requests, but returns loader containing methods such as `load`. |

### Example I

{CODE:java loading_entities_2_1@ClientApi\Session\LoadingEntities.java /}

### Example II

{CODE:java loading_entities_2_2@ClientApi\Session\LoadingEntities.java /}

### Example III

{CODE:java loading_entities_2_3@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

{PANEL:Load - multiple entities}

To load multiple entities at once use one of the following `Load` overloads.

{CODE:java loading_entities_3_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **ids** | Collection&lt;String&gt;, String[], UUID[], Number[] | Iterable or array of ids that should be loaded. |
| **transformer or transformerType** | String or Class | Name or class of a transformer to use on a loaded documents. |
| **configure** | LoadConfigurationFactory  | Additional configuration that should be used during operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult[] | Array of entities in the exact same order as given ids. If document does not exist, value at the appropriate position in array will be `null`. |

### Example I

{CODE:java loading_entities_3_1@ClientApi\Session\LoadingEntities.java /}

### Example II

{CODE:java loading_entities_3_2@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

{PANEL:LoadStartingWith}

To load multiple entities that contain common prefix use `loadStartingWith` method from `advanced()` session operations.

{CODE:java loading_entities_4_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keyPrefix** | String |  prefix for which documents should be returned  |
| **matches** | String | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters). Default: `null` |
| **start** | int | number of documents that should be skipped. Default: `0` |
| **pageSize** | int | maximum number of documents that will be retrieved. Default: `25` |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on server side. Default: `null` |
| **exclude** | String | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters). Default: `null` |
| **configure** | LoadConfigurationFactory | Additional configuration that should be used during operation e.g. transformer parameters can be added. Default: `null` |

| Return Value | |
| ------------- | ----- |
| TResult[] | Array of entities matching given parameters. |

### Example I

{CODE:java loading_entities_4_1@ClientApi\Session\LoadingEntities.java /}

### Example II

{CODE:java loading_entities_4_2@ClientApi\Session\LoadingEntities.java /}

### Example III

{CODE:java loading_entities_4_3@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

{PANEL:Stream}

Entities can be streamed from server using one of the following `stream` methods from `advanced()` session operations.

{CODE:java loading_entities_5_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fromEtag** | Etag | ETag of a document from which stream should start (mutually exclusive with 'startsWith') |
| **startsWith** | String | prefix for which documents should be streamed (mutually exclusive with 'fromEtag'). Default: `null` |
| **matches** | String | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters). Default: `null` |
| **start** | int | number of documents that should be skipped. Default: `0` |
| **pageSize** | int | maximum number of documents that will be retrieved. Default: `Integer.MAX_VALUE`|
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on server side. Default: `null` |

| Return Value | |
| ------------- | ----- |
| CloseableIterator&lt;StreamResult&lt;T&gt;&gt; | Enumerator with entities. |

### Example I

{CODE:java loading_entities_5_1@ClientApi\Session\LoadingEntities.java /}

### Remarks

{INFO Entities loaded using `stream` will be transient (not attached to session). /}

{WARNING:Caution}

**fromEtag** does not do any filtrations on server-side based on the specified in streaming function return type (e.g. `Employee`) and will return all documents from given Etag even if their `Raven-Clr-Type` does not match the return type, which may cause potential casting errors. Set return type to `Object` or `RavenJObject` to address situation when documents with different types might be returned.

{WARNING/}

{PANEL/}

{PANEL:IsLoaded}

To check if entity is attached to session, e.g. has been loaded previously, use `isLoaded` method from `advanced()` session operations.

{CODE:java loading_entities_6_0@ClientApi\Session\LoadingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | String | Entity Id for which check should be performed. |

| Return Value | |
| ------------- | ----- |
| boolean | Indicates if entity with given Id is loaded. |

### Example

{CODE:java loading_entities_6_1@ClientApi\Session\LoadingEntities.java /}

{PANEL/}

## Related articles

- [Opening a session](./opening-a-session)  
