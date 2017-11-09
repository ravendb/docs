# Session : Storing entities in session

To store entities inside the **session** object, use one of the three `Store` methods.

## Syntax

First overload: stores entity in session, then extracts Id from entity or generates a new one if it is not available.

{CODE store_entities_1@ClientApi\Session\StoringEntities.cs /}

Second overload: stores entity in session with given id.

{CODE store_entities_2@ClientApi\Session\StoringEntities.cs /}

Third overload: stores entity in session with given id, forces concurrency check with given change vector.

{CODE store_entities_3@ClientApi\Session\StoringEntities.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity that will be stored |
| **changeVector** | string | Entity changeVector, used for concurrency checks (`null` to skip check) |
| **id** | string | Entity will be stored under this key, (`null` to generate automatically) |

## Example

{CODE store_entities_5@ClientApi\Session\StoringEntities.cs /}

## Related articles

- [Saving changes](./saving-changes)  
