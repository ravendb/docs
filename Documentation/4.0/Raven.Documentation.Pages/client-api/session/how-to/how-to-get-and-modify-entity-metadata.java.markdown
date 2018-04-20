# Session : How to Get and Modify Entity Metadata

When a document is downloaded from the server, it contains various metadata information like ID or current change-vector. This information is stored in a session and is available for each entity using the `getMetadataFor` method from the `advanced` session operations.

## Get the Metadata

{CODE:java get_metadata_1@ClientApi\Session\HowTo\GetMetadata.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which metadata will be returned. |

| Return Value | |
| ------------- | ----- |
| IMetadataDictionary | Returns the metadata for the specified entity. If the `instance` is transient, it will load the metadata from the store and associate the current state of the entity with the metadata from the server. |

### Example

{CODE:java get_metadata_2@ClientApi\Session\HowTo\GetMetadata.java /}


## Modify the Metadata
After getting the metadata from `session.advanced().getMetadataFor` you can modify it just like any other map.
{NOTE Keys in the metadata that starting with @ are reserved for RavenDB use /}

### Example I
{CODE:java modify_metadata_1@ClientApi\Session\HowTo\GetMetadata.java /}

### Example II
{CODE:java modify_metadata_2@ClientApi\Session\HowTo\GetMetadata.java /}
