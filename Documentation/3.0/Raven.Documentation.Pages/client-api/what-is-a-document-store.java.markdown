# What is a document store?

A document store is the main client API object, which establishes and manages the connection channel between an application and a database instance. 
It acts as the connection manager and also exposes methods to perform all operations that you can run against an associated server instance.

The document store object has single URL address that points to RavenDB server, however it can work against multiple databases that exists there.

`DocumentStore` acts against a remote server via HTTP requests.

The document store ensures access to the following client API features:

* [Session](../client-api/session/what-is-a-session-and-how-does-it-work)
* [Commands](../client-api/commands/what-are-commands)
* [Bulk insert](../client-api/bulk-insert/how-to-work-with-bulk-insert-operation)
* [Changes API](../client-api/changes/what-is-changes-api)
* [Conventions](../client-api/configuration/conventions/what-are-conventions)
* [Listeners](../client-api/listeners/what-are-listeners)
* [Aggressive cache](../client-api/how-to/setup-aggressive-caching)
