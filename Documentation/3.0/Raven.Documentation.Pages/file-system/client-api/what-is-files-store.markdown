#What is a files store?

The files store is the main file system client API object that ensures an access to the RavenFS instance. It is an equivalent of [a document store](../../client-api/what-is-a-document-store) for a file system.

The files store is designated to work with a single RavenDB server where there can be multiple file systems. By default it performs actions against a default file system that has to be provided during initialization, but it is also able to work with any existing file system on  an associated server.

The basic interface is the `IFilesStore`. Currently its only implementation is the `FilesStore`, which communicates with the server by sending HTTP requests.

The files store ensures the access to the following client API features:

* Session
* Commands
* Changes API
* Listeners