<<<<<<< HEAD
<<<<<<< HEAD
# Client API: How to Setup a Default Database
=======
# Client API : How to Setup a Default Database
=======
# Client API: How to Setup a Default Database
>>>>>>> RDoc-1529-setting-up-default-database
---
{NOTE: }
>>>>>>> RDoc-1529-setting-up-default-database

* When a document store has a **default database**, each [Session](../client-api/session/what-is-a-session-and-how-does-it-work) or 
[Operation](../client-api/operations/what-are-operations) created through that document store will operate on that database by default.  

* It is also possible to specify a database for a session or an operation individually, whether or not a default database has been set.  

* In this page:  
  * [Example With No Default Database Set](#no_default)  
  * [Example With a Default Database Set](#default)  
{NOTE/}

---
<a name="no_default"/>
{PANEL:Example - With No Default Database Set}

When no default database is set, you will need to specify the database for each session and operation. If no database is specified an exception will be thrown.  
{CODE default_database_1@ClientApi\SetupDefaultDatabase.cs /}

{PANEL/}

<a name="default"/>
{PANEL:Example - With a Default Database Set}

When `Database` is set to `Northwind`, every session and operation created through `store` will operate on the Northwind database by default.
You can still specify a different database on an individual basis.  
{CODE default_database_2@ClientApi\SetupDefaultDatabase.cs /}

{PANEL/}
## Related Articles

### Client API

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)

### Studio

- [Creating a Database](../studio/server/databases/create-new-database/general-flow)
