# Session: How to Get Entity Change-Vector

The change-vector reflects the cluster wide point in time where something happened. It includes the unique database ID, node identifier, and the Etag of the document in the specific node.
When a document is downloaded from the server, it contains various metadata information e.g. ID or current change-vector. Current change-vector is stored within the metadata in session and is available for each entity using the `GetChangeVectorFor` method from the `Advanced` session operations.

## Syntax

{CODE get_change_vector_1@ClientApi\Session\HowTo\GetChangeVector.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which an Etag will be returned. |

| Return Value | |
| ------------- | ----- |
| string | Returns the current change-vector for an entity. Throws an exception if the `instance` is not tracked by the session. |

## Example

{CODE get_change_vector_2@ClientApi\Session\HowTo\GetChangeVector.cs /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)
- [How to Get Entity Last Modified](../../../client-api/session/how-to/get-entity-last-modified)
