# Client API: Setting Up a Default Database

---
{NOTE: }


* When a default database is set in the Document Store:  
  * A Session or Operation will operate on that database by default  
  * A Session or Operation can override the default database by explicitly specifying a different database to work on  

* When a default database is Not set in the Document Store:  
  * The Session/Operation need to explicitly specify the database to operate on  
  * If no database is specified, an exception is thrown  

* In this page:  
  * [Example Without a Default Database](../client-api/setting-up-default-database#example---without-a-default-database)  
  * [Example With a Default Database](../client-api/setting-up-default-database#example---with-a-default-database)  
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

- [Creating a Database](../studio/server/databases/create-new-database/general-flow)
