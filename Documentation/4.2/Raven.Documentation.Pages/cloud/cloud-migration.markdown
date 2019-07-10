#Cloud: Migration
---

{NOTE: }

* Databases can be migrated between any two instances of RavenDB - including between an on-premises server and a cloud server, and [between 
versions 3.x and 4.x of RavenDB](../migration/client-api/introduction).  
* In this page  
  * [Import From Live RavenDB instance](cloud-migration#import-from-live-ravendb-instance)  
  * [Import From File](cloud-migration#import-from-file)  

{NOTE/}

---

{PANEL: Import From Live RavenDB Instance}

One good way to migrate your database is with the 
[import data from RavenDB](../studio/database/tasks/import-data/import-from-ravendb) operation. To do this, the source server needs to 
have the cluster certificate from the destination server:  

**1.** Open the [management studios](../studio/overview) for your source and destination servers. The studio is available at a RavenDB 
server's URL:  

!["Server URLs"](images\migration-001-urls.png "Server URLs")  

**2.** In each studio, click on `Manage certificates`:  

!["Manage Certificates"](images\migration-002-manage-certificates.png "Manage Certificates")  

**3.** Export the `Cluster certificate` from the ***destination*** server:  

!["Cluster Certificate"](images\migration-003-cluster-certificate.png "Cluster Certificate")  

**4.** Import that certificate as a `Client certificate` in the ***source*** server:  

!["Client Certificate"](images\migration-004-client-certificate.png "Client Certificate")  

**5.** Configure the client certificate's `database permissions` to include the database whose data you want to migrate:  

!["Database Permissions"](images\migration-005-database-permissions.png "Database Permissions")  

**6.** In the destination server, select an empty database and go to `Settings`>`Import Data`:  

!["Import Data"](images\migration-006-import-data.png "import data")  

**7.** Enter the URL of the source server. Options will appear for fine-tuning which data is migrated. When you're done, 
click `Migrate Database`:  

!["Import Options"](images\migration-007-options.png "Import Options")  

{PANEL/}

{PANEL: Import From File}
  
Another option is to [export a database](../studio/database/tasks/export-database) from the source server in the 
form of a `.ravenDBDump` file and upload it to another database with the 
[import data from file](../studio/database/tasks/import-data/import-data-file) operation. This option doesn't require
passing certificates:  

**1.** In the source server, select a database to export and go to `Settings`>`Export Database`. After fine-tuning 
which data to export, click `Export Database`.  
**2.** In the destination server, go to `Settings`>`Import Data`, and click on the `From file (.ravendbdump)` tab. 
Select the file and click `Import Database`.  

{PANEL/}

##Related Articles

**Cloud**  
[Overview](cloud-overview)  
[Backup](cloud-backup)  
[Security](cloud-security)  

**Studio**  
[Studio Overview](../studio/overview)  
[Import Data From RavenDB](../studio/database/tasks/import-data/import-from-ravendb)  
[Export Database](../studio/database/tasks/export-database)  
[Import Data From File](../studio/database/tasks/import-data/import-data-file)  

**Migration**  
[Introduction to 3.x to 4.0 Migration](../migration/client-api/introduction)  
[How to Migrate Data from 3.x Server to 4.0](../migration/server/data-migration)  
