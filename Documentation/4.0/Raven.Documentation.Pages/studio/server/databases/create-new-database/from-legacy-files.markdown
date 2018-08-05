# Create a Database : From Legacy Files
---

{NOTE: }

This database creation option is designed for importing database from data files from RavenDB v3.x.

* In this page:  
  * [1. New Database From Backup](../../../../studio/server/databases/create-new-database/from-legacy-files#1.-new-database)  
  * [2. Database Name](../../../../studio/server/databases/create-new-database/from-legacy-files#2.-database-name)  
  * [3. Encryption](../../../../studio/server/databases/create-new-database/from-legacy-files#3.-encryption)  
  * [4. Replication Configuration](../../../../studio/server/databases/create-new-database/from-legacy-files#4.-replication-configuration)
  * [5. Configure Path](../../../../studio/server/databases/create-new-database/from-legacy-files#5.-configure-path)
  * [6. Create](../../../../studio/server/databases/create-new-database/from-legacy-files#6.-create)

{NOTE/}

---

{PANEL: 1. Creating New Database From Legacy Files}

![Figure 1. Create New Database From Legacy Files](images/new-database-from-legacy-1.png "Create New Database From Legacy Files")

{NOTE: }
Open the down arrow and click `New database from legacy files`.
{NOTE/}
{PANEL/}

{PANEL: 2. Database Name}

![Figure 2. Create New Database From Legacy Files - Database name](images/new-database-from-legacy-2.png "Enter Database Name")

{NOTE: }
A database name can be any sequence of characters except for the following:  

* A name cannot start or end with  ' . '  
* A name cannot exceed 230 characters  
* A name cannot contain any of the following:   /, \, :, *, ?, ", <, >, |  
{NOTE/}
{PANEL/}

{PANEL: 3. Encryption}

{NOTE: }
For **Encrypted** database - see [Encrypted Database](../../../../studio/server/databases/create-new-database/encrypted)  
{NOTE/}
{PANEL/}

{PANEL: 4. Replication Configuration}

{NOTE: }
* Note: 
The backup will be restored only to the current node 
After restore, this database can be added to other nodes using the 'Manage group' button.

Learn more about **Manage group** in : [Manage group](../../../database/settings/manage-database-group)  
{NOTE/}
{PANEL/}

{PANEL: 5. Configure Path}

![Figure 5. Create New Database From Legacy Files - Path](images/new-database-from-legacy-5.png "Configure Path")

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

  * The `DataDir` folder can be set in the `settings.json` configuration file (e.g. "DataDir": "RavenData").  
  * If `DataDir` is Not specified in the configuration file, then the database will be created under the RavenDB binaries folder  
    (where the RavenDB dlls are located).  
  * A path can't start with:  $home, '~' or 'appdrive:'
{NOTE/}
{PANEL/}

{PANEL: 6. Create}

![Figure 6. Create New Database From Legacy Files - Create](images/new-database-from-legacy-6.png "Create Database")

{NOTE: }
Click **'Create'** to finish.  
{NOTE/}
{PANEL/}
