# What is a document store?

A document store is the main client API object, which establishes and manages the connection channel between an application and a database instance. 
It acts as the connection manager and also exposes methods to perform all operations that you can run against an associated server instance.

The document store object has single URL address that points to RavenDB server, however it can work against multiple databases that exists there.

There are three types of the document store objects, all of them implement common `IDocumentStore` interface:

* `DocumentStore` acts against a remote server via HTTP requests,
* `EmbeddableDocumentStore` interacts with a RavenDB embedded storage in a user's application by making direct calls to a database (see [RavenDB Embedded mode](../server/installation/embedded))
* `ShardedDocumentStore` works against a sharded RavenDB database (see [How to setup sharding](../client-api/how-to/setup-sharding)).

The document store ensures access to the following client API features:

* [Session](../client-api/session/what-is-a-session-and-how-does-it-work)
* [Commands](../client-api/commands/what-are-commands)
* [Bulk insert](../client-api/bulk-insert/how-to-work-with-bulk-insert-operation)
* [Changes API](../client-api/changes/what-is-changes-api)
* [Conventions]()
* [Listeners](../client-api/listeners/what-are-listeners)
* [Aggressive cache](../client-api/how-to/setup-aggresive-caching)
