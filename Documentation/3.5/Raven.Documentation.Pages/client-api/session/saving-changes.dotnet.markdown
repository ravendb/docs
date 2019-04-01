# Session: Saving changes

Pending session operations e.g. `Store`, `Delete` and many others will not be send to server till `SaveChanges` is called.

##Syntax

{CODE saving_changes_1@ClientApi\Session\SavingChanges.cs /}

##Example

{CODE saving_changes_2@ClientApi\Session\SavingChanges.cs /}

{NOTE:Waiting for indexes}

You can ask the server to wait until the indexes are caught up with this particular write after save changes.
You can also set a timeout and whatever to throw or not. 
You can specify indexes that you want to wait for. If you don't specify anything, RavenDB will automatically select just the indexes that are impacted by this write.

{CODE saving_changes_3@ClientApi\Session\SavingChanges.cs /}

{NOTE/}
## Related articles

- [Opening a session](./opening-a-session)  
- [Deleting entities](./deleting-entities)  
- [Storing entities](./storing-entities)  
