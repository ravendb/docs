# Export Database to a .ravendbdump file

---

{NOTE: }

* Use the **Export Database** view to export data from your database to a `.ravendbdump` file.  

* `.ravendbdump` is RavenDB's format for exporting/importing a database, with backward compatibility 
  between RavenDB versions.  

* In this page:
   * [Export Database View](../../../studio/database/tasks/export-database#export-database-view)  
   * [Advanced Export Options](../../../studio/database/tasks/export-database#advanced-export-options)  

{NOTE/}

---

{PANEL: Export Database View}

The Export Database view allows you to select the data you want to export 
and export it to a `.ravendbdump` file.  

![Export Database View](images/export-database-studio-view.png "Export Database View")

1. **Export Database View**  
   Click to open the Export Database view.  
2. **Documents and Extensions**  
   Select the document entities you want to export.  
    * [Attachments](../../../document-extensions/attachments/what-are-attachments)  
    * [Counters](../../../document-extensions/counters/overview)  
    * [Time Series](../../../document-extensions/timeseries/overview)  
    * [Revisions](../../../document-extensions/revisions/overview)  
3. **Cluster Entities**  
   Select the cluster entities you want to export.  
    * [Indexes](../../../indexes/what-are-indexes)  
      You can remove [Analyzers](../../../indexes/using-analyzers) from exported indexes.  
    * [Identities](../../../client-api/document-identifiers/working-with-document-identifiers)  
    * [Compare Exchange](../../../client-api/operations/compare-exchange/overview)  
    * [Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)  
    * [Configuration and Ongoing Tasks](../../../studio/database/tasks/import-data/import-from-ravendb#customize-configuration-and-ongoing-tasks)  
4. **Include Artificial Documents**  
   Toggle on to export [Artificial Documents](../../../studio/database/indexes/create-map-reduce-index#artificial-documents--vs--regular-documents).  
5. **Include Conflicts**  
   Toggle on to export [Conflicts](../../../studio/database/documents/conflicts-view).  
6. **Encrypt exported file**  
   Toggle on to encrypt the exported `.ravendbdump` file.  
   When enabled, you'll be able to provide the encryption key that will be used.  
   please **store the key in a safe place** so you can provide it when importing 
   the stored file to decrypt the data.  

      ![Provide Encryption Key](images/export-encryption-key.png "Provide Encryption Key")

7. **Advanced**  
   Click to display [advanced export settings](../../../studio/database/tasks/export-database#advanced-export-options).  
8. **Export Database**  
   Click to export the selected data to a `.ravendbdump` file.  

{PANEL/}

{PANEL: Advanced Export Options}

![Display Advanced Options](images/export-display-advanced-settings.png "Display Advanced Options")

Click to display the advanced export options.  

---

![Advanced Export Options](images/export-advanced-settings.png "Advanced Export Options")

1. **Export all collections**  
   **Disable** to select the collections you want to export,  
   or keep **Enabled** to export all collections.  

     ![Export collections](images/export-database-advanced-collections.png "Export collections")

       * Use the **Filter** bar to display the collection whose name includes the specified string.  

2. **Use Transform script**  
   Toggle on to enter a custom JavaScript that will be executed over each exported document.  

     ![Transform Script](images/export-database-advanced-transfrom-script.png "Transform Script")

       * Sample transform javascript: 
         {CODE-BLOCK:javascript}
          var id = doc['@metadata']['@id'];
          if (id === 'orders/999')
          throw 'skip'; // filter-out
          {CODE-BLOCK/}

3. **Customize Configuration and Ongoing Tasks**  
   Toggle on to choose the components to export more specifically.  

     ![Figure 6. Advanced Export Options - Customize Configuration and Ongoing Tasks](images/export-database-advanced-configuration.png "Advanced export options - Customize Configuration and Ongoing Tasks")

4. **Export Command**  
   Click to create a **PowerShell**, **Cmd** or **Bash** command.  
   Running the command you created from the selected command line window will export your data 
   as defined by your settings.  

      ![PowerShell Command](images/export-command-powershell.png "PowerShell Command")

       **A**. Click the **Export Command** button and select the command line to prepare a command.  
       **B**. Click to copy the created export command to the clipboard.  

5. **Close Advanced**  
   Click to close the advanced export options view.  
6. **Export Database**  
   Click to export the selected data by your settings to a `.ravendbdump` file.  

{PANEL/}

## Related Articles

### Studio

- [Import from .ravendbdump](../../../studio/database/tasks/import-data/import-data-file)  

### Ongoing tasks

- [Periodic Backups](../../../studio/database/tasks/backup-task)  
- [External replications](../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
- [ETL](../../../server/ongoing-tasks/etl/basics)  

### Connection Strings

- [Connection Strings](../../../client-api/operations/maintenance/connection-strings/add-connection-string)  

