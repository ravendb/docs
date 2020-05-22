# Migration: How to Migrate Data from 4.x Server to 5.0

{NOTE:Migration from RavenDB 3.x}

If you are interested in migrating Server from earlier version please visit our dedicated [article](https://ravendb.net/docs/article-page/4.2/csharp/migration/server/data-migration). Current article focuses only on data migration between 4.x and 5.0 version of the Server.

{NOTE/}

RavenDB 5.0 supports in-place migration of the data from RavenDB 4.x. After replacing the binaries with the new ones, during first startup, the data will migrate automatically. Please note that after migration, the data cannot be used any longer with 4.x version of RavenDB. Please do a backup of the data before the migration.
