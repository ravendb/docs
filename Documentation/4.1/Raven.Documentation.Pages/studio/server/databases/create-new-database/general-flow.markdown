# Create a Database: General Flow
---

{NOTE: }

* From the Studio, the database creation options are:  
  * **Regular** database - see [below](../../../../studio/server/databases/create-new-database/general-flow#1.-new-database)  
  * **Encrypted** database - see [Encrypted Database](../../../../studio/server/databases/create-new-database/encrypted)  
  * Create a database from a **Backup** copy - see [From Backup](../../../../studio/server/databases/create-new-database/from-backup)  
  * Create a database from a **Previous** RavenDB version database - see [From Legacy File](../../../../studio/server/databases/create-new-database/from-legacy-files)  

* In this page:  
  * [1. New Database](../../../../studio/server/databases/create-new-database/general-flow#1.-new-database)  
  * [2. Database Name](../../../../studio/server/databases/create-new-database/general-flow#2.-database-name)  
  * [3. Configure Replication](../../../../studio/server/databases/create-new-database/general-flow#3.-configure-replication)  
  * [4. Configure Path](../../../../studio/server/databases/create-new-database/general-flow#4.-configure-path)  
  * [5. Create](../../../../studio/server/databases/create-new-database/general-flow#5.-create)

{NOTE/}

---

{PANEL: 1. New Database}

![Figure 1. Create New Database - Button](images/new-database-general-1.png "Create New Database Button")

{NOTE: }
From the databases list view, click the **'New database'** button.  
{NOTE/}
{PANEL/}

{PANEL: 2. Database Name}

![Figure 2. Create New Database - Database name](images/new-database-general-2.png "Enter Database Name")

{NOTE: }

* A database name can be any sequence of **letters**, **digits**, and characters that match the **regex**: **[ _ \ - \ . ]+**  
* A name cannot exceed 128 characters  
* Spaces are not allowed  
* For example:  
  * car_orders_2018  
  * users.payments-2019  
{NOTE/}
{PANEL/}

{PANEL: 3. Configure Replication}

![Figure 3. Create New Database - Replication](images/new-database-general-3.png "Configure Replication")

{NOTE: }

1. **Replication Factor:**  
   Set the number of nodes that will contain this database.   
   The minimum required number is 1.  
   The maximum number is the cluster size (number of nodes in the cluster).  

2. **Dynamic Database Distribution**  
   Upon a node failure, and if this option is checked, the RavenDB server will automatically replicate the database content to another available node in the cluster, 
   (one that doesn't already contain the database) so that replication factor is maintained.  

3. **Setting Replication Nodes Manually**  
   Select the specific initial replication nodes from the cluster for the database to replicate to.  
   If no node is checked, then the replication nodes will be selected randomly from the cluster.  
{NOTE/}
{PANEL/}

{PANEL: 4. Configure Path}

![Figure 4. Create New Database - Path](images/new-database-general-4.png "Configure Path")

{NOTE: }

* Set the directory path for database data.  

* You can use any of the following options:  
  * **Full path** (e.g. Windows: _C:/MyWork/MyDatabaseFolder_, Linux: _/etc/MyWork/MyDatabaseFolder_ )  
    The database will be created in this physical location  
  * **Relative path** (e.g. _MyWork/MyDatabaseFolder_)  
    The database will be created under the `DataDir` folder  
  * **Leave field empty**  
    The Database will be created in `Databases` directory under the `DataDir` folder  

* Note:  

  * The `DataDir` folder can be set in the [settings.json](../../../../server/configuration/configuration-options#json) configuration file (e.g. "DataDir": "RavenData").  
  * If `DataDir` is Not specified in the configuration file, then the database will be created under the RavenDB binaries folder  
    (where the RavenDB dlls are located).  
  * A path can't start with:  $home, '~' or 'appdrive:'
{NOTE/}
{PANEL/}

{PANEL: 5. Create}

![Figure 5. Create New Database - Create](images/new-database-general-5.png "Create Database")

{NOTE: }
Click **'Create'** to finish.  
{NOTE/}
{PANEL/}

## Related Articles

**Studio Articles**:   
[Create a Database : From Backup](../../../../studio/server/databases/create-new-database/from-backup)   
[Create a Database : Encrypted](../../../../studio/server/databases/create-new-database/encrypted)      
[The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task)    

**Client Articles**:  
[Restore](../../../../client-api/operations/maintenance/backup/restore)   
[Operations: How to Restore a Database from Backup](../../../../client-api/operations/server-wide/restore-backup)    
[What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)   
[Backup](../../../../client-api/operations/maintenance/backup/backup)

**Server Articles**:  
[Backup Overview](../../../../server/ongoing-tasks/backup-overview)

**Migration Articles**:  
[Migration](../../../../migration/server/data-migration)
