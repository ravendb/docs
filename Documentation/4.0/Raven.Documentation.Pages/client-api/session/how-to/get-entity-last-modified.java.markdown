# Session : How to Get Entity Last Modified 

When a document is downloaded from the server it contains various metadata information, including the last modified date of the document.  

Last modified date is stored within the metadata in session and is available for each entity using the `getLastModifiedFor` method from the `advanced` session operations.

## Syntax

{CODE:java get_last_modified_1@ClientApi\Session\HowTo\GetLastModified.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **instance** | T | Instance of an entity for which the last modified date will be returned. |

| Return Value | |
| ------------- | ----- |
| Date | Returns the last modified date for an entity. If the `instance` is transient, it will load the document from the server and attach the entity and its metadata to the session. |


## Example

{CODE:java get_last_modified_2@ClientApi\Session\HowTo\GetLastModified.java /}
