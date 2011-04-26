# Basic RavenDB concepts

RavenDB is a database technology based on a client-server architecture. That is to say, data is being stored on a server instance, and data requests from various clients are being made to that instance.

Requests to the server are made using the client API available to any .NET or Silverlight application, or by utilizing the server's RESTful API.

## The RavenDB server

### Launching a server instance

The RavenDB server instance can be instantiated in more than a single way:

* Running the Raven.Server.exe console application (located under /Server/ in the build package).

* Running RavenDB as a service.

* Integrating RavenDB with IIS on your Windows based server.

* Embedding the server in your application.

We will discuss the various deployment options in much more details later in the documentation.

To jump-start your learning process, it is sufficient that you download the latest stable build, unzip it to a folder, and run Server\Raven.Server.exe. You will then see a screen like this:

![Figure 1: Raven.Server.exe](images\raven.server.png)

Notice how a port for the server to listen on has been automatically selected for you, and an Esent data directory has been created and is ready to store your data.

As long as this window will stay open, the RavenDB server is up and running. Pressing Enter will terminate the server - new requests will no more be processed, but all data will be persisted in the data directory.

### Storage types

RavenDB currently supports 2 types of storage engines, which are completely transactional and fail safe - Esent and Munin.

Esent is a native embeddable database engine which is part of Windows, and maintained by Microsoft. Munin is written entirely in managed code, and is a custom project made as part of RavenDB. 

While Munin is useful for testing and temporary in-memory tasks, at this stage only Esent is supported for production usage.

## Client API

Given a RavenDB server, embedded or remote, the Client API allows easy access to it from any .NET language. The Client API exposes all aspects of the RavenDB server to your application in a seamless manner.

In addition to transparently managing all client-server communications, the Client API is also responsible for a complete integrated experience for the .NET consumer application. Among other things, the Client API is responsible for implementing the Unit of Work pattern, applying conventions to the process of saving/loading of data, integrating with System.Transactions, detecting changed entities, batching requests to the server, and more.

Reference Raven.Client.Lightweight.dll (or Raven.Client.Silverlight if you are using Silverlight) from your application, and you are ready to start interacting with the server.

### A document store

First, you are going to need to declare and instantiate a document store.  We go into more details on the various ways there are to instantiate a document store in the next chapter.

A document store instance is your communication channel to the RavenDB server it is pointing at. When created, it is being fed with the location of the server, and upon request it serves a session object which you can use to perform actual database operations with.

### Session

The session object provides a fully transactional way of performing database operations. The session allows the consumer to store data into the database, and load it back when necessary explicitly or using queries.

Other, more advanced operations are also accessible from the Session object, and are discussed in length later in the documentation.

Once a session has been opened, all entities that are retrieved from the server are tracked, and whenever an entity has changed the user can choose whether to save it back changed to the data store, or to discard the changes made locally.

## REST API

The server instance can also be accessed via a RESTful API, making it accessible from other platforms like AJAX queries in webpages or non-Windows applications written in Ruby-on-Rails, for example.

## The Management Studio

Each server instance is manageable via a remotely accessible Silverlight application - The Studio. We discuss its usage later in the docs, but here's how it looks like:

![Figure 2: RavenDB Studio](images\studio.png)
