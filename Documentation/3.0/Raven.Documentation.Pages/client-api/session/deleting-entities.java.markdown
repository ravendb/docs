# Session: Deleting entities

Entities can be marked for deletion by using `delete` method, but will not be removed from server until `saveChanges` is called.

## Syntax

{CODE:java deleting_1@ClientApi\Session\DeletingEntities.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **id** | T, number, UUID or string | instance of entity to delete or entity Id |

## Example 1

{CODE:java deleting_2@ClientApi\Session\DeletingEntities.java /}

## Example 2

{CODE:java deleting_3@ClientApi\Session\DeletingEntities.java /}

{INFO:Information}

If entity is **not** tracked by session, then executing

{CODE:java deleting_4@ClientApi\Session\DeletingEntities.java /}

is equal to doing

{CODE:java deleting_5@ClientApi\Session\DeletingEntities.java /}

You can read more about defer operations [here](./how-to/defer-operations).

{INFO/}

## Related articles

- [Opening a session](./opening-a-session)  
- [Loading entities](./loading-entities)  

