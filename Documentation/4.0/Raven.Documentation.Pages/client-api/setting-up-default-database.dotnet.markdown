# Client API: How to Setup a Default Database

`Database` property allows you to setup a default database for a `DocumentStore`. Implication of setting up a default database is that each time you access [Operations](../client-api/operations/what-are-operations) or create a [Session](../client-api/session/what-is-a-session-and-how-does-it-work) without explicitly passing database on which they should operate on then default database is assumed.

## Example I

{CODE default_database_1@ClientApi\SetupDefaultDatabase.cs /}

## Example II

{CODE default_database_2@ClientApi\SetupDefaultDatabase.cs /}

## Remarks

{NOTE By default value of `Database` property in `DocumentStore` is `null` which means that in any actions that need a database name we will have to specify the database./}

## Related Articles

### Document Store

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Authentication and Authorization](../client-api/setting-up-authentication-and-authorization)

### Studio

- [Creating a Database](../studio/server/databases/create-new-database/general-flow)
