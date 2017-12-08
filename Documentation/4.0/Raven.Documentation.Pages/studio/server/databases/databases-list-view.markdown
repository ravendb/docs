## Databases List View
---

{PANEL}
All your databases are listed in this view    
<ul>
  <li>Database stats can be viewed</li>   
  <li>Actions can be performed on each database</li>
</ul>
{PANEL/}
            
---

![Figure 1. Database Stats](images/database-stats.png "Database Stats")

{PANEL: Database Stats}
**1 - Database State**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A database can be: Online, Offline or Disabled  
<ul>
  <li>Online   - Database is active and ready to use. Read and write actions can be done.</li>  
  <li>Offline  - Database was not used recently, and will become online upon access.</li>
  <li>Disabled - Database has been disabled.</li>
</ul>

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Note: If the database is contained in one of the cluster's nodes, but not in the node that shows in this current view,  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; then _'Remote'_ will be indicated - see more below

**2 - Containing Nodes**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; These are the nodes that contain a replica of the database. 

**3 - Database Size**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total size of database, including documents and indexes.

**4 - Up time**        
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Time that has passed since database last went online. 

**5 - Number of documents**      
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The number of documents in database.

**6 - Latest backup time**      
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Time of the latest database backup.
      
**7 - Number of indexes**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The number of index errors will show if relevant.

**8 - Alerts**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; The number of alerts on this database if there are any.
{PANEL/}

---  

![Figure 2. Database Actions](images/database-actions-1.png "Database Actions")

{PANEL: Database Actions}
**1 - Create new database**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A new database can be created from scratch, from a backup copy, or from existing 3.x data - see more below

**2 - Manage group**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Click to manage the nodes that contain this database

**3 - Disable/Enable the database**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A disabled database cannot be accessed

**4 - More actions**      
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pause indexing, Disable indexing, Compact database - see more below      

**5 - Refresh data**      
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Refresh the database stats data in this view from the server  

**6 - Delete the database**
{PANEL/}

---

![Figure 3. Creating New Database](images/database-actions-2.png "Creating New Database Options")

{PANEL: Creating New Database Options}
**1 - Create new database from scratch**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; See detailed flow in: // TODO: add links to relevant page when exists.. 

**2 - Create new database from an existing backup copy**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; See detailed flow in: // TODO: add links to relevant page when exists..

**3 - Create new database from legacy data files**      
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Create a new database from a 3.x RavenDB version database    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; See detailed flow in: // TODO: add links to relevant page when exists..
{PANEL/}

---

![Figure 4. More Actions](images/database-actions-3.png "More Actions")

{PANEL:More Actions}
**1 - Pause indexing**      
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; All indexes on this database will stop indexing.
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Indexing will be resumed upon a restart to the server.  

**2 - Disable indexing**     
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; All indexes on this database will stop indexing.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Indexing will be NOT be resumed upon a restart to the server.  

**3 - Compact database**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Physically compact the database on disk
{PANEL/}

---

![Figure 5. Remote Database](images/database-actions-4.png "Remote Database")

{PANEL: Remote Database}
If the database is contained in one of the cluster's nodes but Not in the node that shows in this current view, then _'Remote'_ is indicated  

For example, in the above figure, DB2 is contained in nodes A & B  
It is indicated as _'Remote'_ since the current view is for node C

**Note:** Clicking on a remote database will navigate to the documents view for this database on its containing node 
{PANEL/}
