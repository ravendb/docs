﻿# Databases List View
---

{NOTE: }

* All your databases are listed in this view  
  * Database stats can be viewed  
  * Actions can be performed on each database  

* In this page:  
  * [Database Stats](../../studio/database/databases-list-view#database-stats)  
  * [Database Actions](../../studio/database/databases-list-view#database-actions)  
  * [Creating New Database Options](../../studio/database/databases-list-view#creating-new-database-options)  
  * [More Actions](../../studio/database/databases-list-view#more-actions)  
  * [Remote Database](../../studio/database/databases-list-view#remote-database)  
{NOTE/}

---

{PANEL: Database Stats}

![Figure 1. Database Stats](images/database-stats.png "Database Stats")

1. **Database state**    
   A database can be: _Online_ , _Offline_ or _Disabled_        

   * ***Online***   - the database is active and ready to use. Read and write actions can be performed.  
   * ***Offline***  - the database has not been used recently, and will become online upon access.  
   * ***Disabled*** - the database has been disabled.  
  
   * Note:  
     If the database is contained in a cluster's node that is different than the node that is currently viewed in this Studio instance, then 'Remote' will be indicated - see more [below](../../studio/database/databases-list-view#remote-database).

2. **Containing nodes**   
   These are the nodes that contain a copy of the database. 

3. **Database size**    
   The total size of the database, including both documents and indexes.

4. **Up time**        
   The time that has passed since database last came online. 

5. **Number of documents**      
   The number of documents in the database.

6. **Latest backup time**      
   The time of the latest database backup.
      
7. **Number of indexes**    
   The number of index errors will show if relevant.

8. **Alerts**    
   The number of alerts on this database if there are any.

9. **Performance hints**    
   The number of performance hints on this database if there are any.
   
{PANEL/}

{PANEL: Database Actions}

![Figure 2. Database Actions](images/database-actions-1.png "Database Actions")

1. **Create new database**  
   A new database can be created from scratch, from a backup copy, or from existing 3.x data - see more [below](../../studio/database/databases-list-view#creating-new-database-options).

2. **Manage group**  
   Click to manage the nodes that contain this database - see more about [The Database Group](../../studio/database/settings/manage-database-group).

3. **Disable/Enable the database**  
   A disabled database cannot be accessed. 

4. **More actions**  
   Pause indexing, Disable indexing, Compact database - see more [below](../../studio/database/databases-list-view#more-actions). 

5. **Refresh data**  
   Refresh the database stats data in this view from the server.

6. **Delete the database**  
{PANEL/}

{PANEL: Creating New Database Options}

![Figure 3. Creating New Database](images/database-actions-2.png "Creating New Database Options")

1. **Create new database from scratch**   
   See detailed flow [here](../../studio/database/create-new-database/general-flow)  

2. **Create new database from an existing backup copy**   
   See detailed flow [here](../../studio/database/create-new-database/from-backup)  

3. **Create new database from legacy data files**      
   Create a new database from a 3.x RavenDB version database    
   See detailed flow [here](../../studio/database/create-new-database/from-legacy-files)
{PANEL/}

{PANEL: More Actions}

![Figure 4. More Actions](images/database-actions-3.png "More Actions")

1. **Pause indexing**      
   All indexes on this database will stop indexing.
   Indexing will resume upon a restart to the server.  

2. **Disable indexing**     
   All indexes on this database will stop indexing.  
   Indexing will **not** resume upon a restart to the server.  

3. **Compact database**   
   Physically compact the database on disk.
{PANEL/}

{PANEL: Remote Database}

![Figure 5. Remote Database](images/database-actions-4.png "Remote Database")

* If the database is contained in one of the cluster's other nodes, but not in the node you are currently viewing in this instance of the studio,  
  the database state will be ***'Remote'***

* For example, in the above figure, DB2 is contained in nodes A & B. 
  DB2 is labeled ***'Remote'*** because we are currently viewing node C.  

* **Note:** Clicking on a remote database will navigate to the documents view for this database on one of its containing nodes.  
{PANEL/}
