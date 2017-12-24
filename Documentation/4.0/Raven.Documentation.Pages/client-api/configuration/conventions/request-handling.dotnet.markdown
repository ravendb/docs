#Conventions related to request handling

###MaxHttpCacheSize

If you need to modify the maximum http cache size, you can use the following setting:

{CODE MaxHttpCacheSize@ClientApi\Configuration\Conventions\RequestHandling.cs /}

{NOTE Default value of this setting is 512MB on 64 bits, 32MB on 32 bits /}

###ThrowIfQueryPageSizeIsNotSet

To decide if you would like to for any query in session to throw when no explicit page size set, you can use the following setting:

{CODE ThrowIfQueryPageSizeIsNotSet@ClientApi\Configuration\Conventions\RequestHandling.cs /}

{NOTE From 4.0 there is no limitation for number of results returned from server. This can be useful for development purposes to pinpoint all the possible performance bottlenecks. /}

###CustomizeJsonSerializer

If you need to modify `JsonSerializer` object used by the client you can register a customization action:

{CODE customize_json_serializer@ClientApi\Configuration\Conventions\RequestHandling.cs /}

###JsonContractResolver

The default `JsonContractResolver` used by RavenDB will serialize all properties and all public fields. You can change it by providing own implementation of `IContractResolver` interface:

{CODE json_contract_resolver@ClientApi\Configuration\Conventions\RequestHandling.cs /}

{CODE custom_json_contract_resolver@ClientApi\Configuration\Conventions\RequestHandling.cs /}

###PreserveDocumentPropertiesNotFoundOnModel

Controls whatever properties that were not de-serialized to an object properties will be preserved 
during saving a document again. If `false`, those properties will be removed when the document will be saved. Default: `true`.

{CODE preserve_doc_props_not_found_on_model@ClientApi\Configuration\Conventions\RequestHandling.cs /}

###MaxNumberOfRequestsPerSession

Gets or sets maximum number of GET requests per session

{CODE MaxNumberOfRequestsPerSession@ClientApi\Configuration\Conventions\RequestHandling.cs /}

###UseOptimisticConcurrency

Controls whether optimistic concurrency is set to true by default for all future sessions

{CODE UseOptimisticConcurrency@ClientApi\Configuration\Conventions\RequestHandling.cs /}

##Related articles

- [How to send custom request using HttpJsonRequestFactory?](../../how-to/send-custom-request-using-httpjsonrequestfactory)
