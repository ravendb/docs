# Backup & Restore: <br> Frequently Asked Questions  
---

{NOTE: }

* In this page:  
  * [Is there a one-time backup?](../../../../client-api/operations/maintenance/backup/faq#is-there-a-one-time-backup)  
  * [How do I create a backup of my cluster configuration?](../../../../client-api/operations/maintenance/backup/faq#how-do-i-create-a-backup-of-my-cluster-configuration)  
  * [How should the servers' time be set in a multi-node cluster?](../../../../client-api/operations/maintenance/backup/faq#how-should-the-servers-time-be-set-in-a-multi-node-cluster)  
  * [Is an External Replication a good substitute for a backup task?](../../../../client-api/operations/maintenance/backup/faq#is-an-external-replication-task-a-good-substitute-for-a-backup-task)  
  * [Can I simply copy the database folder contents whenever I need to create a backup?](../../../../client-api/operations/maintenance/backup/faq#can-i-simply-copy-the-database-folder-contents-whenever-i-need-to-create-a-backup)  
  * [Does RavenDB automatically delete old backups?](../../../../client-api/operations/maintenance/backup/faq#does-ravendb-automatically-delete-old-backups)  
  * [Are there any locations that backup files should NOT be stored at?](../../../../client-api/operations/maintenance/backup/faq#are-there-any-locations-that-backup-files-should-not-be-stored-at)  
  * [What happens when a backup process fails before it is completed?](../../../../client-api/operations/maintenance/backup/faq#what-happens-when-a-backup-process-fails-before-completion)  

{NOTE/}

---

{PANEL: FAQ}

###Is there a one-time backup?

No. Backup is an on-going task and is meant to back your data up continuously.  

* If you wish, you can trigger it for immediate execution and then disable or delete the task.  
* You can also use [Smuggler](../../../../client-api/smuggler/what-is-smuggler#what-is-smuggler) as an equivalent of a full backup for a single [export](../../../../client-api/smuggler/what-is-smuggler#export) operation.  

---

###How do I create a backup of my cluster configuration?  

Only database contents can be backed up. 
Cluster configuration and nodes setup can be easily re-created, no special backup procedure is needed for it.  

---

###How should the servers' time be set in a multi-node cluster?

The backup task is running on schedule according to the executing server local time.  
It is recommended that you set all nodes to the same time. This way, backup files' time-signatures are consistent even when the backups are created by different nodes.  

---

###Is an External Replication task a good substitute for a backup task?  

Although [External Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task) 
and [Backup](../../../../client-api/operations/maintenance/backup/backup) 
are both ongoing-tasks, they have different aims and behavior.  
See [Backup Task -vs- External Replication Task](../../../../studio/database/tasks/backup-task#backup-task--vs--replication-task).  

---

###Can I simply copy the database folder contents whenever I need to create a backup?  

Simply copying the database folder is **not** a good substitute for RavenDB's backup procedures.  
Creating a backup task is a one-time operation. There really is no reason to do it manually again and again.  
Other advantages of RavenDB's backup system include:  

* **The creation of a reliable point-in-time freeze** of backed-up data.  
* **The assurance of ACID compliancy** for backed up data during interactions with the file system.  

---

###Does RavenDB automatically delete old backups?  

You can configure RavenDB to delete old backups with the `RetentionPolicy` feature.  
If you enable it, RavenDB will delete backups after the `TimeSpan` that you set.  
By default, `RetentionPolicy` is disabled.  

Learn how to change the [RetentionPolicy](../../../../studio/database/tasks/backup-task#retention-policy) via the RavenDB Studio.  
Learn how to change the [RetentionPolicy](../../../../client-api/operations/maintenance/backup/backup#backup-retention-policy) via API.

---

###Are there any locations that backup files should NOT be stored at?  

It is recommended **not** to store backups on the same drive as your database data files, since -  

* Reading from and writing to the same drive can slow down other database operations.  
* Disk space can run low as backups start piling up.  
* Both the database and the backups would be exposed to the same risks.  

---

###What happens when a backup process fails before completion?  

While in progress, the backup content is written to a **.in-progress* file on disk.  

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
- [The Backup Task](../../../../studio/database/tasks/backup-task)  
- [Create Database: from Backup](../../../../studio/server/databases/create-new-database/from-backup)  
- [Create a Database: General Flow](../../../../studio/server/databases/create-new-database/general-flow)  
- [Create a Database: Encrypted](../../../../studio/server/databases/create-new-database/encrypted)  

###Security  
- [Database Encryption](../../../../server/security/encryption/database-encryption)  
- [Security Overview](../../../../server/security/overview)  
- [Authentication and Certification](../../../../server/security/authentication/certificate-configuration)  

###Migration  
- [Migration](../../../../migration/server/data-migration)   
