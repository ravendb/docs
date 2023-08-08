# Conventions

**Conventions** are adjustable RavenDB settings that users 
can adjust to modify client behaviors by their preferences.  

Access conventions via the `Conventions` property of the 
`DocumentStore` object.  
{CODE conventions_1@ClientApi\Configuration\Conventions.cs /}

{INFO: }
Customize conventions **before** `DocumentStore.Initialize()` is called. 
{INFO/}

##MaxHttpCacheSize

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

##UseOptimisticConcurrency

Use `UseOptimisticConcurrency` to control whether optimistic 
concurrency is set to true by default for all future sessions.  
**Default**: `false`  
{CODE UseOptimisticConcurrency@ClientApi\Configuration\Conventions.cs /}

##RequestTimeout

Use `RequestTimeout` to define the global request timeout 
value for all `RequestExecutors` created per database.  
**Default**: `null`
{CODE RequestTimeout@ClientApi\Configuration\Conventions.cs /}

##DisableTopologyUpdates

Use `DisableTopologyUpdates` to disable database topology updates.  
**Default**: `false`  
{CODE DisableTopologyUpdates@ClientApi\Configuration\Conventions.cs /}

##SaveEnumsAsIntegers

`SaveEnumsAsIntegers` determines whether C# `enum` types should be saved as 
integers or strings and instructs the LINQ provider to query enums as integer values.  
**Default**: `false`  
{CODE SaveEnumsAsIntegers@ClientApi\Configuration\Conventions.cs /}

##UseCompression

`UseCompression` determines if the client would send the 
server headers indicating whether compression is to be used.  
**Default**: `true`  
{CODE UseCompression@ClientApi\Configuration\Conventions.cs /}

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

## Changing fields/properties naming convention 

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

## Changing the Identity Separator

Use `IdentityPartsSeparator ` to change the default **separator** for 
automatically-generated document IDs.  
**Default**: `/` (forward slash)  
{CODE IdentityPartsSeparator@ClientApi\Configuration\Conventions.cs /}

* The value can be any `char` except `|` (pipe).  

{NOTE: }
Changing the separator affects these ID generation strategies:  

* [Server-Side ID](../../server/kb/document-identifier-generation#server-side-id)  
* [Identity](../../server/kb/document-identifier-generation#identity)  
* [HiLo Algorithm](../../server/kb/document-identifier-generation#hilo-algorithm)  
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

## HttpClientType

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
