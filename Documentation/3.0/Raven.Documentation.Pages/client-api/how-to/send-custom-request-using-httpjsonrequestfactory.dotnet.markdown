# How to send custom request using HttpJsonRequestFactory?

Custom requests can be send using HttpJsonRequestFactory which can be accessed by `JsonRequestFactory` property in `DocumentStore`. This is the same factory that client uses for communication.

Worth knowing is that factory might not always be accessible. To check if document store contains factory use `HasJsonRequestFactory`. In most of cases value of this property will be `true`. For the time being only `ShardedDocumentStore` does not contain request factory, because there is no single request factory in such document store, so to access factory you must use shards directly.

## Example

{CODE custom_request_1@ClientApi\HowTo\SendCustomRequest.cs /}

## Remarks

{INFO `ForDatabase`, `Docs` extensions and many others are a part of url extensions available in `Raven.Client.Connection` namespace. /}

#### Related articles

TODO