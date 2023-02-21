# Sharding: Import and Export
---

{NOTE: }

* Smuggler is a RavenDB interface with which data can be 
  exported from a database into a dump file and imported 
  from a dump file into a database.  
  Learn to use Smuggler [here](../../../client-api/smuggler/what-is-smuggler).  

* From a user's perspective Smuggler is operated using the 
  same API when the database is sharded and when it isn't, 
  and the same set of features is available in both cases.  
  E.g., a [transform scripts](../../../client-api/smuggler/what-is-smuggler#transformscript) 
  can be used to filter and structure the transferred data 
  when the database is sharded and when it is non-sharded.  

* **Studio** can also be used to 
  [export](../../../studio/database/tasks/export-database) 
  and [import](../../../studio/database/tasks/import-data/import-data-file) 
  data from and to a sharded database, the same way it is 
  done with a non-sharded database.  
  Behind the scenes, Studio uses Smuggler to perform these operations.  

* In this page:  
  * [Export](../../../sharding/client-api/smuggler/import-and-export#export)  
  * [Import](../../../sharding/client-api/smuggler/import-and-export#import)  

{NOTE/}

---

{PANEL: Export}

When smuggler is called to 
[export](../../../client-api/smuggler/what-is-smuggler#export) 
a sharded database:  

* Data exported by the shard node that issued the call will be stored 
  locally, in a `.ravendbdump` file.  
* Data exported by other shards will be streamed to the shard node 
  that issued the call, and added to the same `.ravendbdump` file.  
* The amount of time it would take to complete the export will depend, 
  among other factors, on the user-defined transform script, and on 
  the amount of data stored locally and transferred from remote shards.  

{PANEL/}

{PANEL: Import}

Smuggler can be used to [import](../../../client-api/smuggler/what-is-smuggler#import) 
data into a database from either a [.ravendbdump](../../../sharding/client-api/smuggler/import-and-export#export) 
file or from backup files (full or incremental).  

* **Importing data from a `.ravendbdump` file**  
  There are no preliminary requirements regarding the structure 
  or contents of the database the data is imported to.  
   * The data can be imported into a long-existing database, 
     as well as into database that was created from scratch 
     just before the import.  
   * The data can be imported into both sharded and non-sharded databases.  
     In both cases, the data will be retrieved into the database from 
     a local `.ravendbdump` file.  
     If the database is sharded, the data will be distributed among the shards.  
     If the database is non-sharded, the local database will be replicated.  

* **Importing data from backup files**  
   * Backup files are given an extension that reflects the backup type.  
     A full-backup file, for example, will be given a `.ravendb-full-backup` 
     extension.  
     Regardless of the extension, though, the internal structure of backup 
     files is simlar to that of `.ravendbdump` files.  
   * It **is** therefore possible to import backup files using Smuggler, 
     including those created for a sharded database.  
   * This can be helpful in many ways. We can, for example, divide the 
     data that is currently held by a sharded database, between several 
     databases, by creating new databases and importing into each of them 
     the backup made by a single shard.  

{PANEL/}

## Related articles

**Client API**  
[Create Database](../../../client-api/operations/server-wide/create-database)  
[Smuggler Import](../../../client-api/smuggler/what-is-smuggler#import)  
[RestoreBackupOperation](../../../client-api/operations/maintenance/backup/restore#restoring-a-database:-configuration-and-execution)  

**Server**  
[External Replication](../../../server/ongoing-tasks/external-replication)  

**Studio**  
[Backup Task](../../../studio/database/tasks/backup-task)
[Export Database](../../../studio/database/tasks/export-database)  
[Import](../../../studio/database/tasks/import-data/import-data-file)  
