# Backup & Restore: <br> Frequently Asked Questions  

---

{PANEL: FAQ}

* Q: **Is there a one-time backup?**
* A: No. Backup is an on-going task and is meant to back your data up continuously.  
      * If you wish, you can trigger it for immediate execution and then disable or delete the task.  
      * You can also use [Smuggler](../../../../client-api/smuggler/what-is-smuggler#what-is-smuggler) as an equivalent of a full backup for a single [export](../../../../client-api/smuggler/what-is-smuggler#export) operation.  

---

* Q: **How do I create a backup of my cluster configuration?**  
* A: Only database contents can be backed up. 
     Cluster configuration and nodes setup can be easily re-created, no special backup procedure is needed for it.  

---

* Q: **How should the servers' time be set in a multi-node cluster?**
* A: The backup task is running on schedule according to the executing server local time.  
     It is recommended that you set all nodes to the same time. This way, backup files' time-signatures are consistent even when the backups are created by different nodes.  

---

* Q: **Is an [External Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task) task 
  a good substitute for a backup task?**  
* A: Although both are ongoing-tasks, they have different aims and behavior.  
  See [Backup Task -vs- External Replication Task](../../../../studio/database/tasks/ongoing-tasks/backup-task#backup-task--vs--replication-task).  

---

* Q: **Can I simply copy the database folder contents whenever I need to create a backup?**  
* A: Simply copying the database folder is **not** a good substitute for RavenDB's backup procedures.  
     Creating a backup task is a one-time operation. There really is no reason to do it manually again and again.  
     Other advantages of RavenDB's backup system include:  
      * **The creation of a reliable point-in-time freeze** of backed-up data.  
      * **The assurance of ACID compliancy** for backed up data during interactions with the file system.  

---

* Q: **Does RavenDB automatically delete old backups?**  
* A: No, RavenDB does not automatically remove backup files, you need to take care of it yourself.  
     You can use services like crontab (a Linux scheduling procedure) to create an old-backup-files-removal routine.  

---

* Q: **Are there any locations that backup files should NOT be stored at?**  
* A: It is recommended **not** to store backups on the same drive as your database data files, since -  
   * Reading from and writing to the same drive can slow down other database operations.  
   * Disk space can run low as backups start piling up.  
   * Both the database and the backups would be exposed to the same risks.  

---

* Q: **What happens when a backup process fails before it is completed?**  
* A: While in progress, the backup content is written to a **.in-progress* file on disk.  
      * Once **backup is complete**, the file is renamed to its correct final name.  
      * If the backup process **fails before completion**, the **.in-progress* file remains on disk.  
        This file will not be used in any future Restore processes.  
        
        If the failed process was an incremental-backup task, any future incremental backups will 
        continue from the correct place before the file was created.  

{PANEL/}

## Related Articles  
###Server  
- [Backup Overview](../../../../server/ongoing-tasks/backup-overview)

###Client API  
- [Backup](../../../../client-api/operations/maintenance/backup/backup)  
- [Restore](../../../../client-api/operations/maintenance/backup/restore)  
- [Encrypted-Backup : Create & Restore](../../../../client-api/operations/maintenance/backup/encrypted-backup)  
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
