# Client API: Creating a Document Store
---
{NOTE: }  

* **Creating a document store** is the first step that a RavenDB client application needs to make to begin operating on a database.

* We recommend that your document store implement the [Singleton Pattern](https://csharpindepth.com/articles/Singleton) as demonstrated in the example code 
[below](../client-api/creating-document-store#singleton-implementation).  
  * Creating more than one document store may be resource intensive, and one instance is sufficient for most use cases  
  * If you do create additional document store instances, you should dispose of them after use (document store implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.7.2))  

* All configurations are frozen upon calling `.Initialize()`, so be sure to make any customizations necessary during the creation of a new document store.  

* In this page:  
  * [How to Create a New Document Store](../../client-api/creating-document-store#how-to-create-a-new-document-store)  
      * [The URL List](../../client-api/creating-document-store#the-url-list)
      * [Other Configurations](../../client-api/creating-document-store#other-configurations)
  * [Singleton Implementation](../../client-api/creating-document-store#example---singleton-implementation)  
{NOTE/}

---
{PANEL: How to Create a New Document Store}
####The URL List
When creating a new document store, you will need to pass it an initial list of URLs to the server nodes of the [RavenDB cluster](../server/clustering/overview) you want it to operate on. 
The document store only uses this list when it attempts to make an operation on a given database for the first time. It will attempt to make contact with each server node URL on the list 
until it succeeds. At that point, it will fetch the [Database Group Topology](../../studio/database/settings/manage-database-group), which includes the URLs of all server nodes that contain 
a copy of the database. This list of URLs supersedes the initial list for the purpose of operating on this database.  

This means that the initial list of URLs doesn't necessarily need to include all the server nodes of your cluster. In fact, in most cases the document store will successfully contact the cluster 
through the first URL on its list, and not need the others. However, it's better to have several URLs in this list as a contingency, in case some of the servers are down. If the document store 
reaches the end of its URL list without successfully making contact with any server, it will throw an exception.  <br/><br/>
####Other Configurations
The following configuration options are optional, but they cannot be set or changed after `.Initialize()` is called on the document store. If necessary, you can always make another document 
store and configure it differently.  

- [Default Database](../client-api/setting-up-default-database) - the database that sessions and operations will operate on unless otherwise specified  
- [Conventions](../client-api/configuration/conventions) - a variety of options to customize the Client API behavior  
- [Client Certificate](../client-api/setting-up-authentication-and-authorization) - a X.509 certificate for authentication and authorization  

Finally, call `.Initialize()` to begin using the document store.

{CODE document_store_creation@ClientApi\CreatingDocumentStore.cs /}  

{PANEL/}

{PANEL:Example - Singleton Implementation}

This example demonstrates how to implement the singleton pattern in the initialization of a document store.
{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}  

{PANEL/}

## Related Articles

### Client API

- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
- [Conventions Overview](../client-api/configuration/conventions)
- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)
- [What is a Document Store](../client-api/what-is-a-document-store)
