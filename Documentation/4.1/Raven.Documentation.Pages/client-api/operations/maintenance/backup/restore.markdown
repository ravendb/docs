# Restore
---

{NOTE: }

Being able to restore data is, naturally, an inseparable part of backing it up.  
You can restore backed up databases using the Studio, or client API methods.  

* In this page:  
  * [Restoring a Database](../../../../client-api/operations/maintenance/backup/restore#restoring-a-database)  
     * [Configuration and Execution](../../../../client-api/operations/maintenance/backup/restore#configuration-and-execution)  
     * [Restore Database to a Single Node](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node)  
         * [Optional Settings](../../../../client-api/operations/maintenance/backup/restore#optional-settings)  
     * [Restore Database to Multiple Nodes](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes)  
         * [Restore Database to a Single Node & Replicate it to Other Nodes](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node--replicate-it-to-other-nodes)  
         * [Restore Database to Multiple Nodes Simultaneously](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes-simultaneously)  
  * [Recommended Cautions](../../../../client-api/operations/maintenance/backup/restore#recommended-cautions)  

{NOTE/}

---

{PANEL: Restoring a Database}

####Configuration and Execution  

* To restore your database, configure a `RestoreBackupConfiguration` instance and pass it to `RestoreBackupOperation` for execution.  

* `RestoreBackupOperation`
  {CODE restore_restorebackupoperation@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

* `RestoreBackupConfiguration`
  {CODE restore_restorebackupconfiguration@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  
   * Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **DatabaseName** | string | Name for the new database. |
    | **BackupLocation** | string | Backup file local path. <br> Backup source path **has to be local** for the restoration to continue.|
    | **LastFileNameToRestore** <br> (Optional -<br> omit for default) | string | Last backup file to restore. <br> **Default behavior: Restore all backup files in the folder.** |
    | **DataDirectory** <br> (Optional -<br> omit for default) | string | Database data directory. <br> **Default folder: Under the "Databases" folder, in a folder that carries the restored database's name.** |
    | **EncryptionKey** <br> (Optional -<br> omit for default) | string | A key for an encrypted database. <br> **Default behavior: Try to restore as if DB is unencrypted.**|
    | **DisableOngoingTasks** <br> (Optional -<br> omit for default) | boolean | `true` to disable ongoing tasks after restoring, <br> `false` to enable tasks after restoring. <br> **Default: `false` (tasks DO run when backup is restored)**|
    | **SkipIndexes** <br> (Optional -<br> omit for default) | boolean | `true` to disable indexes import, <br> `false` to enable indexes import. <br> **Default: `false` restore all indexes.**|
  
{NOTE: Make sure your server has permissions to read from `BackupLocation` and write to `DataDirectory`.}
Verify that RavenDB has full access to the backup-files and database folders.
{NOTE/}

{PANEL/}

{PANEL: Restore Database to a Single Node}

*  **Configuration**  
     * Set `DatabaseName` with the **new database name**.  
     * Set `BackupLocation` with a **local path for the backup files**.  

*  **Execution**  
     * Pass `RestoreBackupOperation` the configured `RestoreBackupConfiguration`.  
     * Restore tha database by sending the task to the server.  

* Code Sample:  
     {CODE restore_to_single_node@ClientApi\Operations\Maintenance\Backup\Backup.cs /}


{NOTE: }
####Optional Settings:

* `LastFileNameToRestore`
    * Use **LastFileNameToRestore** if you want to restore backup-files until a certain file is reached and stop there.  
      For example - 
       * These are the files in your backup folder:  
          2018-12-26-09-00.ravendb-full-backup  
          2018-12-26-12-00.ravendb-incremental-backup  
          2018-12-26-15-00.ravendb-incremental-backup  
          2018-12-26-18-00.ravendb-incremental-backup  
       * Feed **LastFileNameToRestore** with the 2018-12-26-12-00 incremental-backup file name:
          {CODE restore_last_file_name_to_restore@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  
       * The full-backup and 12:00 incremental-backup files **will** be restored.  
         The 15:00 and 18:00 files will be **omitted**.  

* `DataDirectory`  
   Choose the location and name of the directory the database will be restored to.  
   {CODE restore_to_specific__data_directory@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

* `EncryptionKey`  
   This is where you need to provide your encryption key if your backup is encrypted.  

* `DisableOngoingTasks`  
   set **DisableOngoingTasks** to **true** to [disable ongoing tasks](../../../../client-api/operations/maintenance/backup/restore#recommended-cautions) after restoration.  
   {CODE restore_disable_ongoing_tasks_true@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
{NOTE/}

{PANEL/}

{PANEL: Restore Database to Multiple Nodes}

####Restore Database to a Single Node & Replicate it to Other Nodes  

The common approach to restoring a cluster is to restore the backed-up database to a single server, and then expand the database group to additional nodes, allowing normal replication.  

* [Restore the backup to a single node](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node).  
* Modify the database group [topology](../../../../server/clustering/rachis/cluster-topology#modifying-the-topology) using [the Studio](../../../../studio/server/cluster/cluster-view#cluster-view-operations) or [code](../../../../server/clustering/cluster-api#cluster--cluster-api) to spread the database to the other nodes.  

---

####Restore Database to Multiple Nodes Simultaneously  

You can create the cluster in advance, and restore the database to multiple nodes simultaneously.  

{NOTE: This procedure is advisable only when restoring a Snapshot.}

* Simultaneously restoring a [logical-backup](../../../../client-api/operations/maintenance/backup/backup#logical-backup-or-simply-backup) by multiple nodes, triggers each node to send change-vector updates to all other nodes.  
* Simultaneously restoring [a snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot) does **not** initiate this behavior, because the databases kept by all nodes are considered identical.  

{NOTE/}

* On the first node, restore the database using its original name.  
* On other nodes, restore the database using different names.  
* Wait for the restoration to complete on all nodes.  
* **Soft-delete** the additional databases (those with altered names).  
   * [Delete](../../../../client-api/operations/server-wide/delete-database#operations--server--how-to-delete-a-database) the databases from the cluster with `HardDelete` set to `false`, to retain the data files on disk.  
* Rename the database folder on all nodes to the original database name.  
* [Expand](../../../../server/clustering/rachis/cluster-topology#modifying-the-topology) the database group to all relevant nodes.  

{PANEL/}

{PANEL: Recommended Cautions}
{WARNING: }

When you create a backup of a database on one machine and restore it to another, you may be interested more in the database itself than in behaviors accompanying it like its ongoing tasks.  

* E.g., an ETL ongoing task from a production cluster may have unwanted results in a testing environment.  

In such cases, **disable** ongoing tasks using the [DisableOngoingTasks](../../../../client-api/operations/maintenance/backup/restore#configuration-and-execution) flag.  

* Code Sample:  
  {CODE restore_disable_ongoing_tasks_true@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{WARNING/}
{PANEL/}

## Related Articles

**Studio Articles**:   
[Create a Database : From Backup](../../../../studio/server/databases/create-new-database/from-backup)   
[Create a Database : General Flow](../../../../studio/server/databases/create-new-database/general-flow)        
[Create a Database : Encrypted](../../../../studio/server/databases/create-new-database/encrypted)      
[The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task)    

**Client Articles**:  
[Operations: How to Restore a Database from Backup](../../../../client-api/operations/server-wide/restore-backup)    
[What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)   
[Backup](../../../../client-api/operations/maintenance/backup/backup)   
[Encrypted-Backup backup & restore](../../../../client-api/operations/maintenance/backup/encrypted-backup)   
**Server Articles**:  
[Backup Overview](../../../../server/ongoing-tasks/backup-overview)

**Migration Articles**:  
[Migration](../../../../migration/server/data-migration) 
