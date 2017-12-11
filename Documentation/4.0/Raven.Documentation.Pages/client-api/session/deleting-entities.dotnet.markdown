# Session : Deleting entities

Entities can be marked for deletion by using `Delete` method, but will not be removed from server until `SaveChanges` is called.

## Syntax

{CODE deleting_1@ClientApi\Session\DeletingEntities.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **id** | T, ValueType or string | instance of entity to delete or entity Id |
| **expectedChangeVector** | string | a change vector to use for concurrency checks

## Example 1

{CODE deleting_2@ClientApi\Session\DeletingEntities.cs /}

{NOTE: Concurrency on Delete}
If UseOptimisticConcurrency is set to 'true' (default 'false'), the Delete() method will use loaded 'employees/1' change vector for concurrency check and might throw ConcurrencyException.
{NOTE/}

## Example 2

{CODE deleting_3@ClientApi\Session\DeletingEntities.cs /}

{NOTE: Concurrency on Delete}
In this overload, the Delete() method will not do any change vector based-based concurrency checks because the change vector for 'employees/1' is unknown
{NOTE/}

{INFO:Information}

If entity is **not** tracked by session, then executing

{CODE deleting_4@ClientApi\Session\DeletingEntities.cs /}

is equal to doing

{CODE deleting_5@ClientApi\Session\DeletingEntities.cs /}

{NOTE: Change Vector in DeleteCommandData}
In this sample the change vector is null - this means that there will be no concurrency checks. A non-null and valid change vector value will trigger a concurrency check. 
{NOTE/}

You can read more about defer operations [here](./how-to/defer-operations).

{INFO/}

## Related articles

- [Opening a session](./opening-a-session)  
- [Loading entities](./loading-entities)  
