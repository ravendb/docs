# Client API: How to Create a Document Store

To create an instance of the `DocumentStore` you need to specify a list of URL addresses that point to RavenDB server nodes.

{CODE:nodejs document_store_ctor@client-api\creatingDocumentStore.js /}

{WARNING:Important}
Do not open a `DocumentStore` using URL addresses that point to nodes outside your cluster.
{WARNING/}

{CODE:nodejs document_store_creation@client-api\creatingDocumentStore.js /}

The above snippet is going to instantiate a communication channel between your application and the local RavenDB server instance.

##Initialization

A `DocumentStore` instance must be initialized before use by calling the `.initialize()` method.

{NOTE:Conventions}

After `DocumentStore` initialization, the conventions are frozen - modification attempts are going to result with error. Conventions need to be set *before* `.initialize()` is called.

{NOTE/}

##Singleton

Because the document store is a heavyweight object, there should only be one instance created per application (a singleton - simple to achieve in Node.js by wrapping it in a module). Typical initialization of a document store looks as follows:

{CODE:nodejs document_store_holder@client-api\creatingDocumentStore.js /}

{NOTE If you use more than one instance of `DocumentStore`, you should dispose it after use by calling its `.dispose()` method. /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)

### Document Store

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
