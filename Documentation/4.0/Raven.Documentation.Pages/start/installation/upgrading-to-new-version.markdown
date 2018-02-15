# Installation : Upgrading to a new version

{INFO On a live system, a minor version upgrade process typically takes around 30 seconds. /}

## Upgrading

Upgrading a RavenDB instance to a new version is very simple. In order to do so, you need to:

0. Download distribution package from [here](https://ravendb.net/downloads).
1. Shutdown the RavenDB server
    * for service (or daemon) - shutdown it before upgrading
    * for console application - execute 'shutdown' command in the RavenDB CLI
2. Remove old RavenDB binaries
    * make sure to _not_ delete your actual data which is in the folders like `RavenData` and _not_ to overwrite your configuration files like `settings.json`.
3. Copy new binaries 
4. Start the server again.

## High availability & Cluster

If you want a zero downtime, please upgrade single cluster node at a time and wait till it becomes a fully fledged node (either Member, Leader or Watcher). The state of the node can be checked in [Cluster View](../../studio/server/cluster/cluster-view).

## Upgrading data files

You don't have to do anything when you upgrade RavenDB to migrate the stored data. However, sometimes our adjustments require changing the file format (called schema version). RavenDB includes support for performing of those kind of migrations automatically on startup if it finds that the stored database is using an old format.

{WARNING Data file migrations are only one way. If you want to move backward and any changes in the file format have occurred, RavenDB will fail to start (with a detailed error message). You can move data between different versions using the import/export tool, which works across all versions of RavenDB. /}

## Remarks

{INFO:Major version upgrade}

RavenDB 4.x does not support automatic upgrading from previous major versions of the product (e.g. 3.5).  

Please read our [migration article](../../migration/server/data-migration) that describes in detail possible data and client migration strategies.

{INFO/}

## Related articles

- [Client API : Backward compatibility](../../client-api/faq/backward-compatibility)
