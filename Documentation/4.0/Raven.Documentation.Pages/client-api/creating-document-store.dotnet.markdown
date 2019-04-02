# Client API : How to Create a Document Store

---
{NOTE: }  

* The [example code](../client-api/creating-document-store#example---typical-initialization) below shows a typical initialization of a document store.  

* After creating a new `DocumentStore` object, apply all necessary configurations. Do not call `.initialize()` on a document store before it is fully configured.  

* We recommend that your document store implement the [Singleton Pattern](https://csharpindepth.com/articles/Singleton) as demonstrated in the example code below.  
  * This is because creating more than one document store is resource intensive, and is be necessary in most usage cases  
  * The document store is thread safe  

* Include a list of URLs to RavenDB servers. These servers should all be nodes of the same cluster.  
  * **Important**: do not open a document store using URL addresses that point to nodes outside your cluster.  

* Apply any other configurations to the document store, such as:  
  * Passing it an [Authentication Certificate](../client-api/setting-up-authentication-and-authorization)  
  * Selecting the [Default Database](../client-api/setting-up-default-database) that you want this document store to operate on  
  * Setting the [Conventions](../client-api/configuration/conventions)  

* Finally, call `.initialize()` to begin using the document store.  
  * Conventions are frozen after initialization.

{NOTE/}

{PANEL:Example - Typical Initialization}

{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}  

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)

### Document Store

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
