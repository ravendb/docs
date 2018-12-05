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

If following language-specific key casing conventions RavenDB clients use different field/properties naming conventions:

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
