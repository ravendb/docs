# Restore
---

{NOTE: }

Being able to restore data is, naturally, an inseparable part of backing it up.  
You can restore backed up databases using the Studio, or client API methods.  

* In this page:  
  * [Restoring a Database](../../../../client-api/operations/maintenance/backup/restore#restoring-a-database)  
     * [To a single node](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node)  
     * [To multiple nodes](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes)  
     * [To multiple nodes AT ONCE](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-multiple-nodes-at-once)  
  * [Recommended Cautions](../../../../client-api/operations/maintenance/backup/restore#recommended-cautions)  

{NOTE/}

---

{PANEL: Restoring a database}

####Configure restoration  

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
        | **LastFileNameToRestore** | string | Last backup file to restore from |
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
        * The default folder will bear the name of the database, inside the "Databases" directory.  
  * Provide the backup files' folder.  
    Backup source path has to be local for the restoration to continue.  
     * Set `BackupLocation` with the local path.  
  * By default, restoration will include the full backup and all the incremental files that follow it.  
    But you can also choose to end the restoration when a chosen file is reached.  
     * Set `LastFileNameToRestore` with the name of the last backup file to restore.  
       E.g. -
        * If you want to restore only the full backup and skip incremental-backup files,  
          set LastFileNameToRestore with the full-backup file name.  
        * If you want to restore incremental-backup files up to a certain file,  
          set LastFileNameToRestore with the name of the last incremental-backup file to restore.  
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

{PANEL/}

{PANEL: Restore Database to multiple nodes}

* [Restore the backup to a single node](../../../../client-api/operations/maintenance/backup/restore#restore-database-to-a-single-node)    
* [Modify the database group topology](../../../../server/clustering/rachis/cluster-topology#modifying-the-topology), to spread the database to the other nodes.  

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

{PANEL/}

{PANEL: Recommended Cautions}
{WARNING: }

When you create a backup of a database on one machine and restore it to another, you may be interested more in the database itself than in behaviors accompanying it like its ongoing tasks.  

* E.g., an ETL ongoing task from a production cluster may have unwanted results in a testing environment.  

In such cases, disable ongoing tasks using the `DisableOngoingTasks` flag.  
DisableOngoingTasks's default setting is FALSE, **allowing** tasks to run when backup is restored.  

* Code Sample:  
  {CODE backup_restore_DisableOngoingTasks@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{WARNING/}
{PANEL/}

## Related Articles  (to be revised, ignore)

####Client
[Backup using code](../../../../client-api/operations/maintenance/backup/backup)  

####Studio
[Backup using the Studio](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Restore using the Studio](../../../../studio/server/databases/create-new-database/from-backup)  
