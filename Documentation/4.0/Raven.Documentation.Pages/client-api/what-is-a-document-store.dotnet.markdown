# Client API: What is a Document Store
---
{NOTE: }

* The **Document Store** is the main Client API object which establishes and manages the communication between your client application and a [RavenDB cluster](../server/clustering/overview). 
Communication is done via HTTP requests.  

* The Document Store holds the [Cluster Topology](../server/clustering/rachis/cluster-topology), the [Authentication Certificate](../client-api/setting-up-authentication-and-authorization), 
and any configurations & customizations that you may have applied.  

* Caching is built in. All requests to the server(s) and their responses are cached within the Document Store.  

* A single instance of the Document Store ([Singleton Pattern](https://csharpindepth.com/articles/Singleton)) should be created per cluster per the lifetime of your application.  

* The Document Store is thread safe - implemented in a thread safe manner.  

* The Document Store exposes methods to perform operations such as:  
  * [Session](../client-api/session/what-is-a-session-and-how-does-it-work) - Use the Session object to perform operations on a specific database  
  * [Operations](../client-api/operations/what-are-operations) - Manage the server with a set of low level operation commands  
  * [Bulk insert](../client-api/bulk-insert/how-to-work-with-bulk-insert-operation) - Useful when inserting a large amount of data  
  * [Conventions](../client-api/configuration/conventions) - Customize the Client API behavior  
  * [Changes API](../client-api/changes/what-is-changes-api) - Receive messages from the server  
  * [Aggressive caching](../client-api/how-to/setup-aggressive-caching) - Configure caching behavior  
  * [Events](../client-api/session/how-to/subscribe-to-events) - Perform custom actions in response to the Session's operations  
  * [Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions) - Define & manage data processing on the client side

{NOTE/}

## Related Articles

### Getting Started

- [Getting Started](../start/getting-started)
- [Setup Wizard](../start/installation/setup-wizard)

### Client API

- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
