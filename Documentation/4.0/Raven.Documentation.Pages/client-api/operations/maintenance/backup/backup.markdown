# Backup
---

{NOTE: }

* The two principle reasons for backing up data, are -
   - **Securing data** in case catastrophe strikes
   - **Freezing data in chosen points-in-time** to retain access to it in [various stages](../../../../client-api/operations/maintenance/backup/backup#point-in-time-backup) of its existence/development.  

* Backup "freezes" ACID captures of your data in chosen points of time.  
* **Routine backup** is considered by RavenDB fundamental.  
    Backup is therefore provided not as a one-time operation, but as an [ongoing task](../../../../studio/database/tasks/ongoing-tasks/general-info).  

* In this page:  
  * [Preview](../../../../client-api/operations/maintenance/backup/backup#preview)  
  * [Backup Types](../../../../client-api/operations/maintenance/backup/backup#backup-types)  
      * [Logical Backup](../../../../client-api/operations/maintenance/backup/backup#logical-backup)  
      * [Snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot)  
  * [Backup Scope](../../../../client-api/operations/maintenance/backup/backup#backup-scope)  
      * [Full Backup](../../../../client-api/operations/maintenance/backup/backup#full-backup)  
      * [Incremental Backup](../../../../client-api/operations/maintenance/backup/backup#incremental-backup)  
      * [Point-in-Time Backup](../../../../client-api/operations/maintenance/backup/backup#point-in-time-backup)  
  * [Backup to Remote Destinations](../../../../client-api/operations/maintenance/backup/backup#backup-to-remote-destinations)  
  * [FAQ](../../../../client-api/operations/maintenance/backup/backup#faq)  
  * [Caution](../../../../client-api/operations/maintenance/backup/backup#caution)  
{NOTE/}

---

{PANEL: Preview}

* **Background Task**  
  Like other ongoing tasks, Backup runs in the background as an asynchronous task.  

* **Backup Types**  
  There are two backup types, **Logical backup** and **Snapshot**.  
   * A "logical backup", also referred to simply as "backup", is a compressed JSON dump of database contents, including documents and other data.  
   * A "SnapShot" is a binary image of the database and journals at a given point-in-time.  
     Using Snapshots is available only for Enterprise subscribers.  

* **Backup Scope**  
   * You can create a **full backup** of your database.  
   * Once the full backup has been prepared, you can periodically supplement it with smaller **Incremental backups**.  

* **Encryption**  
  Stored data can be **Encrypted**, or **Non encrypted**.  
   * When the database is encrypted, a Snapshot of it is encrypted as well.  
   * RavenDB 4.2 and on, supports encryption of logical backups as well.  

* **Compression**  
  All backup files are gzipped: Full "standard" backup dumps, snapshot images, and the incremental backups that supplement both.  
   * Data is compressed using [gzip](https://www.gzip.org/).  


* **Backup Name**  
   * Backup folders and files are named automatically. Their names are constructed of:  
      * Current Date and Time
      * Backed-up Database Name
      * Owner-Node Tag
      * Backup Type ("backup" or "snapshot")
      * Backup Scope ("full-backup" or "incremental-backup")

   * For example:  
      * `2018-12-26-16-17.ravendb-Products-A-backup` is the automatically-generated name of a backup-file's _folder_.  
          * "**2018-12-26-16-17**" - Backup Date and time  
          * "**Products**" - Backed-up Database name  
          * "**A**" - Executing node's tag
          * "**backup**" - Backup type (backup/snapshot)  
      * `2018-12-26-16-17.ravendb-full-backup` is the automatically-generated name of the actual backup _file_.  
          * "**full-backup**" - For a full backup; an incremental backup's name will state "incremental-backup".  

* **Backup contents**  
    * Backed-up data includes database-level and cluster-level contents.  
       * Database-level contents is data contained in the database and related to its state, and additional data types and entities.  

         | Database-level data|
         | ----|
         | Documents |
         | Attachments |
         | Revisions |
         | Counters |
         | Tombstones |
         | Conflicts |

    * Cluster-level data includes additional data, related to the behavior of the cluster.  

         | Cluster-level data|
         | ---- |
         | Database Record (including tasks) |
         | Compare-exchange values |
         | Identities |
         | Indexes <BR> (Logical Backups: Only Index definitions) |
         | Tasks state (Snapshot only) |
{PANEL/}

{PANEL: Backup Types}

####Logical Backup

* Logical Backup is also called "standard backup", or simply "backup".  
  Data is stored in compressed JSON files.  
* During restoration, RavenDB -  
   * Re-inserts all data into the database.  
   * Re-indexes the data.  
* Restoration Time is therefore **slower** than that required for a Snapshot restoration.  
* Needed storage space is **smaller** than that required to store a Snapshot image.  

* Code Sample:
  {CODE logical_full_backup_every_3_hours@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
  Note the usage of [Cron scheduling](https://en.wikipedia.org/wiki/Cron) when setting backup frequency.  

####Snapshot

* **Snapshot** backups are available for Enterprise subscribers only.  
  A "Snapshot" image is a bitwise duplication of the current database structure.  
* During restoration -
   * Re-indexing is not required.  
   * Re-inserting data into the database is not required.  
* Restoration Time is typically **faster** than that needed when restoring a logical backup.  
* Needed storage space is **larger** than that required for a logical backup.  

* Code Sample:  
  Simply provide **Snapshot** as your Backup Type.
  {CODE backup_type_snapshot@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

####Basic comparison between a logical backup and a snapshot:

  | Backup Type | Storage size | Stored Format | Restoration speed | Task characteristics |
  | ------ | ------ | ------ | ------ | ------ |
  | Snapshot | Large  | Compressed Binary Image | Fast | Ongoing Background Task |
  | Backup | Small | Compressed Textual Data | Slow | Ongoing Background Task |

{PANEL/}

{PANEL: Backup scope}

####Full Backup

* A full backup stores all your data from the dawn of time until moment of backup in a single file.  
* There are no preliminary conditions for creating a full backup, It can be done by any node.  

* Full Backup Code Sample:
  {CODE backup_full_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}


####Incremental Backup

* An incremental backup is used to save the changes that occured in your database since the last backup.  

* An incremental-backup file is always a dump of JSON-files.  
  It is so even when the full backup has been a binary snapshot.  

* Incremental Backup files are typically very small.  
* Incremental Backup ownership
   * An incremental backup can be created only by the node that currently owns the incremental-backup task.  
   * The ownership is granted dynamically by the cluster.  
   * A node can be apointed to own the incremental backup task, only after creating a full backup at least once.  

* Incremental Backup Code Sample:
  {CODE backup_incremental_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}


####Point-In-Time Backup  

* An elegant way to keep many restoration points without consuming too much storage space, is to use the **Revisions** feature.  
  * [Enable the Revisions feature](../../../../server/extensions/revisions#configuration).  
  * Routinely create [incremental backup](../../../../client-api/operations/maintenance/backup/backup#incremental-backup).  
  * To preserve free storage, erase revisions after backup.  
  * To restore data as it was in a certain moment -  
     - Locate and restore the correct incremental backup.  
     - Restore the correct [data revision](../../../../client-api/session/revisions/loading#revisions--loading-revisions) within this backup.  
{PANEL/}

{PANEL: Backup to Remote Destinations}

* Backups can be made locally, as well as to a set of remote locations including -  
   * A network path
   * an FTP/SFTP target
   * Amazon S3 
   * Azure Storage 
   * Amazon Glacier 

* RavenDB will store data in a local folder first, and transfer it to the remote destination from the local one.  

* Remote Backup Destinations Code Sample:
  {CODE backup_remote_destinations@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{PANEL/}





{PANEL: FAQ}

 * Q: **Is there a one-time backup?**
  * A: No. Backup is an on-going task, and is meant to back your data up continuously.  
        * If you wish, you can trigger it for immediate execution and then disable the task.  
        * You can also use [Smuggler](../../../../client-api/smuggler/what-is-smuggler#what-is-smuggler) for one-time export operations.  

* Q: **How do I create a backup of the whole cluster?**  
  * A: Backup and restore your database group's data.  
       The cluster and its information regarding the database and nodes can be easily re-created, there's no need for a backup for this.  

* Q: **How should I set nodes' time?**
  * A: The Backup task always uses the server's local time.  
       It is recommended that you set all nodes to the same time. This way, backup files' time-signatures are consistent even when the backups are created by different nodes.  

* Q: **Is [External Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task) a good substitute for ongoing backup?**  
  * A: No, the two procedures [have different aims and behave differently](../../../../studio/database/tasks/ongoing-tasks/backup-task#backup-task--vs--replication-task).  

{PANEL/}

{PANEL: Caution}
{WARNING: }

* Do **not** substitute RavenDB's backup procedures, with simply copying the database folder yourself.  
  Learn the backup procedure and implement it as recommended.  
   * Among other things, the official backup procedure provides -   
      * A reliable point-in-time freeze of backed up data.  
      * An ACIDity of backed-up data, so it wouldn't be dependent upon various files and connections when restored.  
     
* Remove backup files  
   * RavenDB does **not** automatically remove aging backup files. Make sure yourself they are regularly removed.  
     You can use services like crontab (a linux scheduling procedure) to create an old-backups-removal routine.  
   * Remotely backed-up files are stored in a local folder first.  
     Make sure this local folder is not where your database is, to prevent the backup procedure from consuming database storage space.  
     
{WARNING/}
{PANEL/}

## Related Articles

[Restore using code](../../../../client-api/operations/maintenance/backup/restore)  
[Backup using the Studio](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Restore using the Studio](../../../../studio/server/databases/create-new-database/from-backup)  
