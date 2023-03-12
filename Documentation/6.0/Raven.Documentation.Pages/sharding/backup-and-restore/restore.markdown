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
   [original order](../../sharding/backup-and-restore/restore#restore).  
   The backup of shard 1, for example, must be restored as shard 1.
   {WARNING/}
* Backup files can be restored from local shard node storage or from 
  a [remote location](../../sharding/backup-and-restore/backup#backup-storage-local-and-remote).  
* A backed-up sharded database can be restored in part or in full, 
  to a sharded or a non-sharded database.  
* Only [logical](../../server/ongoing-tasks/backup-overview#logical-backup) 
  backups are supported. 
  [Snapshot](../../server/ongoing-tasks/backup-overview#snapshot) 
  backups cannot be created or restored for sharded databases.  
* `.ravendbdump` files (exported from RavenDB databases) and 
  backup files can also be 
  [imported](../../sharding/import-and-export#import) 
  into a database (sharded or non-sharded).  

* In this page:  
  * [Restore](../../sharding/backup-and-restore/restore#restore)  
     * [Set Paths to Backup Files Locations](../../sharding/backup-and-restore/restore#set-paths-to-backup-files-locations)  
     * [Define a Restore Configuration](../../sharding/backup-and-restore/restore#define-a-restore-configuration)  
         * [`RestoreBackupConfigurationBase`](../../sharding/backup-and-restore/restore#section)
     * [Run `RestoreBackupOperation` with the Restore Configuration](../../sharding/backup-and-restore/restore#run--with-the-restore-configuration)  
     * [Examples](../../sharding/backup-and-restore/restore#examples)  

{NOTE/}

---

{PANEL: Restore}

To restore a sharded database, we need to:  

## Set Paths to Backup Files Locations
When a shard stores a backup file, it may store it locally (on the 
shard node's storage) or remotely (supported platforms currently include 
S3 Buckets, Azure Blobs, and Google cloud).  
  
To restore the backup files, we need to provide the restore process with 
each shard's backup folder location.  
The shards' backup folder locations, and additional data regarding the backups, 
are provided in a dictionary of `SingleShardRestoreSetting` objects.  

`SingleShardRestoreSetting`  
{CODE restore_SingleShardRestoreSetting@Sharding\ShardingBackupAndRestore.cs /}

* Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **ShardNumber** | `int` | The shard number that will be given to the restored shard. <br> should normally be similar to the original shard number. |
    | **NodeTag** | `string` | The node to restore the shard on. |
    | **FolderName** | `string` | The name of the folder that holds the backup file/s. |

    {WARNING: }
    When setting **ShardNumber**, please make sure that all shards are 
    given the same numbers they had when they were backed-up.  
    Giving a restored shard a number different than its original number 
    will place buckets on the wrong shards and cause mapping errors.  

    E.g., a backup of shard 1 must be restored as shard 1: `ShardNumber = 1`  
    {WARNING/}

    {WARNING: }
    When restoring a local shard backup, make sure that the backup file 
    resides on the node that the shard's `NodeTag` property is set to, 
    so the restore process can find the file.  

    E.g., if a backup file that's been produced by node `A` is now 
    restored to node `B` (`NodeTag = "B"`), place the backup file in 
    the backup folder of node `B` before initiating the restore operation.  
    {WARNING/}

## Define a Restore Configuration
To restore the database, we pass the 
[Restore Operation](../../client-api/operations/maintenance/backup/restore#restoring-a-database:-configuration-and-execution) 
a **configuration object**.  

* The configuration object inherits properties from the `RestoreBackupConfigurationBase` class 
  ([discussed below](../../sharding/backup-and-restore/restore#section)) 
  and defines additional sharding-specific settings.  

* We choose what configuration object to pass the restore operation, by 
  the backup files' location.  
  The backup files may be located locally (on each shard machine storage) 
  or remotely (in a cloud location).  
  
    | Configuration Object | Backup Location | Additional Properties |
    | -------------------- | --------------- | --------------------- |
    | `RestoreBackupConfiguration` | Local shard storage | None (see [example](../../sharding/backup-and-restore/restore#examples)) |
    | `RestoreFromS3Configuration` | AWS S3 Bucket | `S3Settings` (see S3 [example](../../sharding/backup-and-restore/restore#examples)) |
    | `RestoreFromAzureConfiguration` | MS Azure Blob | `AzureSettings` (see Azure [example](../../sharding/backup-and-restore/restore#examples)) |
    | `RestoreFromGoogleCloudConfiguration` | Google Cloud Bucket | `GoogleCloudSettings` (see Google Cloud [example](../../sharding/backup-and-restore/restore#examples)) |

---

### `RestoreBackupConfigurationBase`
`RestoreBackupConfigurationBase` is a parent class to all the configuration types 
mentioned above, allowing you to set backups **encryption settings** among other options.  
{CODE restore_RestoreBackupConfigurationBase@Sharding\ShardingBackupAndRestore.cs /}

* Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **DatabaseName** | `string` | Name for the new database. |
    | **LastFileNameToRestore** <br> (Optional -<br> omit for default) | `string` | [Last incremental backup file](../../server/ongoing-tasks/backup-overview#restoration-procedure) to restore. <br> **Default behavior: Restore all backup files in the folder.** |
    | **DataDirectory** <br> (Optional -<br> omit for default) | `string` | The new database data directory. <br> **Default folder: Under the "Databases" folder, <br> in a folder that carries the restored database's name.** |
    | **EncryptionKey** <br> (Optional -<br> omit for default) | `string` | A key for an encrypted database. <br> **Default behavior: Try to restore as if DB is unencrypted.**|
    | **DisableOngoingTasks** <br> (Optional -<br> omit for default) | `boolean` | `true` - disable ongoing tasks when Restore is complete. <br> `false` - enable ongoing tasks when Restore is complete. <br> **Default: `false` (Ongoing tasks will run when Restore is complete).**|
    | **SkipIndexes** <br> (Optional -<br> omit for default) | `boolean` | `true` to disable indexes import, <br> `false` to enable indexes import. <br> **Default: `false` restore all indexes.**|
    | **ShardRestoreSettings** | `ShardedRestoreSettings` | a dictionary of `SingleShardRestoreSetting` instances defining <br> paths to backup locations <br> {CODE restore_ShardedRestoreSettings@Sharding\ShardingBackupAndRestore.cs /} |
    | **BackupEncryptionSettings** | `BackupEncryptionSettings` | [Backup Encryption Settings](../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode--key) |
         
    {NOTE: }
    Verify that RavenDB has full access to the backup locations and database files.  
    Make sure your server has write permission to `DataDirectory`.  
    {NOTE/}

## Run `RestoreBackupOperation` with the Restore Configuration
Pass the configuration object you defined to the `RestoreBackupOperation` store operation 
to restore the database.  
{CODE restore_RestoreBackupOperation@Sharding\ShardingBackupAndRestore.cs /}

* Instead of `RestoreBackupConfigurationBase`, use the configuration object 
  you [prepared](../../sharding/backup-and-restore/restore#define-a-restore-configuration):  
   * `RestoreBackupConfiguration` for locally-stored backups  
   * `RestoreFromS3Configuration` to restore from S3  
   * `RestoreFromAzureConfiguration` to restore from Azure  
   * `RestoreFromGoogleCloudConfiguration` to restore from Google Cloud  

## Examples

Here are examples for restoring a sharded database using 
backup files stored locally and remotely.  

{CODE-TABS}
{CODE-TAB:csharp:Local_Storage restore_local-settings@Sharding\ShardingBackupAndRestore.cs /}
{CODE-TAB:csharp:S3_Bucket restore_s3-settings@Sharding\ShardingBackupAndRestore.cs /}
{CODE-TAB:csharp:Azure_Blob restore_azure-settings@Sharding\ShardingBackupAndRestore.cs /}
{CODE-TAB:csharp:Google_Cloud restore_google-cloud-settings@Sharding\ShardingBackupAndRestore.cs /}
{CODE-TABS/}

{PANEL/}

## Related articles

**Client API**  
[Create Database](../../client-api/operations/server-wide/create-database)  
[Smuggler Import](../../client-api/smuggler/what-is-smuggler#import)  
[RestoreBackupOperation](../../client-api/operations/maintenance/backup/restore#restoring-a-database:-configuration-and-execution)  

**Server**  
[External Replication](../../server/ongoing-tasks/external-replication)  
[Logical Backup](../../server/ongoing-tasks/backup-overview#logical-backup)  
[Snapshot Backup](../../server/ongoing-tasks/backup-overview#snapshot)  

**Studio**  
[Export Data](../../studio/database/tasks/export-database)  
[Import Data](../../studio/database/tasks/import-data/import-data-file)  

