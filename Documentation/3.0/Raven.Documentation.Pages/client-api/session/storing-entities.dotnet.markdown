# Session: Storing entities in session

To store entities inside session object use one of the four `Store` methods.

## Syntax

First overload stores entity in session, extracts Id from entity using `Conventions` or generates new one if it is not available.

{CODE store_entities_1@ClientApi\Session\StoringEntities.cs /}

Second overload stores entity in session, extracts Id from entity using `Conventions` or generates new one if it is not available and forces concurrency check with given Etag.

{CODE store_entities_3@ClientApi\Session\StoringEntities.cs /}

Third overload stores entity in session with given id.

{CODE store_entities_2@ClientApi\Session\StoringEntities.cs /}

Fourth overload stores entity in session with given id and forces concurrency check with given Etag.

{CODE store_entities_4@ClientApi\Session\StoringEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity that will be stored |
| **etag** | Etag | Current entity etag, used for concurrency checks (`null` to skip check) |
| **id** | string | Entity will be stored under this key, (`null` to generate automatically) |

## Example

{CODE store_entities_5@ClientApi\Session\StoringEntities.cs /}

## Related articles

- [Saving changes](./saving-changes)  
