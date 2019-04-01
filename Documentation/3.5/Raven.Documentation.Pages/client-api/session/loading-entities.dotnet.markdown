# Session: Loading Entities

There are various methods with many overloads that allow a user to download documents from a database and convert them to entities. This article will cover the following methods:

- [Load](../../client-api/session/loading-entities#load)
- [Load with Includes](../../client-api/session/loading-entities#load-with-includes)
- [Load - multiple entities](../../client-api/session/loading-entities#load---multiple-entities)
- [LoadStartingWith](../../client-api/session/loading-entities#loadstartingwith)
- [Stream](../../client-api/session/loading-entities#stream)
- [IsLoaded](../../client-api/session/loading-entities#isloaded)

{PANEL:Load}


The most basic way to load a single entity is to use one of the `Load` methods.

{CODE loading_entities_1_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string or ValueType | Identifier of a document that will be loaded. |
| **transformer or transformerType** | string or Type | Name or type of a transformer to use on a loaded document. |
| **configure** | Action<ILoadConfiguration> | Additional configuration that should be used during an operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult | Instance of `TResult` or `null` if a document with a given ID does not exist. |

### Example I

{CODE loading_entities_1_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_1_2@ClientApi\Session\LoadingEntities.cs /}

### Non String Identifiers

The above examples show how to load a document by its string key. 

What if a type of your entity's identifier is not a string? You need to use an overload of the `Load` method that takes a parameter which type is `ValueType`. 

### Example III

If you have an entity where an identifier is an integer number:

{CODE class_with_interger_id@ClientApi\Session\LoadingEntities.cs /}

You can load it by specifying an integer value as the identifier:

{CODE loading_entities_1_3@ClientApi\Session\LoadingEntities.cs /}

####Converting to String Identifier

Even if the identifier is a string, you can use the `Load<T>(ValueType)` overload. The client is aware of the [ID generation conventions](../configuration/conventions/identifier-generation/global) (collectionName/number),
so you could load the entity with the key `employees/1` by using the code:

{CODE loading_entities_1_4@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

{PANEL:Load with Includes}

When there is a 'relationship' between documents, those documents can be loaded into a single request call using the `Include + Load` methods.

{CODE loading_entities_2_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string or Expression | Path in documents in which the server should look for a 'referenced' documents. |

| Return Value | |
| ------------- | ----- |
| ILoaderWithInclude | The `Include` method by itself does not materialize any requests, but returns loader containing methods such as `Load`. |

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
| **ids** | IEnumerable<string> or IEnumerable<ValueType> or ValueType[] | Enumerable or array of IDs that should be loaded. |
| **transformer or transformerType** | string or Type | Name or type of a transformer to use on loaded documents. |
| **configure** | Action<ILoadConfiguration> | Additional configuration that should be used during an operation e.g. transformer parameters can be added. |

| Return Value | |
| ------------- | ----- |
| TResult[] | Array of entities in the exact same order as given IDs. If document does not exist, value at the appropriate position in array will be `null`. |

### Example I

{CODE loading_entities_3_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_3_2@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

{PANEL:LoadStartingWith}

To load multiple entities that contain a common prefix, use the `LoadStartingWith` method from the `Advanced` session operations.

{CODE loading_entities_4_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keyPrefix** | string |  the prefix for which documents should be returned  |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | the number of documents that should be skipped  |
| **pageSize** | int | the maximum number of documents that will be retrieved |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on the server side |
| **exclude** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **configure** | Action<ILoadConfiguration> | Additional configuration that should be used during an operation e.g. transformer parameters can be added. |
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

Entities can be streamed from the server using one of the following `Stream` methods from the `Advanced` session operations.

{CODE loading_entities_5_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fromEtag** | Etag | ETag of a document from which stream should start (mutually exclusive with 'startsWith') |
| **startsWith** | string | prefix for which documents should be streamed (mutually exclusive with 'fromEtag') |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on the server side |
| **skipAfter** | string | skip document fetching until given key is found and return documents after that key (default: `null`) |
| **transformer** | String |  name of transformer (default: `null`) |
| **transformerParameters** | Dictionary<string, RavenJToken> | parameters to pass to the transformer (default: `null`) |

| Return Value | |
| ------------- | ----- |
| IEnumerator<[StreamResult](../../glossary/stream-result)> | Enumerator with entities. |

### Example I

{CODE loading_entities_5_1@ClientApi\Session\LoadingEntities.cs /}

## Example 2
Using the following transformer:
{CODE transformer@ClientApi\Session\LoadingEntities.cs /}

StreamDocs using the SimpleTransformer defined above and one supplied parameter:
{CODE loading_entities_5_2@ClientApi\Session\LoadingEntities.cs /}

### Remarks

{INFO Entities loaded using `Stream` will be transient (not attached to session). /}

{WARNING:Caution}

**fromEtag** does not do any filtrations on the server-side based on the specified in streaming function return type (e.g. `Employee`). It will return all documents from given Etag even if their `Raven-Clr-Type` does not match the return type, which may cause potential casting errors. Set return type to `object`, `dynamic` or `RavenJObject` to address a situation where documents with different types might be returned.

{WARNING/}

{PANEL/}

{PANEL:IsLoaded}

To check if entity is attached to session, e.g. it has been loaded previously, use the `IsLoaded` method from `Advanced` session operations.

{CODE loading_entities_6_0@ClientApi\Session\LoadingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | Entity Id for which the check should be performed. |

| Return Value | |
| ------------- | ----- |
| bool | Indicates if an entity with a given ID is loaded. |

### Example

{CODE loading_entities_6_1@ClientApi\Session\LoadingEntities.cs /}

{PANEL/}

## Related Articles

- [Opening a session](./opening-a-session)  
