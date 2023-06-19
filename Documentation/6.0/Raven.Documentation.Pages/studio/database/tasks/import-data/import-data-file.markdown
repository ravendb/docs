# Import data from .ravendbdump file

---

{NOTE: }

* `.ravendbdump` is RavenDB's format for [exportation of data](../export-database) 
  into and from files.  

* RavenDB dump files can be used to easily migrate data from one database 
  to another.  They are commonly used when the database is upgraded.  

* an exported `.ravendbdump` file can be used to migrate the data stored 
  in a non-sharded database into a sharded database.  
  Learn more about this operation in the [section dedicated to it](../../../../sharding/import-and-export#import).  

In this page:

* [Import data to destination server from file](../../../../studio/database/tasks/import-data/import-data-file#import-data-to-destination-server-from-file)  
* [Import options](../../../../studio/database/tasks/import-data/import-data-file#import-options)  
* [Advanced import options](../../../../studio/database/tasks/import-data/import-data-file#advanced-import-options)  
    * [Transform Script](../../../../studio/database/tasks/import-data/import-data-file#transform-script)  
    * [Customize Configuration and Ongoing Tasks](../../../../studio/database/tasks/import-data/import-data-file#customize-configuration-and-ongoing-tasks)  
    * [Copy command as PowerShell](../../../../studio/database/tasks/import-data/import-data-file#copy-command-as-powershell)  

{NOTE/}

---

{PANEL: }

## Import data to destination server from file
  
If you have already [exported a database](../export-database), follow these steps in the studio when you're ready to import from file.  

![Figure 1. Importing Database From File ](images/studio-view-import-fromfile-steps.png "Importing Database From File")

1. **Tasks**  
   In the destination server, click Tasks tab.  
2. **Import Data**.  
   Click to see various impoprt options.  
3. **From file (.ravendbdump)**  
   Seclect to import from a source database which has previously been [exported](../export-database).  
4. **Warning**  
   *Make sure that you are not writing over data that you want to keep*.  
   One option is [to start a new database with the studio](https://ravendb.net/docs/article-page/5.2/csharp/studio/database/create-new-database/general-flow).  
5. **Select file**  
   Select the `.ravendbdump` file that you previously exported from the source server.  
6. **Documents and Extensions**  
  Select desired [options](../../tasks/import-data/import-data-file#import-options).  
  *If you encrypted while exporting* make sure to select **imported file is encrypted**.  
  You will need to paste the encryption key that you saved when creating the encrypted database.  
7. **Advanced**  
   Click to configure [advanced import options]  
8. **Import Database**  
   Click after configuring the new database to finalize the import task.  

{PANEL/}

{PANEL: }

## Import options 

Here you can filter the data you want to import, select configuration and apply a transform script on your documents.

![Figure 2. Import Options](images/import-from-file-options.png "Import Options")


1. [Include Documents](../../../../studio/database/documents/document-view)  
   Toggle to include documents and to enable inclusion of the following document related items:  
    - [Include Attachments](../../../../document-extensions/attachments/what-are-attachments)  
    - [Include Legacy Attachments](../../../../studio/database/create-new-database/from-legacy-files)  
     Determines whether or not legacy attachments contained in the file should be imported where legacy attachments refers to v2.x and v3.x attachments.  
    - [Include Counters](../../../../document-extensions/counters/overview)  
    - [Include Legacy Files](../../../../studio/database/create-new-database/from-legacy-files)  
    - [Include Artificial Documents](../../../../studio/database/indexes/create-map-reduce-index#artificial-documents--vs--regular-documents)  
    - [Include Expired Documents](../../../../server/extensions/expiration)  
    - [Include Revisions](../../../../document-extensions/revisions/overview)  
    - [Include Conflicts](../../../../client-api/cluster/document-conflicts-in-client-side)  
       
2. [Include Indexes](../../../../indexes/what-are-indexes)  
    - [Remove Analyzers](../../../../indexes/using-analyzers)  
    - [Include Identities](../../../../client-api/document-identifiers/working-with-document-identifiers)  
    - [Include Compare Exchange](../../../../client-api/operations/compare-exchange/overview)  
    - [Include Subscriptions](../../../../client-api/data-subscriptions/what-are-data-subscriptions)  
    - [Include Configuration and OngoingTasks](../../../../studio/database/tasks/import-data/import-from-ravendb#customize-configuration-and-ongoing-tasks) 
  
  
3. [Imported file is encrypted](../../../../server/security/overview#encryption)  
 Toggle to include the decryption key when importing data from encrypted file.  
 Make sure that **Encrypt exported file** option was selected when exporting from source database so that the encryption key is included.

 {NOTE:Import settings for items that don't exist in source database}
If any of the options is set but the source database doesn't contain any items of that type, the item will be skipped.
{NOTE/}

{PANEL/}

{PANEL: }

## Advanced import options

Click the **Advanced** button at the bottom of the options view for the following import features.

### Transform Script

In the Studio, select **database** > click **Task** tab > select **Import** > select **From file (.ravendbdump)** > click the **Advanced** button at the bottom > toggle **Use Transform script**.  

![Figure 3. Advanced Import Options - Transform Script](images/import-from-file-advanced-transform-script.png "Advanced Import Options - Transform Script")

- **Use Transform Script**  
  Enabling this **advanced** option allows you to provide a transform javascript, that would operate on each document imported from the file.  

  Sample transform javascript:  

  {CODE-BLOCK:javascript}
  delete this['@metadata']['@change-vector']
  // The script above will delete the change-vector from imported documents
  // and will generate new change vectors during import. 
  // This is very helpfull if the data is imported from a diffrent database group
  // and you want to avoid adding old change vector entries to a new environment. 
  {CODE-BLOCK/}

---

### Customize Configuration and Ongoing Tasks

This **advanced** option enables you to choose whether to import various ongoing tasks, connection strings and advanced configurations.

In the Studio, select **database** > click **Task** tab > select **Import** > select **From file (.ravendbdump)** > click the **Advanced** button at the bottom > toggle **Customize configuration and Ongoing Tasks**.  

![Figure 4. Advanced Import Options - Customize Configuration and Ongoing Tasks](images/import-from-file-advanced-configuration-ongoing-tasks.png "Advanced Import Options - Customize Configuration and Ongoing Tasks")

Toggle to include the following configurations and ongoing tasks.  
If a task is included but doesn't exist in your source database, it will be skipped.  

**Ongoing tasks:**

- [Periodic Backups](../../../../studio/database/tasks/backup-task)  
- [External replications](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
- [ETL Tasks - Extract, Transform, Load](../../../../server/ongoing-tasks/etl/basics)  
- [Pull Replication Sinks](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
- [Pull Replication Hubs](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  

**Other:**

- [Settings](../../../../studio/database/settings/database-settings)  
- [Conflict Solver Configuration](../../../../client-api/operations/server-wide/modify-conflict-solver)  
- [Revisions Configuration](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
- [Document Expiration](../../../../server/extensions/expiration)  
- [Client Configuration](../../../../studio/server/client-configuration)  
- [Custom Sorters](../../../../indexes/querying/sorting#creating-a-custom-sorter)  

**Connection Strings:**

- [Connection Strings](../../../../client-api/operations/maintenance/connection-strings/add-connection-string) used by ETL tasks to authenticate and connect to external databases will be imported.

---

### Copy command as PowerShell

In the Studio, select **database** > click **Task** tab > select **Import** > select **From file (.ravendbdump)** > click the **Advanced** button at the bottom.  

![Figure 5. Import Command Powershell](images/import-command-powershell.png "Import Command Powershell")

- Generates the commands to run the importing logic from PowerShell based on your configurations above.  

{PANEL/}

## Related Articles

### Studio
- [Import from .ravendbdump](../../../../studio/database/tasks/import-data/import-data-file)  

### Ongoing tasks
- [Periodic Backups](../../../../studio/database/tasks/backup-task)  
- [External replications](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
- [ETL](../../../../server/ongoing-tasks/etl/basics)  

### Connection Strings
- [Connection Strings](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)  

### Sharding
- [Sharding Overview](../../../../sharding/overview)
- [Sharding: Export](../../../../sharding/import-and-export#export)  
- [Sharding: Import](../../../../sharding/import-and-export#import)  
