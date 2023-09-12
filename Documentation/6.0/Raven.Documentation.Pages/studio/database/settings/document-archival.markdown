# Document Archival
---

{NOTE: }

* RavenDB documents can be [archived](../../../server/extensions/archival); 
  archiving, and other features' usage of it, can ramp up database performance.  
  Most notably, the default behavior of indexing and of data subscriptions is 
  to skip the processing of archived documents, so the number of indexed and 
  transferred documents can be significantly reduced.  
* An archived document is marked as such by a metadata property, `@archived: true`, 
  so database features, clients, and users would be able to recognize its 
  status and handle it accordingly. Another important metadata property, 
  `@archive-at`, is added a time signature as a value and schedules the 
  document for future archival.  
* In this page, we explain how to configure document archival using Studio.  

{NOTE/}

---

{PANEL: Archival View: Enable Archival}

![Figure 1. Data Archival View](images/data-archival-01.png "Data Archival View")

1. Click to open the settings view.  

2. Click to open the Data Archival view.  

3. Toggle to enable archiving on this ("sampleDB") database.  

4. Toggle (available only when option #3 above is enabled) to provide the frequency 
   by which RavenDB's archiving task scans the database for documents that need to 
   be archived.  
   Default frequency: 60 seconds  
   ![Archival Task Scanning Frequency](images/data-archival-02.png "Archival Task Scanning Frequency")

5. Click to apply your settings.  

6. Click for help using this view.  

7. Click for Licensing info.  

{PANEL/}

{PANEL: Document View: Archiving Properties}

To schedule a document's archival time open the document view and edit the 
document you want to archive.  

![Figure 2. Document View: @archive-at Property](images/data-archival_archive-at-property.png "Document View: @archive-at Property")

1. Add this property to the document's metadata.  
2. Provide the archival time (`UTC` time) as a value.  
3. After adding the `@archive-at` property, the time that remains until the document is archived is displayed here.  

---

![Figure 3. Document View: @archived Property](images/data-archival_archived-property.png "Document View: @archived Property")

1. The document is archived.  
   RavenDB replaced the `@archive-at` flag with an `@archived` flag, 
   indicating that the document is now archived.  

2. Studio recognizes that the document is archived.  

---

![Figure 4. Documents List: Archived Icon](images/data-archival_documents-list-archived-icon.png "Documents List: Archived Icon")

An icon was added to the icons list so archived documents can be recognized at a glance.  

{PANEL/}

{PANEL: Database Settings: Archival Configuration Options}

Open the [Database Settings](../../../studio/database/settings/database-settings) 
view to set feature-specific archiving settings.  
This is where different features, currently indexing and data subscriptions, 
are taught how to handle archived documents.  

![Figure 5. Database Settings: Feature-Specific Archival Configuration](images/data-archival_database-settings-01.png "Database Settings: Feature-Specific Archival Configuration")

1. Click to enter text  
   Type "Archive" to list configuration options related to archiving.  
   The three related options are:  
   [Auto Indexing Handling of Archived Documents](../../../server/extensions/archival#section)  
   [Static Indexing Handling of Archived Documents](../../../server/extensions/archival#section-1)  
   [Data Subscriptions Handling of Archived Documents](../../../server/extensions/archival#section-2)  

2. Click to configure  
   {NOTE: }
   Please edit configuration options only if you're sure you understand them.  
   {NOTE/}

---

![Figure 6. Database Settings: Edit Configuration](images/data-archival_database-settings-02.png "Database Settings: Edit Configuration")

1. Click a feature-specific configuration option to edit its settings.  
2. Toggle to override the current configuration.  
3. Select one of the three options.  
    * `ExcludeArchived`  
      DO not process archived documents  
    * `IncludeArchived`  
      Process archived documents  
    * `ArchivedOnly`
      Process ONLY archived documents  
4. Save your configuration.

{PANEL/}

{PANEL: Index-Specific Archival Configuration}

To set the way a specific index handles archived documents (overriding default 
server/database configuration), open the [Index List view](../../../studio/database/indexes/indexes-list-view) 
and select the index whose behavior you want to set, scroll down for the index 
properties and open the **Archived Data** tab.  

![Figure 7. Index Definition Archived Data Tab](images/data-archival_index-definition-archived-data.png "Index Definition Archived Data Tab")

1. Click to open the Archived Data tab.  
2. Click to determine how this index would handle archived documents.  
   
      ![Archived Docs Handling Options](images/data-archival_archived-docs-handling-options.png "Archived Docs Handling Options")

      Select how this index handles archived documents.  
      **Default** - Do not change the policy set by higher (database/server) configuration levels.  
      **Exclude Archived** - Do not process archived documents (this is the default configuration).  
      **Include Archived** - Process archived documents.  
      **Archived Only** - Process **only** archived documents.  

{PANEL/}

{PANEL: Subscription-Specific Archival Configuration}

To set the way a specific data subscription task handles archived documents (overriding default 
server/database configuration), open the [Subscription Task view](../../../studio/database/indexes/indexes-list-view) 
and create a new subscription.  

![Figure 8. Subscription Archived Data Processing Behavior](images/data-archival_data-subscription-definition-archived-data.png "Subscription Archived Data Processing Behavior")

1. Click to select how this subscription handles archived documents.  
   
      ![Archived Docs Handling Options](images/data-archival_archived-docs-handling-options.png "Archived Docs Handling Options")

      **Default** - Do not change the policy set by higher (database/server) configuration levels.  
      **Exclude Archived** - Do not process archived documents (this is the default configuration).  
      **Include Archived** - Process archived documents.  
      **Archived Only** - Process **only** archived documents.  

{PANEL/}

{PANEL: Data Export/Import Archival Configuration}

To determine whether a data-export task would export archived documents, 
enter the task's configuration (by creating a new task or editing an existing 
one) and toggle the **Include Archived Documents** option as preferred.  

![Figure 9. Data Export Option](images/data-archival_export-task-option.png "Data Export Option")

---

To determine whether a data-import task would import archived documents, 
enter the task's configuration (by creating a new task or editing an existing 
one) and toggle the **Include Archived Documents** option as you prefer.  

![Figure 10. Data Import Option](images/data-archival_import-task-option.png "Data Import Option")

{PANEL/}


## Related Articles

### Extensions
- [Document Archival](../../../server/extensions/archival)  
- [Document Expiration](../../../server/extensions/expiration)  

### Configuration
- [Overview](../../../server/configuration/configuration-options#settings.json)  
- [Database Settings](../../../studio/database/settings/database-settings#view-database-settings)  

### Tasks
- [Smuggler (Import/Export)](../../../client-api/smuggler/what-is-smuggler) 
- [ETL Basics](../../../server/ongoing-tasks/etl/basics)  
- [Regular Replication](../../../server/clustering/replication/replication)  
- [External Replication](../../../server/ongoing-tasks/external-replication)  
- [Hub/Sink Replication](../../../server/ongoing-tasks/hub-sink-replication)  

### Patching
- [Patch By Query](../../../client-api/rest-api/queries/patch-by-query)  
