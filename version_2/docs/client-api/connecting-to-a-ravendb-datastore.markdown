﻿
# Connecting to a RavenDB data store

As we have seen, RavenDB can run in one of two modes: a client/server mode, where communication is made via HTTP; and an embedded mode, in which the client API makes direct calls against the Database API.

The recommended mode for RavenDB to run in is the server mode. We discuss the various deployment options for the server mode later in the documentation.

Since a document store instance is not cheap to create but is thread-safe, the general suggestion is to have one instance of it per-database per-application.

In either mode, when the application shuts down, the document store instance(s) used should be disposed to ensure proper cleanup.

## Running in server mode

To run in a server mode, add a reference to Raven.Client.Lightweight.dll to your application, and after launching the server instance separately connect to it using the following code:

{CODE running_in_server_mode@ClientApi\ConnectionToRavenDbDataStore.cs /}

Where http://myravendb.mydomain.com/ is your the RavenDB server's address.

This will instantiate a communication channel between your application and the RavenDB server instance in the network address you provided.

## Running in embedded mode

When running in embedded mode, the data store is actually a server instance running on top of a local data directory, as opposed to connecting to a separate server instance.

To have this, you will need the entire EmbeddedClient folder from the build package in your solution. Note that you cannot use RavenDB Embedded on the Client Profile, you have to change the project properties to use the full .NET framework profiler. 

After referencing Raven.Client.Embedded.dll, you need to initialize a new instance of EmbeddableDocumentStore. This is done by passing the path to the directory that the database resides in to the EmbeddableDocumentStore (the database will be created if it doesn't exists yet):

{CODE running_in_embedded_mode@ClientApi\ConnectionToRavenDbDataStore.cs /}

## Silverlight support

If you are accessing a RavenDB instance from Silverlight, you are going to need the Silverlight folder from the build package, and add a reference to all the DLLs in it to your application.

Using Silverlight, you can only access an external RavenDB server; there's still no embedded RavenDB implementation for Silverlight available to public use. You initialize a document store in Silverlight using:

{CODE silverlight_support@ClientApi\ConnectionToRavenDbDataStore.cs /}

Where http://myravendb.mydomain.com/ is your the RavenDB server's address.

## Using a connection string

To make things even simpler, the RavenDB Client API supports .NET's named connection strings. You can use that by setting the ConnectionStringName, and the RavenDB client will initialize automatically based on the connection string's parameters:

{CODE using_connection_string@ClientApi\ConnectionToRavenDbDataStore.cs /}

You can then define the connection string in the app.config:

{CODE-START:plain /}
  <connectionStrings>
    <add name="Local" connectionString="DataDir = ~\Data"/>
    <add name="Server" connectionString="Url = http://localhost:8080"/>
    <add name="Secure" connectionString="Url = http://localhost:8080;user=beam;password=up;ResourceManagerId=d5723e19-92ad-4531-adad-8611e6e05c8a"/>
  </connectionStrings>
{CODE-END /}

RavenDB connection string format is:

* _DataDir_ - run in embedded mode, specify which directory to run from. This requires that you'll initialize an *EmbeddableDocumentStore*, not just *DocumentStore*.

* _Url_ - for server mode only, specify where to locate the server.

* _User / Password_ - for server mode only, the credentials to use when accessing the server.

* _Enlist_ - whatever RavenDB should enlist in distributed transactions. Not applicable for Silverlight.

* _ResourceManagerId_ - Optional, for server mode only, the Resource Manager Id that will be used by the Distributed Transaction Coordinator (DTC) service to identify Raven. A custom resource manager id will need to be configured for each Raven server instance when Raven is hosted more than once per machine. Not applicable for Silverlight.

* _Database_ - for server mode only, use a specific database, not the default one. Using this will also ensure that the database exists.

The following are samples of a few RavenDB connection strings:

* Url=http://ravendb.mydomain.com - connect to a remote RavenDB instance at ravendb.mydomain.com, to the default database
* Url=http://ravendb.mydomain.com;Database=Northwind - connect to a remote RavenDB instance at ravendb.mydomain.com, to the Northwind database there
* Url=http://ravendb.mydomain.com;User=user;Password=secret- connect to a remote RavenDB instance at ravendb.mydomain.com, with the specified credentials
* DataDir=~\App_Data\RavenDB;Enlist=False - use embedded mode with the database located in the App_Data\RavenDB folder, without DTC support.

## Configuration

### Conventions

The RavenDB Client API uses several conventions to control how it works, these can be modified at the DocumentStore level.

* _FindIdentityProperty_ - Tell the RavenDB Client API how to find the property serving as the id property (the one holding the document key). Defaults to property named Id.

* _FindTypeTagName_ - Find the tag name for the entity, a tag name is the collection name in which an entity will be enrolled, as well as the default entity key namespace. Defaults to the plural of the entity type. (An entity of type Post would be Posts, an entity of type Person would be People, etc).

* _GenerateDocumentKey_ - Allows you to control the generation of keys for new entities. The rules for returned values follow the Raven document key generation strategies. By default, RavenDB concatenate the type tag name with an increasing numeric id (posts/1, posts/2, post/3, etc).

* _IdentityPartsSeparator_ - A string that allows you to customize part of the document key generation. By default, Raven uses "plural_entity_name/id", which some users don't like because it makes putting the document key in the URL harder in some cases. You can set this to another value (such as "-"), which would generate: "plural_entity_name-id". This is an alternative to replacing the whole document key generation process.
