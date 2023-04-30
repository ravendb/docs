# Client API: Setting up a Default Database

---
{NOTE: }


* A **default database** can be set in the Document Store.  
The default database is used when accessing the Document Store methods without explicitly specifying a database.  

* You can pass a different database when accessing the Document Store methods.  
This database will override the default database for that method action only.  
The default database value itself will Not change.  

* When accessing the Document Store methods, an exception will be thrown if a default database is Not set and if No other database was 
explicitly passed.  

* In this page:  
  * [Example - Without a Default Database](../client-api/setting-up-default-database#example---without-a-default-database)  
  * [Example - With a Default Database](../client-api/setting-up-default-database#example---with-a-default-database)  
{NOTE/}

---
{PANEL:Example - Without a Default Database}

{CODE default_database_1@ClientApi\SetupDefaultDatabase.cs /}

{PANEL/}

{PANEL:Example - With a Default Database}

The default database is defined in the Document Store's `Database` property.
{CODE default_database_2@ClientApi\SetupDefaultDatabase.cs /}

{PANEL/}
## Related Articles

### Client API

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)

### Studio

- [Creating a Database](../studio/database/create-new-database/general-flow)
