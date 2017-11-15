# How to create a document store?

To create an instance of the `DocumentStore` you need to specify a list of URL addresses that points to RavenDB server nodes.

{CODE document_store_creation@ClientApi\CreatingDocumentStore.cs /}

This will instantiate a communication channel between your application and the local RavenDB server instance.

##Initialization

To be able to work on the `DocumentStore` You will have to call `Initialize` method To get the fully initialized instance of `IDocumentStore`.

##Singleton

Because the document store is a heavyweight object, there should only be one instance created per application (singleton). The document store is a thread safe object and its typical
initialization looks like the following:

{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}

{NOTE If you use more than one instance of `DocumentStore` you should dispose it after use. /}


## Related articles

- [What is a document store?](./what-is-a-document-store)
- [How to setup a default database?](./setting-up-default-database)
