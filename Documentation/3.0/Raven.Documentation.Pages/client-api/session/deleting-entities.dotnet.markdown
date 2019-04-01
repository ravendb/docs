# Session: Deleting entities

Entities can be marked for deletion by using `Delete` method, but will not be removed from server until `SaveChanges` is called.

## Syntax

{CODE deleting_1@ClientApi\Session\DeletingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **id** | T, ValueType or string | instance of entity to delete or entity Id |

## Example 1

{CODE deleting_2@ClientApi\Session\DeletingEntities.cs /}

## Example 2

{CODE deleting_3@ClientApi\Session\DeletingEntities.cs /}

{INFO:Information}

If entity is **not** tracked by session, then executing

{CODE deleting_4@ClientApi\Session\DeletingEntities.cs /}

is equal to doing

{CODE deleting_5@ClientApi\Session\DeletingEntities.cs /}

You can read more about defer operations [here](./how-to/defer-operations).

{INFO/}

## Related articles

- [Opening a session](./opening-a-session)  
- [Loading entities](./loading-entities)  
