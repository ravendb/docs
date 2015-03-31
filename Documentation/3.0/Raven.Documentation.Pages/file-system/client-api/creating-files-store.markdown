#Creating a files store

When you create a files store object, you have to provide a server URL and a default file system name (RavenFS does not have `<system>` file system):

{CODE create_fs_1@FileSystem\ClientApi\CreatingFilesStore.cs /}

By default, the file system will be created if it doesn't exist. If you want to overwrite that behavior, look at the optional parameters of the `Initialize` method.

##Initialization and disposal

Note that just as the `DocumentStore` object, the created `FilesStore` instance needs to be explicitly initialized by calling the `Initialize` method.
It returns the `IFilesStore` object which ensures full access to the manage files. Note that the whole code is placed inside the `using` statement because the returned instance implements the `IDisposable`. In a real case scenario the files store should be disposed when an application shuts down. 

##Singleton

There should be only one instance of the `IFilesStore` created per application (singleton). The files store is a thread safe object and its typical initialization looks as follows:

{CODE create_fs_2@FileSystem\ClientApi\CreatingFilesStore.cs /}