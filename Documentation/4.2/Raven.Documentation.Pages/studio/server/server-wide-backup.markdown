#Server-Wide Backup
---

{NOTE: }

* RavenDB regards backup not a one-time operation but an ongoing requirement, and lets you 
  fulfill it almost effortlessly by scheduling ongoing backup tasks that run in the background. 
  For instance, you can invest a few minutes in scheduling a daily full backup task and an hourly 
  incremental backup task, that will from then on require none of your attention.  
  
* You can create [per-database](../../studio/database/tasks/ongoing-tasks/backup-task#backup-task) 
  backup tasks, or minimize your efforts even further and schedule **server-wide** tasks that backup 
  all your databases at once.  
  
* Server-wide backups are similar to their [per-database equivalents](../../studio/database/tasks/ongoing-tasks/backup-task#backup-task): 
  You can create [full](../../client-api/operations/maintenance/backup/backup#full-backup) 
  and [incremental](../../client-api/operations/maintenance/backup/backup#incremental-backup) backup tasks 
  in [logical backup ](../../client-api/operations/maintenance/backup/backup#logical-backup) or 
  [Snapshot](../../client-api/operations/maintenance/backup/backup#snapshot) format, and store your 
  files locally or in a variety of remote locations.  

* In this page:  
  * [Scheduling a server-wide backup task](../../studio/server/server-wide-backup#scheduling-a-server-wide-backup-task)  
  * [Restoring a server-wide backup](../../studio/server/server-wide-backup#restoring-a-server-wide-backup)  

{NOTE/}

---

{PANEL: Scheduling a server-wide backup task}

**To schedule a server-wide backup task:**  
  
Open the menu option **Manage Server**.  
![Figure 1. Server-Wide Backup](images/server-wide-backup_01-manage-server.png)  

---

**Add a task**.  
![Figure 2. Add Task](images/server-wide-backup_02-new-task.png)  

*  
  **A.** Click the **Server-Wide Backup** button  
  **B.** Click the **Add Server-Wide Backup Task** button to add a new task  

---

**Configure the new backup task**.  
![Figure 3. Task Configuration](images/server-wide-backup_03-task-configuration.png)  

* **A.** **Task Name**: this task's name in the server-wide tasks list.  
   ![Figure 4. Task Name](images/server-wide-backup_04-task-name.png)  
  
* **B.** **Backup type**: either "backup" for a [logical backup](../../client-api/operations/maintenance/backup/backup#logical-backup) 
  or "snapshot" for a [snapshot image](../../client-api/operations/maintenance/backup/backup#snapshot).  
  ![Figure 5. Schedule a backup](images/server-wide-backup_05-backup-type.png)  
  
* **C.** **Schedule**: when and how often would full and/or incremental backups be executed.  
  Be aware that scheduling only an incremental backup would still create one full backup, 
  that the incremental backups would then keep updating.  
  ![Figure 6. Schedule a backup](images/server-wide-backup_06-schedule.png)  
 
* **D.** **Retention Policy**: whether and by what age should backup files be deleted.  
  ![Figure 7. Retention Policy](images/server-wide-backup_07-retention-policy.png)  
  
* **E.** **Encryption**: choose whether to [encrypt your backup files](../../client-api/operations/maintenance/backup/encrypted-backup#backup-encryption).  
  If your [database is encrypted](../../studio/server/databases/create-new-database/encrypted#create-a-database-encrypted), you backup will be encrypted using the same key.  
  If your database is **not encrypted**, you can encrypt your backup using the key provided here or a valid key you got from another source.  
  {NOTE: }
  Please be very careful not to lose your encryption key.  
  We do **not** keep copies of it, and losing it means you will no longer have access to your backups' contents.  
  {NOTE/}
  ![Figure 8. Encryption](images/server-wide-backup_08-encryption.png)  
  
* **F.** **Destination**: you can store your backups locally and/or remotely.  
  Both local and remote (e.g. S3 bucket) backups keep your databases in separate child folders under a common root folder.  
  ![Figure 9. Destination](images/server-wide-backup_09-destination-local.png)  
  ![Figure 10. Destination](images/server-wide-backup_10-destination-folders.png)  
{PANEL/}

{PANEL: Restoring a server-wide backup}

There is no difference between restoring databases backed up using the [server-wide procedure](../../studio/server/server-wide-backup#scheduling-a-server-wide-backup-task) 
and ones [backed up separately](../../studio/database/tasks/ongoing-tasks/backup-task#backup-task).  
In either case, you can restore them by [Creating a New Database from Backup](../../studio/server/databases/create-new-database/from-backup#create-a-database-from-backup).  

* There is no way to restore several databases at once.  
* Backups created by the server-wide procedure are kept in separate child folders under a common root folder.  
  When you provide the backup file link, provide that of the specific database you want to restore.  


{PANEL/}
## Related articles
[Per-Database Backup Ongoing Task](../../studio/database/tasks/ongoing-tasks/backup-task#backup-task)  
[Backup Encryption](../../client-api/operations/maintenance/backup/encrypted-backup#backup-encryption)  
[Database Encryption](../../studio/server/databases/create-new-database/encrypted#create-a-database-encrypted)  
