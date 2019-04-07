# Client API: Creating a Document Store
---
{NOTE: }  

* When creating a new document store, make sure to apply all necessary configurations before calling `.initialize()`. 
  * [Conventions](../client-api/configuration/conventions) are frozen upon initialization  

* Pass your document store a list of URLs to your RavenDB servers.  
  * **Important**: these servers must all be nodes of the same cluster  

* Pass your document store an [Authentication Certificate](../client-api/setting-up-authentication-and-authorization) to access secure servers.  

* Select the [Default Database](../client-api/setting-up-default-database) that you want your document store to operate on.  

* Set the conventions and any other customizations you wish to apply.  

* Finally, call `.initialize()` to begin using the document store.  
  * This will instantiate a connection between your application and the associated server(s)  

* We recommend that your document store implement the [Singleton Pattern](https://csharpindepth.com/articles/Singleton) as demonstrated in the example code 
[below](../client-api/creating-document-store#example---typical-initialization).  
  * Creating more than one document store is resource intensive, and one instance is sufficient for most use cases  
  * If you do create additional document store instances, you should dispose of them after use (document store implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.7.2))  
  * The document store is thread safe  

{NOTE/}

{PANEL:Example - Typical Initialization}

This example demonstrates the initialization of a singleton document store, as well as setting the list of URLs and the default database.
{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}  

{PANEL/}

## Related Articles

### Client API

- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)
- [What is a Document Store](../client-api/what-is-a-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
- [Conventions Overview](../client-api/configuration/conventions)
