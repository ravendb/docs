# Session: How to Get Entity Change-Vector

* The change-vector reflects the cluster-wide point in time in which a change occured, and 
  includes the unique database ID, node identifier, and document Etag in the specific node.  

* When a document is downloaded from the server, it contains various metadata information.  
  E.g. ID or current change-vector.  

* The current change-vector is stored within the session metadata and is available for each 
  entity using the `getChangeVectorFor` method from the `advanced` session operations.

## Syntax

{CODE:php get_change_vector_1@ClientApi\Session\HowTo\GetChangeVector.php /}

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **$instance** | `?object` | An instance of an entity for which an Etag will be returned |

| Return Type | Description |
| ----------- | ----------- |
| `?string` | Returns the current change-vector for an entity. Throws an exception if the entity is not tracked by the session. |

## Example

{CODE:php get_change_vector_2@ClientApi\Session\HowTo\GetChangeVector.php /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)
- [How to Get Entity Last Modified](../../../client-api/session/how-to/get-entity-last-modified)
