# Client API : How to Setup a Default Database

`database` property allows you to setup a default database for a `DocumentStore`. Implication of setting up a default database is that each time you access [operations](../clientApi/operations/what-are-operations) or create a [session](../clientApi/session/what-is-a-session-and-how-does-it-work) without explicitly passing database on which they should operate on then default database is assumed.

## Example I

{CODE:nodejs default_database_1@clientApi\setupDefaultDatabase.js /}

## Example II

{CODE:nodejs default_database_2@clientApi\setupDefaultDatabase.js /}

## Remarks

{NOTE By default value of `database` property in `DocumentStore` is `null` which means that in any action requiring a database name, we will have to specify the database. /}

## Related Articles

### Document Store

- [What is a Document Store](../clientApi/what-is-a-document-store)
- [Creating a Document Store](../clientApi/creating-document-store)
- [Setting up Authentication and Authorization](../clientApi/setting-up-authentication-and-authorization)

### Studio

- [Creating a Database](../studio/server/databases/create-new-database/general-flow)
