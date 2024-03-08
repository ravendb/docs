# Session: Deleting Entities

Entities can be marked for deletion by using the `delete()` method, but will not be removed from the server until `save_changes()` is called.  

## Syntax

{CODE:python deleting_1@ClientApi\Session\DeletingEntities.py /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key_or_entity** | `str` or `object` | ID of the document or instance of the entity to delete |
| **expected_change_vector** | `str` | a change vector to use for concurrency checks |

## Example I

{CODE:python deleting_2@ClientApi\Session\DeletingEntities.py /}

{NOTE: Concurrency on Delete}
If use_optimistic_concurrency is set to 'True' (default 'False'), the delete() method will use loaded 'employees/1' change vector for concurrency check and might throw ConcurrencyException.  
{NOTE/}

## Example II

{CODE:python deleting_3@ClientApi\Session\DeletingEntities.py /}

{NOTE: Concurrency on Delete}
The delete() method will not do any change vector based concurrency checks because the change vector for 'employees/1' is unknown.  
{NOTE/}

{INFO:Information}

If entity is **not** tracked by session, then executing:  

{CODE:python deleting_4@ClientApi\Session\DeletingEntities.py /}

is equal to doing:  

{CODE:python deleting_5@ClientApi\Session\DeletingEntities.py /}

{NOTE: Change Vector in DeleteCommandData}
In this sample the change vector is None - this means that there will be no concurrency checks. A not-None and valid change vector value will trigger a concurrency check.  
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
