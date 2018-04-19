#Conventions

Conventions give you an ability to customize the Client API behavior. They are accessible from `DocumentStore` object:

{CODE:java conventions_1@ClientApi\Configuration\Conventions.java /}

You will find many settings to overwrite, allowing you to adjust the client according to your needs. The conventions apply to many different client behaviors, some of them are grouped and described in the separate articles of this section.

{INFO All customizations need to be set before `DocumentStore.initialize()` is called. /}

##MaxHttpCacheSize

If you need to modify the maximum http cache size, you can use the following setting:

{CODE:java MaxHttpCacheSize@ClientApi\Configuration\Conventions.java /}

{NOTE: Default size}

The default value of this setting is configured to 128 MB.

The cache is created per database you use.

{NOTE/}

{NOTE: Disable caching} 

To disable the caching you can set the `maxHttpCacheSize` value to zero:

{CODE:java disable_cache@ClientApi\Configuration\Conventions.java /}

**In this scenario all the requests will be sent to the server to fetch the data.**

{NOTE/}

## MaxNumberOfRequestsPerSession

Gets or sets maximum number of GET requests per session. Default: `30`.

{CODE:java MaxNumberOfRequestsPerSession@ClientApi\Configuration\Conventions.java /}

##UseOptimisticConcurrency

Controls whether optimistic concurrency is set to true by default for all future sessions. Default: `false`.

{CODE:java UseOptimisticConcurrency@ClientApi\Configuration\Conventions.java /}

##DisableTopologyUpdates

Forces to disable updates of database topology. Default: `false`.

{CODE:java DisableTopologyUpdates@ClientApi\Configuration\Conventions.java /}

##SaveEnumsAsIntegers

It determines if Java `Enum` types should be saved as integers or strings. Default: `false`.

{CODE:java SaveEnumsAsIntegers@ClientApi\Configuration\Conventions.java /}
