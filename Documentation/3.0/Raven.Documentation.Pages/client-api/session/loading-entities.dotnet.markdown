# Session : Loading entities

There are various methods with many overloads that allow user to download a documents from database and convert them to entities. This article will cover following methods:

- [Load]()
- [Load with Includes]()
- [Load - multiple entities]()
- [LoadStartingWith]()
- [Stream]()
- [IsLoaded]()

{PANEL:Load}

The most basic way to load single entity is to use one of `Load` methods.

{CODE loading_entities_1_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string or ValueType | Identifier of a document that will be loaded. |
| **transformer or transformerType** | string or Type | Name or type of a transformer to use on a loaded document. |
| **configure** | Action<[ILoadConfiguration]()> | Additional configuration that should be used during operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult | Instance of `TResult` or `null` if document with given Id does not exist. |

### Example I

{CODE loading_entities_1_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_1_2@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

{PANEL:Load with Includes}

When there is a 'relationship' between documents, then those documents can be loaded in a single request call using `Include + Load` methods.

{CODE loading_entities_2_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string or Expression | Path in documents in which server should look for a 'referenced' documents. |

| Return Value | |
| ------------- | ----- |
| [ILoaderWithInclude]() | `Include` method by itself does not materialize any requests, but returns loader containing methods such as `Load`. |

### Example I

{CODE loading_entities_2_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_2_2@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

{PANEL:Load - multiple entities}

To load multiple entities at once use one of the following `Load` overloads.

{CODE loading_entities_3_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **ids** | IEnumerable<string> or IEnumerable<ValueType> or ValueType[] | Enumerable or array of Ids that should be loaded. |
| **transformer or transformerType** | string or Type | Name or type of a transformer to use on a loaded documents. |
| **configure** | Action<[ILoadConfiguration]()> | Additional configuration that should be used during operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult[] | Array of entities in the exact same order as given ids. If document does not exist, value at the appropriate position in array will be `null`. |

### Example I

{CODE loading_entities_3_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_3_2@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

{PANEL:LoadStartingWith}

To load multiple entities that contain common prefix use `LoadStartingWith` method from `Advanced` session operations.

{CODE loading_entities_4_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keyPrefix** | string |  prefix for which documents should be returned  |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on server side |
| **exclude** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **configure** | Action<[ILoadConfiguration]()> | Additional configuration that should be used during operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult[] | Array of entities matching given parameters. |

### Example I

{CODE loading_entities_4_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_4_2@ClientApi\Session\LoadingEntities.cs /}

### Example III

{CODE loading_entities_4_3@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

{PANEL:Stream}

Entities can be streamed from server using one of the following `Stream` methods from `Advanced` session operations.

{CODE loading_entities_5_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fromEtag** | Etag | ETag of a document from which stream should start (mutually exclusive with 'startsWith') |
| **startsWith** | string | prefix for which documents should be streamed (mutually exclusive with 'fromEtag') |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on server side |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult]()> | Enumerator with entities. |

### Example I

{CODE loading_entities_5_1@ClientApi\Session\LoadingEntities.cs /}

### Remarks

Entities loaded using `Stream` will be transient (not attached to session).

{PANEL/}

{PANEL:IsLoaded}

To check if entity is attached to sessiong, e.g. has been loaded previously, use `IsLoaded` method from `Advanced` session operations.

{CODE loading_entities_6_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | Entity Id for which check should be performed. |

| Return Value | |
| ------------- | ----- |
| bool | Indicates if entity with given Id is loaded. |

### Example

{CODE loading_entities_6_1@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

## Related articles

TODO