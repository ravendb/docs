# Session : Storing Entities in Session

To store entities inside the **session** object, use one of the three `Store` methods.

## Syntax

First overload: Stores the entity in a session, then extracts the Id from the entity or generates a new one if it's not available.

{CODE store_entities_1@ClientApi\Session\StoringEntities.cs /}

Second overload: Stores the entity in a session with given id.

{CODE store_entities_2@ClientApi\Session\StoringEntities.cs /}

Third overload: Stores the entity in a session with given id, forces concurrency check with given change vector.

{CODE store_entities_3@ClientApi\Session\StoringEntities.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity that will be stored |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |
| **id** | string | Entity will be stored under this key, (`null` to generate automatically) |

## Example

{CODE store_entities_5@ClientApi\Session\StoringEntities.cs /}

## Related Articles

- [Saving changes](./saving-changes)  
