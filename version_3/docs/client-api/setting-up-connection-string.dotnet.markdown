# Client API : How to setup connection string?

.NET named connection strings are supported by Client API. You can use them by setting the `ConnectionStringName` property value in `DocumentStore`, and the RavenDB client will initialize automatically based on the connection string's parameters.

{CODE connection_string_1@ClientApi\SetupConnectionString.cs /}

You can define the connection string in the application configuration file (`app.config` or `web.config`).

{CODE-START:plain /}
  <connectionStrings>
    <add name="Local" connectionString="DataDir = ~\Data"/>
    <add name="Server" connectionString="Url = http://localhost:8080"/>
    <add name="Secure" connectionString="Url = http://localhost:8080;user=beam;password=up;ResourceManagerId=d5723e19-92ad-4531-adad-8611e6e05c8a"/>
  </connectionStrings>
{CODE-END /}

## Format

RavenDB connection string format is:

ApiKey
:   Type: string   
API key to use when accessing the server.

DataDir (Embedded only)
:   Type: string   
Specify which directory to run from.

Enlist
:   Type: bool  
Indicates if client should enlist in distributed transactions. Default: `True`.

ResourceManagerId
:   Type: Guid   
Resource Manager Id that will be used by the Distributed Transaction Coordinator (DTC) service to identify Raven. A custom resource manager id will need to be configured for each Raven server instance when Raven is hosted more than once per machine.

Url
:   Type: string  
Specify where to locate the server.

Failover
:   Type: string  
Define failover server. Read more [here](../client-api/how-to/setup-failover-servers-and-failover-behavior).

Database or DefaultDatabase
:   Type: string  
Use a specific database, not the default one. Using this will also ensure that the database exists.

User, Password and Domain
:   Type: string  
Credentials to use when accessing the server.

## Examples

The following are samples of a few RavenDB connection strings:

    * Url = http://ravendb.mydomain.com
        * connect to a remote RavenDB instance at ravendb.mydomain.com, to the default database
    * Url = http://ravendb.mydomain.com;Database=Northwind
        * connect to a remote RavenDB instance at ravendb.mydomain.com, to the Northwind database there
    * Url = http://ravendb.mydomain.com;User=user;Password=secret
        * connect to a remote RavenDB instance at ravendb.mydomain.com, with the specified credentials
    * DataDir = ~\App_Data\RavenDB;Enlist=False 
        * use embedded mode with the database located in the App_Data\RavenDB folder, without DTC support.

#### Related articles

TODO