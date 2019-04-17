# Client API: Creating a Document Store
---
{NOTE: }  

* **Creating a Document Store** is the first step that a RavenDB client application needs to make when working with RavenDB.

* We recommend that your Document Store implement the [Singleton Pattern](https://csharpindepth.com/articles/Singleton) as demonstrated in the example code 
[below](../client-api/creating-document-store#creating-a-document-store---example).  
Creating more than one Document Store may be resource intensive, and one instance is sufficient for most use cases.  

* In this page:  
  * [Creating a Document Store - Configuration Options](../client-api/creating-document-store#creating-a-document-store---configuration-options)  
  * [Creating a Document Store - Example](../client-api/creating-document-store#creating-a-document-store---example)  
{NOTE/}

---
{PANEL: Creating a Document Store - Configuration Options}

{WARNING: }
All configurations are frozen upon calling `.Initialize()`. You can create a new document store if you need to change the configurations.  
{WARNING/}

The following properties can be configured when creating a new Document Store:  
 
 * **URL List** - (the only required parameter)  

     * An initial list of URLs to your RavenDB cluster nodes that is used _only_ when accessing a given database for the first time  

     * The Document Store will fetch the [Database Group Topology](../studio/database/settings/manage-database-group) from the first server on the list that it successfully connects with. The URLs contained in the database group topology will supersede the initial URL list for all future calls to that database. An exception is thrown if the Document Store tries and fails to make contact with every server on the initial list.  

     * **Note**: do not open a Document Store with URLs that point outside your cluster, only to nodes of the same cluster  

     * **Note**: this list is not binding, you can always [Add Nodes to Your Cluster](../studio/server/cluster/add-node-to-cluster))   

 * **[Default Database](../client-api/setting-up-default-database)** - the database that sessions and operations will operate on unless otherwise specified (optional parameter)  

 * **[Conventions](../client-api/configuration/conventions)** - a variety of options to customize the Client API behavior, overriding the default conventions (optional parameter)  

 * **[Client Certificate](../client-api/setting-up-authentication-and-authorization)** - X.509 certificate for authentication and authorization (optional parameter)  

After setting the above configurations as necessary, call `.Initialize()` to begin using the document store.  

{PANEL/}

{PANEL: Creating a Document Store - Example}

This example demonstrates how to implement the singleton pattern in the initialization of a Document store, as well as how to edit the configurations.
{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}  

{PANEL/}

## Related Articles

### Client API

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
- [Conventions Overview](../client-api/configuration/conventions)
- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)

