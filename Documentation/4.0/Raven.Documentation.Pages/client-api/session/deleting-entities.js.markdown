# Session: Deleting Entities

Entities can be marked for deletion by using the `delete()` method, but will *not* be removed from the server until `saveChanges()` is called.

## Syntax

{CODE:nodejs deleting_1@client-api\session\deletingEntities.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** or **id** | object or string | instance of the entity to delete or the entity ID |
| **changeVector** | string | a change vector to use for concurrency checks |

## Example I

{CODE:nodejs deleting_2@client-api\session\deletingEntities.js /}

{NOTE: Concurrency on Delete}
If `useOptimisticConcurrency` is set to *true* (default *false*), the `delete()` method will use loaded *employees/1* change vector for concurrency check and might throw `ConcurrencyException`.
{NOTE/}

## Example II

{CODE:nodejs deleting_3@client-api\session\deletingEntities.js /}

{NOTE: Concurrency on Delete}
In this example, the `delete()` method will not do any change vector based concurrency checks because the change vector for *employees/1* is unknown.
{NOTE/}

{INFO:Information}

If entity is **not** tracked by session, then executing

{CODE:nodejs deleting_4@client-api\session\deletingEntities.js /}

is equal to doing

{CODE:nodejs deleting_5@client-api\session\deletingEntities.js /}

{NOTE: Change Vector in DeleteCommandData}
In this sample the change vector is null - this means that there will be no concurrency checks. A non-null and valid change vector value will trigger a concurrency check. 
{NOTE/}

You can read more about defer operations [here](./how-to/defer-operations).

{INFO/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Basics](../../indexes/querying/basics)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
