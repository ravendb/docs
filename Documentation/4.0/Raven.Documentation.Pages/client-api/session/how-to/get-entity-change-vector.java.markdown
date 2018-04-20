# Session : How to Get Entity Change-Vector

The change-vector reflects the cluster wide point in time where something happened. It includes the unique database ID, node identifier, and the Etag of the document in the specific node.
When a document is downloaded from the server, it contains various metadata information e.g. ID or current change-vector. Current change-vector is stored within the metadata in session and is available for each entity using the `getChangeVectorFor` method from the `advanced` session operations.

## Syntax

{CODE:java get_change_vector_1@ClientApi\Session\HowTo\GetChangeVector.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which an Etag will be returned. |

| Return Value | |
| ------------- | ----- |
| String | Returns the current change-vector for an entity. If the `instance` is transient, it will load the document from the server and attach the entity and its metadata to the session. |

## Example

{CODE:java get_change_vector_2@ClientApi\Session\HowTo\GetChangeVector.java /}
