# Session : How to get entity change-vector?

The change-vector reflect the cluster wide point in time where something happened, it include the unique database id, node identifier and the etag of the document in the specific node.
When document is downloaded from server it contains various metadata information e.g. Id or current change-vector. Current change-vector is stored within metadata in session and available for each entity using `GetChangeVectorFor` method from `Advanced` session operations.

## Syntax

{CODE get_change_vector_1@ClientApi\Session\HowTo\GetChangeVector.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which etag will be returned. |

| Return Value | |
| ------------- | ----- |
| string | Returns current change-vector for an entity. If the `instance` is transient it will load document from server and attach entity and its metadata to session. |

## Example

{CODE get_change_vector_2@ClientApi\Session\HowTo\GetChangeVector.cs /}
