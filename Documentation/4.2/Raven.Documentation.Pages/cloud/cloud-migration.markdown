#Cloud: Migration
---

{NOTE: }

Data can be migrated between any two instances of RavenDB, whether they are a local instance or a cloud instance.

{NOTE/}

---

{PANEL: }

One good way to do this is with the 
[import data from RavenDB](../studio/database/tasks/import-data/import-from-ravendb) operation:  

* Open the management studios for your source and destination servers. The studio is available at a RavenDB server's 
URL.  
* In each studio, click on `Manage certificates`:  
  
![](images\CloudScaling_ManageCertificates.png)  
  
* Export the `Cluster certificate` from the *destination* server:  
  
![](images\import-from-raven-export-server-certificate.png)  
  
* Import that certificate as a `Client certificate` in the *source* server:  
  
![](images\import-from-raven-upload-server-cert-as-client-cert.png)  
  
* In the destination server, select an empty database in the destination server, go to `Settings`>`Import Data`, 
and click on the `From RavenDB` tab.  
* Enter the URL of the source server. Options will appear to fine-tune which data is migrated. When you're done, 
click `Migrate Database`:  
  
![](images\import-from-ravendb-options.png)  
  
Another option is to [export a database](../studio/database/tasks/export-database) from the source server in the 
form of a `.ravenDBDump` file and upload it with the 
[import data from file](../studio/database/tasks/import-data/import-data-file) operation:

* In the source server, select a database to export and go to `Settings`>`Export Database`. After fine-tuning 
which data to export, click `Export Database`.  
* In the destination server, go to `Settings`>`Import Data`, and click on the `From file (.ravendbdump)` tab. 
Select the file and click `Import Database`.  

{PANEL/}
