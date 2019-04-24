# Create a Database: From Backup
---

{NOTE: }

* In this page:  
  * [1. New Database From Backup](../../../../studio/server/databases/create-new-database/from-backup#1.-new-database)  
  * [2. Backup Source Configuration](../../../../studio/server/databases/create-new-database/from-backup#2.-database-name)  

{NOTE/}

---

{PANEL: 1. Creating New Database From Backup}

![Figure 1. Create New Database From Backup](images/new-database-from-backup-1.png "Create New Database From Backup")

{NOTE: }
Open the down arrow and click `New database from backup`.
{NOTE/}
{PANEL/}

{PANEL: 2. Backup Source Configuration}

![Figure 2. Backup Source Configuration](images/new-database-from-backup-2.png "Backup Source Configuration")

1. [Database Name](../../../../studio/server/databases/create-new-database/general-flow#2.-database-name)

2. **Backup directory**

3. **Disable ongoing tasks after restore**
    * Disable all ongoing tasks. Learn more about **Ongoing tasks** in [Ongoing tasks](../../../database/tasks/ongoing-tasks/general-info)

4. **Restore point**
    * You can choose restore point from the one available
   
{NOTE: }
 Note: The backup will be restored only to the current node after restore, this database can be added to other nodes using the 'Manage group' button.
 Learn more about **Manage group** in : [Manage group](../../../database/settings/manage-database-group)  
{NOTE/}
{PANEL/}

## Related Articles

**Studio Articles**:   
[Create a Database : General Flow](../../../../studio/server/databases/create-new-database/general-flow)     
[Create a Database : Encrypted](../../../../studio/server/databases/create-new-database/encrypted)   
[The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task) 

**Client Articles**:  
[Restore](../../../../client-api/operations/maintenance/backup/restore)   
[Operations: How to Restore a Database from Backup](../../../../client-api/operations/server-wide/restore-backup)    
[What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)   
[Backup](../../../../client-api/operations/maintenance/backup/backup)

**Server Articles**:  
[Backup Overview](../../../../server/ongoing-tasks/backup-overview)

**Migration Articles**:  
[Migration](../../../../migration/server/data-migration) 
