# Sharding: Backup
---

{NOTE: }

* Sharded databases are backed up using user-defined periodic 
  [backup tasks](../../../../server/ongoing-tasks/backup-overview).  

* Shards can store backup files **locally** (each shard using its 
  own node machine storage) and/or **remotely** (all shards sending 
  backup files to a common remote destination like an AWS S3 bucket).  

* Both [Full](../../../../server/ongoing-tasks/backup-overview#full-backup) 
  and [Incremental](../../../../server/ongoing-tasks/backup-overview#incremental-backup) 
  backups can be created for a sharded database.  

* A [logical](../../../../server/ongoing-tasks/backup-overview#logical-backup) 
  backup **can** be created for a sharded database and restored into either 
  a sharded or a non-sharded database.  

* A [snapshot](../../../../server/ongoing-tasks/backup-overview#snapshot) 
  backup **cannot** be created for a sharded database.  

* A manual [one-time](../../../../studio/database/tasks/backup-task#manually-creating-one-time-backups) 
  backup **can** be created for a sharded database.  

* In this page:  
  * [Backup](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup)  
     * [Sharded and Non-Sharded Backup Tasks](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#sharded-and-non-sharded-backup-tasks)  
     * [Backup Storage: Local and Remote](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-storage-local-and-remote)  
     * [Backup Files Extension and Structure](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-files-extension-and-structure)  
     * [Backup Type](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-type)  
     * [Backup Scope](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-scope)  
     * [Naming Convention](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#naming-convention)  
     * [Server-Wide Backup](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#server-wide-backup)  
     * [Example](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#example)  

{NOTE/}

---

{PANEL: Backup}

## Sharded and Non-Sharded Backup Tasks

From a user's perspective, backing up a sharded database is done by 
defining and running **a single backup task**, just like it is done 
with a non-sharded database.  

Behind the scenes, though, each shard backs up its own slice of the 
database independently from other shards.  

Distributing the backup responsibility between the shards allows 
RavenDB to speed up the backup process and keep backup files in 
manageable proportions no matter what the overall database size is.  

### Non-Sharded DB Backup Tasks

* A complete replica of the database is kept by each cluster node.  
* Any node can therefore be made 
  [responsible](../../../../server/clustering/distribution/highly-available-tasks#responsible-node) 
  for backups by the cluster.  
* The responsible node runs the backup task periodically to create 
  a backup of the entire database.  
  
### Sharded DB Backup Tasks

* Each shard hosts a unique part of the database, so no single node 
  can create a backup of the entire database.  
* After a user defines a backup task, RavenDB automatically creates 
  one backup task per shard, based on the user-defined task.  
  This operation is automatic and requires no additional actions 
  from the user.  
* Each shard appoints [one of its nodes](../../../../sharding/overview#shard-replication) 
  responsible for the execution of the shard's backup task.  
* Each shard backup task can keep the shard's database 
  locally (on the shard machine), and/or remotely (on one 
  or more cloud destinations).  
* A backup task can store backups on multiple destinations, 
  e.g. locally, on an S3 bucket, and on an Azure blob.  
* To [restore](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore) 
  the entire database, the restore process is provided with 
  the locations of the backup folders used by all shards.  
* When restoring the database, the user doesn't have to restore 
  all shard backups. It is possible, for example, to restore only 
  one of the shards. Using this flexibility, a sharded database 
  can easily be split into several databases.  

## Backup Storage: Local and Remote

Backup files can be stored locally and remotely.  
Find a code example [here](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#example).  

* **Local Backup**  
  A shard's backup task may keep backup data locally, 
  using the node's local storage.  

    [Restoring](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#section-2) 
    backup files that were stored locally requires the user to provide 
    the restore process with the location of the backup folder on each 
    shard's node.  

* **Remote location**  
  Backups can also be kept remotely. All shards will transfer 
  the backup files to a common location, using one of the currently 
  supported platforms:  
   * Azure Blob Storage  
   * Amazon S3 Storage  
   * Google Cloud Platform  

    [Restoring](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#section-2) 
    backup files that were stored remotely requires the user to provide 
    the restore process with each shard's backup folder location.  

## Backup Files Extension and Structure

* `.ravendbdump` is the extension given to files when a database 
  is exported using [Studio](../../../../studio/database/tasks/export-database) 
  or [Smuggler](../../../../client-api/smuggler/what-is-smuggler).  
* Backup files use different extensions to indicate the backup type 
  (e.g. `.ravendb-full-backup` for a full-backup file), but their 
  structure is similar to that of `.ravendbdump` files.  
* It **is** therefore possible to import backup files, including those 
  created for a sharded database, into a sharded database using 
  [studio](../../../../studio/database/tasks/import-data/import-data-file) 
  or [smuggler](../../../../client-api/smuggler/what-is-smuggler#import).  

{NOTE: }
Backed-up data includes both 
[database-level and cluster-level content](../../../../server/ongoing-tasks/backup-overview#backup-contents).  
{NOTE/}

## Backup Type

A shard backup task can create a 
[Logical backup](../../../../server/ongoing-tasks/backup-overview#logical-backup) 
only.  

A [Snapshot](../../../../server/ongoing-tasks/backup-overview#snapshot) 
backup **cannot** be created for a sharded database.  

`Logical` backups created for a sharded database can be restored into 
both sharded and non-sharded databases.  

## Backup Scope

A shard backup task can create 
a [Full backup](../../../../server/ongoing-tasks/backup-overview#full-backup) 
with the entire content of the shard, or an 
[Incremental Backup](../../../../server/ongoing-tasks/backup-overview#incremental-backup) 
with just the difference between the current database data and the last backed-up data.  

## Naming Convention

* Backup files created for a sharded database generally follow the same naming 
  [convention](../../../../server/ongoing-tasks/backup-overview#backup-name-and-folder-structure) 
  as non-sharded database backups.  

* Each shard keeps its backup files in a folder whose name consists of:  
   * **Date and Time** (when the folder was created)  
   * **Database Name**  
   * `$` symbol  
   * **Shard Number**  

      The backup folders for a 3-shard database named "Books", 
      for example, can be named:  
      `2023-02-05-16-17.Books$0` for shard 0  
      `2023-02-05-16-17.Books$1` for shard 1  
      `2023-02-05-16-17.Books$2` for shard 2  

## Server-Wide Backup

[Server-wide backup](../../../../client-api/operations/maintenance/backup/backup#server-wide-backup) 
backs up all the databases hosted by the cluster, by creating a backup 
task for each database and executing all tasks at a scheduled time.  

* A server-wide backup will create backups for both non-sharded **and** 
  sharded databases.  
* To create a backup for an entire sharded database, the operation will 
  define and execute a backup task for each shard, behaving as if it was 
  defined manually.  

## Example

The backup task that we define here is similar to the task we 
would define for a non-sharded database. As part of a sharded 
database, however, this task will be re-defined automatically 
by the orchestrator for each shard.  
{CODE backup_remote_destinations@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../../client-api/operations/server-wide/create-database)  

**Server**  
[External Replication](../../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../../../../studio/database/tasks/export-database)  

