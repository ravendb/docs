#Creating a files store

When you create a files store object you have to provide a server URL and a default file system name (RavenFS doesn't have a default `<system>` file system):

{CODE create_fs_1@FileSystem\ClientApi\CreatingFilesStore.cs /}

By default the file system will be created if it doesn't exist, if you want to overwrite that behavior look at optional parameters of `Initialize` method.

##Initialization and disposal

Note that the same like for `DocumentStore` object the created `FilesStore` instance also needs to be explicitly initialized by calling `Initialize` method.
It returns `IFilesStore` object which ensure full access to manage files. Note that the whole code is placed inside `using` statement because the returned object
implements `IDisposable`. In a real case scenario the files store should be disposed when an application shuts down. 

##Singleton

There should be only one instance of `IFilesStore` created per application (singleton). The files store is a thread safe object and its typical initialization looks like below:

{CODE create_fs_2@FileSystem\ClientApi\CreatingFilesStore.cs /}