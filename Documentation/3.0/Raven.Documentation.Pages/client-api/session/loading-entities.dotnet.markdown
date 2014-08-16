# Session : Loading entities

There are various methods with many overloads that allow user to download a documents from database and convert them to entities. This article will cover following methods:

- [Load]()
- [Load with Includes]()
- [Load - multiple entities]()
- [LoadStartingWith]()
- [Stream]()
- [IsLoaded]()

## Load

The most basic way to load single entity is to use one of `Load` methods.

{CODE loading_entities_1_0@ClientApi\Session\LoadingEntities.cs /}

**Parameters**

id
:   Type: string or ValueType   
Identifier of a document that will be loaded.

transformer or transformerType
:   Type: string or Type   
Name or type of a transformer to use on a loaded document.

configure
:   Type: Action<[ILoadConfiguration]()>   
Additional configuration that should be used during operation e.g. transformer parameters can be added.

**Return Value**

Type: TResult   
Instance of `TResult` or `null` if document with given Id does not exist.

### Example I

{CODE loading_entities_1_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_1_2@ClientApi\Session\LoadingEntities.cs /}

## Load with Includes

When there is a 'relationship' between documents, then those documents can be loaded in a single request call using `Include + Load` methods.

{CODE loading_entities_2_0@ClientApi\Session\LoadingEntities.cs /}

**Parameters**

path
:   Type: string or Expression   
Path in documents in which server should look for a 'referenced' documents.

**Return Value**
Type: [ILoaderWithInclude]()   
`Include` method by itself does not materialize any requests, but returns loader containing methods such as `Load`.

### Example I

{CODE loading_entities_2_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_2_2@ClientApi\Session\LoadingEntities.cs /}

## Load - multiple entities

To load multiple entities at once use one of the following `Load` overloads.

{CODE loading_entities_3_0@ClientApi\Session\LoadingEntities.cs /}

**Parameters**

ids
:   Type: IEnumerable<string> or IEnumerable<ValueType> or ValueType[]   
Enumerable or array of Ids that should be loaded.

transformer or transformerType
:   Type: string or Type   
Name or type of a transformer to use on a loaded documents.

configure
:   Type: Action<[ILoadConfiguration]()>   
Additional configuration that should be used during operation e.g. transformer parameters can be added.

**Return Value**

Type: TResult[]   
Array of entities in the exact same order as given ids. If document does not exist, value at the appropriate position in array will be `null`.

### Example I

{CODE loading_entities_3_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_3_2@ClientApi\Session\LoadingEntities.cs /}

## LoadStartingWith

To load multiple entities that contain common prefix use `LoadStartingWith` method from `Advanced` session operations.

{CODE loading_entities_4_0@ClientApi\Session\LoadingEntities.cs /}

**Parameters**   

keyPrefix
:   Type: string   
prefix for which documents should be returned 

matches
:   Type: string   
pipe ('|') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters)  

start
:   Type: int   
number of documents that should be skipped 

pageSize
:   Type: int   
maximum number of documents that will be retrieved

pagingInformation
:   Type: RavenPagingInformation   
used to perform rapid pagination on server side 

exclude
:   Type: string   
pipe ('|') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters)       

configure
:   Type: Action<[ILoadConfiguration]()>   
Additional configuration that should be used during operation e.g. transformer parameters can be added.

### Example I

{CODE loading_entities_4_1@ClientApi\Session\LoadingEntities.cs /}

### Example II

{CODE loading_entities_4_2@ClientApi\Session\LoadingEntities.cs /}

### Example III

{CODE loading_entities_4_3@ClientApi\Session\LoadingEntities.cs /}

## Stream

Entities can be streamed from server using one of the following `Stream` methods from `Advanced` session operations.

{CODE loading_entities_5_0@ClientApi\Session\LoadingEntities.cs /}

**Parameters**   

fromEtag
:   Type: Etag   
ETag of a document from which stream should start (mutually exclusive with 'startsWith')   

startsWith
:   Type: string   
prefix for which documents should be streamed (mutually exclusive with 'fromEtag')   

matches
:   Type: string   
pipe ('|') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters)

start
:   Type: int   
number of documents that should be skipped   

pageSize
:   Type: int   
maximum number of documents that will be retrieved

pagingInformation
:   Type: RavenPagingInformation   
used to perform rapid pagination on server side    

**Return Value**

Type: IEnumerator<[StreamResult]()>   
Enumerator with entities.

### Example I

{CODE loading_entities_5_1@ClientApi\Session\LoadingEntities.cs /}

### Remarks

Entities loaded using `Stream` will be transient (not attached to session).

## IsLoaded

To check if entity is attached to sessiong, e.g. has been loaded previously, use `IsLoaded` method from `Advanced` session operations.

{CODE loading_entities_6_0@ClientApi\Session\LoadingEntities.cs /}

**Parameters**

id
:   Type: string   
Entity Id for which check should be performed.

**Return Value**

Type: bool
Indicates if entity with given Id is loaded.

### Example

{CODE loading_entities_6_1@ClientApi\Session\LoadingEntities.cs /}

#### Related articles

TODO