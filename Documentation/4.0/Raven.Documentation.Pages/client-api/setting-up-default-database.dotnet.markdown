# Client API: How to Setup a Default Database

* When a document store has a **default database**, each [Operation](../client-api/operations/what-are-operations) or [Session](../client-api/session/what-is-a-session-and-how-does-it-work) created through that document store will operate on that database by default.  

* It is also possible to specify the database individually, whether or not a default database has been set.  

* In this page:  
  * [Example With No Default Database](#no_default)  
  * [Example With a Default Database](#default)  
  <a name="no_default"/>
## Example With No Default Database

{CODE default_database_1@ClientApi\SetupDefaultDatabase.cs /}
<a name="default"/>
## Example With a Default Database

{CODE default_database_2@ClientApi\SetupDefaultDatabase.cs /}

## Related Articles

### Client API

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)

### Studio

- [Creating a Database](../studio/server/databases/create-new-database/general-flow)
