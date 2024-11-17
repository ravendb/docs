# Client API: How to Create a Document Store

To create an instance of the `DocumentStore` you need to specify a list of URL addresses that point to RavenDB server nodes.

{WARNING:Important}
Do not open a `DocumentStore` using URL addresses that point to nodes outside your cluster.
{WARNING/}

{CODE:java document_store_creation@ClientApi\CreatingDocumentStore.java /}

This will instantiate a communication channel between your application and the local RavenDB server instance.

##Initialization

To be able to work on the `DocumentStore`, you will have to call the `initialize` method to get the fully initialized instance of `IDocumentStore`.

{NOTE:Conventions}

The conventions are frozen after `DocumentStore` initialization so they need to be set before `initialize` is called.

{NOTE/}

##Singleton

Because the document store is a heavyweight object, there should only be one instance created per application (singleton). The document store is a thread safe object and its typical
initialization looks like the following:

{CODE:java document_store_holder@ClientApi\CreatingDocumentStore.java /}

{NOTE If you use more than one instance of `DocumentStore` you should dispose it after use. /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../client-api/session/what-is-a-session-and-how-does-it-work)

### Document Store

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)
