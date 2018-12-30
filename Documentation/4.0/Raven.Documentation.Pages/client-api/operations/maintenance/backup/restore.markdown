# Restore
---

{NOTE: }

Being able to restore data is, naturally, an inseparable part of backing it up.  
You can restore backed up databases using the Studio, or client API methods.  

* In this page:  
  * [Restoring a Database](../../../../client-api/operations/maintenance/backup/restore#restoring-a-database)  
     * [Restoration Procedure](../../../../client-api/operations/maintenance/backup/restore#restoration-procedure)  
     * [Typical Backup Folder](../../../../client-api/operations/maintenance/backup/restore#typical-backup-folder)  
     * [RestoreBackupConfiguration](../../../../client-api/operations/maintenance/backup/restore#restorebackupconfiguration)  
  * [Restore Database to a single node](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node)  
  * [Restore Database to multiple nodes](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes)  
  * [Restore Database to multiple nodes AT ONCE](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes-at-once)  
  * [Restore data from a precise point in time](../../../../client-api/operations/maintenance/backup/restore#restore-data-from-a-precise-point-in-time)  
  * [Caution](../../../../client-api/operations/maintenance/backup/restore#caution)  

{NOTE/}

---

{PANEL: Restoring a database}

####Restoration procedure  

* In order to restore a database, RavenDB -
   * Browses the folder in which backup files are kept.  
      You need to provide only the containing folder's path.  
   * Restores the **full backup** it finds in this folder.  
      A backup folder contains a single Full backup, and the incremental backups that supplement it.  
   * Restores the **incremental backups** one by one.  
      You can set RavenDB to stop incremental backup restorations, when it reaches a certain file.  

####Typical Backup Folder  

* A typical backup folder may contain files like
   * 2018-12-26-09-00.ravendb-full-backup
   * 2018-12-26-12-00.ravendb-incremental-backup
   * 2018-12-26-15-00.ravendb-incremental-backup
   * 2018-12-26-18-00.ravendb-incremental-backup


####RestoreBackupConfiguration  

* Use `RestoreBackupConfiguration` to initiate restoration.  
  * **Syntax**
{CODE:csharp restore_1@ClientApi\Operations\Server\Restore.cs /}
{CODE:csharp restore_2@ClientApi\Operations\Server\Restore.cs /}

  * **Parameters**

        | Parameter | Value | Functionality |
        | ------------- | ------------- | ----- |
        | **DatabaseName** | string | Name for the restored database |
        | **DataDirectory** | string | Database data directory <br> Optional (use default directory by not setting DataDirectory) |
        | **BackupLocation** | string | Backup files local path |
        | **LastFileNameToRestore** | string | Last incremental backup file to restore from |
        | **EncryptionKey** | string | A key for an encrypted database |
        | **DisableOngoingTasks** | boolean | ongoing tasks will be disabled when backup is restored  |
   

{PANEL/}

{PANEL: Restore database to a single node}

*  **Configuration**  
  * Provide a name for the new database.  
     * Set `DatabaseName` with the new name.  
  * Choose the destination folder (Optional).  
     * Set `DataDirectory` to the new database folder.  
     * If you don't set **DataDirectory** RavenDB will use its default folder.  
        * The default folder is named "Databases", under the RavenDB directory.  
  * Provide the backup files' folder.  
    Backup source path has to be local for the restoration to continue.  
     * Set `BackupLocation` with the local path.  
  * You can make the restoration stop with a chosen incremental-backup file.  
     * Set `LastFileNameToRestore` to the name of the last incremental-backup file you want to restore.  
  * If the backup is encrypted, provide a key to decrypt it.  
     * Set `EncryptionKey` to provide the key.  
        * A Snapshot of an encrypted database, is encrypted as well.  
        * Starting with RavenDB version 4.2, you can encrypt a standard backup as well.  
  * Disable ongoing tasks (Optional)  
     * Set `DisableOngoingTasks` to **true** to disable ongoing tasks.  
       Default settings is **false**.  
       There are circumstances in which disabling ongoing tasks after restoration is advisable, e.g. when backup has been created on a testing machine and the database is restored to a production environment.  
*  **Restore**  
     * Execute `RestoreBackupOperation` with the chosen configuration.  

* Code Sample:  
     {CODE backup_restore@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

####Restore incremental backups  
In order to restore incremental backups,. RavenDB restores the full backup they supplement. You can restore incremental backups, only after restoring You can restore an incremental backup only after restoring the full backup it supplements.  

{PANEL/}

{PANEL: Restore Database to multiple nodes}

* [Restore the backup to a single node](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node)    
* Modify the database group topology, to spread the database to the other nodes.  

####Restore Database to multiple nodes AT ONCE  

* This procedure is advisable only when restoring a Snapshot.
  Simultaneously restoring a regular backup by multiple nodes, triggers the nodes to send a large amount of change-vector updates to each other.  
  When restoring this way from a Snapshot on the other hand, the databases are considered identical and change-vectors are not sent.  
* Simultaneous restoration procedure:
   * On the first node, restore the database using its original name.  
   * On other nodes, restore the DB using different names.  
   * Wait for the restoration to complete on all nodes.  
   * **Soft** delete the additional databases (with the different names) on all nodes.  
     This will remove the databases from the cluster, but retain the data files on disk.  
   * Rename the database folder on all nodes to the original database name.  
   * Expand the database group to all the other relevant nodes.  

* Restore Cluster

{PANEL/}

{PANEL: Restore data from a precise point in time}

* It is sometimes [desirable](../../../../client-api/operations/maintenance/backup/backup#point-in-time-backup) to return the data to its state in a precise point in time.  
  You can maintain precise restoration points, by maintaining and backing up [Revisions](../../../../server/extensions/revisions).  
  To restore a certain revision -  
   * Restore the database  
   * Re-create the cluster, if needed  
   * Restore the relevant revision.  

{PANEL/}

{PANEL: Caution}
{WARNING: }

When you create a backup of a database on one machine and restore it to another, you may be interested more in the database itself than in behaviors accompanying it like its ongoing tasks.  
E.g., an ETL ongoing task that suited a testing machine, may not be suitable for a production environment.  
In such cases, disable ongoing tasks using the .  
{WARNING/}
{PANEL/}

## Related Articles

[Backup using code](../../../../client-api/operations/maintenance/backup/backup)  
[Backup using the Studio](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Restore using the Studio](../../../../studio/server/databases/create-new-database/from-backup)  
