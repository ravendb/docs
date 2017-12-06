## Databases List View
---

All your databases are listed in this view    
Database stats can be viewed and actions can be performed             
            
---
### Database Stats

![Figure 1. Database Stats](images/database-stats.png "Database Stats")

**1 - Database State**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A database can be: Online, Offline or Disabled  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *** _Online_   - Database is active and ready to use, read & write actions can be done  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *** _Offline_  - Database was not used recently, will become online upon access. Documents are unloaded from memory  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *** _Disabled_ - Database can't be accesed   

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Note: If the database is not contained in the node that shows in this view, then _'Remote'_ will be indicated - see more below

**2 - Containing Nodes**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; These are the nodes that contain a replica of the database 

**3 - Database Size**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Total size of database, including documents and indexes 

**4 - Up time**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Time that has passed since database last went online 

**5 - Number of documents**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number of douments in database

**6 - Latest backup time**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Time of latest database backup
      
**7 - Number of indexes**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number of index errors will show if relevant

**8 - Alerts**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number of alerts on this database is exist

---
### Database Actions  

![Figure 2. Database Actions](images/database-actions-1.png "Database Actions")

**1 - Create new database**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A new database can be created from scratch, from a backup copy, or from an existing file - see more below

**2 - Manage group**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Click to manage the nodes that contain this database

**3 - Disable/Enable the database**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; A disabled database cannot be accessed

**4 - More actions**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pause indexing, Disable indexing, Compact database - see more below      

**5 - Refresh data**  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Refresh the database stats data in this view from the server  

**6 - Delete the database**   


---
### Creating New Database Options

![Figure 3. Creating New Database](images/database-actions-2.png "Creating New Database Options")


**1 - Create new database from scratch**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; See detailed flow in: // TODO: add links to relevant page when exists.. 


**2 - Create new database from an existing backup copy**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; See detailed flow in: // TODO: add links to relevant page when exists..


**1 - Create new database from legacy data files**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Create a new database from a 3.x RavenDB version database    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; See detailed flow in: // TODO: add links to relevant page when exists..


---
### More Actions

![Figure 4. More Actions](images/database-actions-3.png "More Actions")

**1 - Pause indexing**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; All indexes on this database will stop indexing  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Indexing will be resumed upon a restart to the server  

**2 - Disable indexing**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; All indexes on this database will stop indexing  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Indexing will be NOT be resumed upon a restart to the server  

**3 - Compact database**    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Physically compact the database on disk


---
### Remote Database

![Figure 5. Remote Database](images/database-actions-4.png "Remote Database")

**Note:** Clicking on a remote database will navigate to the documents view for this database on its containing node 

