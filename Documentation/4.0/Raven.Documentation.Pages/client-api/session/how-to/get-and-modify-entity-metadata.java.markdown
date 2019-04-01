# Session: How to Get and Modify Entity Metadata

When a document is downloaded from the server, it contains various metadata information like ID or current change-vector. This information is stored in a session and is available for each entity using the `getMetadataFor` method from the `advanced` session operations.

## Get the Metadata

{CODE:java get_metadata_1@ClientApi\Session\HowTo\GetMetadata.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which metadata will be returned. |

| Return Value | |
| ------------- | ----- |
| IMetadataDictionary | Returns the metadata for the specified entity. Throws an exception if the `instance` is not tracked by the session. |

### Example

{CODE:java get_metadata_2@ClientApi\Session\HowTo\GetMetadata.java /}


## Modify the Metadata
After getting the metadata from `session.advanced().getMetadataFor` you can modify it just like any other map.
{NOTE Keys in the metadata that starting with @ are reserved for RavenDB use /}

### Example I
{CODE:java modify_metadata_1@ClientApi\Session\HowTo\GetMetadata.java /}

### Example II
{CODE:java modify_metadata_2@ClientApi\Session\HowTo\GetMetadata.java /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get Entity Change-Vector](../../../client-api/session/how-to/get-entity-change-vector)
- [How to Get Entity Last Modified](../../../client-api/session/how-to/get-entity-last-modified)

### Indexes

- [Converting to JSON and Accessing Metadata](../../../indexes/converting-to-json-and-accessing-metadata)

