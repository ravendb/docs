# Bundle: SQL Replication

To provide an easy and flexible way to setup a replication to SQL servers, we have replaced the Index Replication bundle with new mechanism that is a part of a SQL Replication bundle.   

Supported SQL providers:   
* System.Data.SqlClient   
* System.Data.SqlServerCe.4.0   
* System.Data.SqlServerCe.3.5   
* System.Data.OleDb   
* System.Data.OracleClient   
* MySql.Data.MySqlClient   
* Npgsql   

## Setup

To configure SQL Replication, we need to enable the `SQL Replication Bundle` and insert a `SQL Replication Configuration` document into our database. This can be done by using the Studio or manually by inserting proper document `(Raven.Database.Bundles.SqlReplication.SqlReplicationConfig`) under `Raven/SqlReplication/Configuration/name_here` key.

The document format is as follows:   

{CODE sql_replication_1@Server\Extending\Bundles\SqlReplication.cs /}

where:   
* **Id** is a document identifier   
* **Name** is a configuration name   
* **Disabled** marks replication as enabled/disabled   
* **RavenEntityName** contains a name of entities that will be replicated   
* **Script** contains a replication script   
* **FactoryName**, **ConnectionString**, **ConnectionStringName** and **ConnectionStringSettingName** are used to provide connection strings to destination DB in various ways   
* **SqlReplicationTables** is a list of tables to which the documents will be replicated   

## Example

Let us consider a simple scenario, where we have an `Order` with `OrderLines` and we want to setup a replication to MSSQL.

{CODE sql_replication_2@Server\Extending\Bundles\SqlReplication.cs /}

First we need to setup our MSSQL by creating a database with two tables. In our case the database will be called `ExampleDB` and the tables will be called `Orders` and `OrderLines`.      

{CODE-START:json /}
    
					CREATE TABLE [dbo].[OrderLines]
					(
						[Id] int identity primary key,
						[OrderId] [nvarchar](50) NOT NULL,
						[Qty] [int] NOT NULL,
						[Product] [nvarchar](255) NOT NULL,
						[Cost] [int] NOT NULL
					)

					CREATE TABLE [dbo].[Orders]
					(
						[Id] [nvarchar](50) NOT NULL,
						[OrderLinesCount] [int] NOT NULL,
						[TotalCost] [int] NOT NULL
					)
{CODE-END /}

Last step is to insert a document with our configuration. This can be done using `Studio` or manually.

### Manual

{CODE sql_replication_3@Server\Extending\Bundles\SqlReplication.cs /}

#### Related articles

TODO