# Administration: Multiple Databases

RavenDB originally supports multiple databases. If you want to configure the additional databases, you need to create a document, as usual. RavenDB multi database support was explicitly designed to support multi tenancy scenarios, and RavenDB can handle hundreds or thousands of databases on the same instance.   

{WARNING:Having too many databases might hurt performance}
The problem with running many databases on the same hardware is that they compete for a limited amount of resources. 
If your HD can provide 50 MB/sec and 1000 IOPS, and you have 400 databases all trying their best to give you their best performance, you are going to see a rate of 125 KB / sec and 2.5 IOPS per databases.   
In a typical scenario, not all databases are active at the same time, but in most systems, when you get to hundreds of databases, they will compete.
When designing your system, please take this into consideration. Sometimes it would be better to split many databases into several servers.
{WARNING/}

To define a new database you need to create a document with the name "Raven/Databases/[database name]" and the following contents:

{CODE-BLOCK:json}
// Raven/Databases/Northwind
{
    "Settings" : 
    { 
            "Raven/DataDir": "~/Databases/Northwind"
    }
}
{CODE-BLOCK/}

The `Settings` dictionary allows you to modify the RavenDB's configuration for the specified database. The list of available configuration options can be found [here](../../server/configuration/configuration-options#availability-of-configuration-options).

When the document is created, you can access the Northwind database using the same REST based API, yet with the following base endpoint:

{CODE-BLOCK:plain}
    http://localhost:8080/databases/northwind
{CODE-BLOCK/}

Everything else remains unchanged. Note that unlike other databases, there isn't any additional steps that you have to go through. Once the document describing the database is created, RavenDB will create the database as soon as a requests for that database is received.

{NOTE: Accessing a database}
You can access the **Studio** for a specific database with:   
    http://localhost:8080/studio/index.html#databases/documents?&database=database_name
{NOTE/}

You can create a new database from the client API in following way:



- {CODE multiple_databases_2@Server\Configuration/MultipleDatabases.cs /}



- {CODE multiple_databases_1@Server\Configuration/MultipleDatabases.cs /}

The first operation (`EnsureDatabaseExists`) will ensure that the database document exists, and the second one (`OpenSession`) will access the database. All operations performed in the context of the `northwindSession` will apply only to the **Northwind** database.

More methods to create a database can be found [here](../../client-api/commands/how-to/create-delete-database)

## Bundles

All the bundles on the given server will be available to all the databases on this server.

## Isolation

Different databases are completely isolated from one another, and there is no way for data from one database to leak to another database. Documents and attachments for each database are stored in separate physical locations. Indexes defined in each database can work only on the data local to that database.

There is no way to share data between different databases on the same instance. From the point of view of RavenDB, you can treat each database on the server instance as a separate server. You can even setup replication between different databases on the same server instance.

## Backups

Each database has to be backed up independently.

## Working Set

RavenDB's databases were designed with multi tenancy in mind, and are meant to support large number of databases on a single server. In order to do that, RavenDB will keep only the active databases open. If you access a database for the first time, that database will be opened and started, so the next request to that database wouldn't have to pay the cost of opening the database. However, if a database hasn't been accessed for a while, RavenDB will cleanup all resources associated with the database and close it.

This allows RavenDB to manage large numbers of databases, because at any given time, only the active databases are actually taking resources.

## Single instance vs. multiple instances

The reccomendation is having a single server instance running multiple databases, instead of having multiple instances each running a single database.
Having multiple RavenDB instances on the same box allows fine grained control via service manager / IIS for things like memory / CPU limits,
however that is usually not needed and requires separate licenses for each instance.

## Related articles

- [Client API : Commands : How to switch command to a different database?](../../client-api/commands/how-to/switch-commands-to-a-different-database)
