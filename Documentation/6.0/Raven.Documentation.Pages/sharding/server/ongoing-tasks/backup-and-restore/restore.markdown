# Sharding: Restore
---

{NOTE: }

* Logical backups made for the shards of a sharded database can be 
  collected and **restored** into a sharded or a non-sharded database.  
  
* A snapshot made for a single shard can be **restored** to a new 
  **non-sharded** database.  

* `.ravendbdump` files created by exporting shard databases 
  can be **imported** into a new sharded or non-sharded database.  

* In this page:  
  * [Restore](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#restore)  
     * [Restoring a Database: Configuration and Execution](../../../../)  
     * [RestoreBackupOperation](../../../../)  
     * [RestoreBackupConfigurationBase](../../../../)  
     * [SingleShardRestoreSetting](../../../../)  
  * [Import](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#import)  

{NOTE/}

---

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
    | **DatabaseName** | `string` | Name for the new database. |
    | **LastFileNameToRestore** <br> (Optional -<br> omit for default) | `string` | [Last incremental backup file](../../../../../server/ongoing-tasks/backup-overview#restoration-procedure) to restore. <br> **Default behavior: Restore all backup files in the folder.** |
    | **DataDirectory** <br> (Optional -<br> omit for default) | `string` | The new database data directory. <br> **Default folder: Under the "Databases" folder, in a folder that carries the restored database's name.** |
    | **EncryptionKey** <br> (Optional -<br> omit for default) | `string` | A key for an encrypted database. <br> **Default behavior: Try to restore as if DB is unencrypted.**|
    | **DisableOngoingTasks** <br> (Optional -<br> omit for default) | `boolean` | `true` - disable ongoing tasks when Restore is complete. <br> `false` - enable ongoing tasks when Restore is complete. <br> **Default: `false` (Ongoing tasks will run when Restore is complete).**|
    | **SkipIndexes** <br> (Optional -<br> omit for default) | `boolean` | `true` to disable indexes import, <br> `false` to enable indexes import. <br> **Default: `false` restore all indexes.**|
    | **Type** | `RestoreType` | Restore from local or cloud storage <br> {CODE restore_RestoreType@Sharding\Server\OngoingTasks\BackupAndRestore.cs /} |
    | **ShardRestoreSettings** | `ShardedRestoreSettings` | a list of `SingleShardRestoreSetting` instances defining the shard files to restore <br> {CODE restore_ShardedRestoreSettings@Sharding\Server\OngoingTasks\BackupAndRestore.cs /} |
    | **BackupEncryptionSettings** | `BackupEncryptionSettings` | [Backup Encryption Settings](../../../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode--key) |
  
    {WARNING: }
    * Verify that RavenDB has full access to the backup-files and database folders.
    * Make sure your server has permissions to read from `BackupLocation` and write to `DataDirectory`.
    {WARNING/}

#### `SingleShardRestoreSetting`
{CODE restore_SingleShardRestoreSetting@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

{PANEL/}

{PANEL: Import}

`.ravendbdump` files [Exported](../../../../studio/database/tasks/export-database) 
from shard databases can be 
[imported](../../../../studio/database/tasks/import-data/import-data-file#import-data-from-.ravendbdump-file) 
into a new database to create a full or a partial replica of the original database.  

{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../../client-api/operations/server-wide/create-database)  

**Server**  
[External Replication](../../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../../../../studio/database/tasks/export-database)  

