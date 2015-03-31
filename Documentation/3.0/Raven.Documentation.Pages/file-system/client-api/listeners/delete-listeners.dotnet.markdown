#Delete listeners

`IFilesDeleteListener` interface which needs to be implemented if a user wants to introduce a custom action when a delete operation is executed. 
The interface exposes two methods. The first one is invoked before the delete request is sent to the server, the second one is called
after the request was successfully executed.

{CODE delete_interface@FileSystem\ClientApi\Listeners\Delete.cs /}

Note that `BeforeDelete` method returns a boolean result that is used to determine whether a file should be really deleted. If any of the registered
listeners returns `false` then the delete request won't be sent to a server and no `AfterDelete` method will be called.

##Example

To prevent anyone from deleting files that have `Read-Only : true` metadata you can create the following listener:

{CODE example@FileSystem\ClientApi\Listeners\Delete.cs /}