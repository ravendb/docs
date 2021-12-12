# Export Database to a .ravendbdump file

A `.ravendbdump` file is RavenDB's format for exporting/importing a database, with backward compatibility between RavenDB versions.  
To export a `.ravendbdump` file, we need an existing source database which you can then import into an empty destination database.  

 In this page:

  * [Export Database to a .ravendbdump file](../../../studio/database/tasks/export-database#export-database-to-a-.ravendbdump-file)  
  * [Export options](../../../studio/database/tasks/export-database#export-options)  
    
  * [Advanced Export Options](../../../studio/database/tasks/export-database#advanced-export-options)  
    * [Export all collections](../../../studio/database/tasks/export-database#export-all-collections)  
    * [Transform Script](../../../studio/database/tasks/export-database#transform-script)  
    * [Customize Configuration and Ongoing Tasks](../../../studio/database/tasks/export-database#customize-configuration-and-ongoing-tasks)  
    * [Export Command via PowerShell](../../../studio/database/tasks/export-database#export-command-via-powershell)  

![Figure 1. Choose and Export Database](images/export-database-studio-view.png "Choose and Export Database")  

1. Select the database to be exported.  
 It can be selected at the top of the page or by choosing from the detailed list of databases.  
2. Select the `Tasks` tab.  
3. Select `Export Database`.  
  
## Export options 

Here you can filter the data you want to export, add encryption, select collections and configurations, and apply a transform script on your documents.  

![Figure 3. Export Options](images/export-database-options.png "Export Options")

{NOTE:Import settings for items that don't exist in source database}
If any of the options is set but the source database doesn't contain any items of that type, the item will be skipped.
{NOTE/}

1. [Include Documents](../../../studio/database/documents/document-view)  
   If disabled, the following document related items will automatically be excluded too.  
    - [Include Attachments](../../../document-extensions/attachments/what-are-attachments)  
     Determines whether or not legacy attachments contained in the file should be imported where legacy attachments refers to v2.x and v3.x attachments.  
    - [Include Counters](../../../document-extensions/counters/overview)  
    - [Include Artificial Documents](../../../studio/database/indexes/create-map-reduce-index#artificial-documents--vs--regular-documents)  
    - [Include Revisions](../../../server/extensions/revisions)  
    - [Include Conflicts](../../../client-api/cluster/document-conflicts-in-client-side)  

2. [Include Indexes](../../../indexes/what-are-indexes)  
    - [Remove Analyzers](../../../indexes/using-analyzers)  
    - [Include Identities](../../../client-api/document-identifiers/working-with-document-identifiers)  
    - [Include Compare Exchange](../../../client-api/operations/compare-exchange/overview)  
    - [Include Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)  
    - [Include Configuration and OngoingTasks](../../../studio/database/tasks/import-data/import-from-ravendb#customize-configuration-and-ongoing-tasks)  

3. [Imported file is encrypted](../../../server/security/overview#encryption)  
 Includes the decryption key for your destination database to handle encrypted data.  

## Advanced export options

### Export all collections

![Figure 4. Advanced Export Options - Export all collections](images/export-database-advanced-collections.png "Advanced export options - Export all collections")

- **Export all collections**:  
  Determines whether to export All database collections.  
    - If _Export all collections_ is disabled, a list of all database collections will be displayed with an option to filter collections by name.  

---

### Transform Script

![Figure 5. Advanced Export Options - Transform Script](images/export-database-advanced-transfrom-script.png "Advanced export options - Transform Script")

- **Use Transform Script**:  
  Enabling it allows you to provide a transform javascript, that would operate on each document contained by the file.  

{CODE-BLOCK:javascript}
var id = doc['@metadata']['@id'];
if (id === 'orders/999')
    throw 'skip'; // filter-out
{CODE-BLOCK/}

---

### Customize Configuration and Ongoing Tasks

![Figure 6. Advanced Export Options - Customize Configuration and Ongoing Tasks](images/export-database-advanced-configuration.png "Advanced export options - Customize Configuration and Ongoing Tasks")

**Ongoing tasks**:  

- [Periodic Backups](../../../studio/database/tasks/backup-task)  
- [External replications](../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
- [ETL Tasks - Extract, Transform, Load](../../../server/ongoing-tasks/etl/basics)  
- [Pull Replication Sinks](../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
- [Pull Replication Hubs](../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  

**Other:**

- [Settings](../../../studio/database/settings/database-settings)  
- [Conflict Solver Configuration](../../../client-api/operations/server-wide/modify-conflict-solver)  
- [Revisions Configuration](../../../client-api/operations/revisions/configure-revisions)  
- [Document Expiration](../../../server/extensions/expiration)  
- [Client Configuration](../../../studio/server/client-configuration)  
- [Custom Sorters](../../../indexes/querying/sorting#creating-a-custom-sorter)  

**Connection Strings**

- [Connection Strings](../../../client-api/operations/maintenance/connection-strings/add-connection-string) used by ETL tasks to authenticate and connect to external databases will be exported.


---

### Export Command via PowerShell

![Figure 7. Advanced Export Options - Copy command as PowerShell](images/export-command-powershell.png "Advanced export options - Copy command as PowerShell")


- Generates the commands to run the exporting logic from PowerShell.  
