# Sharding: Backup
---

{NOTE: }

* A sharded database is backed up using a user-defined [backup task](../../../../server/ongoing-tasks/backup-overview).  

* Shards can store backup files **locally** (each shard storing backup files 
  on its server machine storage) or **remotely** (all shards sending backup 
  files to a common remote destination).  

* Both [Full](../../../../server/ongoing-tasks/backup-overview#full-backup) 
  and [Incremental](../../../../server/ongoing-tasks/backup-overview#incremental-backup) 
  backups can be created for a sharded database.  

* A [logical](../../../../server/ongoing-tasks/backup-overview#logical-backup) 
  backup **can** be created for a sharded database, and restored into either 
  a sharded or a non-sharded database.  

* A [snapshot](../../../../server/ongoing-tasks/backup-overview#snapshot) 
  backup **cannot** be created for a sharded database.  

* In this page:  
  * [Backup](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup)  
     * [Sharded and Non-Sharded Backup Tasks](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#sharded-and-non-sharded-backup-tasks)  
     * [Backup Storage: Local and Remote](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-storage-local-and-remote)  
     * [Backup Files Extension and Structure](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-files-extension-and-structure)  
     * [Backup Type](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-type)  
     * [Backup Scope](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-scope)  
     * [Nameing Convention](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#nameing-convention)  
     * [Server-Wide Backup](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#server-wide-backup)  
     * [Example](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#example)  

{NOTE/}

---

{PANEL: Backup}

## Sharded and Non-Sharded Backup Tasks

From a user's perspective, backing up a sharded database is done by 
defining and running **a single backup task**, just like it is done 
with a non-sharded database.  

Behind the scenes, though, RavenDB must handle sharded and non-sharded 
backups differently:  

### Non-Sharded DB Backup Tasks
A complete replica of the database is kept by each cluster node.  
Any node can therefore be made 
[responsible](../../../../server/clustering/distribution/highly-available-tasks#responsible-node) 
for backups by the cluster, run the backup task and create a backup 
of the entire database.  
  
### Sharded DB Backup Tasks
Each shard hosts a unique part of the database, so no single node 
can create a backup of the entire database.  
To solve this:  

* After a user defines a backup task, RavenDB automatically defines 
  one backup task per shard, based on the user-defined task.  
  This operation is automatic and requires no additional actions 
  from the user.  
* Each shard appoints [one of its nodes](../../../../sharding/overview#shard-replication) 
  responsible for the execution of the shard's backup task.  
* Each shard backup task keeps its part of the database locally 
  (on the shard machine) or remotely (on a cloud destination).  
* To restore the entire database, the restore process is provided 
  with the addresses of all the database parts so it may collect 
  their contents into a sharded or a non-sharded database.  

## Backup Storage: Local and Remote

Backup files can be stored locally or remotely.  
Fine a code example [here](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#example).  

* **Local Backup**  
  A shard's backup task may keep backup data locally, 
  using the node's local storage.  

    [Restoring](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#section-2) 
    backup files that were stored locally, requires the user 
    to provide the restore process with each shard's address 
    to locate the backup files.  

* **Remote location**  
  Backups can also be kept remotely. All shards will transfer 
  the backup files to a common location, using one of the currently 
  supported platforms:  
   * Azure Blobs  
   * Amazon S3  

    Restoring backup files that were stored remotely requires 
    the user to provide the restore process with the remote 
    location and the unique names given to the files store 
    by each shard.  

## Backup Files Extension and Structure

`.ravendbdump` is the extension given to files when a database 
is exported using [Studio](../../../../studio/database/tasks/export-database) 
or [Smuggler](../../../../client-api/smuggler/what-is-smuggler).  

Backup files use different extensions to indicate the backup type 
(e.g. `.ravendb-full-backup` for a full-backup file), but their 
structure is similar to that of `.ravendbdump` files.  

It **is** therefore possible to import backup files, including those 
created for a sharded database, into a sharded or a non-sharded database 
using [studio](../../../../studio/database/tasks/import-data/import-data-file) 
or [smuggler](../../../../client-api/smuggler/what-is-smuggler#import).  

{NOTE: }
Backed-up data includes both database-level and cluster-level content, as 
for [non-sharded databases](../../../../server/ongoing-tasks/backup-overview#backup-contents).  
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

## Nameing Convention

* Backup files created for a sharded database generally follow the same naming 
  [convention](../../../../server/ongoing-tasks/backup-overview#backup-name-and-folder-structure) 
  as non-sharded database backups.  

* When the backup files are stored in a remote location (like an S3 bucket) 
  and need to be differentiated from one another, the automatically-created 
  folder name they are given is combined of:  
   * Backup Date and Time 
   * Database Name  
   * a `$` symbol  
   * The Shard Number  

      The backup folders for a 3-shard database named "Books", for example, 
      may be named:  
      `2023-02-05-16-17.Books$01` for shard 1  
      `2023-02-05-16-17.Books$02` for shard 2  
      `2023-02-05-16-17.Books$03` for shard 3  

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

The backup task that we define here is defined similarly to how we would define 
a backup task for a non-sharded database. As part of a sharded database, however, 
this task will be re-defined automatically by the orchestrator for each shard.  

{CODE backup_remote_destinations@Sharding\Server\OngoingTasks\BackupAndRestore.cs /}

{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../../client-api/operations/server-wide/create-database)  

**Server**  
[External Replication](../../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../../../../studio/database/tasks/export-database)  

