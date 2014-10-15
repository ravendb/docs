# How to send custom request using HttpJsonRequestFactory?

Custom requests can be send using HttpJsonRequestFactory which can be accessed by `getJsonRequestFactory()` in `DocumentStore`. This is the same factory that client uses for communication.

Worth knowing is that factory might not always be accessible. To check if document store contains factory use `hasJsonRequestFactory`. In most of cases value of this property will be `true`. 

## Example

{CODE:java custom_request_1@ClientApi\HowTo\SendCustomRequest.java /}

## Related articles

TODO