# Restore
---

{NOTE: }

* A backed-up database can be restored to a new database, either by client API methods or through the Studio.  

* In this page:  
  * [Restoring a Database: Configuration and Execution](../../../../client-api/operations/maintenance/backup/restore#restoring-a-database:-configuration-and-execution)  
  * [Restore Database to a Single Node](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node)  
  * [Restore Database to Multiple Nodes](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes)  
     * [Restore to a Single Node & Replicate to Other Nodes](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node--replicate-it-to-other-nodes)  
     * [Restore to Multiple Nodes Simultaneously](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes-simultaneously)  
  * [Recommended Precautions](../../../../client-api/operations/maintenance/backup/restore#recommended-precautions)  

{NOTE/}

---

{PANEL: Restoring a Database: Configuration and Execution}

To restore your database, configure a `RestoreBackupConfiguration` instance and pass it to `RestoreBackupOperation` for execution.  

---

#### `RestoreBackupOperation`
{CODE restore_restorebackupoperation@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

---

#### `RestoreBackupConfiguration`
{CODE restore_restorebackupconfiguration@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

* Parameters:

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **DatabaseName** | string | Name for the new database. |
    | **BackupLocation** | string | Local path of the backup file to be restored. <br> Path **must be local** for the restoration to continue.|
    | **LastFileNameToRestore** <br> (Optional -<br> omit for default) | string | [Last incremental backup file](../../../../server/ongoing-tasks/backup-overview#restoration-procedure) to restore. <br> **Default behavior: Restore all backup files in the folder.** |
    | **DataDirectory** <br> (Optional -<br> omit for default) | string | The new database data directory. <br> **Default folder: Under the "Databases" folder, in a folder that carries the restored database's name.** |
    | **EncryptionKey** <br> (Optional -<br> omit for default) | string | A key for an encrypted database. <br> **Default behavior: Try to restore as if DB is unencrypted.**|
    | **DisableOngoingTasks** <br> (Optional -<br> omit for default) | boolean | `true` - disable ongoing tasks when Restore is complete. <br> `false` - enable ongoing tasks when Restore is complete. <br> **Default: `false` (Ongoing tasks will run when Restore is complete).**|
    | **SkipIndexes** <br> (Optional -<br> omit for default) | boolean | `true` to disable indexes import, <br> `false` to enable indexes import. <br> **Default: `false` restore all indexes.**|
  
    {WARNING: }
    * Verify that RavenDB has full access to the backup-files and database folders.
    * Make sure your server has permissions to read from `BackupLocation` and write to `DataDirectory`.
    {WARNING/}

{PANEL/}

{PANEL: Restore Database to a Single Node}

*  **Configuration**  
     * Set `DatabaseName` with the **new database name**.  
     * Set `BackupLocation` with a **local path for the backup files**.  

*  **Execution**  
     * Pass the configured `RestoreBackupConfiguration` to `RestoreBackupOperation`.  
     * Send the restore-backup operation to the server to start the restoration execution.  

* **Code Sample**:  
     {CODE restore_to_single_node@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

---

####Optional Settings:

* `LastFileNameToRestore`  
   Restore incremental backup files until a certain file is reached, and stop there.  
   For example - 
   * These are the files in your backup folder:  
     2018-12-26-09-00.ravendb-full-backup  
     2018-12-26-12-00.ravendb-incremental-backup  
     2018-12-26-15-00.ravendb-incremental-backup  
     2018-12-26-18-00.ravendb-incremental-backup  
   * Feed **LastFileNameToRestore** with the 2018-12-26-12-00 incremental-backup file name:
     {CODE restore_last_file_name_to_restore@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  
   * The full-backup and 12:00 incremental-backup files **will** be restored.  
     The 15:00 and 18:00 files will **not** be restored.  

* `DataDirectory`  
   Specify the directory into which the database will be restored.
   {CODE restore_to_specific__data_directory@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

* `EncryptionKey`  
   This is where you need to provide your encryption key if your backup is encrypted.  
   {CODE-BLOCK:json}
   restoreConfiguration.EncryptionKey = "your_encryption_key";  
   {CODE-BLOCK/}

* `DisableOngoingTasks`  
   set **DisableOngoingTasks** to **true** to disable the execution of ongoing tasks after restoration.  
   See [Recommended Precautions](../../../../client-api/operations/maintenance/backup/restore#recommended-precautions).
   
   {CODE restore_disable_ongoing_tasks_true@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{PANEL/}

{PANEL: Restore Database to Multiple Nodes}

####Restore Database to a Single Node & Replicate it to Other Nodes  

The common approach to restoring a database that should reside on multiple nodes, is to restore the backed-up 
database to a single server and then expand the database group to additional nodes, allowing normal replication.  

* Verify relevant nodes exist in your cluster. [Add nodes](../../../../server/clustering/cluster-api#add-node-to-the-cluster) as needed.
* Manage the database-group topology.  
  Add a node to the database-group using the [Studio](../../../../studio/database/settings/manage-database-group) 
  or from your [code](../../../../client-api/operations/server-wide/add-database-node), to replicate the database to the other nodes.

---

####Restore Database to Multiple Nodes Simultaneously  

You can create the cluster in advance, and restore the database to multiple nodes simultaneously.  

{NOTE: This procedure is advisable only when restoring a Snapshot.}

* When a [logical-backup](../../../../client-api/operations/maintenance/backup/backup#logical-backup) 
  is restored, each document receives a new change-vector according to the node it resides on.  
  When the database instances synchronize, this change-vector will be updated and be composed of all database nodes tags.  

* When a [snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot) is restored, 
  documents are **not** assigned a new change-vector because the databases kept by all nodes are considered identical.  
  Each document retains the original change-vector it had during backup.  
  When the database instances synchronize, documents' change-vectors do **not** change.  

{NOTE/}

* On the first node, restore the database using its original name.  
* On other nodes, restore the database using different names.  
* Wait for the restoration to complete on all nodes.  
* **Soft-delete** the additional databases (those with altered names) from the cluster.  
  [Soft-delete](../../../../client-api/operations/server-wide/delete-database#operations--server--how-to-delete-a-database) 
  the databases by setting `HardDelete` to `false`, to retain the data files on disk.  
* Rename the database folder on all nodes to the original database name.  
* [Expand](../../../../server/clustering/rachis/cluster-topology#modifying-the-topology) the database group to all relevant nodes.  

{PANEL/}

{PANEL: Recommended Precautions}
{WARNING: }

When restoring a backed-up database, you may be interested only in the restored data 
and not in any ongoing tasks that may have existed during backup.

* E.g., an ETL ongoing task from a production cluster may have unwanted results in a testing environment.  

In such cases, **disable** ongoing tasks using the [DisableOngoingTasks](../../../../client-api/operations/maintenance/backup/restore#configuration-and-execution) flag.  

* Code Sample:  
  {CODE restore_disable_ongoing_tasks_true@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{WARNING/}
{PANEL/}

## Related Articles  
###Server  
- [Backup Overview](../../../../server/ongoing-tasks/backup-overview)

###Client API  
- [Backup](../../../../client-api/operations/maintenance/backup/backup)  
- [Encrypted-Backup : Create & Restore](../../../../client-api/operations/maintenance/backup/encrypted-backup)  
- [Backup FAQ](../../../../client-api/operations/maintenance/backup/faq)  
- [What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)  

###Studio  
- [The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
- [Create Database: from Backup](../../../../studio/server/databases/create-new-database/from-backup)  
- [Create a Database: General Flow](../../../../studio/server/databases/create-new-database/general-flow)  
- [Create a Database: Encrypted](../../../../studio/server/databases/create-new-database/encrypted)  

###Security  
- [Database Encryption](../../../../server/security/encryption/database-encryption)  
- [Security Overview](../../../../server/security/overview)  
- [Authentication and Certification](../../../../server/security/authentication/certificate-configuration)  

###Migration  
- [Migration](../../../../migration/server/data-migration)   
