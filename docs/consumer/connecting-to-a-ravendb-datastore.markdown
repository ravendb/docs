# Connecting to a RavenDB data store

As we have seen, RavenDB can run in one of two modes: a client/server mode, where communication is made via HTTP; and an embedded mode, in which the client API makes direct calls against the Database API.

The recommended mode for RavenDB to run in is the server mode. We discuss the various deployment options for the server mode later in the documentation.

Since a document store instance is not cheap to create but is thread-safe, the general suggestion is to have one instance of it per-database per-application.

In either mode, when the application shuts down, the document store instance(s) used should be disposed to ensure proper cleanup.

## Running in server mode

To run in a server mode, add a reference to Raven.Client.Lightweight.dll to your application, and after launching the server instance separately connect to it using the following code:

<code>
var documentStore = new DocumentStore { Url = "http://myravendb.mydomain.com/" };

documentStore.Initialize();
</code>

Where http://myravendb.mydomain.com/ is your the RavenDB server's address.

This will instantiate a communication channel between your application and the RavenDB server instance in the network address you provided.

## Running in embedded mode

When running in embedded mode, the data store is actually a server instance running on top of a local data directory, as opposed to connecting to a separate server instance.

To have this, you will need the entire EmbeddedClient folder from the build package in your solution.

After referencing Raven.Client.Embedded.dll, you need to initialize a new instance of EmbeddableDocumentStore. This is done by passing the path to the directory that the database resides in to the EmbeddableDocumentStore (the database will be created if it doesn't exists yet):

<code>
var documentStore = new EmbeddableDocumentStore  {  DataDirectory = "path/to/database/directory"  };

documentStore.Initialize();
</code>

## Silverlight support

If you are accessing a RavenDB instance from Silverlight, you are going to need the Silverlight folder from the build package, and add a reference to all the DLLs in it to your application.

Using Silverlight, you can only access an external RavenDB server; there's still no embedded RavenDB implementation for Silverlight available to public use.

## Using a connection string

To make things even simpler, the RavenDB Client API supports .NET's named connection strings. You can use that by setting the ConnectionStringName, and the RavenDB client will initialize automatically based on the connection string's parameters:

`
new DocumentStore 
{
   ConnectionStringName = "MyRavenConStr"
}
`

You can then define the connection string in the app.config:

`
<connectionStrings>
    <add name="Local" connectionString="DataDir = ~\Data"/>
    <add name="Server" connectionString="Url = http://localhost:8080"/>
    <add name="Secure" connectionString="Url = http://localhost:8080;user=beam;password=up;ResourceManagerId=d5723e19-92ad-4531-adad-8611e6e05c8a"/>
</connectionStrings>
`

RavenDB connection string format is:

* _DataDir_ - run in embedded mode, specify which directory to run from.

* _Url_ - run in server mode, specify where to locate the server.

* _User / Password_ - for server mode only, the credentials to use when accessing the server.

* _ResourceManagerId_ - for server mode only, the Resource Manager Id that will be used by the Distributed Transaction Coordinator (DTC) service to identify Raven. A custom resource manager id will need to be configured for each Raven server instance when Raven is hosted more than once per machine.

* _DefaultDatabase_ - use a specific database, not the default one. Using this will also ensure that the database exists.

## Configuration

/// TBD