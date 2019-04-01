# Client API : How to Create a Document Store

---
{NOTE: }  

* The example code [below] shows a typical initialization of a document store.  

* After creating a `new DocumentStore()` object, apply all necessary configurations. Do not call `.initialize()` on a document store before it is fully configured.  

* We recommend that your `DocumentStore` instance implement the [singleton pattern] as demonstrated in the example code.  
  * This is because creating more than one document store is resource intensive, and should not be necessary in most cases  
  * The document store is thread safe  

* Include a **list of URLs** to RavenDB servers. These servers should all be nodes of the same cluster.  

{WARNING: }  
Do not open a `DocumentStore` using URL addresses that point to nodes outside your cluster.  
{WARNING/}  

* Apply any other configurations to the document store, such as:  
  * Passing it an [authentication certificate]  
  * Selecting the [default database] that you want this document store to operate on  
  * Setting the [conventions]  

* Finally, call `.initialize()` to begin using the document store.  

{NOTE/}

##Example Initialization

{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}  

##Initialization

To be able to work on the `DocumentStore`, you will have to call the `Initialize` method to get the fully initialized instance of `IDocumentStore`.

{NOTE:Conventions}

The conventions are frozen after `DocumentStore` initialization so they need to be set before `Initialize` is called.

{NOTE/}

##Singleton

Because the document store is a heavyweight object, there should only be one instance created per application (singleton). The document store is a thread safe object and its typical
initialization looks like the following:



{NOTE If you use more than one instance of `DocumentStore` you should dispose it after use. /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)

### Document Store

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
