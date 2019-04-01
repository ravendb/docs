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

{NOTE: }

* The **Document Store** is the main entry point for the Client API.  

* Manages the connection between a client application and a RavenDB server.  

* Exposes the methods for performing all operations that can be run against the associated server(s).  
  * Use the document store to create new [Session](../client-api/session/what-is-a-session-and-how-does-it-work) objects  

* Holds the authentication certificate, the cache, the cluster topology, and any customizations that may have been applied.  

* The single access point to a particular RavenDB cluster.  
  * Has a list of URL addresses that point to its associated server nodes  
  * Accesses the server via HTTP requests  

* It is recommended that the document store implement the [Singleton Pattern].  

* The document store exposes the following Client API features:  
  * [Session](../client-api/session/what-is-a-session-and-how-does-it-work)  
  * [Operations](../client-api/operations/what-are-operations)  
  * [Bulk insert](../client-api/bulk-insert/how-to-work-with-bulk-insert-operation)  
  * [Changes API](../client-api/changes/what-is-changes-api)  
  * [Conventions](../client-api/configuration/conventions)  
  * [Events](../client-api/session/how-to/subscribe-to-events)  
  * [Aggressive cache](../client-api/how-to/setup-aggressive-caching)  
{NOTE/}

## Related Articles

### Getting Started

- [Getting Started](../start/getting-started)
- [Setup Wizard](../start/installation/setup-wizard)

### Document Store

- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
>>>>>>> RDoc-1529-WhatIsADocumentStore
