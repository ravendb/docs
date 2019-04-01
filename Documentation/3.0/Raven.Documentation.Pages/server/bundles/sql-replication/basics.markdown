# Bundle: SQL Replication: Basics

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

{CODE sql_replication_1@Server\Bundles\SqlReplication.cs /}

where:  

| ------ | ------ |
| **Id** | document identifier |
| **Name** | configuration name |
| **Disabled** | marks replication as enabled/disabled |
| **ParameterizeDeletesDisabled** | disabled the parameterization of deletes |
| **ForceSqlServerQueryRecompile** | forces statement recompilation on SQL Server |
| **QuoteTables** | toggles table name quotation |
| **RavenEntityName** | name of entities (collection) that will be replicated |
| **Script** | replication script |
| **FactoryName**<br />**ConnectionString**<br />**ConnectionStringName**<br />**ConnectionStringSettingName**<br />**PredefinedConnectionStringSettingName** | used to provide connection strings to destination DB in various ways |
| **SqlReplicationTables** | list of tables to which the documents will be replicated, with the ability to turn on append only mode (`InsertOnlyMode`), which will skip any deletions, increasing performance |

## Example

Let us consider a simple scenario, where we have an `Order` with `OrderLines` (from `Northwind`) and we want to setup a replication to MSSQL.

{CODE sql_replication_2@Server\Bundles\SqlReplication.cs /}

First we need to setup our MSSQL by creating a database with two tables. In our case the database will be called `ExampleDB` and the tables will be called `Orders` and `OrderLines`.      

{CODE-BLOCK:json}
CREATE TABLE [dbo].[OrderLines]
(
	[Id] int identity primary key,
	[OrderId] [nvarchar] (50) NOT NULL,
	[Qty] [int] NOT NULL,
	[Product] [nvarchar] (255) NOT NULL,
	[Cost] [decimal] (18,2) NOT NULL
)
CREATE TABLE [dbo].[Orders]
(
	[Id] [nvarchar] (50) NOT NULL,
	[OrderLinesCount] [int] NOT NULL,
	[TotalCost] [decimal] (18,2) NOT NULL
)
{CODE-BLOCK/}

Last step is to insert a document with our configuration. This can be done using `Studio` or manually.

### Manual

{CODE sql_replication_3@Server\Bundles\SqlReplication.cs /}

### Using Studio

In Studio the configuration page is found under **Settings -> SQL Replication**.

![Figure 1: How to setup SQL Replication using Studio?](images\sql_replication_studio.png)

## Custom functions in Script

In `Script` beside [built-in functions](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents#methods-objects-and-variables), custom ones can be introduced. Please visit [this](../../../studio/overview/settings/custom-functions) page if you want to know how to add custom functions.

There also also two additional functions created specifically for SQL Replication:

| ------ |:------:| ------ |
| `varchar(value, size = 50)` | method | Defines parameter type as `varchar` with ability to specify its size (50 if not specified). |
| `nVarchar(value, size = 50)` | method | Defines parameter type as `nvarchar` with ability to specify its size (50 if not specified). |

## Remarks

{INFO:Information}
The script will be called once for each document in the source document collection, with `this` representing the document, and the document id available as `documentId`. Call `replicateTo<TableName>()` (e.g. `replicateToOrders`) for each row you want to write to the database.
{INFO/}

{WARNING:Performance}

For performance reasons, it is required to have a secondary (or primary) index for document key in SQL Tables (in the example above for `Orders.Id` and `OrderLines.OrderId`). Otherwise, especially at scale, performance degradation may occur.

{WARNING/}
