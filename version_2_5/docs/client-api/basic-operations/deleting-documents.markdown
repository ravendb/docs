
### Deleting documents

#### Delete by an object reference

Once a valid reference to a document has been retrieved, the document can be deleted with a call to `Delete` in the following manner:

	session.Delete(existingBlogPost);
	session.SaveChanges();
		
{NOTE Deletes are final and cannot be rolled back, once committed. /} 

#### Delete by ID

If you have the document's ID, and you don't want to load it just for the sake of delete it, you can either defer deleting it using the `Defer` command or doing a direct delete using the `DatabaseCommands`.

##### Delete by ID using the Defer command

Using the `Defer` command in the `Advanced` section of the session API, you can pass a `DeleteCommandData` instance which will instruct the session to delete the document upon the call to the `SaveChanges` method. This will ensure that the delete will be transactional because of its participation in the Unit Of Work of the session.

Here is an example how to use it:

	session.Advanced.Defer(new DeleteCommandData { Key = "posts/1234" });

##### Delete by ID using the DatabaseCommands

You can perform a direct delete using the `DatabaseCommands` property available in the `Advanced` section of the session API.

	Session.Advanced.DocumentStore.DatabaseCommands.Delete("posts/1234", null);

Unlike session operations, `DatabaseCommands` operations execute immediately, and do not wait for `SaveChanges` to be called.