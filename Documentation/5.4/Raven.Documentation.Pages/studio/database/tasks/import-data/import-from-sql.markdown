﻿# Import from SQL
---

{NOTE: }

* SQL Migration allows you to import data from an SQL database to RavenDB.  

* SQL sources that data can be imported from currently include:  
   * **MySQL**  
   * **SQL Server**  
   * **Oracle**  
   * **PostgresSQL**  

* A semi-automatic import process resolves the original business model, 
  taking into account SQL relations represented by linked foreign and 
  primary keys and identifying one-to-one, one-to-many, and many-to-one relations.  

* Primary/Foreign keys must be defined in the source SQL database so the migrator 
  would be able to perform correctly.  

* The SQL Migrator attempts to detect and automatically suggest the best data model.  
  Changes made while preparing the data model maintain data integrity and cohesion.  

* In this page:
  * [Import Data From SQL Source](../../../../studio/database/tasks/import-data/import-from-sql#import-data-from-sql-source)  
     * [Create a New Import Configuration](../../../../studio/database/tasks/import-data/import-from-sql#create-a-new-import-configuration)  
     * [Advanced Import Options](../../../../studio/database/tasks/import-data/import-from-sql#advanced-import-options)  
     * [Run an Existing Import Configuration](../../../../studio/database/tasks/import-data/import-from-sql#run-an-existing-import-configuration)  
  * [SQL Migration](../../../../studio/database/tasks/import-data/import-from-sql#sql-migration)  
     * [Data Conversion: General Settings](../../../../studio/database/tasks/import-data/import-from-sql#data-conversion-general-settings)  
     * [Data Conversion: Table/Collection Settings](../../../../studio/database/tasks/import-data/import-from-sql#data-conversion-tablecollection-settings)  
  * [Filtering and Transforming Documents](../../../../studio/database/tasks/import-data/import-from-sql#filtering-and-transforming-documents)  
  * [Handling Relationships](../../../../studio/database/tasks/import-data/import-from-sql#handling-relationships)  
  * [Data integrity Helpers](../../../../studio/database/tasks/import-data/import-from-sql#data-integrity-helpers)  
{NOTE/}

---

{PANEL: Import Data From SQL Source}

![From SQL Tab](images/sql-migration.png "From SQL Tab")

1. **Tasks**  
   Click to open the Tasks menu.  
2. **Import Data**  
   Click to import data.  
3. **From SQL**  
   Click to import data from an SQL source.  
4. [Create new import configuration](../../../../studio/database/tasks/import-data/import-from-sql#create-a-new-import-configuration)  
   Create a new import configuration from scratch.  
   The configuration can be exported and reused.  
5. [Continue existing migration](../../../../studio/database/tasks/import-data/import-from-sql#run-an-existing-import-configuration)  
   Run an existing import configuration.  

---

### Create a New Import Configuration

Use this option to enter a new import configuration.  
The configuration can then be exported and [reused](../../../../studio/database/tasks/import-data/import-from-sql#run-an-existing-import-configuration).  

![Create New Import Configuration](images/sql-migration-create-new-import.png "Create New Import Configuration")

1. **SQL database driver**  
   Select one of the available database drivers:  
    * Microsoft SQL Server (System.Data.SqlClient)  
    * MySQL Server (MySql.Data.MySqlClient)  
    * MySQL Server (MySqlConnector.MySqlConnectorFactory)  
    * PostgreSQL (Npgsql)  
    * Oracle Database (Oracle.ManagedDataAccess.Client)  
2. **Connection string**  
   Provide a connection string to the data origin server.  
   E.g., the connection string for a local mySQL database can be:  
   `server=127.0.0.1;`  
   `database=sample_schema;port=3306;`  
   `userid=root;password=secret;`  
    {NOTE: }
    Click the **Syntax** caption to display valid connection strings for the different SQL sources.  
    {NOTE/}
3. [Advanced](../../../../studio/database/tasks/import-data/import-from-sql#advanced-import-options)  
   Click for advanced import options.  

4. [Create new import](../../../../studio/database/tasks/import-data/import-from-sql#data-conversion-general-settings)  
   Click to connect the SQL source and import the database schema and data.  

5. **Test Connection**  
   Click to validate the connection string and verify connectivity to the data source.  
   
      ![Successful Connection](images/test-connection-string.png "Successful Connection")

---

### Advanced Import Options

![Advanced Import Options](images/sql-migration-advanced-options.png "Advanced Import Options")

1. These advanced import options are available for **all import types**.  
    * **Convert property names to PascalCase**  
      Property names can be converted to PascalCase.  
      E.g.  `ZIP_CODE` is converted to `ZipCode`  
    * **Trim suffix from property names**  
      Suffix can be removed from property name.  
      E.g. `ADDRESS_ID` becomes `Address`.
    * **Detect many-to-many relationships**  
      Many-to-many relationships can be detected.  
      If all table columns are defined as foreign keys, 
      the relationship is identified as *many-to-many*.  
      To allow efficient modeling of 2-way relationships, 
      Such tables are not imported and both sides of the 
      relationship are linked.  
    * **Include unsupported data types**  

2. This advanced import option is available only when importing data from a PostgresSQL server.  
    * **Specify schemas to use (comma separated)**  
      By default, RavenDB will attempt to import data from a PostgresSQL server using the **Public** schema.  
      To **continue** this behavior keep the above configuration option disabled or provide no alternative 
      schema name in the input area.  
      To **alter** this behavior and import data using other schemas, enable the above configuration option 
      and provide schema names (comma separated, e.g. `public.info_schema,public.prod_schema`) in the input area.  

---

### Run an Existing Import Configuration

If you have previously created and saved an import configuration, 
you can locate and run it here.  

![Use Existing Import Configuration](images/sql-migration-use-existing-configuration.png "Use Existing Import Configuration")

1. Click to locate an existing import configuration on the file system.  
2. Click to [run the selected configuration](../../../../studio/database/tasks/import-data/import-from-sql#data-conversion-general-settings) 
   and migrate data from the SQL source.  

{PANEL/}

{PANEL: SQL Migration}

{INFO: }
The _NoSQL_ data model allows nested arrays/objects, where _SQL_ uses additional 
tables to render such relations, that are often unrelated to the natural data form.  
The nested items satisfy [third normal form (3NF)](https://en.wikipedia.org/wiki/Third_normal_form), 
e.g. by embedding `OrderLines` within an `Order` object to solve a potential *select n+1* problem.  

_SQL_ also uses artificial tables to represent *many-to-many* relationships.  
In _NoSQL_, this can be modeled as a list of foreign object IDs and requires no extra table/collection.  
{INFO/}

---

### Data Conversion: General Settings

When an [import configuration](../../../../studio/database/tasks/import-data/import-from-sql#create-a-new-import-configuration) 
is executed RavenDB connects the SQL source, obtains information about database tables 
and keys, and displays the following view to allow the user to alter the migration and 
target documents structure.  

![Migration Tuning](images/sql-migration-tuning.png "Migration Tuning")

1. **Select Tables**  
   Toggle ON/OFF to select the tables you want to migrate  

2. **Migrate Tables**  
   Click to migrate the selected tables and display a concise execution summary.  
   
      ![Execution Summary](images/sql-migration-execution-summary.png "Execution Summary")

3. **Filter**  
   Filter tables by name  

4. **Export configuration**  
   Click to export the configuration to a file for future use or fine tuning  

5. **Full screen mode**  
   Click to toggle full-screen mode  

6. **Settings**  
   Click to set additional import configuration settings  

      ![Migration Settings](images/sql-migration-tuning_settings.png "Migration Settings")

      * **Batch Size**  
        Enable to change batch size of document insertion  
      * **Convert binary columns to attachments**  
        Enable to add binary content to target documents as **attachments**.  
        Disable to add binary content target documentd as `base64 strings`.  
      * **Partial migration**  
        Enable to limit the number of items imported per table.  
        
           ![Partial Migration](images/sql-migration-tuning_settings_partial-migration.png "Partial Migration")
           
           {NOTE: }
           Please note that limiting the migration may break links between documents.  
           {NOTE/}

7. [Table/Collection Conversion Tuning](../../../../studio/database/tasks/import-data/import-from-sql#data-conversion-tablecollection-settings)  
   Specific details related to the transfer of each table can be altered as well.  
   See additional details below.  

---

### Data Conversion: Table/Collection Settings

![Data Conversion Tuning](images/sql-migration-data-conversion-tuning.png "Data Conversion Tuning")

1. **Schema**  
   Source schema  

2. **Table**  
   Source table  

3. **Collection**  
   Target collection  
   Click to alter the collection name before the conversion  

4. **Show Incoming Links**  
   Shows a relationships count for this table.  
   Click for details regarding each relationship, and to select whether 
   to skip each relationship or embed it in the document.  

5. [Filter/Transform](../../../../studio/database/tasks/import-data/import-from-sql#filtering-and-transforming-documents)  
   Click to alter the **filter query** executed over the source database, 
   and/or the **transform script** used to shape the imported data.  
   
6. **Document ID**  
   The ID given to the new document, and its origin in the source database.  

7. **SQL Column Name**  
   The name of the SQL column the value is taken from.

8. **Document Property**  
   The new document property name and type.  
   Click to alter the property name.  
   
9. **Relationships strip**  
   This strip summarizes relationships identified in the incoming data.  
   Hover above the displayed items for additional information.  

{PANEL/}

{PANEL: Filtering and Transforming Documents}

The migration process offers its users full control over the 
import procedure by allowing them to comfortably **filter** 
the data it reads from the SQL database and easily **transform** 
the data it writes to RavenDB.  
Filtering the read data is gained by altering the query RavenDB 
runs over the SQL server to fetch the data.  
transforming written documents is done using a transform script 
through which all incoming data passes.  

---

Learn [here](../../../../studio/database/tasks/import-data/import-from-sql#data-conversion-general-settings) 
how to connect the source database and open the filter/transform dialog.  

---

![Filter and Transform](images/sql-migration-filter-and-export-I.png "Filter and Transform")

1. Enable this option to open the filter editor.  
   
      ![Filter Query](images/sql-migration-filter-and-export-II.png "Filter Query")
   
      Data rows are read from the source SQL database by executing 
      the query defined in the filter section.  
      A **where** SQL statement can be included in the query to filter 
      the data as you please.  
      E.g. -  
      {CODE-BLOCK: SQL}
      select * from `shop`, `orders`
      where year(Date) >= 2022
      {CODE-BLOCK/}

2. Enable this option to open the transform-script editor.  
   
      ![Transform Script](images/sql-migration-filter-and-export-III.png "Transform Script")
   
      The added transformation script can be used to monitor all 
      or any part of the imported data and shape the final documents 
      as you please.  
      E.g. -  
      {CODE-BLOCK: JavaScript}
      var linesCount = this.OrderLines.Length;
      if (linesCount < 3) {
         throw `skip`; // use this statement to skip a given document
      }
      this.LinesCount = linescount;
      where year(Date) >= 2022
      {CODE-BLOCK/}


3. select a test mode to test your filter and/or transform script before 
   actually importing the data from the source DB to RavenDB.  

      ![Transform Script](images/sql-migration-test-filter-and-export.png "Transform Script")
    
      * **Use first record matching query**  
        This mode runs a query defined in the filter section and gets the first result.  
      * **Specify primary key values to use**  
        With this mode the user is presented with extra fields to provide value for each primary key column.  

{PANEL/}

{PANEL: Handling Relationships}

Let's use an SQL database with the following tables as an example.  

   ![Database ERD (Entity Relationship Diagram)](images/sql-migration-erd.png "Database ERD (Entity Relationship Diagram)")

The database contains 2 relationships, which can be represented as additional document properties.  

   ![SQL Migration - Relationships](images/sql-migration-relationships-1.png "SQL Migration - Relationships")

* The *OrderLines* document will be given an *OrderId* property (with the value *Orders/{ID}*).  
  This is a _linking_ property.  
* On the other side of the relationship **skip** mode is applied, causing this document property to be skipped.  

These settings will present us with a document with the following structure:  

* **OrderLine**  
  {CODE-BLOCK: JSON}
{
    "Quantity": 10,
    "OrderId": "Orders/1",
    "ProductId": "Products/2"
}
  {CODE-BLOCK/}

* **Order**  
  {CODE-BLOCK: JSON}
{
    "Date": "2018-05-05",
    "Username": "accountName",
    "ExtraInfo": "Please call before delivery"
}
  {CODE-BLOCK/}

* **Product**  
  {CODE-BLOCK: JSON}
{
    "Name": "Phone",
    "UnitsInStock": 4
}
  {CODE-BLOCK/}

If we embed *OrderLines* in *Orders* and uncheck the *OrderLines* table (as 
we don't need this artificial collection), we end up with the following structure:

![SQL Migration - Relationships](images/sql-migration-embed.png "SQL Migration - Relationships")

* **Order**  
  {CODE-BLOCK: JSON}
{
    "Date": "2018-05-05",
    "Username": "accountName",
    "ExtraInfo": "Please call before delivery",
    "OrderLines: [
    {
        "Quantity": 10,
        "ProductId": "Products/2"
    }]
}
  {CODE-BLOCK/}

* **Product**  
  {CODE-BLOCK: JSON}
{
    "Name": "Phone",
    "UnitsInStock": 4
}
  {CODE-BLOCK/}

---

The **target document structure** is determined by the relationship and selected mode.  

* **One-to-many** relationship  
  The source table contains a *primary key*  
  The target table contains a *foreign key*  
   * Mode set to **skip** - Property is skipped  
   * Mode set to **link** - Property value contains an array of document identifiers  
   * Mode set to **embed** - Property value contains array of nested documents  
* **Many-to-one** relationship  
  The source table contains a *foreign key*  
  The target table contains a *primary key*  
   * Mode set to **skip** - Property is skipped  
   * Mode set to **link** - Property value contains a single document identifier or null  
   * Mode set to **embed** - Property value contains single nested document  

{NOTE: }
Embedded documents do **not** contain an identifier, as it is redundant in this context.  
{NOTE/}

{PANEL/}

{PANEL: Data Integrity Helpers}

The SQL migrator tries to maintain model integrity throughout the process, applying 
the following checks.  

---

### Table cannot be deselected when at least an incoming relationship is defined.
   
![Table is in use by incoming references](images/sql-migration-table-in-use.png "Table is in use by incoming references")
   
---

### An embedded table can be deselected.

If the table is embedded and is not used by any other relationship, the migrator 
will suggest deselecting root collection.  
E.g., *OrderLines* was embedded within *Orders*:  
The *OrderLines* collection will not be needed, since it was embedded in *Orders*.  
   
![Suggestion to deselect root table](images/sql-migration-deselect.png "Suggestion to deselect root table")
   
---

### A target table for linking is not selected.

This might create a broken link, so the migrator suggests selecting a target table first.  
   
![Suggestion to select target](images/sql-migration-link.png "Suggestion to select target")
   
---

### Deselecting all tables

Before deselecting all tables, the migration process asks whether the user wishes to set 
all relationships to *skip*.  
   
![Skipping all relationships](images/sql-migration-skip-all.png "Skipping all relationships")
  
{PANEL/}

## Related articles

### Import
- [Import from RavenDB](../../../../studio/database/tasks/import-data/import-from-ravendb)  
- [Import from CSV](../../../../studio/database/tasks/import-data/import-from-csv)  
- [Import from Other DBs](../../../../studio/database/tasks/import-data/import-from-other)  
