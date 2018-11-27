# Client API : What is a Document Store

A document store is our main client API object which establishes and manages the connection channel between an application and a database instance. 
It acts as the connection manager and also exposes methods to perform all operations which you can run against an associated server instance.

The document store object has a list of URL addresses that points to RavenDB server nodes.

* `DocumentStore` acts against a remote server via HTTP requests

The document store ensures access to the following client API features:

* [Session](../clientApi/session/what-is-a-session-and-how-does-it-work)
* [Operations](../clientApi/operations/what-are-operations)
* [Conventions](../clientApi/configuration/conventions)
* [Events](../clientApi/session/how-to/subscribe-to-events)
* [Bulk insert](../clientApi/bulk-insert/how-to-work-with-bulk-insert-operation)
* [Changes API](../clientApi/changes/what-is-changes-api)

## Related Articles

### Getting Started

- [Getting Started](../start/getting-started)
- [Setup Wizard](../start/installation/setup-wizard)

### Document Store

- [Creating a Document Store](../clientApi/creating-document-store)
- [Setting up Default Database](../clientApi/setting-up-default-database)
- [Setting up Authentication and Authorization](../clientApi/setting-up-authentication-and-authorization)
