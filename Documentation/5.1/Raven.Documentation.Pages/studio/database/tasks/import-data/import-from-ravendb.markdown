# Import Data from a Live RavenDB Server
---

{NOTE: }

* In addition to importing data from a [.ravendbdump](../../../../studio/database/tasks/import-data/import-data-file) file,  
  data can also be imported directly from an existing database on a **live RavenDB server**.  

* Backward compatibility is supported.  
  You can import a database from a previous RavenDB version into your current server.  

* The process consists of the following:  
  1. [Prepare Servers for the Import Process](../../../../studio/database/tasks/import-data/import-from-ravendb#step-#1:-prepare-servers-for-the-import-process-(secure-4.x-or-newer-servers-only)) (only when importing from a _secure 4.x or newer server_)  
  2. [Access the Import View](../../../../studio/database/tasks/import-data/import-from-ravendb#step-#2:-access-the-import-view)  
  3. [Set the Source Server URL and database](../../../../studio/database/tasks/import-data/import-from-ravendb#step-#3:-set-the-source-server-url-and-database)
  4. [Set Import Options](../../../../studio/database/tasks/import-data/import-from-ravendb#step-#4:-set-import-options)
  5. [Advanced Import Options](../../../../studio/database/tasks/import-data/import-from-ravendb#step-#5:-advanced-import-options)
{NOTE/}

---

{PANEL: Step #1: Prepare Servers for the Import Process (Secure 4.x or newer Servers Only)}

* Before migrating data from a secure source server, the destination server must also be secure.  
  You must pass the certificate from the destination to the source server.  The following steps explain how.

* Perform this step only if your source RavenDB server is a secure verion 4.x (or newer) server that is running on HTTPS.  For other servers, skip this step and continue to [Step 2](../../../../studio/database/tasks/import-data/import-from-ravendb#step-#2:-access-the-import-view).

* To import, the destination server needs to access the existing source database and fetch data from it.  
  To grant such access, you must first register the destination server certificate as a client certificate on your source server.  

* To accomplish this, follow these steps:

![Figure 1. Manage Server-Certificates View](images/importing-exporting-certificates.png "Studio Manage Server-Certificates View")

 A. Click the **Manage Server** tab.  

 B. Select **Certificates**.  
   (See [Certificates Management](../../../../server/security/authentication/certificate-management)) for more information on this Studio view.  
 
 C. Export the destination RavenDB certificate **from the DESTINATION RavenDB server**.  
  ![Figure 2. Export server certificate](images/import-from-raven-export-server-certificate.png "Export the destination server certificate")  

   * Choose **Export Cluster Certificates** option from the **Cluster Certificate** dropdown.  
 
 D. Register this certificate on the **SOURCE RavenDB server**  
  ![Figure 3. Register exported certificate as client certificate](images/import-from-raven-upload-server-cert-as-client-cert.png "Register exported certificate as client certificate")  

   * Choose **Upload client certificate** to upload the exported certificate as the client certificate.  
 
 E. Set the certificate details.  
  ![Figure 4. Import certificate details](images/import-from-raven-upload-server-cert-as-client-cert-details.png "Set certificate details")

1. **Name**  
   Enter a name for this certificate. For future clarity, consider naming each certificate after the role that it will enable in your system (Full Stack Development, HR, Customer, Unregistered Guest, etc...)  
2. **Security Clearance**  
   Set [authorization level](../../../../server/security/authorization/security-clearance-and-permissions) for this certificate. Read about [Security Clearance](../../../server/security/authorization/security-clearance-and-permissions#authorization-security-clearance-and-permissions) to choose appropriate level.  
3. **Certificate file**  
   Upload the `.pfx` certificate file from the destination server installation folder.  
4. **Certificate Passphrase**  
   (Optional) Set a password for this certificate.  
4. **Database permissions**  
   Select databases and permission levels for this certificate.  
   If you choose *User* security clearance, you can give access to specific databases on the server and configure [User](../../../server/security/authorization/security-clearance-and-permissions#user) authorization levels for this certificate.  
 
 F. Click **Upload** to complete the process.  
   The uploaded certificate will be added to the list of registered client certificates on this server.  



{PANEL/}


{PANEL: Step #2: Access the Import View}

   
![Figure 5. Databases List](images/import-from-ravendb-db-list.png "Databases List View")

 On the **destination** RavenDB server, select a **database** into which the data will be imported.  
  
 {WARNING: }
  
  Verify that this database is empty as the import will overwrite any existing content.  

{WARNING/}

---
   
![Figure 6. Import Data](images/import-from-ravendb-from-ravendb.png "Go to Import Data View")

 1. Click **Tasks** tab.  
 2. Select **Import Data**.  
 3. Select **From RavenDB**.  


{PANEL/}

---

{PANEL: Step #3: Set the Source Server URL and database}

* Specify the source server URL and select the database to import the data from.  

![Figure 8. Import configuration](images/import-from-ravendb-configuration.png "Import Configuration")

1. **Server URL**  
   Paste URL of the server you want to import from.  
2. **Server Version**  
   The version of the server that you want to import from will show here once you enter the URL.  
3. **Database Name**  
   Enter the name of the database that you want to migrate your data from.  

{PANEL/}

{PANEL: Step #4: Set Import Options}

* Filter the data you want to import.  
* Customize advanced configuration and apply a transform script under the [Advanced Import Options](../../../../studio/database/tasks/import-data/import-from-ravendb#step-#5:-advanced-import-options).

![Figure 9. Import Options](images/import-from-ravendb-options.png "Import Options")

{NOTE: }
 Toggling Import Options determines whether the following items will be imported:  
 {NOTE/}

1. 
 - [Include Documents](../../../../studio/database/documents/document-view)  
  If disabled, the following document related items will automatically be disabled too.  
   - [Include Attachments](../../../../document-extensions/attachments/what-are-attachments)  
   - [Include Counters](../../../../document-extensions/counters/overview)  
   - [Include Time Series](../../../../document-extensions/timeseries/overview)  
   - [Include Revisions](../../../../server/extensions/revisions)  
   - [Include Conflicts](../../../../client-api/cluster/document-conflicts-in-client-side)  
2. 
 - [Include Indexes](../../../../indexes/what-are-indexes)  
    - [Remove Analyzers](../../../../indexes/using-analyzers)  
3. 
 - [Include Identities](../../../../client-api/document-identifiers/working-with-document-identifiers)  
 - [Include Compare Exchange](../../../../client-api/operations/compare-exchange/overview)  
 - [Include Subscriptions](../../../../client-api/data-subscriptions/what-are-data-subscriptions)  
 - [Include Configuration and OngoingTasks](../../../../studio/database/tasks/import-data/import-from-ravendb#customize-configuration-and-ongoing-tasks) 


{NOTE:Importing an item that doesn't exist}
If any of the options is set but the source database doesn't contain any items of that type, the type will be skipped.  
{NOTE/}
{PANEL/}

{PANEL: Step #5: Advanced Import Options}

Click the **Advanced** button at the bottom of the options view for the following import features.

### Transform Script

![Figure 10. Advanced Import Options - Transform Script](images/import-from-ravendb-advanced-transform-script.png "Advanced Import Options - Transform Script")

* **Use Transform Script**:  
  When enabled, the supplied javascript will be executed on each document before importing the document.  

{CODE-BLOCK:javascript}
// Example 1
delete this['@metadata']['@change-vector']
// The script above will delete the existing change-vector from imported documents,
// New change vectors will be generated during the import.
// This is very helpful if the data is imported from a different database-group
// and you want to avoid adding old change-vector entries to a new environment.

// Example 2
this.collection = this['@metadata']['@collection'];
// This script will create a new 'collection' property in each imported document.
{CODE-BLOCK/}
{NOTE/}



### Customize Configuration and Ongoing Tasks

![Figure 11. Advanced Import Options - Customize Configuration and Ongoing Tasks](images/import-from-ravendb-advanced-configuration-ongoing-tasks.png "Advanced Import Options - Customize Configuration and Ongoing Tasks")

1. **Ongoing tasks:**

 - [Periodic Backups](../../../../studio/database/tasks/backup-task)  
 - [External replications](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
 - [ETL Tasks - Extract, Transform, Load](../../../../server/ongoing-tasks/etl/basics)  
 - [Pull Replication Sinks](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
 - [Pull Replication Hubs](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  

2. **Other:**

 - [Settings](../../../../studio/database/settings/database-settings)  
 - [Conflict Solver Configuration](../../../../client-api/operations/server-wide/modify-conflict-solver)  
 - [Revisions Configuration](../../../../client-api/operations/revisions/configure-revisions)  
 - [Document Expiration](../../../../server/extensions/expiration)  
 - [Client Configuration](../../../../studio/server/client-configuration)  
 - [Custom Sorters](../../../../indexes/querying/sorting#creating-a-custom-sorter)  

3. **Connection Strings:**

 - [Connection Strings](../../../../client-api/operations/maintenance/connection-strings/add-connection-string) used by ETL tasks to authenticate and connect to external databases will be imported.

{PANEL/}
