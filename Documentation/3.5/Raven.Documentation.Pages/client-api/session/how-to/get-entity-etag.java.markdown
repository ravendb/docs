# Session: How to get entity etag?

When document is downloaded from server it contains various metadata information e.g. Id or current etag. Current etag is stored within metadata in session and available for each entity using `getEtagFor` method from `advanced` session operations.

## Syntax

{CODE:java get_etag_1@ClientApi\Session\HowTo\GetEtag.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which etag will be returned. |

| Return Value | |
| ------------- | ----- |
| ETag | Returns current etag for an entity. If the `instance` is transient it will load document from server and attach entity and its metadata to session. |

## Example

{CODE:java get_etag_2@ClientApi\Session\HowTo\GetEtag.java /}
