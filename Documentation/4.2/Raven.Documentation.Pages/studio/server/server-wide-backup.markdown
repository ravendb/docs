#Server-Wide Backup
---

{NOTE: }

* RavenDB lets you define a backup task per-database  
_-or-_  
  a **Server-Wide Backup Task** that backs up **all** the databases in your cluster.

* When scheduling a **Server-Wide Backup Task**, RavenDB actually creates a 
  [regular ongoing backup task](../../studio/database/tasks/ongoing-tasks/backup-task) 
  for each database in the cluster, and a backup will be created for each database at the specified scheduled time.  
  The prefix '**Server Wide Backup**' is added to the name of the created ongoing backup tasks.  
  
* Server-Wide backups are similar to their per-database equivalents.  
  You can create [Full](../../client-api/operations/maintenance/backup/backup#full-backup) 
  and [Incremental](../../client-api/operations/maintenance/backup/backup#incremental-backup) backup tasks 
  in [Logical Backup ](../../client-api/operations/maintenance/backup/backup#logical-backup) or 
  [Snapshot](../../client-api/operations/maintenance/backup/backup#snapshot) format, and store your 
  files locally or in a variety of remote locations.  

* In this page:  
  * [Scheduling a Server-Wide Backup Task](../../studio/server/server-wide-backup#scheduling-a-server-wide-backup-task)  
  * [Restoring a Database From a Server-Wide Backup](../../studio/server/server-wide-backup#restoring-a-database-from-a-server-wide-backup)  
  * [Server-Wide Backup Tasks in the Database Tasks View](../../studio/server/server-wide-backup#server-wide-backup-tasks-in-the-database-tasks-view)  
  * [The Responsible Node](../../studio/server/server-wide-backup#the-responsible-node)  

{NOTE/}

---

{PANEL: Scheduling a Server-Wide Backup Task}

Click the **Manage Server** main-menu item.  

![Figure 1. Manage-Server Menu Item](images/server-wide-backup_01-manage-server.png "Figure 1. Manage-Server Menu Item")

---

**The Server-Wide Backup View**  

![Figure 2. Add Server-Wide Backup Task](images/server-wide-backup_02-new-task.png "Figure 2. Add Server-Wide Backup Task")

1. Click the **Server-Wide Backup** menu item  
2. Click the **Add Server-Wide Backup Task** button to add a new task  

---

**Configure the new Backup Task**.  

![Figure 3. Task Configuration](images/server-wide-backup_03-task-configuration.png "Figure 3. Task Configuration")

The settings are similar to those of a [regular backup task](../../studio/database/tasks/ongoing-tasks/backup-task#backup-task).  

1. **Task Name**  
   Enter the Server-Wide Backup Task name.  
  
2. **Backup type**  
   Select 'Backup' for a [logical backup](../../client-api/operations/maintenance/backup/backup#logical-backup) 
   or 'Snapshot' for a [snapshot image](../../client-api/operations/maintenance/backup/backup#snapshot).  
  
3. **Schedule**  
   Define task contents (Full and/or Incremental) and schedule execution time.  
 
4. **Retention Policy**  
   Define the minimum amount of time to keep Backups (and Snapshots) in the system.  
  
5. **Encryption**  

   ![Figure 4. Backup Encryption](images/server-wide-backup_04-encryption.png "Figure 4. Backup Encryption")

   * Backup files (both [logical backups](../../client-api/operations/maintenance/backup/backup#logical-backup) 
     and [snapshot images](../../client-api/operations/maintenance/backup/backup#snapshot)) 
     of **encrypted** databases will always be **encrypted**,  
     regardless of the settings here, using the database encryption key.  
   * Logical backups of **unencrypted** databases, will be **encrypted** using the key provided here.  
     You can use either the key suggested by RavenDB, or a valid key from any other source.  
   * Logical backups of **encrypted** databases will use the database's encryption key,  
     even if you provide a different key here.  
   * Snapshot images of **unencrypted** databases will always be **unencrypted**, regardless of the settings here,  
     since a snapshot is a faithful bitmap copy of the database.  

     {INFO: }
      **Encryption Summary**:  
      
      Database Encryption | Backup Type |  Backup Encryption and Key Used  
      ---- | ---- | ----
      Encrypted | Logical Backup | Backup is encrypted using the database key, <br> **even if you provide a different key**  
      Encrypted | Snapshot Image | Backup is encrypted using the database key  
      Not Encrypted | Logical Backup | Backup is encrypted using the key you provide  
      Not Encrypted | Snapshot Image | Backup is not encrypted, <br> **even if encryption is enabled**  
      {INFO/}

6. **Destination**  
   Backup files can be stored locally and/or remotely.  
   Backup files are created in a separate child folder per database, under a common root folder.  

   ![Figure 5. Backup Destination Folder](images/server-wide-backup_05-destination-local.png "Figure 5. Backup Destination Folder")

{PANEL/}

{PANEL: Restoring a Database From a Server-Wide Backup}

There is no difference between restoring a database from a backup file created by the 
[Server-Wide procedure](../../studio/server/server-wide-backup#scheduling-a-server-wide-backup-task) 
and a [separately created backup](../../studio/database/tasks/ongoing-tasks/backup-task#backup-task).  
In both cases, you can restore the database by 
[Creating a New Database from Backup](../../studio/server/databases/create-new-database/from-backup#create-a-database-from-backup).  

* While the Server-Wide Backup Task creates backups for all your databases at once, 
  the restore procedure can only restore a single database at a time.  
* When restoring a database from a backup file created by the Server-Wide procedure, make sure you provide the 
  link to the specific database backup file and not the common root folder that was created by the Server-Wide task.

{PANEL/}

---

{PANEL: Server-Wide Backup Tasks in the Database Tasks View}

The Server-Wide Backup Tasks created for each database, can be viewed in the database's Ongoing-Tasks view.  
You can use this view to see a task's details, or to trigger an immediate backup.  
To edit or modify a Server-Wide task, navigate back to the [Server-Wide Backup view](#scheduling-a-server-wide-backup-task).  

* Choose the database you're interested in.  

  ![Figure 6. Choose Database](images/ongoing-tasks-view_00-choose-database.png "Figure 6. Choose Database")

* Open the 'Manage Ongoing Tasks' View.  

  ![Figure 7. Manage-Ongoing-Tasks View](images/ongoing-tasks-view_01.png "Figure 7. Manage-Ongoing-Tasks View")

   1. Click to open the **Manage Ongoing Tasks** view for the selected database.  
   2. Use this shortcut to navigate directly to the Server-Wide Backup view.  
   3. This is a **regular backup task**, defined only on this database.  
   4. This is a **Server-Wide Backup Task**, with its detailed-view toggled on.  
      Though it is a Server-Wide task, clicking its *Backup Now* button would back up only the currently-chosen database.  
   5. Click this button to **toggle this task's detailed-view** on or off.  
   6. This is another **Server-Wide Backup Task**, with its detailed-view toggled off.  

{PANEL/}

---

{PANEL: The Responsible Node}

* When defining a Server-Wide backup, the user is not given the option to select a 
  [responsible node](../../studio/server/server-wide-backup#the-responsible-node) manually, 
  since the responsible node can differ per database depending on the nodes the database resides on 
  (the [Database Group](../../studio/database/settings/manage-database-group#database-group)).  
  For each database, the responsible node for the task is set by the cluster to one of the database group nodes.

* Since defining a Server-Wide backup actually creates regular backup tasks, 
  the behavior of a Server-Wide Backup Task when the cluster or responsible-node is down is identical to that of a regular backup task.  
  See [Backup Task - When Cluster or Node are Down](../../studio/database/tasks/ongoing-tasks/backup-task#backup-task---when-cluster-or-node-are-down).  

* A graphical view of the database group shows which node is responsible for which task.  
  
  ![Figure 8. Topology View](images/ongoing-tasks-view_02-topology-view.png "Figure 8. Topology View")

{PANEL/}

##Related articles  

**Server**  
[Backup Overview](../../server/ongoing-tasks/backup-overview)  

**Studio**  
[Ongoing Backup Tasks](../../studio/database/tasks/ongoing-tasks/backup-task)  
[Restore: Create a Database From Backup](../../studio/server/databases/create-new-database/from-backup#create-a-database-from-backup)  
[Database Encryption](../../studio/server/databases/create-new-database/encrypted#create-a-database-encrypted)  

**Client API**  
[Backup Encryption](../../client-api/operations/maintenance/backup/encrypted-backup#backup-encryption)  
