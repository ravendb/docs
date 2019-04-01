# Session: Loading entities

There are various methods with many overloads that allow user to download a documents from database and convert them to entities. This article will cover following methods:

- [Load](../../client-api/session/loading-entities#load)
- [Load with Includes](../../client-api/session/loading-entities#load-with-includes)
- [Load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
- [LoadStartingWith](../../client-api/session/loading-entities#loadstartingwith)
- [Stream](../../client-api/session/loading-entities#stream)
- [IsLoaded](../../client-api/session/loading-entities#isloaded)

{PANEL:Load}

The most basic way to load single entity is to use one of `Load` methods.

{CODE loading_entities_1_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string or ValueType | Identifier of a document that will be loaded. |
| **transformer or transformerType** | string or Type | Name or type of a transformer to use on a loaded document. |
| **configure** | Action<ILoadConfiguration> | Additional configuration that should be used during operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult | Instance of `TResult` or `null` if document with given Id does not exist. |

### Example I

{CODE loading_entities_1_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_1_2@ClientApi\Session\LoadingEntities.cs /}

### Non string identifiers

The above examples show how to load a document by its string key. What if a type of your entity's identifier is not a string? You need to use an overload of the `Load` method that takes a parameter which type is `ValueType`. 

### Example III

If you have an entity where an identifier is an integer number:

{CODE class_with_interger_id@ClientApi\Session\LoadingEntities.cs /}

you can load it by specifying an integer value as the identifier:

{CODE loading_entities_1_3@ClientApi\Session\LoadingEntities.cs /}

####Converting to string identifier

Even if the identifier is a string, you can use the `Load<T>(ValueType)` overload. The client is aware of the [ID generation conventions](../configuration/conventions/identifier-generation/global) (collectionName/number),
so you could load the entity with key `employees/1` by using the code:

{CODE loading_entities_1_4@ClientApi\Session\LoadingEntities.cs /}

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

{CODE loading_entities_2_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_2_2@ClientApi\Session\LoadingEntities.cs /}

### Example III

{CODE loading_entities_2_3@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

{PANEL:Load - multiple entities}

To load multiple entities at once use one of the following `Load` overloads.

{CODE loading_entities_3_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **ids** | IEnumerable<string> or IEnumerable<ValueType> or ValueType[] | Enumerable or array of Ids that should be loaded. |
| **transformer or transformerType** | string or Type | Name or type of a transformer to use on a loaded documents. |
| **configure** | Action<ILoadConfiguration> | Additional configuration that should be used during operation e.g. transformer parameters can be added. |

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
| **configure** | Action<ILoadConfiguration> | Additional configuration that should be used during operation e.g. transformer parameters can be added. |
| **skipAfter** | string | skip document fetching until given key is found and return documents after that key (default: `null`) |

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
| **skipAfter** | string | skip document fetching until given key is found and return documents after that key (default: `null`) |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult](../../glossary/stream-result)> | Enumerator with entities. |

### Example I

{CODE loading_entities_5_1@ClientApi\Session\LoadingEntities.cs /}

### Remarks

{INFO Entities loaded using `Stream` will be transient (not attached to session). /}

{WARNING:Caution}

**fromEtag** does not do any filtrations on server-side based on the specified in streaming function return type (e.g. `Employee`) and will return all documents from given Etag even if their `Raven-Clr-Type` does not match the return type, which may cause potential casting errors. Set return type to `object`, `dynamic` or `RavenJObject` to address situation when documents with different types might be returned.

{WARNING/}

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

{CODE loading_entities_6_1@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

## Related articles

- [Opening a session](./opening-a-session)  
