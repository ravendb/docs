# Session: How to get entity metadata?

When document is downloaded from server it contains various metadata information e.g. Id or current etag. This information is stored in session and available for each entity using `GetMetadataFor` method from `Advanced` session operations.

## Syntax

{CODE get_metadata_1@ClientApi\Session\HowTo\GetMetadata.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which metadata will be returned. |

| Return Value | |
| ------------- | ----- |
| RavenJObject | Returns entity metadata. If the `instance` is transient it will load document from server and attach entity and its metadata to session. |

## Example

{CODE get_metadata_2@ClientApi\Session\HowTo\GetMetadata.cs /}
