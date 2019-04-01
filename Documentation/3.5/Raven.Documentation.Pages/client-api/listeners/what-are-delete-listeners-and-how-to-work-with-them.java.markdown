# Listeners: What are delete listeners and how to work with them?

`IDocumentDeleteListener` interface which needs to be implemented if a user wants to add a custom logic or an action when a delete operation is executed. 
The interface contains only one method that is invoked before the delete request is sent to the server:

{CODE:java document_delete_interface@ClientApi\Listeners\Delete.java /}

##Example

To prevent anyone from deleting documents we can create `preventDeleteListener` with can be implemented as follows:

{CODE:java document_delete_example@ClientApi\Listeners\Delete.java /}
