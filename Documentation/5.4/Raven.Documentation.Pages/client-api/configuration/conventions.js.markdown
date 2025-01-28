# Conventions

{NOTE: }

* __Conventions__ in RavenDB are customizable settings that users can configure to tailor client behaviors according to their preferences.
 
* In this page:  
  * [How to set conventions](../../client-api/configuration/conventions#how-to-set-conventions)  
  * [Conventions:](../../client-api/configuration/conventions#conventions:)  
      [customFetch](../../client-api/configuration/conventions#customfetch)  
      [disableAtomicDocumentWritesInClusterWideTransaction](../../client-api/configuration/conventions#disableatomicdocumentwritesinclusterwidetransaction)  
      [disableTopologyUpdates](../../client-api/configuration/conventions#disabletopologyupdates)  
      [findCollectionName](../../client-api/configuration/conventions#findcollectionname)  
      [findJsType](../../client-api/configuration/conventions#_findjstype)  
      [findJsTypeName](../../client-api/configuration/conventions#_findjstypeName)  
      [firstBroadcastAttemptTimeout](../../client-api/configuration/conventions#firstbroadcastattempttimeout)  
      [identityPartsSeparator](../../client-api/configuration/conventions#identitypartsseparator)  
      [loadBalanceBehavior](../../client-api/configuration/conventions#loadbalancebehavior)  
      [loadBalancerContextSeed](../../client-api/configuration/conventions#loadbalancebehavior)  
      [loadBalancerPerSessionContextSelector](../../client-api/configuration/conventions#loadbalancebehavior)  
      [maxHttpCacheSize](../../client-api/configuration/conventions#maxhttpcachesize)  
      [maxNumberOfRequestsPerSession](../../client-api/configuration/conventions#maxnumberofrequestspersession)  
      [readBalanceBehavior](../../client-api/configuration/conventions#readbalancebehavior)  
      [requestTimeout](../../client-api/configuration/conventions#requesttimeout)  
      [secondBroadcastAttemptTimeout](../../client-api/configuration/conventions#secondbroadcastattempttimeout)  
      [sendApplicationIdentifier](../../client-api/configuration/conventions#sendapplicationidentifier)  
      [shouldIgnoreEntityChanges](../../client-api/configuration/conventions#shouldignoreentitychanges)  
      [storeDatesInUtc](../../client-api/configuration/conventions#storedatesinutc)  
      [storeDatesWithTimezoneInfo](../../client-api/configuration/conventions#storedateswithtimezoneinfo)  
      [syncJsonParseLimit](../../client-api/configuration/conventions#syncjsonparselimit)  
      [throwIfQueryPageSizeIsNotSet](../../client-api/configuration/conventions#throwifquerypagesizeisnotset)  
      [transformClassCollectionNameToDocumentIdPrefix](../../client-api/configuration/conventions#transformclasscollectionnametodocumentidprefix)  
      [useCompression](../../client-api/configuration/conventions#usecompression)  
      [useJsonlStreaming](../../client-api/configuration/conventions#usejsonlstreaming)  
      [useOptimisticConcurrency](../../client-api/configuration/conventions#useoptimisticconcurrency)  
      [waitForIndexesAfterSaveChangesTimeout](../../client-api/configuration/conventions#waitforindexesaftersavechangestimeout)  
      [waitForNonStaleResultsTimeout](../../client-api/configuration/conventions#waitfornonstaleresultstimeout)  
      [waitForReplicationAfterSaveChangesTimeout](../../client-api/configuration/conventions#waitforreplicationaftersavechangestimeout)  

{NOTE/}

---

{PANEL: How to set conventions}

* Access the conventions via the `conventions` property of the `DocumentStore` object.

* The conventions set on a Document Store will apply to ALL [sessions](../../client-api/session/what-is-a-session-and-how-does-it-work) and [operations](../../client-api/operations/what-are-operations) associated with that store.
 
* Customizing the conventions can only be set __before__ calling `documentStore.initialize()`.  
  Trying to do so after calling _initialize()_ will throw an exception.

{CODE:nodejs conventions_1@client-api\configuration\conventions.js /}

{PANEL/}

{PANEL: Conventions:}

{NOTE: }

#### customFetch

---

* Use the `customFetch` convention to override the default _fetch_ method.  
  This method is useful to enable RavenDB Node.js client on CloudFlare Workers.

* DEFAULT: undefined

{CODE:nodejs customFetchSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### disableAtomicDocumentWritesInClusterWideTransaction

---

* EXPERT ONLY:   
  Use the `disableAtomicDocumentWritesInClusterWideTransaction` convention to disable automatic  
  atomic writes with cluster write transactions.

* When set to `true`, will only consider explicitly added compare exchange values to validate cluster wide transactions.

* DEFAULT: `false`

{CODE:nodejs disableAtomicDocumentWritesInClusterWideTransactionSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### disableTopologyUpdates

---

* When setting the `disableTopologyUpdates` convention to `true`,  
  no database topology updates will be sent from the server to the client (e.g. adding or removing a node).
 
* DEFAULT: `false`

{CODE:nodejs disableTopologyUpdatesSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### findCollectionName

---

* Use the `findCollectionName` convention to define a function that will customize the collection name  
  from given type.

* DEFAULT: The collection name will be the plural form of the type name.

{CODE:nodejs findCollectionNameSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### findJsType

---

* Use the `findJsType` convention to define a function that finds the class of a document (if exists).

* The type is retrieved from the `Raven-Node-Type` property under the `@metadata` key in the document.

* DEFAULT: `null`

{CODE:nodejs findJsTypeSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### findJsTypeName

---

* Use the `findJsTypeName` convention to define a function that returns the class type name from a given type.

* The class name will be stored in the entity metadata.

* DEFAULT: `null`

{CODE:nodejs findJsTypeNameSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### firstBroadcastAttemptTimeout

---

* Use the `firstBroadcastAttemptTimeout` convention to set the timeout for the first broadcast attempt.  
 
* In the first attempt, the request executor will send a single request to the selected node.  
  Learn about the "selected node" in: [Client logic for choosing a node](../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).
 
* A [second attempt](../../client-api/configuration/conventions#secondbroadcastattempttimeout) will be held upon failure.
 
* DEFAULT: `5 seconds`

{CODE:nodejs firstBroadcastAttemptTimeoutSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### identityPartsSeparator

---

* Use the `identityPartsSeparator` convention to set the default **ID separator** for automatically-generated document IDs.
 
* DEFAULT: `/` (forward slash)  
 
* The value can be any `char` except `|` (pipe).
 
* Changing the separator affects these ID generation strategies:  
  * [Server-Side ID](../../server/kb/document-identifier-generation#strategy--2)
  * [Identity](../../server/kb/document-identifier-generation#strategy--3)
  * [HiLo Algorithm](../../server/kb/document-identifier-generation#strategy--4)

{CODE:nodejs identityPartsSeparatorSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### loadBalanceBehavior
#### loadBalancerPerSessionContextSelector
#### loadBalancerContextSeed

---

* Configure the __load balance behavior__ by setting the following conventions:
  * `loadBalanceBehavior`
  * `loadBalancerPerSessionContextSelector`
  * `loadBalancerContextSeed`

* Learn more in the dedicated [Load balance behavior](../../client-api/configuration/load-balance/load-balance-behavior) article.

{NOTE/}
{NOTE: }

#### maxHttpCacheSize

---

* Use the `MaxHttpCacheSize` convention to set the maximum HTTP cache size.  
  This setting will affect all the databases accessed by the Document Store.
 
* DEFAULT: `128 MB`

* __Disabling Caching__:

    * To disable caching globally, set `MaxHttpCacheSize` to zero.
    * To disable caching per session, see: [Disable caching per session](../../client-api/session/configuration/how-to-disable-caching).

* Note: RavenDB also supports Aggressive Caching.  
  Learn more about that in article [Setup aggressive caching](../../client-api/how-to/setup-aggressive-caching).

{CODE:nodejs maxHttpCacheSizeSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### maxNumberOfRequestsPerSession

---

* Use the `maxNumberOfRequestsPerSession` convention to set the maximum number of requests per session.
 
* DEFAULT: `30`
 
{CODE:nodejs maxNumberOfRequestsPerSessionSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### readBalanceBehavior

---

* Configure the __read request behavior__ by setting the `readBalanceBehavior` convention.

* Learn more in the dedicated [Read balance behavior](../../client-api/configuration/load-balance/read-balance-behavior) article.

{NOTE/}
{NOTE: }

#### requestTimeout

---

* Use the `requestTimeout` convention to define the global request timeout value for all `RequestExecutors` created per database.
 
* DEFAULT: `null` (the default HTTP client timout will be applied - 12h)

{CODE:nodejs requestTimeoutSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### secondBroadcastAttemptTimeout

---

* Use the `secondBroadcastAttemptTimeout` convention to set the timeout for the second broadcast attempt.
 
* Upon failure of the [first attempt](../../client-api/configuration/conventions#firstbroadcastattempttimeout) the request executor will resend the command to all nodes simultaneously.
 
* DEFAULT: `30 seconds`

{CODE:nodejs secondBroadcastAttemptTimeoutSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### sendApplicationIdentifier

---

* Use the `sendApplicationIdentifier` convention to `true` to enable sending a unique application identifier to the RavenDB Server.

* Setting to _true_ allows the server to issue performance hint notifications to the client, 
  e.g. during robust topology update requests which could indicate a Client API misuse impacting the overall performance.

* DEFAULT: `true`

{CODE:nodejs sendApplicationIdentifierSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### shouldIgnoreEntityChanges

---

* Set the `shouldIgnoreEntityChanges` convention to disable entity tracking for certain entities.  
 
* Learn more in [Customize tracking in conventions](../../client-api/session/configuration/how-to-disable-tracking#customize-tracking-in-conventions).

{NOTE/}
{NOTE: }

#### storeDatesInUtc

---

* When setting the `storeDatesInUtc` convention to `true`,  
  DateTime values will be stored in the database in UTC format.

* DEFAULT: `false`

{CODE:nodejs storeDatesInUtcSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### storeDatesWithTimezoneInfo

---

* When setting the `storeDatesWithTimezoneInfo` to `true`,  
  DateTime values will be stored in the database with their time zone information included.

* DEFAULT: `false`

{CODE:nodejs storeDatesWithTimezoneInfoSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### syncJsonParseLimit

---

* Use the `syncJsonParseLimit` convention to define the maximum size for the _sync_ parsing of the JSON data responses received from the server.  
  For data exceeding this size, the client switches to _async_ parsing.

* DEFAULT: `2 * 1_024 * 1_024`

{CODE:nodejs syncJsonParseLimitSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### throwIfQueryPageSizeIsNotSet

---

* When setting the `throwIfQueryPageSizeIsNotSet` convention to `true`,  
  an exception will be thrown if a query is performed without explicitly setting a page size.

* This can be useful during development to identify potential performance bottlenecks
  since there is no limitation on the number of results returned from the server.

* DEFAULT: `false`

{CODE:nodejs throwIfQueryPageSizeIsNotSetSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### transformClassCollectionNameToDocumentIdPrefix

---

* Use the `transformTypeCollectionNameToDocumentIdPrefix` convention to define a function that will  
  customize the document ID prefix from the the collection name.

* DEFAULT:  
  By default, the document id prefix is determined as follows:

| Number of uppercase letters in collection name   | Document ID prefix                                          |
|--------------------------------------------------|-------------------------------------------------------------|
| `<= 1`                                           | Use the collection name with all lowercase letters          |
| `> 1`                                            | Use the collection name as is, preserving the original case |

{CODE:nodejs transformClassCollectionNameToDocumentIdPrefixSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### useCompression

---

* Set the `useCompression` convention to true in order to accept the __response__ in compressed format and the automatic decompression of the HTTP response content.

* A `Gzip` compression is always applied when sending content in an HTTP request.
 
* DEFAULT: `true`  

{CODE:nodejs useCompressionSyntax@client-api\configuration\conventions.js /}

{NOTE/}

{NOTE: }

#### useJsonlStreaming

---

* Set the `useJsonlStreaming` convention to `true` when streaming query results as JSON Lines (JSONL) format.

* DEFAULT: `true`

{CODE:nodejs useJsonlStreamingSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### useOptimisticConcurrency

---

* When setting the `useOptimisticConcurrency` convention to `true`,  
  Optimistic Concurrency checks will be applied for all sessions opened from the Document Store. 

* Learn more about Optimistic Concurrency and the various ways to enable it in article  
  [how to enable optimistic concurrency](../../client-api/session/configuration/how-to-enable-optimistic-concurrency).

* DEFAULT: `false`  

{CODE:nodejs useOptimisticConcurrencySyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### waitForIndexesAfterSaveChangesTimeout

---

* Use the `waitForIndexesAfterSaveChangesTimeout` convention to set the default timeout for the 
  `documentSession.advanced.waitForIndexesAfterSaveChanges` method.  
 
* DEFAULT: 15 Seconds

{CODE:nodejs waitForIndexesAfterSaveChangesTimeoutSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### waitForNonStaleResultsTimeout

---

* Use the `waitForNonStaleResultsTimeout` convention to set the default timeout used by the  
  `waitForNonStaleResults` method when querying.  
 
* DEFAULT: 15 Seconds  

{CODE:nodejs waitForNonStaleResultsTimeoutSyntax@client-api\configuration\conventions.js /}

{NOTE/}
{NOTE: }

#### waitForReplicationAfterSaveChangesTimeout

---

* Use the `waitForReplicationAfterSaveChangesTimeout` convention to set the default timeout for the  
  `documentSession.advanced.waitForReplicationAfterSaveChanges`method.  
 
* DEFAULT: 15 Seconds  

{CODE:nodejs waitForReplicationAfterSaveChangesTimeoutSyntax@client-api\configuration\conventions.js /}

{NOTE/}
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
