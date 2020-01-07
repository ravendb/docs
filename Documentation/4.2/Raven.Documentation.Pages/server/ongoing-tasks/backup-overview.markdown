﻿# Backup Overview
---

{NOTE: }

* Maintaining a proper backup routine ensures that you'd be able to restore your data to its state in chosen points of time.  
  Use this overview as an introduction to backing up and restoring your databases.  

* The two principal reasons for backing up your database are -  
   * **Securing data** in case catastrophe strikes.  
   * **Freezing data in chosen points-in-time** to retain access to it in various stages of its existence/development.  

* RavenDB's Backup is an **Ongoing task**.  
   * Routinely backing up your data is a fundamental aspect of your database maintenance.  
     Backup is therefore provided not as a one-time operation, but as an [ongoing task](../../studio/database/tasks/ongoing-tasks/general-info) that runs in the background.  
     It is configured once and then executed periodically according to the defined schedule.  

* In this page:  
  * [Backup Type](../../server/ongoing-tasks/backup-overview#backup-type)  
  * [Backup Contents](../../server/ongoing-tasks/backup-overview#backup-contents)  
  * [Backup Scope: Full or Incremental](../../server/ongoing-tasks/backup-overview#backup-scope:-full-or-incremental)  
  * [Backup Name and Folder Structure](../../server/ongoing-tasks/backup-overview#backup-name-and-folder-structure)  
  * [Encryption](../../server/ongoing-tasks/backup-overview#encryption)  
  * [Compression](../../server/ongoing-tasks/backup-overview#compression)  
  * [Restoration Procedure](../../server/ongoing-tasks/backup-overview#restoration-procedure)  
{NOTE/}

---

{PANEL: Backup Type}

There are two backup types: [Logical-backup](../../client-api/operations/maintenance/backup/backup#logical-backup) (or simply "Backup") and [Snapshot](../../client-api/operations/maintenance/backup/backup#snapshot).  

* **Logical Backup**  
  A logical backup is a compressed JSON dump of database contents, including documents and additional data.  
* **SnapShot**  
  A snapshot is a binary image of the [database and journals](../../server/storage/directory-structure#storage--directory-structure) at a given point-in-time.  
  {NOTE: }
  Snapshots are only available for _Enterprise subscribers_.  
  {NOTE/}

{PANEL/}

{PANEL: Backup Contents}

Backed-up data includes both database-level and cluster-level contents, as detailed below.  

| Database-level data |
| ----|
| Documents |
| Attachments |
| Revisions |
| Counters |
| Tombstones |
| Conflicts |
| Subscriptions |

| Cluster-level data|
|---- |
| Database Record |
| Compare-exchange values |
| Identities |
| Indexes (Logical-Backups: Index definitions only) |
| Ongoing Tasks configuration (4.0 Snapshots only, 4.2 Logical-backups & Snapshots) |

{PANEL/}

{PANEL: Backup Scope: Full or Incremental}

You can set the Backup task to create either **full** or **incremental** backups during its periodical executions.  

* **Full Backup**  
  A full backup contains **all** current database contents and configuration.  
  * The creation of a full-backup file normally **takes longer** and **requires more storage space** than the creation of an incremental-backup file.  

* **Incremental Backup**  
  An incremental backup contains only **the difference** between the current database data and the last backed-up data.  
  * An incremental-backup file is normally **faster to create** and **smaller** than a full-backup file.  
  * When an incremental-backup task is executed, it checks for the existence of a previous backup file.  
    If such a file doesn't exist, the first backup created will be a full backup.  
    Subsequent backups will be incremental.  

* **A Typical Configuration**  
  A typical configuration would include quick incremental-backup runs that "fill the gaps" between full backups.  
  * For example -  
    A **full-backup** task set to run **every 12 hours**,  
    and an **incremental-backup** task that runs **every 30 minutes**.  

{PANEL/}

{PANEL: Backup Name and Folder Structure}

####Naming

Backup folders and files are **named automatically**.  

* Their names are constructed of:  
  Current Date and Time  
  Backed-up Database Name  
  Owner-Node Tag  
  Backup Type ("backup" or "snapshot")  
  Backup Scope ("full-backup" or "incremental-backup")  

* For example:  
  * `2018-12-26-16-17.ravendb-Products-A-backup` is the name automatically given to a backup _folder_.  
     * "**2018-12-26-16-17**" - Backup Date and time  
     * "**Products**" - Backed-up Database name  
     * "**A**" - Executing node's tag  
     * "**backup**" - Backup type (backup/snapshot)  
 * `2018-12-26-16-17.ravendb-full-backup` is the name automatically given to the backup _file_ inside this folder.  
     * "**full-backup**" - For a full backup; an incremental backup's name will state "incremental-backup".  

---

####Folder Structure

A typical backup folder holds a single full-backup file and a list of incremental-backup files that supplement it.  
Each incremental backup file contains only the delta from its predecessor backup file.

* For example -  
  2018-12-26-09-00.ravendb-full-backup  
  2018-12-26-12-00.ravendb-incremental-backup  
  2018-12-26-15-00.ravendb-incremental-backup  
  2018-12-26-18-00.ravendb-incremental-backup  

{PANEL/}

{PANEL: Encryption}

Stored backup data can be [Encrypted](../../client-api/operations/maintenance/backup/encrypted-backup) or Unencrypted.  

{PANEL/}

{PANEL: Compression}

* A backup always consists of a single compressed file.  
  It is so for all backup formats: Full "logical" backup dumps, Snapshot images, and the Incremental backups that supplement both.  
* Data is compressed using [gzip](https://www.gzip.org/).  

{PANEL/}

{PANEL: Restoration Procedure}

In order to restore a database - 

* [Provide RavenDB](../../client-api/operations/maintenance/backup/restore#restoring-a-database:-configuration-and-execution) 
  with the backup folder's path.
* RavenDB will browse this folder and restore the full-backup found in it.  
* RavenDB will then restore the incremental-backups one by one, up to and including the last one.
  You can set `LastFileNameToRestore` to 
  [stop restoration](../../client-api/operations/maintenance/backup/restore#optional-settings) 
  at a specific backup-file.


{PANEL/}

## Related Articles  
###Client API  
- [Backup](../../client-api/operations/maintenance/backup/backup)  
- [Restore](../../client-api/operations/maintenance/backup/restore)  
- [Encrypted-Backup : Create & Restore](../../client-api/operations/maintenance/backup/encrypted-backup)  
- [Backup FAQ](../../client-api/operations/maintenance/backup/faq)  
- [What Is Smuggler](../../client-api/smuggler/what-is-smuggler)  

###Studio  
- [The Backup Task](../../studio/database/tasks/ongoing-tasks/backup-task)  
- [Create Database: From Backup](../../studio/server/databases/create-new-database/from-backup)  
- [Create a Database: General Flow](../../studio/server/databases/create-new-database/general-flow)  
- [Create a Database: Encrypted](../../studio/server/databases/create-new-database/encrypted)  

###Security  
- [Database Encryption](../../server/security/encryption/database-encryption)  
- [Security Overview](../../server/security/overview)  
- [Authentication and Certification](../../server/security/authentication/certificate-configuration)  

###Migration  
- [Migration](../../migration/server/data-migration)   
