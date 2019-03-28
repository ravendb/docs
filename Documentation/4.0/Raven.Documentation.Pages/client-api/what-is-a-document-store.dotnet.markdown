<<<<<<< HEAD
# Client API: What is a Document Store

A document store is our main client API object which establishes and manages the connection channel between an application and a database instance. 
It acts as the connection manager and also exposes methods to perform all operations which you can run against an associated server instance.

The document store object has a list of URL addresses that points to RavenDB server nodes.

* `DocumentStore` acts against a remote server via HTTP requests, implementing a common `IDocumentStore` interface

The document store ensures access to the following client API features:

* [Session](../client-api/session/what-is-a-session-and-how-does-it-work)
* [Operations](../client-api/operations/what-are-operations)
* [Bulk insert](../client-api/bulk-insert/how-to-work-with-bulk-insert-operation)
* [Changes API](../client-api/changes/what-is-changes-api)
* [Conventions](../client-api/configuration/conventions)
* [Events](../client-api/session/how-to/subscribe-to-events)
* [Aggressive cache](../client-api/how-to/setup-aggressive-caching)

## Related Articles

### Getting Started

- [Getting Started](../start/getting-started)
- [Setup Wizard](../start/installation/setup-wizard)

### Document Store

- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
=======
# Client API : What is a Document Store

---

{PANEL}

* The **Document Store** is a Client API object which establishes and manages the connection channel between a client application and a RavenDB server instance  
  * Accesses the server via HTTP requests  

* Exposes methods for performing all the operations that can be run against the associated server(s)  

* Holds the cache, the cluster topology, and any configurations and customizations that may have been applied  

* It is the single access point to a particular RavenDB cluster  
  * Has a list of URL addresses that point to server nodes  

* Should implement the [Singleton Pattern]  

The document store exposes the following client API features:

* [Session](../client-api/session/what-is-a-session-and-how-does-it-work)
* [Operations](../client-api/operations/what-are-operations)
* [Bulk insert](../client-api/bulk-insert/how-to-work-with-bulk-insert-operation)
* [Changes API](../client-api/changes/what-is-changes-api)
* [Conventions](../client-api/configuration/conventions)
* [Events](../client-api/session/how-to/subscribe-to-events)
* [Aggressive cache](../client-api/how-to/setup-aggressive-caching)

{PANEL/}

## Related Articles

### Getting Started

- [Getting Started](../start/getting-started)
- [Setup Wizard](../start/installation/setup-wizard)

### Document Store

- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
>>>>>>> RDoc-1529-WhatIsADocumentStore
