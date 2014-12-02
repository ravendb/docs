# .NET Client API

So far we have spoken in abstracts, about NoSQL in general and RavenDB in particular. In this chapter we focus on the .NET Client API. We will go through all the steps required to perform basic CRUD operations using RavenDB, familiarizing ourselves with RavenDB APIs, concepts and workings.

## What is the .NET Client API?

Given a RavenDB server, embedded or remote, the Client API allows easy access to it from any .NET language. The .NET Client API exposes all aspects of the RavenDB server to your application in a seamless manner.

In addition to transparently managing all client-server communications, the Client API is also responsible for a complete integrated experience for the .NET consumer application. Among other things, the Client API is responsible for implementing the Unit of Work pattern, applying conventions to the process of saving/loading of data, integrating with `System.Transactions`, batching requests to the server, caching, and more.

The easiest way to start using RavenDB is by using nuget, but you can also reference the DLLs provided with the build package downloaded from our build server. A complete guide for doing so can be found in the [Quickstart tutorials](../intro/quickstart/adding-ravendb-to-your-application).

## .NET Client API design guidelines

The RavenDB Client API design intentionally mimics the widely familiar NHibernate API. The API is composed of the following main classes:

* _IDocumentSession_ - The document session is used to interact with the RavenDB database, load data from the database, query the database, save and delete. The session objects are cheap to create and are not thread safe. If an exception is thrown by any `IDocumentSession` method, the behavior of all of the methods (except `Dispose`) is undefined. Instances of this interface implement the Unit of Work pattern, change tracking and all other goodies we mentioned above like transaction management. When using the Client API, most of the operations you will do will be through a session object.  

* _IDocumentStore_ - The session factory, which is expensive to create, thread safe and should only be created once per application. The Document Store is in charge of the actual client/server communication, and is what holding the conventions related to saving/loading data and any other global configuration, such as the http cache for that server.

### The Document Store

First, you are going to need to declare and instantiate a document store. We go into more details on the various ways there are to instantiate a document store in the next section.

A document store instance is your communication channel to the RavenDB server it is pointing at. When created, it is being fed with the location of the server, and upon request it serves a session object which you can use to perform actual database operations with.

### The Session

The session object provides a fully transactional way of performing database operations. The session allows the consumer to store data into the database, and load it back when necessary using queries or by document id.

Other, more advanced operations are also accessible from the Session object, and are discussed in length later in this chapter.

Once a session has been opened, all entities that are retrieved from the server are tracked, and whenever an entity has changed the user can choose whether to save it back changed to the data store, or to discard the changes made locally.

## POCO serialization

By default, all POCOs are serialized in the following manner:

* All properties with a getter are serialized, regardless of their visibility (public, private or protected)

* All public fields are serialized, non-public fields are ignored

This behavior can be customized, as discussed [later in this chapter](advanced/custom-serialization).

## Document IDs

When using the Client API, each POCO (Plain Old Clr Object - a .NET class instance) stored in RavenDB is considered a _Document_. When stored, it is serialized to JSON and then saved to the database. Entities of different types (for example, objects of classes BlogPost, User, and Comment) are grouped by their type and added to one collection by default, creating new documents with IDs like `users/1`, `blogposts/1` and `comments/1`. Note how the class name is used to create the collection name, in its plural form.

{INFO As we mentioned before, this ID format is merely a convention, and document IDs can be any string. /}

RavenDB will automatically assign IDs to objects you save into it, whether or not you have an Id property in your objects. We will discuss this [later in the chapter](basic-operations/saving-new-document).
