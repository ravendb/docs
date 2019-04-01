# Listeners: What are listeners?

The concept of listeners provides users with a mechanism to perform custom actions, in response to operations taken in a session. 
The listener implements an interface whose methods are called when a particular action is executed on an entity or querying is run.

There are five types of available listeners:

* Document Conflict listeners (IDocumentConflictListener),
* Document Conversion listeners (IDocumentConversionListener)
* Document Delete listeners (IDocumentDeleteListener)
* Document Store listeners (IDocumentStoreListener)
* Document Query listeners (IDocumentQueryListener)

In order to add new listener you must register it in `DocumentStore`:

{CODE register_listener@ClientApi\Listeners\General.cs /}

You can also set all your listeners at once by the following method:

{CODE set_listeners@ClientApi\Listeners\General.cs /}

