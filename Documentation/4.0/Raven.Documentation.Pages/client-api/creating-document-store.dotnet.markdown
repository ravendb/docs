# Client API: Creating a Document Store
---
{NOTE: }  

* **Creating a Document Store** is the _first step_ that a RavenDB client application needs to make when working with RavenDB.

* We recommend that your Document Store implement the [Singleton Pattern](https://csharpindepth.com/articles/Singleton) as demonstrated in 
the example code [below](../client-api/creating-document-store#creating-a-document-store---example).  
Creating more than one Document Store may be resource intensive, and one instance is sufficient for most use cases.  

* In this page:  
  * [Creating a Document Store - Configuration](../client-api/creating-document-store#creating-a-document-store---configuration)  
  * [Creating a Document Store - Example](../client-api/creating-document-store#creating-a-document-store---example)  
{NOTE/}

---
{PANEL: Creating a Document Store - Configuration}

The following properties can be configured when creating a new Document Store:  
 
* **Urls** (required)  

    * An initial URLs list of your RavenDB cluster nodes that is used when the client accesses the database for the _first_ time.  

    * Upon the first database access, the client will fetch the [Database Group Topology](../studio/database/settings/manage-database-group) 
    from the first server on this list that it successfully connected to. An exception is thrown if the client fails to connect with neither 
    of the servers specified on this list. The URLs from the Database Group Topology will supersede this initial URLs list for any future 
    access to that database.  

    * **Note**: Do not create a Document Store with URLs that point to servers outside of your cluster.  

    * **Note**: This list is not binding. You can always modify your cluster later dynamically, add new nodes or remove existing ones as 
    necessary. Learn more in [Cluster View Operations](../server/cluster/cluster-view#cluster-view-operations).  

* **[Database](../client-api/setting-up-default-database)** (optional)  
  The default database which the Client will work against.  
  A different database can be specified when creating a [Session](../client-api/session/opening-a-session) if needed.  

* **[Conventions](../client-api/configuration/conventions)** (optional)  
  Customize the Client behavior with a variety of options, overriding the default settings.  

* **[Certificate](../client-api/setting-up-authentication-and-authorization)** (optional)  
  X.509 certificate used to authenticate the client to the RavenDB server  

After setting the above configurations as necessary, call `.Initialize()` to begin using the Document Store.  

{WARNING: }
The Document Store is immutable - all above configuration are frozen upon calling .Initialize().  
Create a new document store object if you need different default configuration values.  
{WARNING/}

{PANEL/}

{PANEL: Creating a Document Store - Example}

This example demonstrates how to implement the singleton pattern in the initialization of a Document Store, as well as how to set initial 
default configurations.

{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}  

{PANEL/}

## Related Articles

### Client API

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
- [Conventions Overview](../client-api/configuration/conventions)
- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)

