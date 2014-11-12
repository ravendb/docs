# RavenDB Bundles

RavenDB in its core is a document database, and as such it is feature complete and very functional. A user may require additional functionality, such that is not implemented by the core, and this is where the bundles come in.

## Built-in

The following bundles are available out of the box, to extend RavenDB's capabilities in several common scenarios:

* [Compression](../../server/extending/bundles/compression) - reduces the cost of I\O operations and data size on disk.
* [Encryption](../../server/extending/bundles/encryption) - enables data encryption.
* [Expiration](../../server/extending/bundles/expiration) - removes expired documents automatically.
* [Quotas](../../server/extending/bundles/quotas) - to put size limits on a database.
* [Replication](../../server/scaling-out/replication) - provides a way to replicate data between servers.
* [SQL Replication](../../server/extending/bundles/sql-replication) - replicates a RavenDB documents to SQL Server.
* [Versioning](../../server/extending/bundles/versioning) - automatic versioning of documents upon updates or deletes.
* [Periodic Backup](../../server/extending/bundles/periodic-backup) - configure backups.
* [Scripted Index Results](../../server/extending/bundles/scripted-index-results) - attach custom scripts to index.

{INFO Built-in bundles can be selectively added to a specific database using `Raven/ActiveBundles` property in the database document. The documentation describing how to manage database configuration documents can be found [here](../../administration/configuration). /}

## Custom

Beside the built-in bundles in RavenDB distribution package you can find other bundles that need to be placed in `Plugins` directory (see below for more information):

* [Authorization](../../server/extending/bundles/authorization) - allows to manage user groups, roles and permissions.
* [Cascade Delete](../../server/extending/bundles/cascade-delete)
* [Index Replication](../../server/extending/bundles/index-replication)
* [Unique Constraints](../../server/extending/bundles/unique-constraints) - adds the ability to define unique constraints to RavenDB documents.

### Installing a custom bundle

Installing a custom Bundle is as easy as dropping the assembly (and all its dependencies if any) into the `Plugins` directory of the RavenDB installation. Some bundles may also require some minimal configurations.

The path to the `Plugins` folder is configurable, see (configurations). By default it is `(RavenDB server)\Plugins`.

Included in the RavenDB distribution package are two scripts, enabling easy installation and update of Bundles.

* Raven-GetBundles.ps1
* Raven-UpdateBundles.ps1

Which you can use use to get new bundles or update the bundles you already have to a new version.

{NOTE Note that most bundles have to be setup when you create the database, and they cannot be removed afterward. So consider carefully your bundles strategy. You can usually add a bundle and then configure its behavior (effectively turning it on/off) at runtime, but adding/removing bundles is something that can safely happen only when the database is created. /}
