# Sharding: Backup
---

{NOTE: }

* Sharded databases are backed up using [backup tasks](../../../../server/ongoing-tasks/backup-overview).  

* A different backup task is required for each shard.  
  E.g., a 3-shard database requires three backup tasks, one per shard.  

* You **can** create [full](../../../../server/ongoing-tasks/backup-overview#full-backup) 
  and [Incremental](../../../../server/ongoing-tasks/backup-overview#incremental-backup) 
  backups for a sharded database.  

* You can create a 
  [logical backup](../../../../server/ongoing-tasks/backup-overview#logical-backup) 
  of a sharded database and restore it into a sharded database.  

* You can create a 
  [snapshot](../../../../server/ongoing-tasks/backup-overview#snapshot) 
  of a single shard, but not of an entire sharded database.  
  The restored shard's snapshot will create a 
  [non-sharded database](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#restore).  

* In this page:  
  * [Backup](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup)  
     * [Backup Format and Contents](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-format-and-contents)  
     * [Backup Type](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-type)  
     * [Backup Scope](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-scope)  
     * [Backup File and Folder Name](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-file-and-folder-name)  
     * [Server-Wide Backup](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#server-wide-backup)  
     * [Backup to Local and Remote Destinations](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-to-local-and-remote-destinations)

{NOTE/}

---

{PANEL: Backup}

Like non-sharded databases, sharded databases are backed up using 
backup tasks. There are severel similarities and differences between the two:  

* **Non-sharded Database Backup**  
  A complete replica of the database is kept by each node of the hosting cluster.  
  It therefore takes a **single backup task**, operated by one of the cluster nodes, 
  to create a complete database backup.  
  Only one of the cluster nodes is made 
  [responsible](../../../../server/clustering/distribution/highly-available-tasks#responsible-node) 
  for the execution of the backup task.  

* **Sharded Database Backup**  
  A part of the database is hosted by each shard.  
  It therefore takes **multiple backup tasks**, one per shard, to create 
  a complete backup of all database parts.  
   * Only one of the [shard nodes](../../../../sharding/overview#shard-replication) 
     is made responsible for the execution of this shard's backup task.  
   * Each shard stores its part of the database in a unique location.  
     Shard databases can be stored locally or on the cloud.  
   * To restore the entire database, the backup files made by all 
     the shards need to be collected and re-united.  

---

#### Backup Format and Contents

Shards backup files are formatted similarly to the `.ravendbdump` files 
created when a database is exported [using Studio](../../../../studio/database/tasks/export-database) 
or [Smuggler](../../../../client-api/smuggler/what-is-smuggler).  

Backed-up data includes both database-level and cluster-level contents, as 
for [non-sharded databases](../../../../server/ongoing-tasks/backup-overview#backup-contents).  

---

#### Backup Type

A shard backup task can create a 
[Logical backup](../../../../server/ongoing-tasks/backup-overview#logical-backup) 
or a [Snapshot](../../../../server/ongoing-tasks/backup-overview#snapshot).  

{NOTE: }
**Note**, however, that a `snapshot` backup of a shard 
[cannot](../../../../sharding/server/ongoing-tasks/backup-and-restore/restore#restore) 
be directly restored to a sharded database.  
The snapshot of a shard **can** be restored as a new non-sharded database.  
{NOTE/}

`Logical` shard backups **can** be restored to either a sharded or a non-sharded database.  

---

#### Backup Scope

A shard backup task can create 
a [Full backup](../../../../server/ongoing-tasks/backup-overview#full-backup) 
with the entire content of the shard, or an 
[Incremental Backup](../../../../server/ongoing-tasks/backup-overview#incremental-backup) 
with just the difference between the current database data and the last backed-up data.  

---

#### Backup File and Folder Name

Shard backup folders and files are generally named 
[similarly](../../../../server/ongoing-tasks/backup-overview#backup-name-and-folder-structure) 
to non-sharded database backups.  

Unlike a non-sharded database, shard database folder names includes a **$** 
sign followed by the shard number.  
The backup folders of a 3-shard database named "Books", for example, will be named:  
`Books$01` for the backup folder of shard 1  
`Books$02` for the backup folder of shard 2  
`Books$03` for the backup folder of shard 3  

---

#### Server-Wide Backup

[Server-wide backup](../../../../client-api/operations/maintenance/backup/backup#server-wide-backup) 
backs up all cluster databases by creating a backup task for each database and 
executing all tasks at a scheduled time.  

A different backup task will be created for each shard of each sharded database.  
Each task will be executed by the node responsible for backup at this task's shard.  

---

#### Backup to Local and Remote Destinations

As with non-sharded databases, shards can be backed up to local and remote destinations.  

Defining the destinations is 
[no different than for non-sharded databases](../../../../client-api/operations/maintenance/backup/backup#backup-to-local-and-remote-destinations).  
When a backup task is defined for a sharded database, RavenDB will automatically 
create a task per shard. When the scheduled time arrives, each shard's responsible 
node will run the shard's task and store the files in the specified location, in 
a folder whose name [include the shard number](../../../../sharding/server/ongoing-tasks/backup-and-restore/backup#backup-file-and-folder-name).  

{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../../client-api/operations/server-wide/create-database)  

**Server**  
[External Replication](../../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Export Database](../../../../studio/database/tasks/export-database)  

