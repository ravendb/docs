# Basic RavenDB concepts

RavenDB is a database technology based on a client-server architecture. That is to say, data is being stored on a server instance, and data requests from various clients are being made to that instance.

Requests to the server are made using the client API available to any .NET or Silverlight application, or by utilizing the server's RESTful API.

## The RavenDB server

### Launching a server instance

The RavenDB server instance can be instantiated in several ways:

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

## Client API

Given a RavenDB server, embedded or remote, the Client API allows easy access to it from any .NET language. The Client API exposes all aspects of the RavenDB server to your application in a seamless manner.

In addition to transparently managing all client-server communications, the Client API is also responsible for a complete integrated experience for the .NET consumer application. Among other things, the Client API is responsible for implementing the Unit of Work pattern, applying conventions to the process of saving/loading of data, integrating with `System.Transactions`, detecting changed entities, batching requests to the server, and more.

Reference Raven.Client.Lightweight.dll (or Raven.Client.Silverlight if you are using Silverlight) from your application, and you are ready to start interacting with the server.

### Client API design guidelines

The RavenDB Client API design intentionally mimics the widely familiar NHibernate API. The API is composed of the following main classes:

* _IDocumentStore_ - This is expensive to create, thread safe and should only be created once per application. The Document Store is used to create DocumentSessions, to hold the conventions related to saving/loading data and any other global configuration, such as the http cache for that server.

* _IDocumentSession_ - Instances of this interface are created by the DocumentStore, they are cheap to create and are not thread safe. If an exception is thrown by any IDocumentSession method, the behavior of all of the methods (except Dispose) is undefined. The document session is used to interact with the Raven database, load data from the database, query the database, save and delete. Instances of this interface implement the Unit of Work pattern and change tracking. 

* _IDocumentQuery_ - Allows querying the indexes on the RavenDB server. We discuss querying later in this part of the documentation.

### A document store

First, you are going to need to declare and instantiate a document store.  We go into more details on the various ways there are to instantiate a document store in the next chapter.

A document store instance is your communication channel to the RavenDB server it is pointing at. When created, it is being fed with the location of the server, and upon request it serves a session object which you can use to perform actual database operations with.

### Session

The session object provides a fully transactional way of performing database operations. The session allows the consumer to store data into the database, and load it back when necessary using queries or by document id.

Other, more advanced operations are also accessible from the Session object, and are discussed in length later in the documentation.

Once a session has been opened, all entities that are retrieved from the server are tracked, and whenever an entity has changed the user can choose whether to save it back changed to the data store, or to discard the changes made locally.

### POCO serialization

By default, all POCOs are serialized in the following manner:

* All properties with a getter and a setter are serialized, regardless of their visibility (public, private or protected)

* Property with no setter is ignored

* All public fields are serialized, non-public fields are ignored

This behavior can be customized, see the page under Advanced.

## REST API

The server instance can also be accessed via a RESTful API, making it accessible from other platforms like AJAX queries in webpages or non-Windows applications written in Ruby-on-Rails, for example.

## Documents, Collections and Document unique identifiers

A single data entity in RavenDB is called a _Document_, and all Documents are persisted in RavenDB as JSON documents. A _Collection_ is a set of Documents with unique ids sharing the same RavenDB entity type (See: Metadata). Collections are only there to help you: they are virtual constructs that have no physical meaning to the database. It may be easy to think of Collections as tables and documents as rows in a schema-less database.

In RavenDB each document has its own unique global id, in the sense that if one was trying to store two different entities under the same id (`users/1` for example) - only the last one will be persisted. This id is usually combined of the collection name and the entity's uniqe id within the collection, i.e. `users/1`, but as we will see in a moment document ids are independent of the entity type, and therefore doesn't always contain the collection name.

When using the Client API, each POCO (or: .NET object) stored in RavenDB is considered a _Document_. When stored, it is serialized to JSON and then saved to the database. Entities of different types (for example, objects of classes BlogPost, User, and Comment) are grouped by their type and added to one collection by default, creating new documents with ids like `users/1`, `blogposts/1` and `comments/1`. Note how the class name is used to create the collection name, in its plural form.

If you have an Id property on your entities, RavenDB will populate that property with the entity id. That property isn't mandatory, however, and you can have entities without an Id property, in which case RavenDB will manage the entity id internally. Although numeric and `Guid` ids are supported, it is recommended that you keep it as a string.

RavenDB supports 3 ways of figuring out a unique id, or a document key, for a newly saved document:

1. By default, the HiLo algorithm is used. Whenever you create a new `DocumentStore` object and it connects to a RavenDB server, HiLo keys are exchanged and your connection is assigned a range of values it can use in every call to `session.Store(entity)` with a new entity. A new set of values is negotiated automatically if the range was consumed.

2. RavenDB also supports the notion of Identity, if you need ids to be consecutive, or when you want to specify a key name that is different from what RavenDB will produce for you. By creating a string `Id` property in your entity, and setting it to a value ending with a slash (/), you can tell RavenDB to use that as a key perfix for your entity. That prefix followed by the next available integer id for it will be your entity's id _after_ you call `SaveChanges()`.

3. You can assign an id manually, but then if a document already exists in your RavenDB server under the same key it will be overwritten.

{NOTE It is important to understand that a `Collection` is merely a convention, not something that is enforced by RavenDB. There is absolutely nothing that would prevent you from saving a `Post` with the document id of "users/1", and that would overwrite any existing document with the id "users/1", regardless of which collection it belongs to./}

You can also setup a custom id generation strategy by supply a `DocumentKeyGenerator`, like so:

  store.Conventions.DocumentKeyGenerator = entity => store.Conventions.GetTypeTagName(entity.GetType()) +"/";
  
This will instruct RavenDB to use identity id generation strategy for all the entities that this document store manages.

## The Management Studio

Each server instance is manageable via a remotely accessible Silverlight application - The Management Studio. It can be accessed by pointing your favorite browser to the address (and port) the server is listening on.

We discuss the Studio and how to use it in more depth later in the docs, but here's how it looks like:

![Figure 2: RavenDB Studio](images\studio.png)
