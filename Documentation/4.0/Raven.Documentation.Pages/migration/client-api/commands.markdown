# Migration: DatabaseCommands has been removed

`IDatabaseCommands` and `IAsyncDatabaseCommands` has been replaced in RavenDB.Client 4.0 by operations. Please read the dedicated documentation section about [operations](../../client-api/operations/what-are-operations) available in the client API.

## Checking if document exists

| 3.x | 4.0 |
|:---:|:---:|
| {CODE exists_1@Migration\ClientApi\Commands.cs /} | {CODE exists_2@Migration\ClientApi\Commands.cs /} |
