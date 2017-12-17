## Create Database - General Flow
---
<br/>

### New Database
![Figure 1. Create New Database - Button](images/new-database-general-1.png "Create New Database Button")
{INFO: }
From the databases list view click the 'New database' button.
{INFO/}
<br/>
### Database Name
![Figure 2. Create New Database - Database name](images/new-database-general-2.png "Enter Database Name")
{INFO: }
A database name can be any sequence of characters except for the following rules:  

* Name can't start or end with a '.'  
* Name can't exceed 230 characters  
* Name can't contain any of: /,\,:,*,?,",<,>,|  
{INFO/} 
<br/>
### Configure Replication
![Figure 3. Create New Database - Replication](images/new-database-general-3.png "Configure Replication")
{INFO: }

1. **Replication Factor:**  
   Set the number of replicas your datatbase should have in the cluster.   
   The minumun required number is 1. 
   The maximum number is the cluster size (number of nodes in the cluster).

2. **Dynamic Node Distribution**   
   Upon a node failure, and if this option is checked,   
   RavenDB server will automatically replicate the database content to another available node in the cluster  
   (one that doesn't already contain the the database) so that replication facor is maintained.

3. **Setting replication nodes manually**        
   Select the specific initial replication nodes from the cluster for the database to replicate to.  
   If not checked than replication nodes will be selected randomly from the cluster.
{INFO/} 
<br/>
### Configure Path
![Figure 4. Create New Database - Path](images/new-database-general-4.png "Configure Path")
{INFO: }
Set the directory path for database data.  
If not specified then the default value will be as the database name.
{INFO/}
<br/>
### Create
![Figure 5. Create New Database - Create](images/new-database-general-5.png "Create Database")
{INFO: }
Click 'Create' to finish.   
{INFO/}
