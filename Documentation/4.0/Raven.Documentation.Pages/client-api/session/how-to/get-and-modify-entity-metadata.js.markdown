# Session: How to Get and Modify Entity Metadata

When a document is downloaded from the server, it contains various metadata information like ID or current change-vector. This information is stored in a session and is available for each entity using the `getMetadataFor` method from the `advanced` session operations.

## Get the Metadata

{CODE:nodejs get_metadata_1@client-api\session\howTo\getMetadata.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity for which metadata will be returned. |

| Return Value | |
| ------------- | ----- |
| object | Returns the metadata for the specified entity. Throws an exception if the `instance` is not tracked by the session. |

### Example

{CODE:nodejs get_metadata_2@client-api\session\howTo\getMetadata.js /}


## Modify the Metadata
After getting the metadata from `session.advanced.getMetadataFor()` you can modify it.
{NOTE Keys in the metadata that starting with *@* are reserved for RavenDB use /}

### Example I
{CODE:nodejs modify_metadata_1@client-api\session\howTo\getMetadata.js /}

### Example II
{CODE:nodejs modify_metadata_2@client-api\session\howTo\getMetadata.js /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get Entity Change-Vector](../../../client-api/session/how-to/get-entity-change-vector)
- [How to Get Entity Last Modified](../../../client-api/session/how-to/get-entity-last-modified)

### Indexes

- [Converting to JSON and Accessing Metadata](../../../indexes/converting-to-json-and-accessing-metadata)

