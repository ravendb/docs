# Sharding: Backup & Restore
---

{NOTE: }

* **Backing up** a sharded database bares many similarities to 
  backing up a non sharded database.  

* Backed up data can be imported  
  

* Backed up data can be Restored


* In this page:  
  * [Backup](../../../sharding/server/ongoing-tasks/backup-and-restore#backup)  
     * [Backup Format and Contents](../../../sharding/server/ongoing-tasks/backup-and-restore#backup-format-and-contents)  
     * [Backup Type](../../../sharding/server/ongoing-tasks/backup-and-restore#backup-type)  
     * [Backup Scope](../../../sharding/server/ongoing-tasks/backup-and-restore#backup-scope)  
     * [Backup File and Folder Name](../../../sharding/server/ongoing-tasks/backup-and-restore#backup-file-and-folder-name)  
     * [Server-Wide Backup](../../../sharding/server/ongoing-tasks/backup-and-restore#server-wide-backup)  
     * [Backup to Local and Remote Destinations](../../../sharding/server/ongoing-tasks/backup-and-restore#backup-to-local-and-remote-destinations)
  * [Import](../../../sharding/server/ongoing-tasks/backup-and-restore#import)
  * [Restore](../../../sharding/server/ongoing-tasks/backup-and-restore#restore)  
     * [](../../../)  
     * [](../../../)  
     * [](../../../)  
     * [](../../../)  
     * [](../../../)  
     * [](../../../)  

{NOTE/}

---

{PANEL: Backup}

Like non-sharded databases, sharded databases are backed up using 
backup tasks. There are severel similarities and differences between the two:  

* **Non-sharded Database Backup**  
  A complete replica of the database is kept by each node of the hosting cluster.  
  It therefore takes a **single backup task**, operated by one of the cluster nodes, 
  to create a complete database backup.  
  Only one of the cluster nodes is made 
  [responsible](../../../server/clustering/distribution/highly-available-tasks#responsible-node) 
  for the execution of the backup task.  

* **Sharded Database Backup**  
  A part of the database is hosted by each shard.  
  It therefore takes **multiple backup tasks**, one per shard, to create 
  a complete backup of all database parts.  
   * Only one of the [shard nodes](../../../sharding/overview#shard-replication) 
     is made responsible for the execution of this shard's backup task.  
   * Each shard stores its part of the database in a unique location.  
     Shard databases can be stored locally or on the cloud.  
   * To restore the entire database, the backup files made by all 
     the shards need to be collected and re-united.  

---

#### Backup Format and Contents

Shards backup files are formatted similarly to the `.ravendbdump` files 
created when a database is exported [using Studio](../../../studio/database/tasks/export-database) 
or [Smuggler](../../../client-api/smuggler/what-is-smuggler).  

Backed-up data includes both database-level and cluster-level contents, as 
for [non-sharded databases](../../../server/ongoing-tasks/backup-overview#backup-contents).  

---

#### Backup Type

A shard backup task can create a 
[Logical backup](../../../server/ongoing-tasks/backup-overview#logical-backup) 
or a [Snapshot](../../../server/ongoing-tasks/backup-overview#snapshot).  

{NOTE: }
**Note**, however, that a `snapshot` backup of a shard 
[cannot](../../../sharding/server/ongoing-tasks/backup-and-restore#restore) 
be directly restored to a sharded database.  
The snapshot of a shard **can** be restored as a new non-sharded database.  
{NOTE/}

`Logical` shard backups **can** be restored to either a sharded or a non-sharded database.  

---

#### Backup Scope

A shard backup task can create 
a [Full backup](../../../server/ongoing-tasks/backup-overview#full-backup) 
with the entire content of the shard, or an 
[Incremental Backup](../../../server/ongoing-tasks/backup-overview#incremental-backup) 
with just the difference between the current database data and the last backed-up data.  

---

#### Backup File and Folder Name

Shard backup folders and files are generally named 
[similarly](../../../server/ongoing-tasks/backup-overview#backup-name-and-folder-structure) 
to non-sharded database backups.  

Unlike a non-sharded database, shard database folder names includes a **$** 
sign followed by the shard number.  
The backup folders of a 3-shard database named "Books", for example, will be named:  
`Books$01` for the backup folder of shard 1  
`Books$02` for the backup folder of shard 2  
`Books$03` for the backup folder of shard 3  

---

#### Server-Wide Backup

[Server-wide backup](../../../client-api/operations/maintenance/backup/backup#server-wide-backup) 
backs up all cluster databases by creating a backup task for each database and 
executing all tasks at a scheduled time.  

A different backup task will be created for each shard of each sharded database.  
Each task will be executed by the node responsible for backup at this task's shard.  

---

#### Backup to Local and Remote Destinations

As with non-sharded databases, shards can be backed up to local and remote destinations.  

Defining the destinations is 
[no different than for non-sharded databases](../../../client-api/operations/maintenance/backup/backup#backup-to-local-and-remote-destinations).  
When a backup task is defined for a sharded database, RavenDB will automatically 
create a task per shard. When the scheduled time arrives, each shard's responsible 
node will run the shard's task and store the files in the specified location, in 
a folder whose name [include the shard number](../../../sharding/server/ongoing-tasks/backup-and-restore#backup-file-and-folder-name).  

---

#### Import

`.ravendbdump` files [Exported](../../../studio/database/tasks/export-database) 
from shard databases can be 
[imported](../../../studio/database/tasks/import-data/import-data-file#import-data-from-.ravendbdump-file) 
into a new database to create a full or a partial replica of the original database.  

{PANEL/}

{PANEL: Restore}

To restore a sharded database, provide the restore operation 
with the shards backup files in the original order in which 
they were backed up.  

#### Restoring a Database: Configuration and Execution

* To restore the database:  
  {CODE-BLOCK:json}
var restoreOperation = new RestoreBackupOperation(new RestoreBackupConfiguration
  {
     DatabaseName = databaseName,
     ShardRestoreSettings = settings
  });
  {CODE-BLOCK/}

    * Configure a `RestoreBackupConfiguration` instance.  
    * Define all shard-related information within the instance's `ShardRestoreSettings` property 
      including, for each shard, the shard number, node tag, and backup file/s folder name.  
    * Pass the defined `RestoreBackupConfiguration` instance to `RestoreBackupOperation` for execution.  


---

#### `RestoreBackupOperation`
This is the restore operation.
{CODE restore_RestoreBackupOperation@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

---

#### `RestoreBackupConfigurationBase`
This is the restore operation configuration.  
{CODE restore_RestoreBackupConfigurationBase@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

* Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **DatabaseName** | string | Name for the new database. |
    | **LastFileNameToRestore** <br> (Optional -<br> omit for default) | string | [Last incremental backup file](../../../../server/ongoing-tasks/backup-overview#restoration-procedure) to restore. <br> **Default behavior: Restore all backup files in the folder.** |
    | **DataDirectory** <br> (Optional -<br> omit for default) | string | The new database data directory. <br> **Default folder: Under the "Databases" folder, in a folder that carries the restored database's name.** |
    | **EncryptionKey** <br> (Optional -<br> omit for default) | string | A key for an encrypted database. <br> **Default behavior: Try to restore as if DB is unencrypted.**|
    | **DisableOngoingTasks** <br> (Optional -<br> omit for default) | boolean | `true` - disable ongoing tasks when Restore is complete. <br> `false` - enable ongoing tasks when Restore is complete. <br> **Default: `false` (Ongoing tasks will run when Restore is complete).**|
    | **SkipIndexes** <br> (Optional -<br> omit for default) | boolean | `true` to disable indexes import, <br> `false` to enable indexes import. <br> **Default: `false` restore all indexes.**|
    | **Type** | RestoreType | Restore from local or cloud storage (see options below) |
    | **ShardRestoreSettings** | ShardRestoreSettings  | a list of `SingleShardRestoreSetting` instances defining the shard files to restore |
    | **BackupEncryptionSettings** | BackupEncryptionSettings  | [Backup Encryption Settings](../../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode--key) |
  
    {WARNING: }
    * Verify that RavenDB has full access to the backup-files and database folders.
    * Make sure your server has permissions to read from `BackupLocation` and write to `DataDirectory`.
    {WARNING/}

#### `RestoreType`
{CODE restore_RestoreType@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

#### `ShardedRestoreSettings`
{CODE restore_ShardedRestoreSettings@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

#### `SingleShardRestoreSetting`
{CODE restore_SingleShardRestoreSetting@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}



{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../client-api/operations/server-wide/create-database)  

**Server**  
[External Replication](../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../../../studio/database/tasks/export-database)  

