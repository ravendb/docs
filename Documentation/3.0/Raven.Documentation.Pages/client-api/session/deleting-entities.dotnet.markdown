# Session : Deleting entities

Entities can be marked for deletion by using `Delete` method, but will not be removed from server untill `SaveChanges` is called.

## Syntax

{CODE deleting_1@ClientApi\Session\DeletingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **id** | T, ValueType or string | instance of entity to delete or entity Id |

## Example 1

{CODE deleting_2@ClientApi\Session\DeletingEntities.cs /}

## Example 2

{CODE deleting_3@ClientApi\Session\DeletingEntities.cs /}

## Related articles

TODO