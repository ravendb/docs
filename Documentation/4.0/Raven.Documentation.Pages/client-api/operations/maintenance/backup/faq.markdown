# Frequently Asked Questions regarding Backup and Restore
---

{PANEL: FAQ}

 * Q: **Is there a one-time backup?**
* A: No. Backup is an on-going task, and is meant to back your data up continuously.  
      * If you wish, you can trigger it for immediate execution and then disable the task.  
      * You can also use [Smuggler](../../../../client-api/smuggler/what-is-smuggler#what-is-smuggler) for one-time [export](../../../../client-api/smuggler/what-is-smuggler#export) operations.  

---

* Q: **How do I create a backup of the whole cluster?**  
* A: Backup and restore your database group's data.  
     The cluster and its information regarding the database and nodes can be easily re-created, there's no need for a backup for this.  

---

* Q: **How should I set nodes' time?**
* A: The Backup task always uses the server's local time.  
     It is recommended that you set all nodes to the same time. This way, backup files' time-signatures are consistent even when the backups are created by different nodes.  

---

* Q: **Is [External Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task) a good substitute for ongoing backup?**  
* A: No, the two procedures [have different aims and behave differently](../../../../studio/database/tasks/ongoing-tasks/backup-task#backup-task--vs--replication-task).  

---

* Q: Can't I simply copy the databases directory whenever I need to create a backup?   
* A: Simply copying the databases folder is **not** a good substitute for RavenDB's backup procedures.  
     Creating a backup routine is a one-time operation. There really is no reason to do it manually again and again.  
     RavenDB's own backup also has a few advantages. Among them:  
      * RavenDB creates a reliable point-in-time freeze of backed-up data.  
      * RavenDB ensures backed ACIDity, preventing any dependencies upon files or connections during restoration.  

---

* Q: Does RavenDB automatically delete old backups?  
* A: No, RavenDB does not automatically remove backup files, you need to take care of it yourself.  
     You can use services like crontab (a linux scheduling procedure) to create an old-backup-files-removal routine.  

---

* Q: Are there any locations in which I better not store backup files?  
* A: It is recommended not to store your backups in your database's location, to avoid an even remotely-possible scenario of a database storage distress.  
     
{PANEL/}

## Related Articles (to be revised, ignore)

####Client
[Restore using code](../../../../client-api/operations/maintenance/backup/restore)  

####Studio
[Backup using the Studio](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Restore using the Studio](../../../../studio/server/databases/create-new-database/from-backup)  
