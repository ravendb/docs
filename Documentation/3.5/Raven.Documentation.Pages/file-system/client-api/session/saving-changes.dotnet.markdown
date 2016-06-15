#Saving changes

Pending registered operations (`RegisterUpload`, `RegisterRename`, `RegisterFileDeletion` or metadata changes) will not be send to the server until  the `SaveChangesAsync` is called.

##Syntax

{CODE save_changes_1@FileSystem\ClientApi\Session\SavingChanges.cs /}

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous save operation |


##Applying changes

The RavenFS session, in contrast to the [`IDocumentSession`](../../../client-api/session/what-is-a-session-and-how-does-it-work), does not send changes as a batch operation in a single call. Each registered file modification will be executed in a separate request. 

If the exception occurs when the file change is applied, the exception will be thrown by the `SaveChangesAsync` and any pending modifications will be canceled.

##Example

{CODE save_changes_2@FileSystem\ClientApi\Session\SavingChanges.cs /}