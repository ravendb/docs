# Create a Database: From Backup
---

{NOTE: }

* In this page:  
  * [New Database From Backup - Studio Databases View](../../../../studio/server/databases/create-new-database/from-backup#new-database-from-backup---studio-databases-view)  
  * [New Database From Backup - Studio Backups View](../../../../studio/server/databases/create-new-database/from-backup#new-database-from-backup---studio-backups-view)  
  * [Backup Source Configuration](../../../../studio/server/databases/create-new-database/from-backup#backup-source-configuration)  

{NOTE/}

---

{PANEL: New Database From Backup - Studio Databases View}

![Figure 1. Create New Database From Backup](images/new-database-from-backup-1.png "Create New Database From Backup")

 1. Click on the **Databases** tab in the Studio.  
 2. Click the down arrow on the **New database** button.  
 3. Select **New database from backup**  

{PANEL/}

{PANEL: New Database From Backup - Studio Backups View}
Another way to access the New Database From Backup interface is via the Studio Backups view.

![Figure 1. Create New Database From Backup](images/backup-task-view1.png "Create New Database From Backup")

1. Click the **Tasks** tab and select **Backups**.
2. Click the **Restore a database from a backup** button.

{PANEL/}

{PANEL: Backup Source Configuration}

![Figure 2. Backup Source Configuration](images/new-database-from-backup-2.png "Backup Source Configuration")

1. [Name your new database](../../../../studio/server/databases/create-new-database/general-flow#2.-database-name).  

2. Select the source (from your local machine or various cloud-based options).  

3. Select the location that you set when you [created your backup](../../../../studio/database/tasks/backup-task).  
   * If your source is a cloud-based database, you will likely need to enter credentials and passwords as well as location.  

4. Disabling [ongoing tasks](../../../database/tasks/ongoing-tasks/general-info) will likely require these tasks to be created again.  

5. Skipping [indexes](../../../../indexes/what-are-indexes) will require your indexes to be built anew.  

6. Choose a restore point from the backups available on the source and location chosen.  
   
{NOTE: }
 Note: The backup will be restored only to the current node after restore, this database can be added to other nodes using the 'Manage group' button.
 Learn more about **Manage group** in : [Manage group](../../../database/settings/manage-database-group)  
{NOTE/}
{PANEL/}



## Related Articles

**Studio Articles**:   
[Create a Database : General Flow](../../../../studio/server/databases/create-new-database/general-flow)     
[Create a Database : Encrypted](../../../../studio/server/databases/create-new-database/encrypted)   
[The Backup Task](../../../../studio/database/tasks/backup-task) 

**Client Articles**:  
[Restore](../../../../client-api/operations/maintenance/backup/restore)   
[Operations: How to Restore a Database from Backup](../../../../client-api/operations/server-wide/restore-backup)    
[What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)   
[Backup](../../../../client-api/operations/maintenance/backup/backup)

**Server Articles**:  
[Backup Overview](../../../../server/ongoing-tasks/backup-overview)

**Migration Articles**:  
[Migration](../../../../migration/server/data-migration) 
