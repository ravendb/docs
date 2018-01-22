#Conventions

Conventions give you an ability to customize the Client API behavior. They are accessible from `DocumentStore` object:

{CODE conventions_1@ClientApi\Configuration\Conventions.cs /}

You will find many settings to overwrite, allowing you to adjust the client according to your needs. The conventions apply to many different client behaviors, some of them are grouped and described in the separate articles of this section.

{INFO All customizations need to be set before `DocumentStore.Initialize()` is called. /}

{PANEL:MaxHttpCacheSize}

If you need to modify the maximum http cache size, you can use the following setting:

{CODE MaxHttpCacheSize@ClientApi\Configuration\Conventions.cs /}

{NOTE Default value of this setting is 512MB on 64 bits, 32MB on 32 bits /}

{PANEL/}

{PANEL:MaxNumberOfRequestsPerSession}

Gets or sets maximum number of GET requests per session. Default: `30`.

{CODE MaxNumberOfRequestsPerSession@ClientApi\Configuration\Conventions.cs /}

{PANEL/}

{PANEL:UseOptimisticConcurrency}

Controls whether optimistic concurrency is set to true by default for all future sessions. Default: `false`.

{CODE UseOptimisticConcurrency@ClientApi\Configuration\Conventions.cs /}

{PANEL/}

{PANEL:DisableTopologyUpdates}

Forces to disable updates of database topology. Default: `false`.

{CODE DisableTopologyUpdates@ClientApi\Configuration\Conventions.cs /}

{PANEL/}

{PANEL:SaveEnumsAsIntegers}

It determines if C# `enum` types should be saved as integers or strings and instruct the Linq provider to query enums as integer values. Default: `false`.

{CODE SaveEnumsAsIntegers@ClientApi\Configuration\Conventions.cs /}

{PANEL/}


