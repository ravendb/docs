# Installation: Upgrading to a New Version

{INFO On a live system, a minor version upgrade process typically takes around 30 seconds. /}

## Upgrading

Upgrading a RavenDB instance to a new version is very simple. To do so:  

1. Download a distribution package from [here](https://ravendb.net/downloads).

2. Shut RavenDB Server down.  
    * for a service (or daemon) - shutdown before upgrading.  
    * for a console application - execute a 'shutdown' command in the RavenDB CLI.  

3. Remove old RavenDB binaries
    * **Avoid** deleting your actual data, contained in folders like `RavenData`.  
    * **Avoid** overwriting your configuration files, e.g. [settings.json](../../server/configuration/configuration-options#settings.json).  
    * **Avoid** deleting your certificate file (ending with `.pfx` or `.pem`).  
    * For additional information see the [certificate page](../../server/security/authentication/certificate-configuration).  
    * These steps are strictly necessary when updating to version 5.1.  
      To update to lower versions, overriding the old binaries may be sufficient - although removing them is recommended.  

4. Copy the new binaries.  

5. Restart the server.  

## High Availability & Cluster

For zero downtime during a version upgrade, please upgrade a single cluster node at 
a time and wait until it becomes a fully fledged node (either Member, Leader, or Watcher).  
The state of the node can be checked in [Cluster View](../../studio/cluster/cluster-view).  

## Upgrading Data Files

While upgrade RavenDB, no special action is needed to migrate the stored data.  
However, sometimes our adjustments require changing the file format ("schema version").  
If RavenDB finds during startup that the stored database uses an old format, it 
will automatically perform this kind of migration.  

{WARNING: }
Migrating data files is only one type of migration.  
If you try to downgrade to an older RavenDB version after making any changes in data files 
format, RavenDB will fail to start with a detailed error message.  
Data can also be migrated, across all RavenDB versions, using 
[Export](../../studio/database/tasks/export-database)/[Import](../../studio/database/tasks/import-data/import-from-ravendb), 
[Backup](../../studio/database/tasks/backup-task)/[Restore](../../studio/database/create-new-database/from-backup), 
or [External replication](../../studio/database/tasks/ongoing-tasks/external-replication-task).  
{WARNING/}

## Remarks

{INFO:Major version upgrade}

* RavenDB `4.x` does **not** support automatic upgrading from previous major versions of the product (e.g. `3.5`).  
  Please read the [migration article](../../migration/server/data-migration) for additional data and client migration strategies.
* If you have a license for a RavenDB version older than `6.x`, upgrading to version `6.0` will require you 
  to upgrade your license key.  
   * Read [here](../../start/licensing/replace-license#upgrade-a-license-key-for-ravendb-6.x) how to upgrade 
     and replace your existing license key.  
   * Read [here](../../migration/server/data-migration) about migrating from one RavenDB version to another.  

{INFO/}

## Related Articles

### Installation

- [System Requirements](../../start/installation/system-requirements)
- [System Configuration Recommendations](../../start/installation/system-configuration-recommendations)
- [Deployment Considerations](../../start/installation/deployment-considerations)

### Client API

- [Backward Compatibility](../../client-api/faq/backward-compatibility)
