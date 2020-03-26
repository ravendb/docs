# Conventions

Conventions give you the ability to customize the Client API behavior. They are accessible from `DocumentStore` object:

{CODE:nodejs conventions_1@client-api\configuration\conventions.js /}

You will find many settings to overwrite, allowing you to adjust the client according to your needs. The conventions apply to many different client behaviors. Some of them are grouped and described in the separate articles of this section.

{INFO All customizations need to be set before `DocumentStore.initialize()` is called. /}

##MaxHttpCacheSize

If you need to modify the maximum http cache size, you can use the following setting:

{CODE:nodejs MaxHttpCacheSize@client-api\configuration\conventions.js /}

{NOTE: Default size}

The default value of this setting is configured to 128 MB.

The cache is created per database you use.

{NOTE/}

{NOTE: Disable caching} 

To disable the caching globally you can set the `maxHttpCacheSize` value to zero:

{CODE:nodejs disable_cache@client-api\configuration\conventions.js /}

**In this scenario, all the requests will be sent to the server to fetch the data.**

{NOTE/}

## MaxNumberOfRequestsPerSession

Gets or sets maximum number of GET requests per session. Default: `30`.

{CODE:nodejs MaxNumberOfRequestsPerSession@client-api\configuration\conventions.js /}

##UseOptimisticConcurrency

Controls whether optimistic concurrency is set to true by default for all future sessions. Default: `false`.

{CODE:nodejs UseOptimisticConcurrency@client-api\configuration\conventions.js /}

##DisableTopologyUpdates

Forces you to disable updates of database topology. Default: `false`.

{CODE:nodejs DisableTopologyUpdates@client-api\configuration\conventions.js /}

##UseCompression

It determines if the client will send headers to the server indicating that it allows compression to be used. Default: `true`.

{CODE:nodejs UseCompression@client-api\configuration\conventions.js /}

## Changing fields/properties naming convention 

By default whatever casing convention you use in your entities' fields will be reflected server-side.

If following language-specific field casing conventions RavenDB clients use different field/properties naming conventions:

| Language | Default convention | Example |
| ------------- | ----- | --- |
| C# | PascalCase | OrderLines |
| Java | camelCase | orderLines |
| JavaScript | camelCase | orderLines |

This can be configured to allow inter-language operability e.g. store data PascalCase, but keep fields in the application code camelCase.

### Example: storing data PascalCase, have camelCase in application entities

If you'd like to transform field names to be PascalCase server-side, but keep using camelCase in your Node.js application. You need to set 2 properties (since JS is not aware of local classes/objects field names):

* `conventions.remoteEntityFieldNameConvention` - for transforming data before it's sent *to* the server

* `conventions.entityFieldNameConvention` - for transforming data once it's loaded *from* the server

You have to set *property naming strategy*:

{CODE:nodejs PropertyCasing@client-api\configuration\conventions.js /}

## Dates storage conventions

By default all dates are stored in local time with no timezone information. There are 2 document conventions allowing to adjust storage of dates:

- store.conventions.storeDatesWithTimezoneInfo 
- store.conventions.storeDatesInUtc 

### StoreDatesWithTimezoneInfo

This convention allows you to store dates along with the timezone information, by storing dates in the following format: `YYYY-MM-DDTHH:mm:ss.SSS0000Z`. JavaScript's Date object's minimal precision is 1 ms, hence the zeroes in the date format for the nanoseconds. Default: `false`.

{CODE:nodejs StoreDatesWithTimezoneInfo@client-api\configuration\conventions.js /}

### StoreDatesInUtc

If enabled, dates are going to be stored in UTC instead of local time. Default: `false`.

{CODE:nodejs StoreDatesInUtc@client-api\configuration\conventions.js /}

## Entity types registration - registerEntityType()

Type information about the entity and its contents is by default stored in the document metadata. Based on that its types are revived when loaded from the server. Conventions object exposes a method `registerEntityType(type)` allowing you to register types of your entities and their subobjects in order to:

* revive object types when loading from the server

* avoid passing **documentType** argument every time on methods loading entites e.g. `load()`, `query()` etc.

{INFO: Entity type registration }
To avoid passing **documentType** argument every time, you can register the type in the document conventions using the `registerEntityType()` method before calling DocumentStore's `initialize()` like so:

{CODE:nodejs query_1_8@client-api\session\querying\howToQuery.js /}

{INFO/}

If you fail to do so, entities (and all subobjects) loaded from the server are going to be plain object literals and not instances of the original type they were stored with.

## Storing object literals - findCollectionNameForObjectLiteral()

In order to comfortably use object literals as entities set the function getting collection name based on the content of the object - `store.conventions.findCollectionNameForObjectLiteral()`

{CODE:nodejs storing_literals_1@client-api\session\storingEntities.js /}

If you fail to do so, your object literal based entites will land up in *@empty* collection having an *UUID* for an ID.

## Related Articles

### Conventions

- [Querying](../../client-api/configuration/querying)
- [Serialization](../../client-api/configuration/serialization)
- [Load Balance & Failover](../../client-api/configuration/load-balance-and-failover)

### Document Identifiers

- [Working with Document Identifiers](../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../client-api/configuration/identifier-generation/type-specific)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
