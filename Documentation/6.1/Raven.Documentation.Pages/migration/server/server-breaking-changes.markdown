# Migration: Server Breaking Changes
---

{NOTE: }
The features listed in this page were available in former RavenDB versions.  
In RavenDB `6.1.x`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:  
   * [MSSQL connection string requires an Encrypt property](../../migration/server/server-breaking-changes#mssql-connection-string-requires-an-encrypt-property)  
   * [Corax handling of complex JSON objects in static indexes is configurable](../../migration/server/server-breaking-changes#corax-handling-of-complex-json-objects-in-static-indexes-is-configurable)  
   * [Customizable identifier parts separator](../../migration/server/server-breaking-changes#customizable-identifier-parts-separator)  

{NOTE/}

---

{PANEL: MSSQL connection string requires an Encrypt property}

To establish a connection with an MSSQL server via ETL, RavenDB is required 
by the `Microsoft.Data.SqlClient` package it utilizes (which replaces the 
deprecated `System.Data.SqlClient` package we've been using in previous versions) 
to include in its connection string an `Encrypt` property that would determine 
whether to encrypt the connection or not.  

Some RavenDB versions preceding `6.1` (down to `6.0.105`) added this 
property to their connection strings without bothering their users, setting 
it to [Encrypt=Optional](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlconnectionencryptoption.optional?view=sqlclient-dotnet-standard-5.2#microsoft-data-sqlclient-sqlconnectionencryptoption-optional) 
and leaving the connection unencrypted unless users set it differently on 
their own accord.  
From RavenDB 6.1 on, we no longer include this property in MSSQL connection 
strings and users are required to explicitly choose whether to encrypt the 
connection or not.  

You can go on using [Encrypt=Optional](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlconnectionencryptoption.optional?view=sqlclient-dotnet-standard-5.2#microsoft-data-sqlclient-sqlconnectionencryptoption-optional) 
and leave your connection unencrypted, or include [Encrypt=Mandatory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlconnectionencryptoption.mandatory?view=sqlclient-dotnet-standard-5.2#microsoft-data-sqlclient-sqlconnectionencryptoption-mandatory) 
or [Encrypt=Strict](https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlconnectionencryptoption.strict?view=sqlclient-dotnet-standard-5.2#microsoft-data-sqlclient-sqlconnectionencryptoption-strict) 
in your connection string and provide the server you use with a valid certificate to encrypt the connection.  

Provide the connection string using code 
([like so](../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-sql-connection-string)) 
or via Studio:  

![SQL ETL task](images/breaking-changes_SQL-ETL-task.png "SQL ETL task")

{PANEL/}

{PANEL: Corax handling of complex JSON objects in static indexes is configurable}

The behavior of RavenDB's [Corax](../../indexes/search-engine/corax) search engine while 
handling [complex JSON objects](../../indexes/search-engine/corax#handling-of-complex-json-objects) 
in **static indexes** is now configurable using the `Indexing.Corax.Static.ComplexFieldIndexingBehavior` 
configuration option (the [handling of auto indexes](../../indexes/search-engine/corax#if-corax-encounters-a-complex-property-while-indexing) 
remains unchanged).  

* By default, `ComplexFieldIndexingBehavior` is set to **`Throw`**, instructing the search 
  engine to throw a [NotSupportedInCoraxException](../../indexes/search-engine/corax#if-corax-encounters-a-complex-property-while-indexing) 
  exception when it encounters a complex field in a static index.  

* If you prefer it, you can set `ComplexFieldIndexingBehavior` to **`Skip`** to disable the 
  indexing of complex fields without throwing an exception or raising a notification.  

{INFO: }

* The configuration option will apply only to [new static indexes](../../indexes/search-engine/corax#if-corax-encounters-a-complex-property-while-indexing), 
  created after the release of RavenDB `6.1`. It will not affect older indexes.  
* [ComplexFieldIndexingBehavior](../../server/configuration/indexing-configuration#indexing.corax.static.complexfieldindexingbehavior) 
  can be set for a particular index as well as for all indexes.  
* Though complex fields cannot be indexed, they **can** still be [stored and projected](../../indexes/search-engine/corax#revise-index-definition-and-fields-usage).  
* To search by the contents of a static index's complex field, you can convert 
  it to a string (using `ToString` on the field value in the index definition). 
  It is recommended, though, to index [individual properties](../../indexes/search-engine/corax#index-a-simple-property-contained-in-the-complex-field) 
  of the complex field.  

{INFO/}

{PANEL/}

{PANEL: Customizable identifier parts separator}

Picking an [identifier parts separator](../../server/kb/document-identifier-generation#id-generation-by-server) 
allows you to choose which character would be placed as a separator between ID parts 
when new documents are given their IDs.  

This configuration is available in the database level as well as server-wide, but in 
versions lower than RavenDB `6.1` its server-wide level wasn't implemented even if 
a new separator was selected.  

RavenDB `6.1` applies your identifier parts separator selection in the server-wide level 
as well. This means that if you selected a separator in a RavenDB version lower than `6.1` 
and you now migrate to `6.1`, your selected separator **will become active**.  

Please be aware of this change and check this setting before migrating.  

![Identity parts separator](images/breaking-changes_identity-parts-separator.png "Identity parts separator")

After making this change, creating a new document with the identity prefix `|`, e.g. `user|`, 
will apply your new separator.  

![New separator](images/breaking-changes_new-separator.png "New separator")

{PANEL/}

## Related Articles

### Changes API
- [Changes API](../../client-api/changes/what-is-changes-api)  
- [Tracking operations](../../client-api/changes/how-to-subscribe-to-operation-changes)  

### Studio
- [Identity parts separator](../../studio/server/client-configuration#set-the-client-configuration-(server-wide))  
- [SQL connection string](../../studio/database/tasks/import-data/import-from-sql#create-a-new-import-configuration)  

### Server
- [ID Generation](../../server/kb/document-identifier-generation#id-generation-by-server)

### Corax
- [Corax](../../indexes/search-engine/corax)  
- [Complex fields](../../indexes/search-engine/corax#handling-of-complex-json-objects)  
- [Auto indexes](../../indexes/search-engine/corax#if-corax-encounters-a-complex-property-while-indexing)  
