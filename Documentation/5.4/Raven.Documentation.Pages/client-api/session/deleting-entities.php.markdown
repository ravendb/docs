# Session: Deleting Entities

Entities can be marked for deletion by using the `delete` method, but will not be removed from the server until `saveChanges` is called.  

## Syntax

{CODE:php deleting_1@ClientApi\Session\DeletingEntities.php /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | `T` | instance of the entity to delete |
| **id** | `string` | ID of the entity to delete |
| **expectedChangeVector** | `string` | a change vector to use for concurrency checks |

## Example I

{CODE:php deleting_2@ClientApi\Session\DeletingEntities.php /}

{NOTE: Concurrency on Delete}
If UseOptimisticConcurrency is set to 'true' (default 'false'), the Delete() method will use loaded 'employees/1' change vector for concurrency check and might throw ConcurrencyException.  
{NOTE/}

## Example II

{CODE:php deleting_3@ClientApi\Session\DeletingEntities.php /}

{NOTE: Concurrency on Delete}
In this overload, the Delete() method will not do any change vector based concurrency checks because the change vector for 'employees/1' is unknown.  
{NOTE/}

{INFO:Information}

If entity is **not** tracked by session, then executing:  

{CODE:php deleting_4@ClientApi\Session\DeletingEntities.php /}

is equal to doing:  

{CODE:php deleting_5@ClientApi\Session\DeletingEntities.php /}

{NOTE: Change Vector in DeleteCommandData}
In this sample the change vector is null - this means that there will be no concurrency checks. A non-null and valid change vector value will trigger a concurrency check.  
{NOTE/}

You can read more about defer operations [here](../../client-api/session/how-to/defer-operations).  

{INFO/}

## Related Articles  

### Session  

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work)  
- [Opening a Session](../../client-api/session/opening-a-session)  
- [Loading Entities](../../client-api/session/loading-entities)  
- [Saving Changes](../../client-api/session/saving-changes)  
- [How To: Enable Optimistic Concurrency](../../client-api/session/configuration/how-to-enable-optimistic-concurrency)  

### Querying  

- [Query Overview](../../client-api/session/querying/how-to-query)

### Document Store  

- [What is a Document Store](../../client-api/what-is-a-document-store)  
