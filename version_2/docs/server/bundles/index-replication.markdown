#Index Replication Bundle
RavenDB is a great database for storing documents, and the ability to create indexes on top of documents make extracting information out of the indexes very easy. There are situations, especially for reports, when you still want to use a relational database. The Index Replication Bundle is meant to handle just those scenarios. It is capable of replicating an index a table in a relational database.

##How it works?

Let us consider the following document:

{CODE indexreplication1@Server\Bundles\IndexReplication.cs /}

In a relational database, we would need at least two tables to represent this information. The Index Replication bundle is not about replicating documents to a relational database, for the simple reason that it is a highly complex problem (which requires  the use of a full ORM like [NHibernate](http://nhforge.org/) to do so).  You can't directly replicate between a document and a RDBMS table, because the format of a document simply cannot be made to work on a relational database.

But RavenDB's indexes are meant to extract information from a document into a flat structure, perfect for replicating directly to a relation database table. Let us consider the following trivial index:

{CODE indexreplication2@Server\Bundles\IndexReplication.cs /}

It is pretty clear that we can easily replicate this index results (because it is a flat data structure) to a relational database. And that is precisely how the Index Replication bundle works. It replicate all the index updates to a table in a relational database.

##Installation

Installing the Index Replication bundle is as simple as installing any other bundle. Simply drop the Raven.Bundles.IndexReplication.dll to the Plugins directory and restart the server.

##Configuration
Because the Index Replication bundle needs to connect to a relational database, it needs the appropriate connection string. To avoid security concerns, specifying the appropriate connection strings is done in the app.config file in the <connectionStrings/> element, like so:

    <connectionStrings>  
    	<add name="Reports" 
            providerName="System.Data.SqlClient"   
            connectionString="Data Source=.\sqlexpress;Initial Catalog=QuestionReports;Integrated Security=SSPI;"/>  
    </connectionStrings>

Using this approach, you can [encrypt the connectionStrings section](http://msdn.microsoft.com/en-us/library/zhhddkxy.aspx) and avoid have a connection string in plain text.

Specifying  the providerName definition is mandatory. The following is a list of supported providers:

* SQL Server - System.Data.SqlClient
* Oracle - Oracle.DataAccess.Client
* MySQL - MySql.Data
* PostgresSQL - Npgsql

Additional providers will likely work as well, but haven't been tested.

The last step to configure the Index Replication bundle is to tell it which indexes to replicate and how to do it. We do that by creating a replication definition document, which looks like this:

{CODE indexreplication3@Server\Bundles\IndexReplication.cs /}

* Id is used to define to which index this document applies to, using the simple ("Raven/IndexReplication/"+ indexName) naming convention.
* ColumnMapping is used to select which fields are replicated (the keys, of the left) and to which columns they map (the values, on the right).
* ConnectionStringName species the appropriate connection string to use from the app.config file.
* PrimaryColumnName is used to uniquely identify each row. The primary key name must not appear in the ColumnsMapping section.
* TableName is the table to replicate the index to.

##Implications
Replicating the index to a relation database table is done as part of the indexing process, which means that indexing (for the replicated indexes only) would be slower. We recommend using dedicated indexes for replication, and never query them if indexing speed is important.

Replicated fields must be set to Store.Yes, if the field isn't set to Store.Yes, the Index Replication bundle will be unable to replicate its value to the relational table.

##What is being replicated?
It is important to note that the bundle does not replicate the index itself, it only replicate changes to the index made once the configuration is set. This is a fine distinction, but an important one. What it means is that if you have an existing index, without the matching IndexReplicationDestination document, it will not be replicated. Once you add a matching IndexReplicationDestination document, all new changes to the index will be replicated, but existing documents, which are already indexed, will not be replicated. You can [reset the index](http://ravendb.net/docs/theory/indexes/indexing?version=2.0) to force replication of all the matching documents as a result of the re-indexing process.

##Error handling

Errors in replicating indexes to relational database table will be logged to the server log and to the global error log (available in the Global Statistics page). They will not stop indexing for the index, nor will they count against the error quota of the index.

It is important to understand that if there was some transient error in replicating to the relational database (for example, because the relational database server was unreachable for a few minutes), documents that were indexed during that time will not be replicated to the relational database table once connectivity is restored.
