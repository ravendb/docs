# Backup & Restore Overview
---

{NOTE: }

* Use this overview as an introduction to backing up and restoring your databases.  

* **Backup**  
   * The two principle reasons for backing up your database are -  
      * **Securing data** in case catastrophe strikes.  
      * **Freezing data in chosen points-in-time** to retain access to it in [various stages](../../../../client-api/operations/maintenance/backup/backup#point-in-time-backup) of its existence/development.  
   * RavenDB's Backup is an **Ongoing task**.  
      * RavenDB considers **Routine backup** a fundamental aspect of your database maintenance.  
        Backup is therefore provided not as a one-time operation, but as an [ongoing task](../../../../studio/database/tasks/ongoing-tasks/general-info).  
        It is configured and executed once, and then continuously produces updated backups.  
      * The Backup tasks works **in the background**.  
        Like the other ongoing tasks, Backup runs in the background as an asynchronous task.  

* **Restore**  
    Maintaining a proper backup routine ensures that you'd be able to restore your data to its state at nearly any chosen point of time.  

* In this page:  
  * [Backing up and Restoring a database](../../../../client-api/operations/maintenance/backup/overview#backing-up-and-restoring-a-database)  
     * [Backup scope: Full or Incremental](../../../../client-api/operations/maintenance/backup/overview#backup-scope-full-or-incremental)  
     * [A typical backup folder](../../../../client-api/operations/maintenance/backup/overview#a-typical-backup-folder)  
     * [Restoration procedure](../../../../client-api/operations/maintenance/backup/overview#restoration-procedure)  
  * [Overview](../../../../client-api/operations/maintenance/backup/overview#overview)  
      * [Backup Type: Logical Backup and Snapshot](../../../../client-api/operations/maintenance/backup/overview#backup-type-logical-backup-or-snapshot)  
      * [Encryption](../../../../client-api/operations/maintenance/backup/overview#encryption)  
      * [Compression](../../../../client-api/operations/maintenance/backup/overview#compression)  
      * [Backup Name](../../../../client-api/operations/maintenance/backup/overview#backup-name)  
      * [Backup contents](../../../../client-api/operations/maintenance/backup/overview#backup-contents)  
{NOTE/}

---

{PANEL: Backing up and Restoring a database}

####Backup scope: Full or Incremental

* You can set the Backup task to create either **full** or **incremental** backups during its periodical executions.  
   * A **full-backup file** contains **all** current database contents and configuration.  
      * Creating a full-backup file normally takes longer and requires more storage space than creating an incremental-backup file.  
   * An **incremental-backup file** contains only **the difference** between present data and data already backed up.  
      * An incremental-backup file is normally faster to create and smaller to keep than a full-backup file.  
      * An incremental-backup file always **supplements** a previous backup-file.  
        If you set the Backup task to create incremental backups but a previous backup file doesn't exist -  
        Backup will create a **full backup** first.  
        Subsequent backups will be incremental.  

---

####A typical backup folder

A typical backup folder holds a single full-backup file, and a list of incremental-backup files.  

* Each incremental backup updates its predecessors, and the whole structure illustrates the backup's chronology.  
* Folder contents sample:  
   * 2018-12-26-09-00.ravendb-full-backup
   * 2018-12-26-12-00.ravendb-incremental-backup
   * 2018-12-26-15-00.ravendb-incremental-backup
   * 2018-12-26-18-00.ravendb-incremental-backup

---

####Restoration procedure

In order to restore a database, RavenDB -  

* Browses the backup folder.  
  On your part, you need only to provide the backup-folder's path.  
* Restores the **full backup** it finds in this folder.  
  A backup folder typically contains a single full backup, and the incremental backups that supplement it.  
* Restores **incremental backups** one by one.  
  By default, RavenDB will restore all incremental backup files to the last.  
  You can also use `LastFileNameToRestore` to [stop](../../../../client-api/operations/maintenance/backup/restore#restore-backup-configuration) restoration when a certain file is reached.  

{PANEL/}

{PANEL: Overview}

####Backup Type: Logical Backup or Snapshot  

There are two backup types: [Logical backup](../../../../client-api/operations/maintenance/backup/backup#logical-backup-or-simply-backup) (or simply "Backup") and [Snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot).  

* A Logical backup is a compressed JSON dump of database contents, including documents and other data.  
* A SnapShot is a binary image of the [database and journals](../../../../server/storage/directory-structure#storage--directory-structure) at a given point-in-time.  
   * Using Snapshots is available only for _Enterprise subscribers_.  

---

####Encryption

Stored data can be **Encrypted** or **Unencrypter**.  

* Snapshot encryption  
   * The snapshot of an [encrypted database](../../../../server/security/encryption/database-encryption), is encrypted.  
   * The snapshot of an unencrypted database, is unencrypted.  
* Logical backup encryption  
   * With RavenDB 4.0 and 4.1, you can create only an unencrypted logical backup.  
   * With RavenDB 4.2 and on, you can encrypt a logical backup as well.  

---

####Compression

* A backup always consists of a single compressed file.  
  It is so for all backup formats: full "Logical" backup dumps, snapshot images, and the incremental backups that supplement both.  
* Data is compressed using [gzip](https://www.gzip.org/).  

---

####Backup Name

Backup folders and files are named automatically. Their names are constructed of:  

* Current Date and Time  
* Backed-up Database Name  
* Owner-Node Tag  
* Backup Type ("backup" or "snapshot")  
* Backup Scope ("full-backup" or "incremental-backup")  

For example:  

* `2018-12-26-16-17.ravendb-Products-A-backup` is the name automatically given to a backup _folder_.  
    * "**2018-12-26-16-17**" - Backup Date and time  
    * "**Products**" - Backed-up Database name  
    * "**A**" - Executing node's tag
    * "**backup**" - Backup type (backup/snapshot)  
* `2018-12-26-16-17.ravendb-full-backup` is the name automatically given to the backup file inside this folder.  
    * "**full-backup**" - For a full backup; an incremental backup's name will state "incremental-backup".  

---

####Backup contents

Backed-up data includes database-level and cluster-level contents.  

* Database-level contents is data contained in the database and related to its state, and additional data types and entities.  
* Cluster-level data includes additional data, related to the behavior of the cluster.  

  | Database-level data | Cluster-level data|
  | ----|---- |
  | Documents | Database Record (including tasks) |
  | Attachments | Compare-exchange values |
  | Revisions | Identities |
  | Counters | Indexes <BR> (Logical Backups: Only Index definitions) |
  | Tombstones | Tasks state (Snapshot only) |
  | Conflicts |

{PANEL/}

## Related Articles  (to be revised, ignore)

####Client
[Restore using code](../../../../client-api/operations/maintenance/backup/restore)  

####Studio
[Backup using the Studio](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Restore using the Studio](../../../../studio/server/databases/create-new-database/from-backup)  
