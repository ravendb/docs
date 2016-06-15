#Conventions related to request handling

###UseParallelMultiGet

Instruct the client to do parallel MultiGet processing when handling lazy requests. It is enabled by default.

{CODE:java use_paraller_multi_get@ClientApi\Configuration\Conventions\RequestHandling.java /}

###HandleForbiddenResponse

The function that begins the handling of forbidden responses.

{CODE:java handle_forbidden_response@ClientApi\Configuration\Conventions\RequestHandling.java /}

###HandleUnauthorizedResponse

It handles of unauthenticated responses, usually by authenticating against the oauth server.

{CODE:java handle_unauthorized_response@ClientApi\Configuration\Conventions\RequestHandling.java /}


###JsonContractResolver

The default `JsonContractResolver` used by RavenDB will serialize all properties and all public fields. You can change it by providing own implementation of `DeserializationProblemHandler` class:

{CODE:java json_contract_resolver@ClientApi\Configuration\Conventions\RequestHandling.java /}

{CODE:java custom_json_contract_resolver@ClientApi\Configuration\Conventions\RequestHandling.java /}

###PreserveDocumentPropertiesNotFoundOnModel

Controls whatever properties that were not de-serialized to an object properties will be preserved 
during saving a document again. If `false`, those properties will be removed when the document will be saved. Default: `true`.

{CODE:java preserve_doc_props_not_found_on_model@ClientApi\Configuration\Conventions\RequestHandling.java /}

##Related articles

- [How to send custom request using HttpJsonRequestFactory?](../../how-to/send-custom-request-using-httpjsonrequestfactory)