﻿# Ongoing Tasks: SQL ETL
---

{NOTE: }

* **SQL ETL** is a task that creates an [ETL process](../../../server/ongoing-tasks/etl/basics) where data from a RavenDB database is extracted, transformed, and loaded into a relational database as the destination.

* In this page:  
  * [Supported relational databases](../../../server/ongoing-tasks/etl/sql#supported-relational-databases)
  * [Creating the SQL ETL task](../../../server/ongoing-tasks/etl/sql#creating-the-sql-etl-task)
  * [Configuring the SQL tables](../../../server/ongoing-tasks/etl/sql#configuring-the-sql-tables)
  * [Transformation scripts](../../../server/ongoing-tasks/etl/sql#transformation-scripts)
     * [The `loadTo` method](../../../server/ongoing-tasks/etl/sql#themethod)
     * [Loading to multiple tables](../../../server/ongoing-tasks/etl/sql#loading-to-multiple-tables)
     * [Loading related documents](../../../server/ongoing-tasks/etl/sql#loading-related-documents)
     * [Loading attachments](../../../server/ongoing-tasks/etl/sql#loading-attachments)
     * [Loading to VARCHAR and NVARCHAR columns](../../../server/ongoing-tasks/etl/sql#loading-to-varchar-and-nvarchar-columns)
     * [Loading to specific column types](../../../server/ongoing-tasks/etl/sql#loading-to-specific-column-types)
     * [Filtering](../../../server/ongoing-tasks/etl/sql#filtering)
     * [Accessing the metadata](../../../server/ongoing-tasks/etl/sql#accessing-the-metadata)
     * [Document extensions](../../../server/ongoing-tasks/etl/sql#document-extensions)
  * [Advanced options](../../../server/ongoing-tasks/etl/sql#advanced-options)
  * [Transaction processing](../../../server/ongoing-tasks/etl/sql#transaction-processing)
  * [Creating the SQL ETL task from the Client API](../../../server/ongoing-tasks/etl/sql#creating-the-sql-etl-task-from-the-client-api)

{NOTE/}

---

{PANEL: Supported relational databases}

* RavenDB supports ETL processes to the following relational databases:
    * Microsoft SQL Server
    * PostgreSQL
    * MySQL
    * Oracle

* You must specify the provider type for the target relational database when setting up the  
  [SQL connection string](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-sql-connection-string).

* Before starting with SQL ETL, you need to create tables in the target relational database.  
  These tables will serve as the destinations for records generated by the ETL scripts.

{PANEL/}

{PANEL: Creating the SQL ETL task}

To create an SQL ETL task using the Client API, see [Creating the SQL ETL task from the Client API](../../../server/ongoing-tasks/etl/sql#creating-the-sql-etl-task-from-the-client-api).  
To create an SQL ETL task from the Studio open `Tasks -> Ongoing Tasks`.  

![Configure SQL ETL task](images/sql-etl-setup.png "Create the SQL ETL Task")

{PANEL/}

{PANEL: Configuring the SQL tables}

Define the target tables where the SQL ETL task will load data.

![Define SQL tables](images/sql-etl-tables.png "Define the SQL Tables")

#### Document ID Column  

* For each table, you must specify a column that will store the document ID column.
  RavenDB will populate this column with the source document ID, enabling the handling of document updates and deletions.

* Note that the specified column does not need to be the primary key of the table.

* For performance reasons, you should define indexes on the SQL tables on the relational database side,  
  at least on the column used to store the document ID.

#### Insert only

* The SQL ETL process updates documents in the relational database using DELETE and INSERT statements.

* If your system is _append-only_, you can enable the "Insert Only" toggle to instruct RavenDB to insert data without executing DELETE statements beforehand.
  This can provide a significant performance boost for systems of this kind.

{PANEL/}

{PANEL: Transformation scripts}

The [basic characteristics](../../../server/ongoing-tasks/etl/basics) of an SQL ETL script are similar to those of other ETL types.  
The script defines what data to **extract** from the source document, how to **transform** this data,  
and which SQL table to **load** it to.

A single SQL ETL task can have multiple transformation scripts.  
The script is defined per collection, and it cannot be empty.  
The script is executed per document from the source collection once the document is created or modified.

---

### The&nbsp;`loadTo`&nbsp;method

To specify which SQL table to load the data into, use either of the following methods in your script.  
The two methods are equivalent, offering alternative syntax:

* **`loadTo<TableName>(obj)`**
    * Here the target table is specified as part of the function name.
    * The target _&lt;TableName&gt;_ in this syntax is Not a variable and cannot be used as one,  
      it is simply a string literal of the target's name.

* **`loadTo('TableName', obj)`**
    * Here the target table is passed as an argument to the method.
    * Separating the table name from the `loadTo` command makes it possible to include symbols like `'-'` and `'.'` in table names.
      This is not possible when the `loadTo<TableName>` syntax is used because including special characters in the name of a JavaScript function makes it invalid.

  | Parameter       | Type   | Description                                                                                                                      |
  |-----------------|--------|----------------------------------------------------------------------------------------------------------------------------------|
  | **TableName**   | string | The name of the target SQL table                                                                                                 |
  | **obj**         | object | The object to transfer                                                                                                           |

For example, the following two calls, which load data to "OrdersTable", are equivalent:

* `loadToOrdersTable(obj)`
* `loadTo('OrdersTable', obj)`

---

### Loading to multiple tables

The `loadTo` method can be called multiple times in a single script.
That allows you to split a single `Order` document having `Lines` collection into two tables and insert multiple rows:

The following is a sample script that processes documents from the Orders collection:

{CODE-BLOCK:JavaScript}
// Create an orderData object
// ==========================
var orderData = {
    Id: id(this),
    OrderLinesCount: this.Lines.length,
    TotalCost: 0
};

// Update the orderData's TotalCost field
// ======================================
for (var i = 0; i < this.Lines.length; i++) {
    var line = this.Lines[i];    
    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
    orderData.TotalCost += cost;

    // Load the object to SQL table 'OrdersTable'
    // ==========================================
    loadToOrderLines({
        OrderId: id(this),
        Qty: line.Quantity,
        Product: line.Product,
        Cost: line.PricePerUnit
    });
}

orderData.TotalCost = Math.round(orderData.TotalCost * 100) / 100;

// Load to SQL table 'Orders'
// ==========================
loadToOrders(orderData);
{CODE-BLOCK/}

---

### Loading related documents

Use the `load` method to load a related document with the specified ID during script execution.

{CODE-BLOCK:javascript}
var company = load(this.Company);
{CODE-BLOCK/}

---

### Loading Attachments

You can store binary data that is kept as attachments in RavenDB using the `loadAttachment()` method.  
For example, if you have the following _Attachments_ table:

{CODE-BLOCK:sql}
CREATE TABLE [dbo].[Attachments]
(
    [Id] int identity primary key,
    [OrderId] [nvarchar](50) NOT NULL,
    [AttachmentName] [nvarchar](50) NULL,
    [Data] [varbinary](max) NULL
)
{CODE-BLOCK/}

then you can define the following script that loads the document's attachments:

{CODE-BLOCK:javascript}
var attachments = this['@metadata']['@attachments'];

for (var i = 0; i < attachments.length; i++) {
    var attachment = {
        OrderId: id(this),
        AttachmentName: attachments[i].Name,
        Data: loadAttachment(attachments[i].Name)
    };
    loadToAttachments(attachment);
}
{CODE-BLOCK/}

* Attachments can be also accessed using the `getAttachments()` helper function   
  (instead of grabbing them from metadata).
* The existence of an attachment can be checked by the `hasAttachment(name)` function.

---

### Loading to VARCHAR and NVARCHAR columns

Two additional functions are designed specifically for working with VARCHAR and NVARCHAR types:

| --- | --- |
| `varchar(value, size = 50)` | Defines the parameter type as VARCHAR, with the option to specify its size<br>(default is 50 if not provided). |
| `nvarchar(value, size = 50)` |  Defines the parameter type as NVARCHAR, with the option to specify its size<br>(default is 50 if not specified). |

{CODE-BLOCK:javascript}
var names = this.Name.split(' ');

loadToUsers(
{
    FirstName: varchar(names[0], 30),
    LastName: nvarchar(names[1]),
});
{CODE-BLOCK/}

---

### Loading to specific column types

The SQL type of the target column can be explicitly specified in the SQL ETL script.  
This is done by defining the `Type` and the `Value` properties for the data being loaded.

   * **Type**:  
     The type specifies the SQL column type the value is loaded to.  
     The type should correspond to the data types used in the target relational database.  

     Supported enums for `Type` include:  
       * _SqlDbType_ - see [Microsoft SQL Server](https://learn.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql)  
       * _NpgsqlDbType_ - see [PostgreSQL](https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlDbType.html)  
       * _MySqlDbType_ - see [MySQL Data Types](https://dev.mysql.com/doc/refman/8.4/en/data-types.html)  
       * _OracleDbType_ - see [Oracle Data Types](https://docs.oracle.com/en/database/oracle/oracle-database/19/sqlrf/Data-Types.html)  
     
     Some databases allow combining enum values using `|`.  
     For example, using `Array | Double` for the Type is valid for PostgreSQL.  

     If no type is specified, the column type will be detected automatically.  

   * **Value**:  
     The value contains the actual data to be loaded into the column.

{CODE-BLOCK:javascript}
var orderData = {
    Id: id(this),
    OrderLinesCount: this.OrderLines.length,
    Quantities: {
        // Specify the Type and Value for 'Quantities':
        // ============================================ 
        Type: 'Array | Double',
        Value: this.OrderLines.map(function(l) {return l.Quantity;})
    },
    Products: {
        // Specify the Type and Value for 'Products':
        // ==========================================
        Type: 'Array | Text',
        Value: this.OrderLines.map(function(l) {return l.Product;})
    },
};

// Load the data into the 'Orders' table
loadToOrders(orderData);
{CODE-BLOCK/}

---

### Filtering

To filter some documents out from the ETL, simply omit the `loadTo` call:

{CODE-BLOCK:javascript}
if (this.ShipTo.Country === 'USA') {
    // Load only orders shipped to USA
    loadToOrders({ ... });
}
{CODE-BLOCK/}

---

### Accessing the metadata

You can access the metadata in the following way:

{CODE-BLOCK:javascript}
var value = this['@metadata']['custom-metadata-key'];
{CODE-BLOCK/}

---

### Document extensions

The SQL ETL task does not support sending [Counters](../../../document-extensions/counters/overview), 
[Time series](../../../document-extensions/timeseries/overview), or [Revisions](../../../document-extensions/revisions/overview).

{PANEL/}

{PANEL: Advanced options}

* **Command timeout**  
  Number of seconds after which SQL command will timeout.  
  It overrides the value defined in the [ETL.SQL.CommandTimeoutInSec](../../../server/configuration/etl-configuration#etl.sql.commandtimeoutinsec) configuration key.  
  Default: `null` (use provider default).
* **Parameterized deletes**  
  Control whether DELETE statements generated during the ETL process use parameterized SQL queries,  
  rather than embedding values directly in the query.  
  Default: `true`.
* **Table quotation**  
  Control whether table names in the generated SQL statements are enclosed in quotation marks.  
  Default: `true`.
* **Force recompile query**  
  Control whether to force the SQL Server to recompile the query statement using (`OPTION(RECOMPILE)`).  
  Default: `false`.

{PANEL/}

{PANEL: Transaction processing}

All records created in a single ETL run, one for each `loadTo` call, are sent in a single batch and processed within the same transaction.

{PANEL/}

{PANEL: Creating the SQL ETL task from the Client API}

{CODE add_sql_etl_connection_string @Server\OngoingTasks\ETL\SqlEtl.cs /}
{CODE add_sql_etl_task@Server\OngoingTasks\ETL\SqlEtl.cs /}

---

`SqlEtlConfiguration`:

| Property                 | Type                    | Description                                                                                                                |
|--------------------------|-------------------------|----------------------------------------------------------------------------------------------------------------------------|
| **Name**                 | `string`                | The SQL ETL task name.                                                                                                     |
| **ConnectionStringName** | `string`                | The registered connection string name.                                                                                     |
| **SqlTables**            | `List<SqlEtlTable>`     | A list of SQL tables that the scripts will load data to.                                                                   |
| **Transforms**           | `List<Transformation>`  | Your transformation scripts.                                                                                               |
| **QuoteTables**          | `bool`                  | Control whether table names in the generated SQL statements are enclosed in quotation marks.<br>Default is `true`.         |
| **ParameterizeDeletes**  | `bool`                  | Control whether DELETE statements generated during the ETL process use parameterized SQL queries.<br>Default is `true`.    |
| **ForceQueryRecompile**  | `bool`                  | Set to `true` to force the SQL Server to recompile the query statement using (`OPTION(RECOMPILE)`).<br>Default is `false`. |
| **CommandTimeout**       | `int?`                  | Number of seconds after which the SQL command will timeout.                                                                |

`SqlEtlTable`:

| Property             | Type     | Description                                                                                                                  |
|----------------------|----------|------------------------------------------------------------------------------------------------------------------------------|
| **TableName**        | `string` | The table name your script will load data to.                                                                                |
| **DocumentIdColumn** | `string` | The column in the destination table that will store the document IDs.                                                        |
| **InsertOnlyMode**   | `bool`   | When set to `true`, RavenDB will insert data directly without executing DELETE statements beforehand.<br>Default is `false`. |

{PANEL/}

## Related Articles

### ETL

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [RavenDB ETL Task](../../../server/ongoing-tasks/etl/raven)

### Client API

- [How to Add ETL](../../../client-api/operations/maintenance/etl/add-etl)
- [How to Update ETL](../../../client-api/operations/maintenance/etl/update-etl)
- [How to Reset ETL](../../../client-api/operations/maintenance/etl/reset-etl)

### Studio

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
