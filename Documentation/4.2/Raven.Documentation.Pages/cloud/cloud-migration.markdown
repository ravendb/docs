# Cloud: Migration
---

{NOTE: }

Databases can be migrated between any two instances of RavenDB, including migration between on-premises and 
cloud servers and between [different RavenDB versions](../migration/client-api/introduction).  

* In this page  
  * [Import From Live RavenDB instance](cloud-migration#import-from-live-ravendb-instance)  
  * [Import From File](cloud-migration#import-from-file)  
  * [Documents Recently Deleted from Source Database](cloud-migration#documents-recently-deleted-from-source-database)  

{NOTE/}

---

{PANEL: Import From Live RavenDB Instance}

You can easily migrate your database using the [import data from RavenDB](../studio/database/tasks/import-data/import-from-ravendb) operation.  
In order to do so, the **source server** needs to have the **destination server's cluster certificate**.  

Open the [Management Studio](../studio/overview) of each server.  
Each server's Studio is available in the [Portal](../cloud/portal/cloud-portal#cloud-portal)'s 
[Product tab](../cloud/portal/cloud-portal-products-tab#cloud-account-portal-products).  

Learn how to [install the certificate and access RavenDB Cloud Studio](../cloud/cloud-overview#ravendb-studio---graphic-user-interface) if this is your first time using Cloud Studio.

If you have already installed the certificate and accessed Studio, 
click the URLs in the Products tab to open a Studio GUI for each node.  

!["Server URLs"](images\migration-001-urls.png "Server URLs")

---

In each studio, select server dashboard and click **Manage certificates**.  

!["Manage Certificates"](images\migration-cloud-studio-manage-certificates.png "Manage Certificates")


---

Export the **destination server**'s **Cluster certificate**.  

!["Cluster Certificate"](images\migration-003-cluster-certificate.png "Cluster Certificate")

---

Import the certificate as a **Client Certificate** by the **source server**.  

!["Client Certificate"](images\migration-004-client-certificate.png "Client Certificate")

---

Configure the Client Certificate's **Database Permissions** to include the database whose data you want to migrate.  

!["Database Permissions"](images\migration-005-database-permissions.png "Database Permissions")

---

In the Destination Server, create or select an empty database and open its **Tasks --> Import Data** option.  

!["Import Data"](images\migrating-data-from-ravendb-steps.png "import data")

1. Select the **Tasks** tab.  
2. Select **Import Data**.  
3. Select the **From RavenDB** tab.  
4. Select the destination server.  
5. **Make sure that you are not writing over data that you want to keep**. One option is [to start a new database with the studio](https://ravendb.net/docs/article-page/5.2/csharp/studio/database/create-new-database/general-flow).  If you create a new one, choose the new database server in step 4 (above).  
6. Enter the URL of the source server and choose which data to migrate.  
7. Click **Migrate Database**  



{PANEL/}

{PANEL: Import From File}
  
Another option is to [export a database](../studio/database/tasks/export-database) from the source server in the 
`.ravenDBDump` format, and upload it to another database using the 
[import data from file](../studio/database/tasks/import-data/import-data-file) operation.  
This option doesn't require passing certificates:  

---

#### First export the data from source server  

!["Export Data to File"](images\studio-view-export-database-tofile-steps.png "Export Data to File")

1. In the source server, select a database to export and go to **Tasks tab**.  
2. Select **Export Database**.  
3. Change the destination file name if you'd prefer (optional).  
4. Select desired options.  
 Note that **Encrypt Exported File** is off by default.  
5. After choosing which data to export, click **Export Database**.  

---

#### Next import the data to destination server from file

!["Import Data from File"](images\studio-view-import-fromfile-steps.png "Import Data from File")

1. In the destination server, go to **Tasks** tab.  
2. Select **Import Data**.  
3. **Make sure that you are not writing over data that you want to keep**.  
 One option is [to start a new database with the studio](https://ravendb.net/docs/article-page/5.2/csharp/studio/database/create-new-database/general-flow).  
4. Select the `.ravendbdump` file that you previously exported from the source server.  
5. Select desired options.  
 **If you encrypted while exporting** make sure to select **imported file is encrypted**.  
6. Click **Import Database**.  

{PANEL/}

##Documents Recently Deleted from Source Database  

* If you've deleted documents from the source database in the last few minutes before live-importing, 
the deleted documents will still appear in the destination database. 
This is because after deleting a document, a [tombstone](../glossary/tombstone) is left behind as a signal for backups and 
various behind-the-scene processes.  
* Once all of these processes have been completed, 
the tombstones are cleaned. They are cleaned every 5 minutes by default. You can configure the [tombstone cleaner time intervals](../server/configuration/tombstone-configuration).   
* After they are cleaned, performing another live-import will show that the documents have been deleted.  
  


##Related Articles

**Cloud**  
[Overview](../cloud/cloud-overview)  
[Backup And Restore](../cloud/cloud-backup-and-restore)  
[Security](../cloud/cloud-security)  
  
[Portal](../cloud/portal/cloud-portal)  
[Tiers and Instances](../cloud/cloud-instances)  
  
[RavenDB on Burstable Instances](https://ayende.com/blog/187681-B/running-ravendb-on-burstable-cloud-instances)  
[AWS CPU Credits](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/burstable-credits-baseline-concepts.html)  

**Studio**  
[Studio Overview](../studio/overview)  
[Import Data From RavenDB](../studio/database/tasks/import-data/import-from-ravendb)  
[Export Database](../studio/database/tasks/export-database)  
[Import Data From File](../studio/database/tasks/import-data/import-data-file)  

**Migration**  
[Introduction to 3.x to 4.0 Migration](../migration/client-api/introduction)  
[How to Migrate Data from 3.x Server to 4.0](../migration/server/data-migration)  
