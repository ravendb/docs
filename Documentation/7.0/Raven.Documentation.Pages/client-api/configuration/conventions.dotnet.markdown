# Conventions

{NOTE: }

* **Conventions** in RavenDB are customizable settings that users can configure to tailor client behaviors according to their preferences.

* In this page:
    * [How to set conventions](../../client-api/configuration/conventions#how-to-set-conventions)
    * [Conventions:](../../client-api/configuration/conventions#conventions:)  
      [AddIdFieldToDynamicObjects](../../client-api/configuration/conventions#addidfieldtodynamicobjects)  
      [AggressiveCache.Duration](../../client-api/configuration/conventions#aggressivecache.duration)  
      [AggressiveCache.Mode](../../client-api/configuration/conventions#aggressivecache.mode)  
      [AsyncDocumentIdGenerator](../../client-api/configuration/conventions#asyncdocumentidgenerator)  
      [CreateHttpClient](../../client-api/configuration/conventions#createhttpclient)  
      [DisableAtomicDocumentWritesInClusterWideTransaction](../../client-api/configuration/conventions#disableatomicdocumentwritesinclusterwidetransaction)  
      [DisableTcpCompression](../../client-api/configuration/conventions#disabletcpcompression)  
      [DisableTopologyCache](../../client-api/configuration/conventions#disabletopologycache)  
      [DisableTopologyUpdates](../../client-api/configuration/conventions#disabletopologyupdates)  
      [DisposeCertificate](../../client-api/configuration/conventions#disposecertificate)  
      [FindClrType](../../client-api/configuration/conventions#findclrtype)  
      [FindClrTypeName](../../client-api/configuration/conventions#findclrtypename)  
      [FindClrTypeNameForDynamic](../../client-api/configuration/conventions#findclrtypenamefordynamic)  
      [FindCollectionName](../../client-api/configuration/conventions#findcollectionname)  
      [FindCollectionNameForDynamic](../../client-api/configuration/conventions#findcollectionnamefordynamic)  
      [FindIdentityProperty](../../client-api/configuration/conventions#findidentityproperty)  
      [FindIdentityPropertyNameFromCollectionName](../../client-api/configuration/conventions#findidentitypropertynamefromcollectionname)  
      [FindProjectedPropertyNameForIndex](../../client-api/configuration/conventions#findprojectedpropertynameforindex)  
      [FindPropertyNameForDynamicIndex](../../client-api/configuration/conventions#findpropertynamefordynamicindex)  
      [FindPropertyNameForIndex](../../client-api/configuration/conventions#findpropertynameforindex)  
      [FirstBroadcastAttemptTimeout](../../client-api/configuration/conventions#firstbroadcastattempttimeout)  
      [HttpClientType](../../client-api/configuration/conventions#httpclienttype)  
      [HttpVersion](../../client-api/configuration/conventions#httpversion)  
      [IdentityPartsSeparator](../../client-api/configuration/conventions#identitypartsseparator)  
      [LoadBalanceBehavior](../../client-api/configuration/conventions#loadbalancebehavior)  
      [LoadBalancerContextSeed](../../client-api/configuration/conventions#loadbalancebehavior)  
      [LoadBalancerPerSessionContextSelector](../../client-api/configuration/conventions#loadbalancebehavior)  
      [MaxHttpCacheSize](../../client-api/configuration/conventions#maxhttpcachesize)  
      [MaxNumberOfRequestsPerSession](../../client-api/configuration/conventions#maxnumberofrequestspersession)  
      [Modify serialization of property name](../../client-api/configuration/conventions#modify-serialization-of-property-name)  
      [OperationStatusFetchMode](../../client-api/configuration/conventions#operationstatusfetchmode)  
      [PreserveDocumentPropertiesNotFoundOnModel](../../client-api/configuration/conventions#preservedocumentpropertiesnotfoundonmodel)  
      [ReadBalanceBehavior](../../client-api/configuration/conventions#readbalancebehavior)  
      [RequestTimeout](../../client-api/configuration/conventions#requesttimeout)  
      [ResolveTypeFromClrTypeName](../../client-api/configuration/conventions#resolvetypefromclrtypename)  
      [SaveEnumsAsIntegers](../../client-api/configuration/conventions#saveenumsasintegers)  
      [SecondBroadcastAttemptTimeout](../../client-api/configuration/conventions#secondbroadcastattempttimeout)  
      [SendApplicationIdentifier](../../client-api/configuration/conventions#sendapplicationidentifier)  
      [ShouldIgnoreEntityChanges](../../client-api/configuration/conventions#shouldignoreentitychanges)  
      [TopologyCacheLocation](../../client-api/configuration/conventions#topologycachelocation)  
      [TransformTypeCollectionNameToDocumentIdPrefix](../../client-api/configuration/conventions#transformtypecollectionnametodocumentidprefix)  
      [UseHttpCompression](../../client-api/configuration/conventions#usehttpcompression)  
      [UseHttpDecompression](../../client-api/configuration/conventions#usehttpdecompression)  
      [HttpCompressionAlgorithm](../../client-api/configuration/conventions#httpcompressionalgorithm)  
      [UseOptimisticConcurrency](../../client-api/configuration/conventions#useoptimisticconcurrency)  
      [WaitForIndexesAfterSaveChangesTimeout](../../client-api/configuration/conventions#waitforindexesaftersavechangestimeout)  
      [WaitForNonStaleResultsTimeout](../../client-api/configuration/conventions#waitfornonstaleresultstimeout)  
      [WaitForReplicationAfterSaveChangesTimeout](../../client-api/configuration/conventions#waitforreplicationaftersavechangestimeout)

{NOTE/}

---

{PANEL: How to set conventions}

* Access the conventions via the `Conventions` property of the `DocumentStore` object.

* The conventions set on a Document Store will apply to ALL [sessions](../../client-api/session/what-is-a-session-and-how-does-it-work) and [operations](../../client-api/operations/what-are-operations) associated with that store.

* Customizing the conventions can only be set **before** calling `DocumentStore.Initialize()`.  
  Trying to do so after calling _Initialize()_ will throw an exception.

{CODE conventions_1@ClientApi\Configuration\Conventions.cs /}

{PANEL/}

{PANEL: Conventions:}

{CONTENT-FRAME: }

#### AddIdFieldToDynamicObjects

---

* Use the `AddIdFieldToDynamicObjects` convention to determine whether an `Id` field is automatically added  
  to [dynamic objects](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/interop/using-type-dynamic) when [storing new entities](../../client-api/session/storing-entities) via the session.

* DEFAULT: `true`

{CODE AddIdFieldToDynamicObjectsSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### AggressiveCache.Duration

---

* Use the `AggressiveCache.Duration` convention to define the [aggressive cache](../../client-api/how-to/setup-aggressive-caching) duration period.

* DEFAULT: `1 day`

{CODE AggressiveCacheDurationSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### AggressiveCache.Mode

---

* Use the `AggressiveCache.Mode` convention to define the [aggressive cache](../../client-api/how-to/setup-aggressive-caching) mode.  
  (`AggressiveCacheMode.TrackChanges` or `AggressiveCacheMode.DoNotTrackChanges`)

* DEFAULT: `AggressiveCacheMode.TrackChanges`

{CODE AggressiveCacheModeSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### AsyncDocumentIdGenerator

---

* Use the `AsyncDocumentIdGenerator` convention to define the document ID generator method 
  used when storing a document without explicitly specifying its `Id`.

* You can override this global ID generator for specific object types using the [RegisterAsyncIdConvention](../../client-api/configuration/identifier-generation/type-specific) convention.

* DEFAULT:  
  The default document ID generator is the `GenerateDocumentIdAsync` method, which is part of the `HiLoIdGenerator` object within the _DocumentStore_.
  This method implements the [HiLo algorithm](../../client-api/document-identifiers/hilo-algorithm) to ensure efficient ID generation when storing a document without explicitly specifying its `Id`.

{CODE AsyncDocumentIdGenerator@ClientApi\Configuration\Conventions.cs /}
{CODE AsyncDocumentIdGeneratorSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### CreateHttpClient

---

* Use the `CreateHttpClient` convention to modify the HTTP client your client application uses.

* For example, implementing your own HTTP client can be useful when you'd like your clients to provide the server with tracing info.

* If you override the default `CreateHttpClient` convention we advise that you also set the HTTP client type
  correctly using the [HttpClientType](../../client-api/configuration/conventions#httpclienttype) convention.

{CODE CreateHttpClient@ClientApi\Configuration\Conventions.cs /}
{CODE CreateHttpClientSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### DisableAtomicDocumentWritesInClusterWideTransaction

---

* EXPERT ONLY:   
  Use the `DisableAtomicDocumentWritesInClusterWideTransaction` convention to disable automatic  
  atomic writes with cluster write transactions.

* When set to `true`, will only consider explicitly-added compare exchange values to validate cluster-wide transactions.

* DEFAULT: `false`

{CODE DisableAtomicDocumentWritesInClusterWideTransactionSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### DisableTcpCompression

---

* When setting the `DisableTcpCompression` convention to `true`, TCP data will not be compressed.

* DEFAULT: `false`

{CODE DisableTcpCompressionSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### DisableTopologyCache

---

* By default, the client caches the cluster's topology in `*.raven-cluster-topology` files on disk.  
  When all servers provided in the `DocumentStore.Urls` property are down or unavailable,
  the client will load the topology from the latest file and try to connect to nodes that are not listed in the URL property.

* This behavior can be disabled when setting the `DisableTopologyCache` convention to `true`.  
  In such a case:

    * The client will not load the topology from the cache upon failing to connect to a server.
    * Even if the client is configured to [receive topology updates](../../client-api/configuration/conventions#disabletopologyupdates) from the server,
      no topology files will be saved on disk, thus preventing the accumulation of these files.

* DEFAULT: `false`

{CODE DisableTopologyCacheSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### DisableTopologyUpdates

---

* When setting the `DisableTopologyUpdates` convention to `true`,  
  no database topology updates will be sent from the server to the client (e.g. adding or removing a node).

* DEFAULT: `false`

{CODE DisableTopologyUpdatesSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### DisposeCertificate

---

* When setting the `DisposeCertificate` convention to `true`,  
  the `DocumentStore.Certificate` will be disposed of during DocumentStore disposal. 

* DEFAULT: `true`

{CODE DisposeCertificateSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindClrType

---

* Use the `FindClrType` convention to define a function that finds the CLR type of a document.

* DEFAULT:  
  The CLR type is retrieved from the `Raven-Clr-Type` property under the `@metadata` key in the document.

{CODE FindClrType@ClientApi\Configuration\Conventions.cs /}
{CODE FindClrTypeSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindClrTypeName

---

* Use the `FindClrTypeName` convention to define a function that returns the CLR type name from a given type.

* DEFAULT: Return the entity's full name, including the assembly name.

{CODE FindClrTypeNameSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindClrTypeNameForDynamic

---

* Use the `FindClrTypeNameForDynamic` convention to define a function that returns the CLR type name  
  from a dynamic entity.

* DEFAULT: The dynamic entity type is returned.

{CODE FindClrTypeNameForDynamic@ClientApi\Configuration\Conventions.cs /}
{CODE FindClrTypeNameForDynamicSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindCollectionName

---

* Use the `FindCollectionName` convention to define a function that will customize 
  the collection name from a given type.

* DEFAULT: The collection name will be the plural form of the type name.

{CODE FindCollectionName@ClientApi\Configuration\Conventions.cs /}
{CODE FindCollectionNameSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindCollectionNameForDynamic

---

* Use the `FindCollectionNameForDynamic` convention to define a function that will customize the  
  collection name from a dynamic type.

* DEFAULT: The collection name will be the entity's type.

{CODE FindCollectionNameForDynamic@ClientApi\Configuration\Conventions.cs /}
{CODE FindCollectionNameForDynamicSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindIdentityProperty

---

* Use the `FindIdentityProperty` convention to define a function that finds the specified ID property  
  in the entity.

* DEFAULT: The entity's `Id` property serves as the ID property.

{CODE FindIdentityProperty@ClientApi\Configuration\Conventions.cs /}
{CODE FindIdentityPropertySyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindIdentityPropertyNameFromCollectionName

---

* Use the `FindIdentityPropertyNameFromCollectionName` convention to define a function that customizes
  the entity's ID property from the collection name.

* DEFAULT: Will use the `Id` property.

{CODE FindIdentityPropertyNameFromCollectionName@ClientApi\Configuration\Conventions.cs /}
{CODE FindIdentityPropertyNameFromCollectionNameSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindProjectedPropertyNameForIndex

---

* Use the `FindProjectedPropertyNameForIndex` convention to define a function that customizes the  
  projected fields names that will be used in the RQL sent to the server when querying an index.

* Given input: The indexed document type, the index name, the current path, and the property path  
  that is used in a query.

* DEFAULT: `null`

{CODE FindProjectedPropertyNameForIndexSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindPropertyNameForDynamicIndex

---

* Use the `FindPropertyNameForDynamicIndex` convention to define a function that customizes the  
  property name that will be used in the RQL sent to the server when making a dynamic query.

* Given input: The indexed document type, the index name, the current path, and the property path  
  that is used in a query predicate.

{CODE FindPropertyNameForDynamicIndex@ClientApi\Configuration\Conventions.cs /}
{CODE FindPropertyNameForDynamicIndexSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FindPropertyNameForIndex

---

* Use the `FindPropertyNameForIndex` convention to define a function that customizes the name of the  
  index-field property that will be used in the RQL sent to the server when querying an index.

* Given input: The indexed document type, the index name, the current path, and the property path  
  that is used in a query predicate.

* DEFAULT: `[].` & `.` are replaced by `_`

{CODE FindPropertyNameForIndex@ClientApi\Configuration\Conventions.cs /}
{CODE FindPropertyNameForIndexSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### FirstBroadcastAttemptTimeout

---

* Use the `FirstBroadcastAttemptTimeout` convention to set the timeout for the first broadcast attempt.

* In the first attempt, the request executor will send a single request to the selected node.  
  Learn about the "selected node" in: [Client logic for choosing a node](../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).

* A [second attempt](../../client-api/configuration/conventions#secondbroadcastattempttimeout) will be held upon failure.

* DEFAULT: `5 seconds`

{CODE FirstBroadcastAttemptTimeout@ClientApi\Configuration\Conventions.cs /}
{CODE FirstBroadcastAttemptTimeoutSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### HttpClientType

---

* Use the `HttpClientType` convention to set the type of HTTP client you're using.

* RavenDB uses the HTTP type internally to manage its cache.

* If you override the [CreateHttpClient](../../client-api/configuration/conventions#createhttpclient) convention to use a non-default HTTP client,  
  we advise that you also set `HttpClientType` so it returns the client type you are actually using.

{CODE HttpClientType@ClientApi\Configuration\Conventions.cs /}
{CODE HttpClientTypeSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### HttpVersion

---

* Use the `HttpVersion` convention to set the Http version the client will use when communicating  
  with the server.

* DEFAULT:
    * When this convention is explicitly set to `null`, the default HTTP version provided by your .NET framework is used.
    * Otherwise, the default HTTP version is set to `System.Net.HttpVersion.Version20` (HTTP 2.0).

{CODE HttpVersionSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### IdentityPartsSeparator

---

* Use the `IdentityPartsSeparator` convention to set the default **ID separator** for automatically generated document IDs.

* DEFAULT: `/` (forward slash)

* The value can be any `char` except `|` (pipe).

* Changing the separator affects these ID generation strategies:
    * [Server-Side ID](../../server/kb/document-identifier-generation#strategy--2)
    * [Identity](../../server/kb/document-identifier-generation#strategy--3)
    * [HiLo Algorithm](../../server/kb/document-identifier-generation#strategy--4)

{CODE IdentityPartsSeparatorSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### LoadBalanceBehavior
#### LoadBalancerPerSessionContextSelector
#### LoadBalancerContextSeed

---

* Configure the **load balance behavior** by setting the following conventions:
    * `LoadBalanceBehavior`
    * `LoadBalancerPerSessionContextSelector`
    * `LoadBalancerContextSeed`

* Learn more in the dedicated [Load balance behavior](../../client-api/configuration/load-balance/load-balance-behavior) article.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### MaxHttpCacheSize

---

* Use the `MaxHttpCacheSize` convention to set the maximum HTTP cache size.  
  This setting will affect all the databases accessed by the Document Store.

* DEFAULT:

  | System   | Usable Memory                                                                                         | Default Value              |
  |----------|-------------------------------------------------------------------------------------------------------|----------------------------|
  | 64-bit   | Lower than or equal to 3GB <br> Greater than 3GB and Lower than or equal to 6GB <br> Greater than 6GB | 64MB <br> 128MB <br> 512MB |
  | 32-bit   |                                                                                                       | 32MB                       |

* **Disabling Caching**:

    * To disable caching globally, set `MaxHttpCacheSize` to zero.
    * To disable caching per session, see: [Disable caching per session](../../client-api/session/configuration/how-to-disable-caching).

* Note: RavenDB also supports Aggressive Caching.  
  Learn more about this in the [Setup aggressive caching](../../client-api/how-to/setup-aggressive-caching) article.

{CODE MaxHttpCacheSize@ClientApi\Configuration\Conventions.cs /}
{CODE disable_cache@ClientApi\Configuration\Conventions.cs /}
{CODE MaxHttpCacheSizeSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### MaxNumberOfRequestsPerSession

---

* Use the `MaxNumberOfRequestsPerSession` convention to set the maximum number of requests per session.

* DEFAULT: `30`

{CODE MaxNumberOfRequestsPerSessionSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Modify serialization of property name

---

* Different clients use different casing conventions for entity field names. For example:

  | Language   | Default casing  | Example    |
  |------------|-----------------|------------|
  | C#         | PascalCase      | OrderLines |
  | Java       | camelCase       | orderLines |
  | JavaScript | camelCase       | orderLines |

* By default, when saving an entity, the naming convention used by the client is reflected in the JSON document properties on the server-side.  
  This default serialization behavior can be customized to facilitate language interoperability.

* **Example**:

  Set `CustomizeJsonSerializer` and `PropertyNameConverter` to serialize an entity's properties as camelCase from a C# client:

  {CODE PropertyCasing@ClientApi\Configuration\Conventions.cs /}
  {CODE FirstChar@ClientApi\Configuration\Conventions.cs /}
  {CODE SerializationSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### OperationStatusFetchMode

---

* Use the `OperationStatusFetchMode` convention to set the way an [operation](../../client-api/operations/what-are-operations) is getting its status when [waiting for completion](../../client-api/operations/what-are-operations#wait-for-completion).

* DEFAULT:  
  By default, the value is set to `ChangesApi` which uses the WebSocket protocol underneath when a connection is established with the server.

* On some older systems like Windows 7 the WebSocket protocol might not be available due to the OS and .NET Framework limitations.
  To bypass this issue, the value can be changed to `Polling`.

{CODE OperationStatusFetchMode@ClientApi\Configuration\Conventions.cs /}
{CODE OperationStatusFetchModeSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### PreserveDocumentPropertiesNotFoundOnModel

---

* Loading a document using a different model will result in the removal of the missing model properties  
  from the loaded entity, and no exception is thrown.

* Setting the `PreserveDocumentPropertiesNotFoundOnModel` convention to `true`  
  allows the client to check (via [whatChanged](../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#get-session-changes)
  or via [WhatChangedFor](../../client-api/session/how-to/check-if-entity-has-changed#get-entity-changes) methods)
  for the missing properties on the entity after loading the document.

* DEFAULT: `true`

{CODE PreserveDocumentPropertiesNotFoundOnModelSyntax@ClientApi\Configuration\Conventions.cs /}

{NOCONTENT-FRAMETE/}
{CONTENT-FRAME: }

#### ReadBalanceBehavior

---

* Configure the **read request behavior** by setting the `ReadBalanceBehavior` convention.

* Learn more in the dedicated [Read balance behavior](../../client-api/configuration/load-balance/read-balance-behavior) article.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### RequestTimeout

---

* Use the `RequestTimeout` convention to define the global request timeout value for all `RequestExecutors` created per database.

* DEFAULT: `null` (the default HTTP client timeout will be applied - 12h)

{CODE RequestTimeout@ClientApi\Configuration\Conventions.cs /}
{CODE RequestTimeoutSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### ResolveTypeFromClrTypeName

---

* Use the `ResolveTypeFromClrTypeName` convention to define a function that resolves the CLR type  
  from the CLR type name.

* DEFAULT: The type is returned.

{CODE ResolveTypeFromClrTypeName@ClientApi\Configuration\Conventions.cs /}
{CODE ResolveTypeFromClrTypeNameSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### SaveEnumsAsIntegers

---

* When setting the `SaveEnumsAsIntegers` convention to `true`,  
  C# `enum` types will be stored and queried as integers, rather than their string representations.

* DEFAULT: `false` (save as strings)

{CODE SaveEnumsAsIntegersSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### SecondBroadcastAttemptTimeout

---

* Use the `SecondBroadcastAttemptTimeout` convention to set the timeout for the second broadcast attempt.

* Upon failure of the [first attempt](../../client-api/configuration/conventions#firstbroadcastattempttimeout) the request executor will resend the command to all nodes simultaneously.

* DEFAULT: `30 seconds`

{CODE SecondBroadcastAttemptTimeout@ClientApi\Configuration\Conventions.cs /}
{CODE SecondBroadcastAttemptTimeoutSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### SendApplicationIdentifier

---

* Use the `SendApplicationIdentifier` convention to `true` to enable sending a unique application identifier to the RavenDB Server.

* Setting to _true_ allows the server to issue performance hint notifications to the client,
  e.g. during robust topology update requests which could indicate a Client API misuse impacting the overall performance.

* DEFAULT: `true`

{CODE SendApplicationIdentifierSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### ShouldIgnoreEntityChanges

---

* Set the `ShouldIgnoreEntityChanges` convention to disable entity tracking for certain entities.

* Learn more in [Customize tracking in conventions](../../client-api/session/configuration/how-to-disable-tracking#customize-tracking-in-conventions).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### TopologyCacheLocation

---

* Use the `TopologyCacheLocation` convention to change the location of the topology cache files   
  (`*.raven-database-topology` & `*.raven-cluster-topology`).

* Directory existence and writing permissions will be checked when setting this value.

* DEFAULT: `AppContext.BaseDirectory` (The application's base directory)

{CODE TopologyCacheLocation@ClientApi\Configuration\Conventions.cs /}
{CODE TopologyCacheLocationSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### TransformTypeCollectionNameToDocumentIdPrefix

---

* Use the `TransformTypeCollectionNameToDocumentIdPrefix` convention to define a function that will  
  customize the document ID prefix from the collection name.

* DEFAULT:  
  By default, the document id prefix is determined as follows:

| Number of uppercase letters in collection name   | Document ID prefix                                          |
|--------------------------------------------------|-------------------------------------------------------------|
| `<= 1`                                           | Use the collection name with all lowercase letters          |
| `> 1`                                            | Use the collection name as is, preserving the original case |

{CODE TransformTypeCollectionNameToDocumentIdPrefixSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### UseHttpCompression

---

* When setting the `UseHttpCompression` convention to `true`,  
  then `Gzip` compression will be used when sending content of HTTP request.

* When the convention is set to `false`, content will not be compressed.

* DEFAULT: `true`

{CODE UseHttpCompressionSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### UseHttpDecompression

---

* When setting the `UseHttpDecompression` convention to `true`,  
  the client can accept compressed HTTP response content and will use zstd/gzip/deflate decompression methods.

* DEFAULT: `true`

{CODE UseHttpDecompressionSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}

{CONTENT-FRAME: }

#### HttpCompressionAlgorithm

---

* Use this convention to set the HTTP compression algorithm 
  (see [UseHttpDecompression](../../client-api/configuration/conventions#usehttpcompression) above).  

* DEFAULT: `Zstd`
  {INFO: Default compression algorithm}
   In RavenDB versions up to `6.2`, HTTP compression is set to `Gzip` by default.  
   In RavenDB versions from `7.0` on, the default has changed and is now `Zstd`.  
  {INFO/}

{CODE HttpCompressionAlgorithm@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}


{CONTENT-FRAME: }

#### UseOptimisticConcurrency

---

* When setting the `UseOptimisticConcurrency` convention to `true`,  
  Optimistic Concurrency checks will be applied for all sessions opened from the Document Store.

* Learn more about Optimistic Concurrency and the various ways to enable it in the 
  [how to enable optimistic concurrency](../../client-api/session/configuration/how-to-enable-optimistic-concurrency) 
  article.  

* DEFAULT: `false`

{CODE UseOptimisticConcurrencySyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### WaitForIndexesAfterSaveChangesTimeout

---

* Use the `WaitForIndexesAfterSaveChangesTimeout` convention to set the default timeout for the
  `DocumentSession.Advanced.WaitForIndexesAfterSaveChanges` method.

* DEFAULT: 15 Seconds

{CODE WaitForIndexesAfterSaveChangesTimeout@ClientApi\Configuration\Conventions.cs /}
{CODE WaitForIndexesAfterSaveChangesTimeoutSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### WaitForNonStaleResultsTimeout

---

* Use the `WaitForNonStaleResultsTimeout` convention to set the default timeout used by the  
  `WaitForNonStaleResults` method when querying.

* DEFAULT: 15 Seconds

{CODE WaitForNonStaleResultsTimeout@ClientApi\Configuration\Conventions.cs /}
{CODE WaitForNonStaleResultsTimeoutSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### WaitForReplicationAfterSaveChangesTimeout

---

* Use the `WaitForReplicationAfterSaveChangesTimeout` convention to set the default timeout for the  
  `DocumentSession.Advanced.WaitForReplicationAfterSaveChanges`method.

* DEFAULT: 15 Seconds

{CODE WaitForReplicationAfterSaveChangesTimeout@ClientApi\Configuration\Conventions.cs /}
{CODE WaitForReplicationAfterSaveChangesTimeoutSyntax@ClientApi\Configuration\Conventions.cs /}

{CONTENT-FRAME/}
{PANEL/}

## Related Articles

### Conventions

- [Querying](../../client-api/configuration/querying)
- [Serialization](../../client-api/configuration/serialization)
- [Load Balancing Client Requests](../../client-api/configuration/load-balance/overview)

### Document Identifiers

- [Working with Document Identifiers](../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../client-api/configuration/identifier-generation/type-specific)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
