# How to setup a default database?

`Database` property allows you to setup a default database for a `DocumentStore`. Implication of setting up a default database is that each time you access [Operations](../client-api/operations/what-are-operations) or create a [Session](../client-api/session/what-is-a-session-and-how-does-it-work) without explicitly passing database on which they should operate on then default database is assumed.

## Example I

{CODE default_database_1@ClientApi\SetupDefaultDatabase.cs /}

## Example II

{CODE default_database_2@ClientApi\SetupDefaultDatabase.cs /}

## Remarks

{NOTE By default value of `Database` property in `DocumentStore` is `null` which means that in any actions that need a databae name we will have to specify the database./}

## Related articles

- [How to create a document store?](./creating-document-store)
