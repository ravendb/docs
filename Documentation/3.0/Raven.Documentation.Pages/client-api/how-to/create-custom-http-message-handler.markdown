#How to create a custom message handler?

RavenDB client uses `HttpClient` under the hood to talk to RavenDB server through HTTP. In order to customize how it should deal
with sending and receiving requests you might overwrite its default message handler using `DocumentStore.HttpMessageHandlerFactory`.

##Example

To properly setup windows auth with unsafe connection sharing use the following code:

{CODE custom_handler_provided@ClientApi\HowTo\CreateCustomHttpMessageHandler.cs /}