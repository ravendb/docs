# Session: How to Get Entity Last Modified 

When a document is downloaded from the server it contains various metadata details, 
including the last modified date of the document.  

Last modified date is stored within the session metadata and is available for each 
entity using the `get_last_modified_for` method from the `advanced` session operations.

## Syntax

{CODE:python get_last_modified_1@ClientApi\Session\HowTo\GetLastModified.py /}

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **entity** | `object` | Instance of an entity for which the last modified date will be returned. |

| Return Type | Description |
| ----------- | ----------- |
| `datetime` | Returns the last modified date for an entity. Throws an exception if the `object` is not tracked by the session. |

## Example

{CODE:python get_last_modified_2@ClientApi\Session\HowTo\GetLastModified.py /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Get and Modify Entity Metadata](../../../client-api/session/how-to/get-and-modify-entity-metadata)
- [How to Get Entity Change-Vector](../../../client-api/session/how-to/get-entity-change-vector)
