#Conventions related to request handling

###UseParallelMultiGet

Instruct the client to do parallel MultiGet processing when handling lazy requests. It is enabled by default.

{CODE use_paraller_multi_get@ClientApi\Configuration\Conventions\RequestHandling.cs /}


###AllowMultipuleAsyncOperations

Enable or disable multiple async requests per client instance. By default only a single concurrent async request is allowed.

{CODE allow_multiple_async_ops@ClientApi\Configuration\Conventions\RequestHandling.cs /}

###HandleForbiddenResponseAsync

The async function that begins the handling of forbidden responses.

{CODE handle_forbidden_response_async@ClientApi\Configuration\Conventions\RequestHandling.cs /}

###HandleUnauthorizedResponseAsync

It begins the handling of unauthenticated responses, usually by authenticating against the oauth server in async manner.

{CODE handle_unauthorized_response_async@ClientApi\Configuration\Conventions\RequestHandling.cs /}


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

##Related articles

- [How to send custom request using HttpJsonRequestFactory?](../../how-to/send-custom-request-using-httpjsonrequestfactory)