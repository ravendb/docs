# Session: Deleting entities

Entities can be marked for deletion by using `delete` method, but will not be removed from server until `save_changes` is called.

## Syntax

{CODE:python deleting_1@ClientApi\Session\DeletingEntities.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **key_or_entity** | entity, str or entity | instance of entity to delete or entity Id |

The `delete` method can be used with entity or str. by calling delete with entity `delete_by_entity` will be executed

## Example 1

{CODE:python deleting_2@ClientApi\Session\DeletingEntities.py /}

## Example 2

{CODE:python deleting_3@ClientApi\Session\DeletingEntities.py /}

## Example 3

{CODE:python deleting_4@ClientApi\Session\DeletingEntities.py /}

## Related articles

- [Opening a session](./opening-a-session)  
- [Loading entities](./loading-entities)  
