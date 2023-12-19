# Conventions

* **Conventions** are adjustable RavenDB settings that users 
  can configure to modify client behaviors by their preferences.  
* Access conventions through the `DocumentStore` object 
  `Conventions` property.  

* In this page:  
   * [Client Conventions](../../client-api/configuration/conventions#client-conventions)  
   * [MaxHttpCacheSize](../../client-api/configuration/conventions#maxhttpcachesize)  
   * [MaxNumberOfRequestsPerSession](../../client-api/configuration/conventions#maxnumberofrequestspersession)  
   * [UseOptimisticConcurrency](../../client-api/configuration/conventions#useoptimisticconcurrency)  
   * [RequestTimeout](../../client-api/configuration/conventions#requesttimeout)  
   * [DisableTopologyUpdates](../../client-api/configuration/conventions#disabletopologyupdates)  
   * [SaveEnumsAsIntegers](../../client-api/configuration/conventions#saveenumsasintegers)  
   * [UseHttpCompression, UseHttpDecompression](../../client-api/configuration/conventions#usehttpcompression-usehttpdecompression)  
   * [OperationStatusFetchMode](../../client-api/configuration/conventions#operationstatusfetchmode)  
   * [Change fields/properties Naming Convention](../../client-api/configuration/conventions#change-fieldsproperties-naming-convention)  
   * [IdentityPartsSeparator](../../client-api/configuration/conventions#identitypartsseparator)  
   * [TopologyCacheLocation](../../client-api/configuration/conventions#topologycachelocation)  
   * [AddIdFieldToDynamicObjects](../../client-api/configuration/conventions#addidfieldtodynamicobjects)  
   * [ShouldIgnoreEntityChanges](../../client-api/configuration/conventions#shouldignoreentitychanges)  
   * [WaitForIndexesAfterSaveChangesTimeout](../../client-api/configuration/conventions#waitforindexesaftersavechangestimeout)  
   * [WaitForReplicationAfterSaveChangesTimeout](../../client-api/configuration/conventions#waitforreplicationaftersavechangestimeout)  
   * [WaitForNonStaleResultsTimeout](../../client-api/configuration/conventions#waitfornonstaleresultstimeout)  
   * [CreateHttpClient](../../client-api/configuration/conventions#createhttpclient)  
   * [HttpClientType](../../client-api/configuration/conventions#httpclienttype)  

{PANEL: Client Conventions}

Access conventions via the `Conventions` property of the 
`DocumentStore` object.  
{CODE conventions_1@ClientApi\Configuration\Conventions.cs /}

{NOTE: }
Customize conventions **before** `DocumentStore.Initialize()` is called. 
{NOTE/}

## MaxHttpCacheSize

Use `MaxHttpCacheSize` as follows to modify the maximum HTTP cache size.  
{CODE MaxHttpCacheSize@ClientApi\Configuration\Conventions.cs /}

**Default**:

| System | Usable Memory | Default Value |
| -------| ------------- | ------------- |
| 64-bit | Lower than or equal to 3GB <br> Greater than 3GB and Lower than or equal to 6GB <br> Greater than 6GB | 64MB <br> 128MB <br> 512MB |
| 32-bit | | 32MB |

* Caching is set **per database**.  
* **Disabling Caching**:  
  To disable caching globally, set `MaxHttpCacheSize` to zero.
  {CODE disable_cache@ClientApi\Configuration\Conventions.cs /}
  {WARNING: }
  Please be aware that if cache is disabled **ALL** data requests will be sent to the server.  
  {WARNING/}

## MaxNumberOfRequestsPerSession

Use `MaxNumberOfRequestsPerSession` to get or set the maximum number of GET requests per session.  
**Default**: `30`  
{CODE MaxNumberOfRequestsPerSession@ClientApi\Configuration\Conventions.cs /}

## UseOptimisticConcurrency

Use `UseOptimisticConcurrency` to control whether optimistic 
concurrency is set to true by default for all future sessions.  
**Default**: `false`  
{CODE UseOptimisticConcurrency@ClientApi\Configuration\Conventions.cs /}

## RequestTimeout

Use `RequestTimeout` to define the global request timeout 
value for all `RequestExecutors` created per database.  
**Default**: `null`
{CODE RequestTimeout@ClientApi\Configuration\Conventions.cs /}

## DisableTopologyUpdates

Use `DisableTopologyUpdates` to disable database topology updates.  
**Default**: `false`  
{CODE DisableTopologyUpdates@ClientApi\Configuration\Conventions.cs /}

## SaveEnumsAsIntegers

`SaveEnumsAsIntegers` determines whether C# `enum` types should be saved as 
integers or strings and instructs the LINQ provider to query enums as integer values.  
**Default**: `false`  
{CODE SaveEnumsAsIntegers@ClientApi\Configuration\Conventions.cs /}

##UseHttpCompression, UseHttpDecompression

It determines if the client will send headers to the server indicating that it allows compression/decompression to be used.  
Default: `true`.  

{CODE UseHttpCompression@ClientApi\Configuration\Conventions.cs /}
{CODE UseHttpDecompression@ClientApi\Configuration\Conventions.cs /}

## OperationStatusFetchMode

`OperationStatusFetchMode` changes the way the operation is 
fetching the operation status when waiting for completion.  
{NOTE: }
**By default**, the value is set to `ChangesApi` which uses the WebSocket 
protocol underneath when a connection is established with the server.  
On some older systems like Windows 7 the WebSocket protocol might not 
be available due to the OS and .NET Framework limitations. To bypass 
this issue, the value can be changed to `Polling`.  
{NOTE/}
{CODE OperationStatusFetchMode@ClientApi\Configuration\Conventions.cs /}

## Change fields/properties Naming Convention

Different clients may use different casing conventions for entity field names.  
E.g., here are the field naming defaults for clients of a few languages.  

| Language | Default convention | Example |
| ------------- | ----- | --- |
| C# | PascalCase | OrderLines |
| Java | camelCase | orderLines |
| JavaScript | camelCase | orderLines |

Whatever naming convention a client uses, is by default reflected on the server-side.  
These defauls can be customized, e.g. to allow inter-language operability.  

### Example: Using camelCase by a C# client

Make a C# client use camelCase by setting `CustomizeJsonSerializer` and `PropertyNameConverter `.  

{CODE FirstChar@ClientApi\Configuration\Conventions.cs /}
{CODE PropertyCasing@ClientApi\Configuration\Conventions.cs /}

## IdentityPartsSeparator

Use the `IdentityPartsSeparator` convention to change the default 
**ID (Identity) separator** for automatically-generated document IDs.  
**Default**: `/` (forward slash)  
{CODE IdentityPartsSeparator@ClientApi\Configuration\Conventions.cs /}

* The value can be any `char` except `|` (pipe).  

{NOTE: }
Changing the separator affects these ID generation strategies:  

* [Server-Side ID](../../server/kb/document-identifier-generation#strategy--2)
* [Identity](../../server/kb/document-identifier-generation#strategy--3)
* [HiLo Algorithm](../../server/kb/document-identifier-generation#strategy--4)
{NOTE/}

## TopologyCacheLocation

Use `TopologyCacheLocation` to change the location of topology cache files.  
Setting this value will check directory existance and writing permissions.  
**Default**: The application's base directory (`AppContext.BaseDirectory`)

{CODE TopologyCacheLocation@ClientApi\Configuration\Conventions.cs /}

## AddIdFieldToDynamicObjects

Use `AddIdFieldToDynamicObjects` to determine whether an Id field is automatically added to dynamic objects.  
**Default**: `true`  
{CODE AddIdFieldToDynamicObjects@ClientApi\Configuration\Conventions.cs /}

## ShouldIgnoreEntityChanges

Configure this function to disable entity tracking for certain entities.  
Learn more in [Customize tracking in conventions](../../client-api/session/configuration/how-to-disable-tracking#customize-tracking-in-conventions).

## WaitForIndexesAfterSaveChangesTimeout

Use `WaitForIndexesAfterSaveChangesTimeout` to set the default timeout 
for the DocumentSession.Advanced.WaitForIndexesAfterSaveChanges method.  
**Default**: 15 Seconds  
{CODE WaitForIndexesAfterSaveChangesTimeout@ClientApi\Configuration\Conventions.cs /}

## WaitForReplicationAfterSaveChangesTimeout

Use `WaitForReplicationAfterSaveChangesTimeout` to set the default timeout 
for the DocumentSession.Advanced.WaitForReplicationAfterSaveChanges method.  
**Default**: 15 Seconds  
{CODE WaitForReplicationAfterSaveChangesTimeout@ClientApi\Configuration\Conventions.cs /}

## WaitForNonStaleResultsTimeout

Use `WaitForNonStaleResultsTimeout` to set the default timeout WaitForNonStaleResults 
methods used when querying.  
**Default**: 15 Seconds  
{CODE WaitForNonStaleResultsTimeout@ClientApi\Configuration\Conventions.cs /}

## CreateHttpClient

Use the `CreateHttpClient` convention to modify the HTTP client your 
client application uses.  
Implementing your own  HTTP client can be useful when, for example, 
you'd like your clients to provide the server with tracing info.  
{CODE CreateHttpClient@ClientApi\Configuration\Conventions.cs /}

{NOTE: }
If you override the default `CreateHttpClient` convention we advise 
that you also set the HTTP client type correctly using the 
[HttpClientType](../../client-api/configuration/conventions#httpclienttype) convention.  
{NOTE/}

## HttpClientType

Use `HttpClientType` to get or set the type of HTTP client you're using.  
{CODE HttpClientType@ClientApi\Configuration\Conventions.cs /}

{NOTE: }
RavenDB uses the HTTP type internally to manage its cache. If you 
override the [CreateHttpClient](../../client-api/configuration/conventions#createhttpclient) 
convention to use a non-default HTTP client, we advise that you also 
set `HttpClientType` so it returns the client type you are actually using.  
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
