# Conventions

Conventions give you the ability to customize the Client API behavior. They are accessible from `DocumentStore` object:

{CODE:java conventions_1@ClientApi\Configuration\Conventions.java /}

You will find many settings to overwrite, allowing you to adjust the client according to your needs. The conventions apply to many different client behaviors. Some of them are grouped and described in the separate articles of this section.

{INFO All customizations need to be set before `DocumentStore.initialize()` is called. /}

##MaxHttpCacheSize

If you need to modify the maximum http cache size, you can use the following setting:

{CODE:java MaxHttpCacheSize@ClientApi\Configuration\Conventions.java /}

{NOTE: Default size}

The default value of this setting is configured to 128 MB.

The cache is created per database you use.

{NOTE/}

{NOTE: Disable caching} 

To disable the caching globally you can set the `maxHttpCacheSize` value to zero:

{CODE:java disable_cache@ClientApi\Configuration\Conventions.java /}

**In this scenario, all the requests will be sent to the server to fetch the data.**

{NOTE/}

## MaxNumberOfRequestsPerSession

Gets or sets maximum number of GET requests per session. Default: `30`.

{CODE:java MaxNumberOfRequestsPerSession@ClientApi\Configuration\Conventions.java /}

##UseOptimisticConcurrency

Controls whether optimistic concurrency is set to true by default for all future sessions. Default: `false`.

{CODE:java UseOptimisticConcurrency@ClientApi\Configuration\Conventions.java /}

##DisableTopologyUpdates

Forces you to disable updates of database topology. Default: `false`.

{CODE:java DisableTopologyUpdates@ClientApi\Configuration\Conventions.java /}

##SaveEnumsAsIntegers

It determines if Java `Enum` types should be saved as integers or strings. Default: `false`.

{CODE:java SaveEnumsAsIntegers@ClientApi\Configuration\Conventions.java /}


##UseCompression

It determines if the client will send headers to the server indicating that it allows compression to be used. Default: `true`.

{CODE:java UseCompression@ClientApi\Configuration\Conventions.java /}

## Changing fields/properties naming convention 

By default whatever casing convention you use in your entities' fields will be reflected server-side.

If following language-specific field casing conventions RavenDB clients use different field/properties naming conventions:

| Language | Default convention | Example |
| ------------- | ----- | --- |
| C# | PascalCase | OrderLines |
| Java | camelCase | orderLines |
| JavaScript | camelCase | orderLines |

This can be configured to allow inter-language operability e.g. store data PascalCase, but keep fields in the application code camelCase.

### Using PascalCase in Java client

You have to set *property naming strategy*:

{CODE:java PropertyCasing@ClientApi\Configuration\Conventions.java /}


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
