## Basic concepts

RavenDB is a database technology based on a client-server architecture. Data is stored on a server instance and data requests from one or more clients are made to that instance.

Requests to the server are made using either:

* The [.NET Client API](../client-api) - available to any .NET or Silverlight application
* The [RESTful API](../http-api) - directly accessing the RESTful API exposed by the server

If you are a .NET developer, using the Client API is the easiest way for you to work with RavenDB, as it provides a great amount of features and a very slick API.

The RESTful API makes RavenDB accessible from other platforms like AJAX queries in web pages or non-Windows applications written in Ruby on Rails, for example.

### The RavenDB server

#### Launching a server instance

The RavenDB server instance can be instantiated in several ways:

* Running the Raven.Server.exe console application (located under `/Server/` in the build package).

* Running RavenDB as a service.

* Integrating RavenDB with IIS on your Windows based server.

* Embedding the server in your application.

The various deployment options are discussed in more details in the [Server side section](..\server).

To jump-start your learning process, it is sufficient that you 

1. download the [latest stable build](http://ravendb.net/downloads)
2. unzip it to a folder
3. run Server\\Raven.Server.exe

You should see a screen like this:

![Figure 1: Raven.Server.exe](images/raven.server.png)

Notice that a port for the server to listen on has already been automatically selected for you, and a data directory has been created and is ready to store your data. This is RavenDB in debug mode.

For production usage, you'll generally run it in IIS or as a Windows Service.

As long as this window stays open, the RavenDB server is running. Pressing Enter will terminate the server - new requests will no longer be processed, but all data will be persisted in the data directory.

### Storage types

RavenDB currently supports 2 types of storage engines, both of which are completely transactional and fail safe:

* Esent
* Munin.

Esent is a native embeddable database engine which is part of Windows, and maintained by Microsoft.

Munin is written entirely in managed code specifically for its use as part of RavenDB. 

While Munin is useful for testing and temporary in-memory tasks, at this stage only Esent is supported for production usage.

### Documents, Collections and Document unique identifiers

A single data entity in RavenDB is called a _Document_ and all Documents are persisted in RavenDB as [JSON documents](http://json.org).

The JSON format was selected because:

* it can store hierarchies
* it is human readable
* it is as minimalistic as it gets.

Every document has metadata attached to it.  By default it only contains data that is used internally by RavenDB (for example - the `Raven-Entity-Name` attribute which stores the entity type for the document).

A _Collection_ is a set of Documents sharing the same RavenDB entity type. It is not a "database table", but rather a logical way of thinking of document groups. A Collection is an entirely virtual construct that has no physical meaning to the database.

With RavenDB, each document has its own unique global ID, in the sense that if one was trying to store two different entities under the same id (`users/1` for example) - the second write will overwrite the first one without any warning.

The convention in RavenDB is to have a document ID that is a combination of the collection name and the entity's unique id within the collection, i.e. `users/1`. However, that is only a convention: document IDs are independent of the entity type, and therefore don't have to contain the name of the collection they are assigned to.

### The RavenDB Management Studio

Every RavenDB server instance is manageable via a remotely accessible Silverlight application - The RavenDB Management Studio.

It can be accessed by pointing your favorite browser to the address (and port) the server is listening on.

[Here](..\studio) is where we discuss the Studio and how to use it in depth.

This is what the RavenDB Management Studio looks like:

![Figure 2: RavenDB Studio](images/studio.png)

