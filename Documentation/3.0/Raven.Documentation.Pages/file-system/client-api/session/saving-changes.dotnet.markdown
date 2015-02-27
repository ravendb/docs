#Saving changes

Pending registered operations (`RegisterUpload`, `RegisterRename`, `RegisterFileDeletion` or metadata changes) will not be send to server till `SaveChangesAsync` is called.

##Syntax

{CODE save_changes_1@FileSystem\ClientApi\Session\SavingChanges.cs /}

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous save operation |


##Applying changes

The RavenFS session in contrast to [`IDocumentSession`](../../../client-api/session/what-is-a-session-and-how-does-it-work) does not send
changes as a batch operation in a single call. Each registered file modification will be executed in a separate request. 

If an exception occurs when a file change is applied then the exception will be thrown by `SaveChangesAsync` and pending modifications will be canceled.

##Example

{CODE save_changes_2@FileSystem\ClientApi\Session\SavingChanges.cs /}