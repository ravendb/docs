# Session : How to Get and Modify Entity Metadata

When a document is downloaded from the server, it contains various metadata information e.g. ID or current change-vector. This information is stored in session and is available for each entity using the `GetMetadataFor` method from the `Advanced` session operations.

## Get the metadata

{CODE get_metadata_1@ClientApi\Session\HowTo\GetMetadata.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which metadata will be returned. |

| Return Value | |
| ------------- | ----- |
| IMetadataDictionary | Returns the metadata for the specified entity. If the `instance` is transient, it will load the metadata from the store and associate the current state of the entity with the metadata from the server. |

### Example

{CODE get_metadata_2@ClientApi\Session\HowTo\GetMetadata.cs /}


## Modify the metadata
After getting the metadata from ```session.session.Advanced.GetMetadataFor``` you can modify it just like any other dictionary.
{NOTE ```Constants.Documents.Metadata``` class can help you with the defult convention keys of the metadata /}

{CODE modify_metadata_1@ClientApi\Session\HowTo\GetMetadata.cs /}
