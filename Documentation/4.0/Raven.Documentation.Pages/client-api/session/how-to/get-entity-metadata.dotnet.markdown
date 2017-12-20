# Session : How to Get Entity Metadata

When a document is downloaded from the server, it contains various metadata information e.g. Id or current change-vector. This information is stored in session and is available for each entity using the `GetMetadataFor` method from `Advanced` session operations.

## Syntax

{CODE get_metadata_1@ClientApi\Session\HowTo\GetMetadata.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which metadata will be returned. |

| Return Value | |
| ------------- | ----- |
| IMetadataDictionary | Returns the metadata for the specified entity. If the `instance` is transient, it will load the metadata from the store and associate the current state of the entity with the metadata from the server. |

## Example

{CODE get_metadata_2@ClientApi\Session\HowTo\GetMetadata.cs /}
