# Sharding: Restore
---

{NOTE: }

* A sharded database's backup is a set of backup files that were 
  created by the database's shards.  
* To restore a sharded database, we need to pass the restore 
  operation paths to the locations of the backup files so it 
  can retrieve and restore them.  
   {WARNING: }
   Shards must be restored in their 
   [original order](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#restore):  
   the backup of shard 1, for example, must be restored as shard 1.
   {WARNING/}
* Backup files can be restored from local shard storage or from 
  remote locations like an S3 Bucket or an Azure Blob.  
* A backed up sharded database can be restored in part or in full, 
  to a sharded or a non-sharded database.  
* Only [logical](../../../../server/ongoing-tasks/backup-overview#logical-backup) 
  backups are supported. 
  [Snapshot](../../../../server/ongoing-tasks/backup-overview#snapshot) 
  backups cannot be created or restored for sharded databases.  
* `.ravendbdump` files (exported from RavenDB databases) and 
  backup files can also be 
  [imported](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#import) 
  into a sharded database.  

* In this page:  
  * [Restore](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#restore)  
     * [Set Paths to Backup Files Locations](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#set-paths-to-backup-files-locations)  
     * [Define a Restore Configuiration](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#define-a-restore-configuiration)  
         * [`RestoreBackupConfigurationBase`](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#section)
     * [Run `RestoreBackupOperation` with the Restore Configuration](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#run--with-the-restore-configuration)  
     * [Examples](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#examples)  
  * [Import](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#import)  

{NOTE/}

---

{PANEL: Restore}

To restore a sharded database, we need to:  

## Set Paths to Backup Files Locations
When a shard stores a backup file, it may store it locally (on the shard 
machine's storage) or remotely (on an S3 Bucket or an Azure Blob).  
  
To restore the backup files, we need to provide the restore process with 
paths to the files' locations.  
The paths, and additional data regarding the backups, are provided in 
a dictionary of `SingleShardRestoreSetting` objects.  

`SingleShardRestoreSetting`  
{CODE restore_SingleShardRestoreSetting@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

* Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **ShardNumber** | `int` | The original shard number, that should normally also be the restored shard's number. |
    | **NodeTag** | `string` | The original node the shard resided on, that should normally also be the restored shard's node. |
    | **FolderName** | `string` | The name of the folder that holds the backup file/s. |

    {WARNING: }
    When setting **ShardNumber**, please make sure that all shards are 
    given the same numbers they had when they were backed up.  
    E.g., a backup of shard 1 must be restored as shard 1 (`ShardNumber` = 1).  
    
    Giving restored shards numbers other than those they had originally 
    will place buckets on the wrong shards and cause mapping errors.  
    {WARNING/}

    {WARNING: }
    When setting **NodeTag** for shards whose backups were stored locally, 
    please make sure they are given the same node tags they had when they 
    were backed up.  
    E.g., a backup of a shard locally-stored on node B, must be restored 
    to node B (`NodeTag = "B"`). 
    
    Using node tags other than the original ones for locally-stored shards 
    may cause data retrieval errors, misidentification of documents, etc.  
    {WARNING/}

## Define a Restore Configuiration
To restore the database, we pass the 
[Restore Operation](../../../../client-api/operations/maintenance/backup/restore#restoring-a-database:-configuration-and-execution) 
a **configuration object**.  

* The configuration object inherits properties from the `RestoreBackupConfigurationBase` class 
  ([discussed below](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#section)), 
  and defines additional sharding-specific settings.  

* We choose what configuration object to pass the restore operation, by 
  the backup files' location.  
  The backup files may be located locally (on each shard machine storage) 
  or remotely (in a cloud location).  
  
    | Configuration Object | Backup Location | Additional Properties |
    | -------------------- | --------------- | --------------------- |
    | `RestoreBackupConfiguration` | Local shard storage | None (see [example](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#examples)) |
    | `RestoreFromS3Configuration` | AWS S3 Buckets | S3Settings (see S3 [example](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#examples)) |
    | `RestoreFromAzureConfiguration` | MS Azure Blobs | AzureSettings (see Azure [example](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#examples)) |

---

### `RestoreBackupConfigurationBase`
`RestoreBackupConfigurationBase` is a parent class to all the configuration types 
mentioned above, allowing you to set backups **encryption settings** among other options.  
{CODE restore_RestoreBackupConfigurationBase@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

* Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **DatabaseName** | `string` | Name for the new database. |
    | **LastFileNameToRestore** <br> (Optional -<br> omit for default) | `string` | [Last incremental backup file](../../../../server/ongoing-tasks/backup-overview#restoration-procedure) to restore. <br> **Default behavior: Restore all backup files in the folder.** |
    | **DataDirectory** <br> (Optional -<br> omit for default) | `string` | The new database data directory. <br> **Default folder: Under the "Databases" folder, <br> in a folder that carries the restored database's name.** |
    | **EncryptionKey** <br> (Optional -<br> omit for default) | `string` | A key for an encrypted database. <br> **Default behavior: Try to restore as if DB is unencrypted.**|
    | **DisableOngoingTasks** <br> (Optional -<br> omit for default) | `boolean` | `true` - disable ongoing tasks when Restore is complete. <br> `false` - enable ongoing tasks when Restore is complete. <br> **Default: `false` (Ongoing tasks will run when Restore is complete).**|
    | **SkipIndexes** <br> (Optional -<br> omit for default) | `boolean` | `true` to disable indexes import, <br> `false` to enable indexes import. <br> **Default: `false` restore all indexes.**|
    | **ShardRestoreSettings** | `ShardedRestoreSettings` | a dictionary of `SingleShardRestoreSetting` instances defining <br> paths to backup locations <br> {CODE restore_ShardedRestoreSettings@Sharding\Server\OngoingTasks\BackupAndRestore.cs /} |
    | **BackupEncryptionSettings** | `BackupEncryptionSettings` | [Backup Encryption Settings](../../../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode--key) |
         
    {NOTE: }
    Verify that RavenDB has full access to the backup locations and database files.  
    Make sure your server has write permission to `DataDirectory`.  
    {NOTE/}

## Run `RestoreBackupOperation` with the Restore Configuration
Pass the configuration object you defined to the `RestoreBackupOperation` store operation 
to restore the database.  
{CODE restore_RestoreBackupOperation@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

* Instead of `RestoreBackupConfigurationBase`, use the configuration object 
  you prepared: `RestoreBackupConfiguration` for locally-stored backups, 
  `RestoreFromS3Configuration` to restore from S3, or `RestoreFromAzureConfiguration` 
  to restore from Azure.  

## Examples

Here are examples for restoring a sharded database using 
backups stored locally, on S3 buckets, or on Azure blobs.  

{CODE-TABS}
{CODE-TAB:csharp:Local_Storage restore_local-settings@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}
{CODE-TAB:csharp:S3_Bucket restore_s3-settings@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}
{CODE-TAB:csharp:Azure_Blob restore_azure-settings@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Import}

`.ravendbdump` files, as well as full and incremental backups, 
can be imported into a sharded database using 
[studio](../../../../studio/database/tasks/import-data/import-data-file) 
or [smuggler](../../../../client-api/smuggler/what-is-smuggler#import).  

This is helpful, for example, when we wan to create a new database 
out of a single shard, or to restore only a part of a database.  
  
{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../../client-api/operations/server-wide/create-database)  
[Smuggler Import](../../../../client-api/smuggler/what-is-smuggler#import)  
[RestoreBackupOperation](../../../../client-api/operations/maintenance/backup/restore#restoring-a-database:-configuration-and-execution)  

**Server**  
[External Replication](../../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../../../../studio/database/tasks/export-database)  
[Import](../../../../studio/database/tasks/import-data/import-data-file)  
