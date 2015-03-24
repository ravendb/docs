#What are listeners?

The concept of listeners provides users with a mechanism to perform custom actions, in response to operations taken in a session. 
The listener implements an interface whose methods are called when a particular action is executed on an file.

The main object is `FilesSessionListeners` exposed by `IFilesStore`. It aggregates listeners of the following types:

* file delete listeners (IFilesDeleteListener),
* file metadata change listeners (IMetadataChangeListener),
* file conflicts listeners (IFilesConflictListener).

You can register any number of listeners for a given type. You can either set the whole collection of listeners at once by calling:

{CODE set_listeners@FileSystem\ClientApi\Listeners\General.cs /}

or register a particular listener by the following method:

{CODE register_listener@FileSystem\ClientApi\Listeners\General.cs /}


