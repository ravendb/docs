# RavenDB Bundles

RavenDB in its core is a document database, and as such it is feature complete and very functional. A user may require additional functionality, such that is not implemented by the core, and this is where the bundles come in.

## Built-in

The following bundles are available out of the box, to extend RavenDB's capabilities in several common scenarios:

* [Compression](compression) - reduces the cost of I\O operations and data size on disk.
* [Encryption](encryption) - enables data encryption.
* [Expiration](expiration) - removes expired documents automatically.
* [Quotas](quotas) - to put size limits on a database.
* [Replication](../../scaling-out/replication) - provides a way to replicate data between servers.
* [SQL Replication](sql-replication) - replicates a RavenDB documents to SQL Server.
* [Versioning](versioning) - automatic versioning of documents upon updates or deletes.
* [Periodic Backups](periodic-backups) - configure backups.
* [Indexed Properties](indexed-properties)

{INFO Built-in bundles can be selectively added to a specific database using `Raven/ActiveBundles` property in the database document. The documentation describing how to manage database configuration documents can be found [here](../../multiple-databases). /}

## Custom

Beside the built-in bundles in RavenDB distribution package you can find other bundles that need to be placed in `Plugins` directory (see below for more information):

* [Authorization](authorization) - allows to manage user groups, roles and permissions.
* [Cascade Delete](cascade-delete)
* [Index Replication](index-replication)
* [Unique Constraints](unique-constraints) - adds the ability to define unique constraints to RavenDB documents.

### Installing a custom bundle

Installing a custom Bundle is as easy as dropping the assembly (and all its dependencies if any) into the `Plugins` directory of the RavenDB installation. Some bundles may also require some minimal configurations.

The path to the `Plugins` folder is configurable, see (configurations). By default it is `(RavenDB server)\Plugins`.

Included in the RavenDB distribution package are two scripts, enabling easy installation and update of Bundles.

* Raven-GetBundles.ps1
* Raven-UpdateBundles.ps1

Which you can use use to get new bundles or update the bundles you already have to a new version.

{NOTE Note that most bundles have to be setup when you create the database, and they cannot be removed afterward. So consider carefully your bundles strategy. You can usually add a bundle and then configure its behavior (effectively turning it on/off) at runtime, but adding/removing bundles is something that can safely happen only when the database is created. /}
