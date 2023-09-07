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

4. Toggle (available only when 3 is enabled) to enter the frequency by which the server 
   scans the database for documents that should be archived.  
   Default configuration: 60 seconds.  

5. Click to save your settings.  

6. Click for information about archiving and this view.  

{PANEL/}

{PANEL: Document View: Archiving Properties}

![Figure 2. Document View: @archive-at Property](images/document-archive-at-property.png "Document View: @archive-at Property")

1. Edit the document to schedule its archiving.  
   To schedule the archiving of this document enter the document view, 
   edit the document, and add it a metadata `@archive-at` property with 
   the archival time (`UTC`) as a value.  

---

![Figure 3. Document View: @archived Property](images/document-archived-property.png "Document View: @archived Property")

1. The document is archived.  
   RavenDB replaced the `@archive-at` flag with an `@archived` flag, 
   indicating that the document is now archived.  

2. Studio recognizes that the document is archived.  

---

![Figure 4. Documents List: Archived Icon](images/documents-list-archived-icon.png "Documents List: Archived Icon")

An icon was added to the icons list so archived documents can be recognized in a glance.  

{PANEL/}

{PANEL: Database Settings: Archival Configuration Options}

Open the [Database Settings](../../../studio/database/settings/database-settings) 
view to set feature-specific archiving settings.  
This is where different features, currently indexing and data subscriptions, 
are taught how to handle archived documents.  

![Figure 5. Database Settings: Feature-Specific Archival Configuration](images/database-settings-01.png "Database Settings: Feature-Specific Archival Configuration")

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

![Figure 6. Database Settings: Edit Configuration](images/database-settings-02.png "Database Settings: Edit Configuration")

1. Click a feature-specific configuration option to edit its settings.  
2. Toggle to override current configuration.  
3. Select one of the three options.  
    * `ExcludeArchived`  
      DO not process archived documents  
    * `IncludeArchived`  
      Process archived documents  
    * `ArchivedOnly`
      Process ONLY archived documents  
4. Save your configuration.

{PANEL/}


## Related Articles

- [Documents Expiration](../../../server/extensions/expiration)  
