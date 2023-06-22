# Sharding: Import and Export
---

{NOTE: }

* Smuggler is a RavenDB interface with which data can be 
  exported from a database into a dump file and imported 
  from a dump file into a database.  
  Learn to use Smuggler [here](../client-api/smuggler/what-is-smuggler).  

* Smuggler is operated using the same API when the database 
  is sharded and when it isn't, and the same set of features 
  is available in both cases.  
  E.g., a [transform script](../client-api/smuggler/what-is-smuggler#transformscript) 
  can be used to filter and structure the transferred data 
  when the database is sharded and when it is non-sharded.  

* **Studio** can also be used to 
  [export](../studio/database/tasks/export-database) 
  and [import](../studio/database/tasks/import-data/import-data-file) 
  data from and to a sharded database, the same way it is 
  done with a non-sharded database.  
  Behind the scenes, Studio uses Smuggler to perform these operations.  

* In this page:  
  * [Export](../sharding/import-and-export#export)  
  * [Import](../sharding/import-and-export#import)  

{NOTE/}

---

{PANEL: Export}

When smuggler is called to 
[export](../client-api/smuggler/what-is-smuggler#export) 
a sharded database:  

* Data exported by the shard node that issued the call will be stored 
  locally, in a `.ravendbdump` file.  
* Data exported by other shards will be transferred to the shard 
  node that issued the call and added to the same `.ravendbdump` file.  
* The amount of time it takes to complete the export process depends 
  upon factors like the amount of data stored locally and transferred 
  from remote shards, network latency, and the user-defined transform script.  

See a code example [here](../client-api/smuggler/what-is-smuggler#example).  

{PANEL/}

{PANEL: Import}

Smuggler can be used to [import](../client-api/smuggler/what-is-smuggler#import) 
data into a database from either a [.ravendbdump](../sharding/import-and-export#export) 
file or from backup files (full or incremental).  

When data is imported into a sharded database one of the shard nodes 
is appointed **orchestrator**; the orchestrator retrieves items from 
the `.ravendbdump` or backup files, gathers the imported items into 
batches, and distributes them among the shards.  

## Importing data from a `.ravendbdump` file

There are no preliminary requirements regarding the structure 
or contents of the database the data is imported to.  

* The data can be imported into a long-existing database, 
  as well as into a database just created.  
* The data can be imported into both sharded and non-sharded databases.  
  In both cases, the data will be retrieved into the database from 
  a local `.ravendbdump` file.  
* If the database is sharded, the imported data will be distributed among the shards.  
  In case the shard has [several nodes](../sharding/overview#shard-replication), 
  each shard database will be replicated to all the nodes of this shard.  

See a code example [here](../client-api/smuggler/what-is-smuggler#example-1).  

## Importing data from backup files

* Backup files are given an extension that reflects the backup type.  
  A full-backup file, for example, will be given a `.ravendb-full-backup` 
  extension.  
  Regardless of the extension, the internal structure of backup 
  files is similar to that of `.ravendbdump` files.  
  It **is** therefore possible to import backup files.  
* Unlike the [restore](../client-api/operations/maintenance/backup/restore) operation, 
  which creates a new database from the backup files, importing 
  a backup file adds the imported data to data already stored in 
  the database.  
* Import can be helpful in the following cases, among others:  
   * a backup created for a non-sharded database can be imported 
     into a sharded database.  
     This allows the migration of data from a non-sharded database 
     to a sharded one.  
   * When a sharded database is backed up, each shard creates a backup 
     of its own database. If you want to split the sharded database 
     into multiple databases, you can simply import the backup of each 
     shard into a newly created (sharded or not) database.  

{NOTE: }
[Incremental backups](../server/ongoing-tasks/backup-overview#backup-scope:-full-or-incremental) 
contain only the changes that have been made in the database since the last full backup.  
Import the full backup first, and then the incremental backups that complement it.  
{NOTE/}

{PANEL/}

## Related articles

### Client API
- [Smuggler](../client-api/smuggler/what-is-smuggler)  

### Server
- [Backup Overview](../server/ongoing-tasks/backup-overview)  
- [Incremental backup](../server/ongoing-tasks/backup-overview#backup-scope:-full-or-incremental)
- [Responsible Node](../server/clustering/distribution/highly-available-tasks#responsible-node)  

### Studio
- [Export Data](../studio/database/tasks/export-database)  
- [Import Data](../studio/database/tasks/import-data/import-data-file)  

### Sharding
- [Creating a Sharded DB](../sharding/administration/studio-admin#creating-a-sharded-database)  
