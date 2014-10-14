# Session : Deleting entities

Entities can be marked for deletion by using `delete` method, but will not be removed from server untill `saveChanges` is called.

## Syntax

{CODE:java deleting_1@ClientApi\Session\DeletingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **id** | T, number, UUID or string | instance of entity to delete or entity Id |

## Example 1

{CODE:java deleting_2@ClientApi\Session\DeletingEntities.java /}

## Example 2

{CODE:java deleting_3@ClientApi\Session\DeletingEntities.java /}

## Related articles

TODO