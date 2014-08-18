# How to create a document store?

To create an instance of the `DocumentStore` you need to specify the URL address that points to the RavenDB server. The URL must contains the TCP port on which the server is listening (the default port number is 8080).

{CODE document_store_creation@ClientApi\CreatingDocumentStore.cs /}

This will instantiate a communication channel between your application and the local RavenDB server instance. Alternatively instead of providing the server URL during initialization
you can specify a name of the [connection string](../client-api/setting-up-connection-string).

##Initialization and disposal

The above code contains two things you should pay special attention. The first one is that you have to call `Initialize` method on the `DocumentStore` to get the fully initialized instance of `IDocumentStore` that you will be able to work with.
The second thing you should note is that the whole `DocumentStore` initialization code is placed inside the `using` statement to ensure the object disposal. This is needed to guarantee a proper cleanup. In a real case scenario it should be done when an application shuts down. 

##Singleton

Because the document store is a heavyweight object, it should be only one instance created per application (singleton). The document store is a thread safe object and its typical
initialization looks like below:

{CODE document_store_holder@ClientApi\CreatingDocumentStore.cs /}