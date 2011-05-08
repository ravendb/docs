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

We will discuss the various deployment options in more detail later in the documentation.

To jump-start your learning process, it is sufficient that you download the [latest stable build](http://ravendb.net/downloads), unzip it to a folder, and run Server\Raven.Server.exe. You will then see a screen like this:

![Figure 1: Raven.Server.exe](images\raven.server.png)

Notice how a port for the server to listen on has been automatically selected for you, and a data directory has been created and is ready to store your data. This is RavenDB in debug mode, for production usage, you'll generally run it in IIS or as a Service.

As long as this window will stay open, the RavenDB server is up and running. Pressing Enter will terminate the server - new requests will no more be processed, but all data will be persisted in the data directory.

### Storage types

RavenDB currently supports 2 types of storage engines, both of which are completely transactional and fail safe - Esent and Munin.

Esent is a native embeddable database engine which is part of Windows, and maintained by Microsoft. Munin is written entirely in managed code specifically for its use as part of RavenDB. 

While Munin is useful for testing and temporary in-memory tasks, at this stage only Esent is supported for production usage.

## Document Identifiers

Each entity being stored to the database is serialized into JSON, and called a _Document_.

Entities of different types are grouped by their type, into what is being called a _Collection_. In other words, a _collection_ is a set of documents sharing the same type, but with unique ids. If one was to store two entities of the same type under the same id - only the last one stored will be persisted. Collections are there to help you, they are virtual constructs that have no physical meaning to the database. For example, using the same id when storing two entities of different types will result, again, in only the last one stored in the database.

It is easy to think of collections as tables and documents as rows in a schema-less database. But you have to remember that the ids of documents are global, not scoped to the level of the collection. Document ids in RavenDB are strings, and are usually prefixed with the collection name:

* posts/1
* blogs/1
* users/1

Note that those are three *different* ids, "posts/1", "blogs/1" and "users/1". You can think of it (and indeed, this is how it works by default) as the collection name and the collection id, but it is important to understand that this is merely a convention, not something that is enfored by RavenDB. There is absolutely nothing that would prevent you from saving a Post with the document id of "users/1", and that would overwrite any existing document with the id "users/1", regardless of which collection it belongs to.

In your POCO classes, you can also use non string ids, which will be converted to the conventional format by RavenDB automatically. Integers and Guids are supported for such ids, but the general recommendation is that you'll use string ids.

## Client API

Given a RavenDB server, embedded or remote, the Client API allows easy access to it from any .NET language. The Client API exposes all aspects of the RavenDB server to your application in a seamless manner.

In addition to transparently managing all client-server communications, the Client API is also responsible for a complete integrated experience for the .NET consumer application. Among other things, the Client API is responsible for implementing the Unit of Work pattern, applying conventions to the process of saving/loading of data, integrating with System.Transactions, detecting changed entities, batching requests to the server, and more.

Reference Raven.Client.Lightweight.dll (or Raven.Client.Silverlight if you are using Silverlight) from your application, and you are ready to start interacting with the server.

### Client API design guidelines

The RavenDB Client API design intentionally mimics the widely familiar NHibernate API. The API is composed of the following main classes:

* _IDocumentStore_ - This is expensive to create, thread safe and should only be created once per application. The Document Store is used to create DocumentSessions, to hold the conventions related to saving/loading data and any other global configuration.

* _IDocumentSession_ - Instances of this interface are created by the DocumentStore, they are cheap to create and not thread safe. If an exception is thrown by an IDocumentSession method, the behavior of all of the methods (except Dispose) is undefined. The document session is used to interact with the Raven database, load data from the database, query the database, save and delete. Instances of this interface implement the Unit of Work pattern and change tracking. 

* _IDocumentQuery_ - Allows querying the indexes on the RavenDB server. We discuss querying later in this part of the documentation.

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

Each server instance is manageable via a remotely accessible Silverlight application - The Management Studio. It can be accessed by pointing your favorite browser to the address (and port) the server is listening on.

We discuss the Studio and how to use it in more depth later in the docs, but here's how it looks like:

![Figure 2: RavenDB Studio](images\studio.png)
